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
			NpgsqlCommand leer = new NpgsqlCommand("SELECT * FROM meteorologicohora " +
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
				double[] filaActual = new double[19];
				filaActual[0] = normalizar(fecha((DateTime)leido [0]),1,13);//Mes con los dias de decimales
				filaActual[1] = normalizar(hora((DateTime)leido [0]),0, 24);//Hora
				filaActual [2] = normalizar ((double)leido [1],0,30);//velocidad_viento
				filaActual [3] = normalizar ((Int16)leido [2],0,360);//direccion_viento
				filaActual [4] = normalizar ((double)leido [3],-10,55);//temperatura
				filaActual [5] = normalizar ((Int16)leido [4],0,100);//humedad_relativa
				filaActual [6] = normalizar ((Int32)leido [5],0,1400);//mp10
				filaActual [7] = normalizar ((Int32)leido [6],0,1700);//radiacion_solar
				filaActual [8] = normalizar ((Int32)leido [7],440,600);//presion_atmosferica
				filaActual [9] = normalizar ((double)leido [9],0,47);//precipitaciondia1
				filaActual [10] = normalizar ((double)leido [10],0,47);//precipitaciondia2
				filaActual [11] = normalizar ((double)leido [11],0,47);//precipitaciondia3
				filaActual [12] = normalizar ((double)leido [12],0,47);//precipitaciondia4
				filaActual [13] = normalizar ((double)leido [13],0,47);//precipitaciondia5
				filaActual [14] = normalizar ((double)leido [15],0,363000);//evaporaciondia1
				filaActual [15] = normalizar ((double)leido [16],0,363000);//evaporaciondia2
				filaActual [16] = normalizar ((double)leido [17],0,363000);//evaporaciondia3
				filaActual [17] = normalizar ((double)leido [18],0,363000);//evaporaciondia4
				filaActual [18] = normalizar ((double)leido [19],0,363000);//evaporaciondia5

				tabla.Add (filaActual);
			}
			return tabla;
        }

		public static double fecha(DateTime fecha)
		{
			return fecha.Month + (((99.99999*fecha.Day)/DateTime.DaysInMonth(fecha.Year, fecha.Month))/100);
		}

		public static double hora(DateTime fecha)
		{
			return fecha.Hour;
		}

		public static double normalizar(double valor, double minimo, double maximo)
		{
			//(X_i - X.min) / (X.max - X.min)
			double valorNormalizado = (valor - minimo)/(maximo-minimo);
			if (valorNormalizado < 0 && valor != -1)
				Console.WriteLine ("Valor fuera de rango en: " + valor + " minimo: " + minimo + " maximo: " + maximo);
			return valorNormalizado;
		}

		public static double normalizar(int valor, int minimo, int maximo)
		{
			//(X_i - X.min) / (X.max - X.min)
			double valorNormalizado = ((double)(valor - minimo))/((double)(maximo-minimo));
			if (valorNormalizado < 0 && valor != -1)
				Console.WriteLine ("Valor fuera de rango en: " + valor + " minimo: " + minimo + " maximo: " + maximo);
			return valorNormalizado;
		}
    }
}
