using Npgsql;
using System;
using System.Data;
using System.Collections.Generic;

namespace RedesNeuronalesArtificiales.BaseDeDatos
{
	public class Conexion
    {
		
		public static List<double[]> datosMeteorologicos(DateTime inicio, DateTime fin)
		{
			string datos_conexion = "Server="+Configuracion.SERVIDOR +";" +
									"Port="+Configuracion.PUERTO+";" +
									"CommandTimeout=500000;" +
									"User Id="+Configuracion.USUARIO+";" +
									"Password="+Configuracion.CONTRASEÑA+";" +
									"Database=meteorologico;";

            NpgsqlConnection conexion = new NpgsqlConnection(datos_conexion);
            conexion.Open();
			string fechaInicio = inicio.Year + "-" + inicio.Month + "-" + inicio.Day + " " + inicio.Hour + ":" + inicio.Minute + ":" + inicio.Second;
			string fechaFinal = fin.Year + "-" + fin.Month + "-" + fin.Day + " " + fin.Hour + ":" + fin.Minute + ":" + fin.Second;
			NpgsqlCommand leer = new NpgsqlCommand("SELECT * FROM meteorologicoprocesado " +
															"WHERE fecha BETWEEN '"+fechaInicio+"' AND '"+fechaFinal+"' " +
															"ORDER BY fecha ", conexion);
			//Console.WriteLine ("SELECT * FROM meteorologicoprocesado " +
			//	"WHERE fecha BETWEEN '"+fechaInicio+"' AND '"+fechaFinal+"' " +
			//	"ORDER BY fecha ");
            NpgsqlDataReader leido = leer.ExecuteReader();

			Console.WriteLine ("Creando Lista");
			List<double[]> tabla = new List<double[]> ();
            while (leido.Read())
            {
				double[] filaActual = new double[24];
				filaActual[1] = fechaDifusa((DateTime)leido [0], 1);//Verano
				filaActual[2] = fechaDifusa((DateTime)leido [0], 2);//Otoño
				filaActual[3] = fechaDifusa((DateTime)leido [0], 3);//Invierno
				filaActual[4] = fechaDifusa((DateTime)leido [0], 4);//Primavera
				filaActual[5] = horaDifusa((DateTime)leido [0], 1);//Dia
				filaActual[6] = fechaDifusa((DateTime)leido [0], 2);//Noche
				filaActual [7] = normalizar ((double)leido [1],0,30);//velocidad_viento
				filaActual [8] = normalizar ((Int16)leido [2],0,360);//direccion_viento
				filaActual [9] = normalizar ((double)leido [3],-40,50);//temperatura
				filaActual [10] = normalizar ((Int16)leido [4],0,100);//humedad_relativa
				filaActual [11] = normalizar ((Int32)leido [5],0,10000);//mp10
				filaActual [12] = normalizar ((Int32)leido [6],0,1700);//radiacion_solar
				filaActual [13] = normalizar ((Int32)leido [7],400,600);//presion_atmosferica
				filaActual [14] = normalizar ((double)leido [8],0,20000);//precipitaciondia1
				filaActual [15] = normalizar ((double)leido [9],0,20000);//precipitaciondia2
				filaActual [16] = normalizar ((double)leido [10],0,20000);//precipitaciondia3
				filaActual [17] = normalizar ((double)leido [11],0,20000);//precipitaciondia4
				filaActual [18] = normalizar ((double)leido [12],0,20000);//precipitaciondia5
				filaActual [19] = normalizar ((double)leido [13],0,37000);//evaporaciondia1
				filaActual [20] = normalizar ((double)leido [14],0,37000);//evaporaciondia2
				filaActual [21] = normalizar ((double)leido [15],0,37000);//evaporaciondia3
				filaActual [22] = normalizar ((double)leido [16],0,37000);//evaporaciondia4
				filaActual [23] = normalizar ((double)leido [17],0,37000);//evaporaciondia5

				tabla.Add (filaActual);
			}
			return tabla;
        }

		public static double fechaDifusa(DateTime fecha, int estacion)
		{
			//1 -> Verano
			//2 -> Otoño
			//3 -> Invierno
			//4 -> Primavera
			return 0;
		}

		public static double horaDifusa(DateTime fecha, int diaNoche)
		{
			//1 -> Dia
			//2 -> Noche
			return 0;
		}

		public static double normalizar(double valor, double minimo, double maximo)
		{
			//(X_i - X.min) / (X.max - X.min)
			return (valor - minimo)/(maximo-minimo);
		}

		public static double normalizar(int valor, int minimo, int maximo)
		{
			//(X_i - X.min) / (X.max - X.min)
			return ((double)(valor - minimo))/((double)(maximo-minimo));
		}
    }
}