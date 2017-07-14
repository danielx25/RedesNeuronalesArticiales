using Npgsql;
using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;

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
									"Database="+Configuracion.DATABASE+";";

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

			Console.WriteLine ("Creando Lista Meteorologico...");
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
			Console.WriteLine ("Terminado");
			return tabla;
        }

		public static List<double[,]> datosPorRangoMp10(DateTime inicio, DateTime fin)
		{
			string datos_conexion = "Server="+Configuracion.SERVIDOR +";" +
				"Port="+Configuracion.PUERTO+";" +
				"CommandTimeout=500000;" +
				"User Id="+Configuracion.USUARIO+";" +
				"Password="+Configuracion.CONTRASEÑA+";" +
                "Database=" + Configuracion.DATABASE + ";";

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

			Console.WriteLine ("Creando Lista Meteorologico por rango de mp10..");
			List<double[]> sinAlerta = new List<double[]> ();
			List<double[]> alerta1 = new List<double[]> ();
			List<double[]> alerta2 = new List<double[]> ();
			List<double[]> alerta3 = new List<double[]> ();
			List<double[]> alerta4 = new List<double[]> ();
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


				if((Int32)leido [5] <= 150)//Sin Alerta
					sinAlerta.Add (filaActual);
				else if((Int32)leido [5] > 150 && (Int32)leido [5] <= 250)//Alerta 1
					alerta1.Add(filaActual);
				else if((Int32)leido [5] > 250 && (Int32)leido [5] <= 350)//Alerta 2
					alerta2.Add(filaActual);
				else if((Int32)leido [5] > 350 && (Int32)leido [5] <= 500)//Alerta 3
					alerta3.Add(filaActual);
				else if((Int32)leido [5] > 500)//Alerta 4
					alerta4.Add(filaActual);
			}
			List<double[,]> tabla = new List<double[,]> ();
			tabla.Add (listaAArreglo (sinAlerta));//Sin Alerta
			tabla.Add (listaAArreglo (alerta1));//Alerta 1
			tabla.Add (listaAArreglo (alerta2));//Alerta 2
			tabla.Add (listaAArreglo (alerta3));//Alerta 3
			tabla.Add (listaAArreglo (alerta4));//Alerta 4

			Console.WriteLine ("Terminado");
			return tabla;
		}

		private static double[,] listaAArreglo (List<double[]> entrada)
		{
			/*
			double[,] matriz = new double[entrada[0].Length, entrada.Count];
			for (int x = 0; x < entrada[0].Length; x++) {
				for(int y=0; y<entrada.Count; y++)
				{
					double[] fila = entrada [y];
					matriz [x, y] = fila [x];
				}
			}
			return matriz;
			*/
			double[,] matriz = new double[entrada[0].Length, entrada.Count];
			for (int x = 0; x < entrada.Count; x++) {
				double[] fila = entrada [x];
				for(int y=0; y<fila.Length; y++)
				{
					matriz [y, x] = fila [y];
				}
			}
			return matriz;
		}

		public static Hashtable datosMitigacion(DateTime inicio, DateTime fin)
		{
			string datos_conexion = "Server="+Configuracion.SERVIDOR +";" +
				"Port="+Configuracion.PUERTO+";" +
				"CommandTimeout=500000;" +
				"User Id="+Configuracion.USUARIO+";" +
				"Password="+Configuracion.CONTRASEÑA+";" +
                "Database=" + Configuracion.DATABASE + ";";

			NpgsqlConnection conexion = new NpgsqlConnection(datos_conexion);
			conexion.Open();
			string fechaInicio = inicio.Year + "-" + inicio.Month + "-" + inicio.Day + " " + inicio.Hour + ":" + inicio.Minute + ":" + inicio.Second;
			string fechaFinal = fin.Year + "-" + fin.Month + "-" + fin.Day + " " + fin.Hour + ":" + fin.Minute + ":" + fin.Second;
			NpgsqlCommand leer = new NpgsqlCommand("SELECT * FROM mitigacion " +
				"WHERE fecha BETWEEN '"+fechaInicio+"' AND '"+fechaFinal+"' " +
				"ORDER BY fecha ", conexion);
			NpgsqlDataReader leido = leer.ExecuteReader();

			Console.WriteLine ("Creando Lista Mitigación");
			Hashtable tabla = new Hashtable ();
			while (leido.Read())
			{
				tabla.Add ((DateTime)leido[0], leido);
			}
			Console.WriteLine ("Terminado");
			return tabla;
		}

		public static Hashtable datosDetencionPalas(DateTime inicio, DateTime fin)
		{
			string datos_conexion = "Server="+Configuracion.SERVIDOR +";" +
				"Port="+Configuracion.PUERTO+";" +
				"CommandTimeout=500000;" +
				"User Id="+Configuracion.USUARIO+";" +
				"Password="+Configuracion.CONTRASEÑA+";" +
                "Database=" + Configuracion.DATABASE + ";";

			NpgsqlConnection conexion = new NpgsqlConnection(datos_conexion);
			conexion.Open();
			string fechaInicio = inicio.Year + "-" + inicio.Month + "-" + inicio.Day + " " + inicio.Hour + ":" + inicio.Minute + ":" + inicio.Second;
			string fechaFinal = fin.Year + "-" + fin.Month + "-" + fin.Day + " " + fin.Hour + ":" + fin.Minute + ":" + fin.Second;
			NpgsqlCommand leer = new NpgsqlCommand("SELECT * FROM detencion_palas " +
				"WHERE fecha_inicio BETWEEN '"+fechaInicio+"' AND '"+fechaFinal+"' ", conexion);
			NpgsqlDataReader leido = leer.ExecuteReader();

			Console.WriteLine ("Creando Lista Detención de Palas");
			Hashtable tabla = new Hashtable ();
			while (leido.Read())
			{
				Console.WriteLine (((DateTime)leido[4]) - ((DateTime)leido[3]));
				//tabla.Add ((DateTime)leido[3], leido);
			}
			Console.WriteLine ("Terminado");
			return tabla;
		}

		public static Hashtable datosDetencionChancado(DateTime inicio, DateTime fin)
		{
			string datos_conexion = "Server="+Configuracion.SERVIDOR +";" +
				"Port="+Configuracion.PUERTO+";" +
				"CommandTimeout=500000;" +
				"User Id="+Configuracion.USUARIO+";" +
				"Password="+Configuracion.CONTRASEÑA+";" +
                "Database=" + Configuracion.DATABASE + ";";

			NpgsqlConnection conexion = new NpgsqlConnection(datos_conexion);
			conexion.Open();
			string fechaInicio = inicio.Year + "-" + inicio.Month + "-" + inicio.Day + " " + inicio.Hour + ":" + inicio.Minute + ":" + inicio.Second;
			string fechaFinal = fin.Year + "-" + fin.Month + "-" + fin.Day + " " + fin.Hour + ":" + fin.Minute + ":" + fin.Second;
			NpgsqlCommand leer = new NpgsqlCommand("SELECT * FROM detencion_chancado " +
				"WHERE fecha_inicio BETWEEN '"+fechaInicio+"' AND '"+fechaFinal+"' " +
				"ORDER BY fecha ", conexion);
			NpgsqlDataReader leido = leer.ExecuteReader();

			Console.WriteLine ("Creando Lista Detencion Chancado");
			Hashtable tabla = new Hashtable ();
			while (leido.Read())
			{
				//tabla.Add ((DateTime)leido[2], leido);
			}
			Console.WriteLine ("Terminado");
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
