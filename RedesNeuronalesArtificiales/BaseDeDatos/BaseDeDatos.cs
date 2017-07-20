using Npgsql;
using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;

namespace RedesNeuronalesArtificiales.BaseDeDatos
{
	public class Conexion
    {
		
		public static List<double[]> datosMeteorologicos(DateTime inicio, DateTime fin, int mp10Minimo)
		{
			string datos_conexion = "Server="+Configuracion.SERVIDOR +";" +
									"Port="+Configuracion.PUERTO+";" +
									"CommandTimeout=500000;" +
									"User Id="+Configuracion.USUARIO+";" +
									"Password="+Configuracion.CONTRASEÑA+";" +
									"Database="+Configuracion.BASEDEDATOS+";";

			//Meteorologicos
            NpgsqlConnection conexion = new NpgsqlConnection(datos_conexion);
            conexion.Open();
			string fechaInicio = inicio.Year + "-" + inicio.Month + "-" + inicio.Day + " " + inicio.Hour + ":" + inicio.Minute + ":" + inicio.Second;
			string fechaFinal = fin.Year + "-" + fin.Month + "-" + fin.Day + " " + fin.Hour + ":" + fin.Minute + ":" + fin.Second;
			NpgsqlCommand meteorologicos = new NpgsqlCommand("SELECT * FROM meteorologicohora " +
															"WHERE fecha BETWEEN '"+fechaInicio+"' AND '"+fechaFinal+"' " +
															"and mp10 >= "+mp10Minimo+" ORDER BY fecha ", conexion);

			NpgsqlDataReader datosMeteorologicosLeidos = meteorologicos.ExecuteReader();

			//Palas
			List<HashSet<DateTime>> palas = Conexion.datosDetencionPalas();
			//Chancado
			List<HashSet<DateTime>> chancados = Conexion.datosDetencionChancado();
			//Mitigacion
			Hashtable mitigacion = Conexion.datosMitigacion();

			Console.WriteLine ("Creando Lista Meteorologico...");
			List<double[]> tabla = new List<double[]> ();
			while (datosMeteorologicosLeidos.Read())
            {
				double[] filaActual = new double[38];
				DateTime fechaActual = (DateTime)datosMeteorologicosLeidos [0];
				filaActual[0] = Difuso.verano(fechaActual);//Verano
				filaActual[1] = Difuso.invierno(fechaActual);//Invierno
				filaActual[2] = Entrada.normalizar(fechaActual.Hour,0, 24);//Hora
				filaActual [3] = Entrada.normalizar ((double)datosMeteorologicosLeidos [1],0,30);//velocidad_viento
				filaActual [4] = Entrada.normalizar ((Int16)datosMeteorologicosLeidos [2],0,360);//direccion_viento
				filaActual [5] = Entrada.normalizar ((double)datosMeteorologicosLeidos [3],-10,55);//temperatura
				filaActual [6] = Entrada.normalizar ((Int16)datosMeteorologicosLeidos [4],0,100);//humedad_relativa
				filaActual [7] = Entrada.normalizar ((Int32)datosMeteorologicosLeidos [5],0,800);//mp10
				filaActual [8] = Entrada.normalizar ((Int32)datosMeteorologicosLeidos [6],0,1700);//radiacion_solar
				filaActual [9] = Entrada.normalizar ((Int32)datosMeteorologicosLeidos [7],440,600);//presion_atmosferica
				filaActual [10] = Entrada.normalizar ((double)datosMeteorologicosLeidos [9],0,47);//precipitaciondia1
				filaActual [11] = Entrada.normalizar ((double)datosMeteorologicosLeidos [10],0,47);//precipitaciondia2
				filaActual [12] = Entrada.normalizar ((double)datosMeteorologicosLeidos [11],0,47);//precipitaciondia3
				filaActual [13] = Entrada.normalizar ((double)datosMeteorologicosLeidos [12],0,47);//precipitaciondia4
				filaActual [14] = Entrada.normalizar ((double)datosMeteorologicosLeidos [13],0,47);//precipitaciondia5
				filaActual [15] = Entrada.normalizar ((double)datosMeteorologicosLeidos [15],0,363000);//evaporaciondia1
				filaActual [16] = Entrada.normalizar ((double)datosMeteorologicosLeidos [16],0,363000);//evaporaciondia2
				filaActual [17] = Entrada.normalizar ((double)datosMeteorologicosLeidos [17],0,363000);//evaporaciondia3
				filaActual [18] = Entrada.normalizar ((double)datosMeteorologicosLeidos [18],0,363000);//evaporaciondia4
				filaActual [19] = Entrada.normalizar ((double)datosMeteorologicosLeidos [19],0,363000);//evaporaciondia5

				//Palas y Chancadores
				filaActual [20] = estado(palas[0], fechaActual);
				filaActual [21] = estado(palas[1], fechaActual);
				filaActual [22] = estado(palas[2], fechaActual);
				filaActual [23] = estado(palas[3], fechaActual);
				filaActual [24] = estado(palas[4], fechaActual);
				filaActual [25] = estado(palas[5], fechaActual);
				filaActual [26] = estado(palas[6], fechaActual);
				filaActual [27] = estado(palas[7], fechaActual);
				filaActual [28] = estado(chancados[0], fechaActual);
				filaActual [29] = estado(chancados[1], fechaActual);

				//Mitigación
				string clave = fechaActual.Year+"-"+fechaActual.Month+"-"+fechaActual.Day;
				double[] filaMitigacion = (double[])mitigacion[clave];
				if (filaMitigacion == null) {
					filaMitigacion = new double[] {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};
				}
				filaActual [30] = Entrada.normalizar(valorHora(filaMitigacion[0], filaMitigacion[1], fechaActual), 0, 7);//chaxa camion dia y noche
				filaActual [31] = Entrada.normalizar(valorHora(filaMitigacion[2], filaMitigacion[3], fechaActual), 0, 8);//movitec camion dia y noche
				double valorNoche = 0;
				if (filaMitigacion [4] == -1)
					valorNoche = -1;
				filaActual [32] = Entrada.normalizar(valorHora(filaMitigacion[4], valorNoche, fechaActual), 0, 4);//das camion dia
				filaActual [33] = Entrada.normalizar(Math.Round((filaMitigacion[5]/24),1),0,90240);//cnorte consumo de agua
				filaActual [34] = Entrada.normalizar(Math.Round((filaMitigacion[6]/24),1),0,4480);//cmovil consumo de agua
				filaActual [35] = Entrada.normalizar(Math.Round((filaMitigacion[7]/24),1),0,1500);//cachimba1 consumo de agua
				filaActual [36] = Entrada.normalizar(Math.Round((filaMitigacion[8]/24),1),0,2270);//cachimba2 consumo de agua
				filaActual [37] = Entrada.normalizar(Math.Round((filaMitigacion[9]/24),1),0,27000);//gerencia consumo de agua

				tabla.Add (filaActual);
			}
			Console.WriteLine ("Terminado");
			return tabla;
        }

		public static List<double[,]> datosPorRangoMp10(DateTime inicio, DateTime fin, int mp10Minimo)
		{
			string datos_conexion = "Server="+Configuracion.SERVIDOR +";" +
				"Port="+Configuracion.PUERTO+";" +
				"CommandTimeout=500000;" +
				"User Id="+Configuracion.USUARIO+";" +
				"Password="+Configuracion.CONTRASEÑA+";" +
				"Database="+Configuracion.BASEDEDATOS+";";

			//Meteorologicos
			NpgsqlConnection conexion = new NpgsqlConnection(datos_conexion);
			conexion.Open();
			string fechaInicio = inicio.Year + "-" + inicio.Month + "-" + inicio.Day + " " + inicio.Hour + ":" + inicio.Minute + ":" + inicio.Second;
			string fechaFinal = fin.Year + "-" + fin.Month + "-" + fin.Day + " " + fin.Hour + ":" + fin.Minute + ":" + fin.Second;
			NpgsqlCommand meteorologicos = new NpgsqlCommand("SELECT * FROM meteorologicohora " +
				"WHERE fecha BETWEEN '"+fechaInicio+"' AND '"+fechaFinal+"' " +
				"and mp10 >= "+mp10Minimo+" ORDER BY fecha ", conexion);

			NpgsqlDataReader leido = meteorologicos.ExecuteReader();

			//Palas
			List<HashSet<DateTime>> palas = Conexion.datosDetencionPalas();
			//Chancado
			List<HashSet<DateTime>> chancados = Conexion.datosDetencionChancado();
			//Mitigacion
			Hashtable mitigacion = Conexion.datosMitigacion();

			Console.WriteLine ("Creando Lista Meteorologico por rango de mp10..");
			List<double[]> sinAlerta = new List<double[]> ();
			List<double[]> alerta1 = new List<double[]> ();
			List<double[]> alerta2 = new List<double[]> ();
			List<double[]> alerta3 = new List<double[]> ();
			List<double[]> alerta4 = new List<double[]> ();
			while (leido.Read())
			{
				double[] filaActual = new double[38];
				DateTime fechaActual = (DateTime)leido [0];
				filaActual[0] = Difuso.verano(fechaActual);//Verano
				filaActual[1] = Difuso.invierno(fechaActual);//Invierno
				filaActual[2] = Entrada.normalizar(fechaActual.Hour,0, 24);//Hora
				filaActual [3] = Entrada.normalizar ((double)leido [1],0,30);//velocidad_viento
				filaActual [4] = Entrada.normalizar ((Int16)leido [2],0,360);//direccion_viento
				filaActual [5] = Entrada.normalizar ((double)leido [3],-10,55);//temperatura
				filaActual [6] = Entrada.normalizar ((Int16)leido [4],0,100);//humedad_relativa
				filaActual [7] = Entrada.normalizar ((Int32)leido [5],0,800);//mp10
				filaActual [8] = Entrada.normalizar ((Int32)leido [6],0,1700);//radiacion_solar
				filaActual [9] = Entrada.normalizar ((Int32)leido [7],440,600);//presion_atmosferica
				filaActual [10] = Entrada.normalizar ((double)leido [9],0,47);//precipitaciondia1
				filaActual [11] = Entrada.normalizar ((double)leido [10],0,47);//precipitaciondia2
				filaActual [12] = Entrada.normalizar ((double)leido [11],0,47);//precipitaciondia3
				filaActual [13] = Entrada.normalizar ((double)leido [12],0,47);//precipitaciondia4
				filaActual [14] = Entrada.normalizar ((double)leido [13],0,47);//precipitaciondia5
				filaActual [15] = Entrada.normalizar ((double)leido [15],0,363000);//evaporaciondia1
				filaActual [16] = Entrada.normalizar ((double)leido [16],0,363000);//evaporaciondia2
				filaActual [17] = Entrada.normalizar ((double)leido [17],0,363000);//evaporaciondia3
				filaActual [18] = Entrada.normalizar ((double)leido [18],0,363000);//evaporaciondia4
				filaActual [19] = Entrada.normalizar ((double)leido [19],0,363000);//evaporaciondia5

				//Palas y Chancadores
				filaActual [20] = estado(palas[0], fechaActual);
				filaActual [21] = estado(palas[1], fechaActual);
				filaActual [22] = estado(palas[2], fechaActual);
				filaActual [23] = estado(palas[3], fechaActual);
				filaActual [24] = estado(palas[4], fechaActual);
				filaActual [25] = estado(palas[5], fechaActual);
				filaActual [26] = estado(palas[6], fechaActual);
				filaActual [27] = estado(palas[7], fechaActual);
				filaActual [28] = estado(chancados[0], fechaActual);
				filaActual [29] = estado(chancados[1], fechaActual);

				//Mitigación
				string clave = fechaActual.Year+"-"+fechaActual.Month+"-"+fechaActual.Day;
				double[] filaMitigacion = (double[])mitigacion[clave];
				if (filaMitigacion == null) {
					filaMitigacion = new double[] {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};
				}
				filaActual [30] = Entrada.normalizar(valorHora(filaMitigacion[0], filaMitigacion[1], fechaActual), 0, 7);//chaxa camion dia y noche
				filaActual [31] = Entrada.normalizar(valorHora(filaMitigacion[2], filaMitigacion[3], fechaActual), 0, 8);//movitec camion dia y noche
				double valorNoche = 0;
				if (filaMitigacion [4] == -1)
					valorNoche = -1;
				filaActual [32] = Entrada.normalizar(valorHora(filaMitigacion[4], valorNoche, fechaActual), 0, 4);//das camion dia
				filaActual [33] = Entrada.normalizar(Math.Round((filaMitigacion[5]/24),1),0,90240);//cnorte consumo de agua
				filaActual [34] = Entrada.normalizar(Math.Round((filaMitigacion[6]/24),1),0,4480);//cmovil consumo de agua
				filaActual [35] = Entrada.normalizar(Math.Round((filaMitigacion[7]/24),1),0,1500);//cachimba1 consumo de agua
				filaActual [36] = Entrada.normalizar(Math.Round((filaMitigacion[8]/24),1),0,2270);//cachimba2 consumo de agua
				filaActual [37] = Entrada.normalizar(Math.Round((filaMitigacion[9]/24),1),0,27000);//gerencia consumo de agua

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
			double[,] matriz = new double[entrada.Count, entrada[0].Length];
			for (int x = 0; x < entrada.Count; x++) {
				double[] fila = entrada [x];
				for(int y=0; y<fila.Length; y++)
				{
					matriz [x, y] = fila [y];
				}
			}
			return matriz;
		}

		private static Hashtable datosMitigacion()
		{
			string datos_conexion = "Server="+Configuracion.SERVIDOR +";" +
				"Port="+Configuracion.PUERTO+";" +
				"CommandTimeout=500000;" +
				"User Id="+Configuracion.USUARIO+";" +
				"Password="+Configuracion.CONTRASEÑA+";" +
				"Database="+Configuracion.BASEDEDATOS+";";

			NpgsqlConnection conexion = new NpgsqlConnection(datos_conexion);
			conexion.Open();
			NpgsqlCommand leer = new NpgsqlCommand("SELECT * FROM mitigacion WHERE cachimba2 >= 0", conexion);
			NpgsqlDataReader leido = leer.ExecuteReader();

			/*
				//Mitigacion
			fecha timestamp without time zone,
			chaxadia double precision,
			chaxanoche double precision,
			movitecdia double precision,
			movitecnoche double precision,
			dasdia double precision,
			cnorte double precision,
			cmovil double precision,
			cachimba1 double precision,
			cachimba2 double precision,
			gerencia double precision
			*/

			Console.WriteLine ("Creando Lista Mitigación");
			Hashtable tabla = new Hashtable ();
			while (leido.Read())
			{
				double[] datos = new double[] {
					(double)leido[1],
					(double)leido[2],
					(double)leido[3],
					(double)leido[4],
					(double)leido[5],
					(double)leido[6],
					(double)leido[7],
					(double)leido[8],
					(double)leido[9],
					(double)leido[10]};
				string clave = ((DateTime)leido[0]).Year+"-"+((DateTime)leido[0]).Month+"-"+((DateTime)leido[0]).Day;
				if (tabla [clave] != null)
					tabla.Remove (clave);
				tabla.Add (clave, datos);
			}
			Console.WriteLine ("Terminado");
			return tabla;
		}

		private static List<HashSet<DateTime>> datosDetencionPalas()
		{
			string datos_conexion = "Server="+Configuracion.SERVIDOR +";" +
				"Port="+Configuracion.PUERTO+";" +
				"CommandTimeout=500000;" +
				"User Id="+Configuracion.USUARIO+";" +
				"Password="+Configuracion.CONTRASEÑA+";" +
				"Database="+Configuracion.BASEDEDATOS+";";

			NpgsqlConnection conexion = new NpgsqlConnection(datos_conexion);
			conexion.Open();
			NpgsqlCommand leer = new NpgsqlCommand("SELECT * FROM detencion_palas " +
				"WHERE fecha_inicio is not null and fecha_termino is not null", conexion);
			NpgsqlDataReader leido = leer.ExecuteReader();

			/*
				//Palas
			lugar text,
			banco text,
			equipo text,
			fecha_inicio timestamp without time zone,
			fecha_termino timestamp without time zone,
			tipo_actividad text
			*/

			Console.WriteLine ("Creando Lista Detención de Palas");
			HashSet<DateTime> pala1 = new HashSet<DateTime> ();
			HashSet<DateTime> pala3 = new HashSet<DateTime> ();
			HashSet<DateTime> pala4 = new HashSet<DateTime> ();
			HashSet<DateTime> pala5 = new HashSet<DateTime> ();
			HashSet<DateTime> pala7 = new HashSet<DateTime> ();
			HashSet<DateTime> pala8 = new HashSet<DateTime> ();
			HashSet<DateTime> pala10 = new HashSet<DateTime> ();
			HashSet<DateTime> pala11 = new HashSet<DateTime> ();

			while (leido.Read())
			{
				DateTime fechaInicio = (DateTime)leido [3];
				DateTime fechaTermino = (DateTime)leido [4];
				TimeSpan resta = fechaTermino - fechaInicio;
				string equipo = (string)leido [2];
				if (resta.TotalDays < 10 && resta.TotalMilliseconds > 0) {
					if (equipo.Replace("-","").Equals ("pa01", StringComparison.OrdinalIgnoreCase) || equipo.Replace("-","").Equals ("pa1", StringComparison.OrdinalIgnoreCase)) {
						agregarElemento (pala1, fechaInicio, fechaTermino);
					} else if (equipo.Replace("-","").Equals ("pa03", StringComparison.OrdinalIgnoreCase) || equipo.Replace("-","").Equals ("pa3", StringComparison.OrdinalIgnoreCase)) {
						agregarElemento (pala3, fechaInicio, fechaTermino);
					} else if (equipo.Replace("-","").Equals ("pa04", StringComparison.OrdinalIgnoreCase) || equipo.Replace("-","").Equals ("pa4", StringComparison.OrdinalIgnoreCase)) {
						agregarElemento (pala4, fechaInicio, fechaTermino);
					} else if (equipo.Replace("-","").Equals ("pa05", StringComparison.OrdinalIgnoreCase) || equipo.Replace("-","").Equals ("pa5", StringComparison.OrdinalIgnoreCase)) {
						agregarElemento (pala5, fechaInicio, fechaTermino);
					} else if (equipo.Replace("-","").Equals ("pa07", StringComparison.OrdinalIgnoreCase) || equipo.Replace("-","").Equals ("pa7", StringComparison.OrdinalIgnoreCase)) {
						agregarElemento (pala7, fechaInicio, fechaTermino);
					} else if (equipo.Replace("-","").Equals ("cf08", StringComparison.OrdinalIgnoreCase) || equipo.Replace("-","").Equals ("cf8", StringComparison.OrdinalIgnoreCase)) {
						agregarElemento (pala8, fechaInicio, fechaTermino);
					} else if (equipo.Replace("-","").Equals ("cf10", StringComparison.OrdinalIgnoreCase)) {
						agregarElemento (pala10, fechaInicio, fechaTermino);
					} else if (equipo.Replace("-","").Equals ("cf11", StringComparison.OrdinalIgnoreCase)) {
						agregarElemento (pala11, fechaInicio, fechaTermino);
					} else {
						Console.WriteLine (equipo);
					}
				}
			}
			List<HashSet<DateTime>> tabla = new List<HashSet<DateTime>> ();
			tabla.Add (pala1);
			tabla.Add (pala3);
			tabla.Add (pala4);
			tabla.Add (pala5);
			tabla.Add (pala7);
			tabla.Add (pala8);
			tabla.Add (pala10);
			tabla.Add (pala11);
			Console.WriteLine ("Terminado");
			return tabla;
		}

		private static List<HashSet<DateTime>> datosDetencionChancado()
		{
			string datos_conexion = "Server="+Configuracion.SERVIDOR +";" +
				"Port="+Configuracion.PUERTO+";" +
				"CommandTimeout=500000;" +
				"User Id="+Configuracion.USUARIO+";" +
				"Password="+Configuracion.CONTRASEÑA+";" +
				"Database="+Configuracion.BASEDEDATOS+";";

			NpgsqlConnection conexion = new NpgsqlConnection(datos_conexion);
			conexion.Open();
			NpgsqlCommand leer = new NpgsqlCommand("SELECT * FROM detencion_chancado  WHERE fecha_inicio is not null and fecha_termino is not null", conexion);
			NpgsqlDataReader leido = leer.ExecuteReader();

			Console.WriteLine ("Creando Lista Detencion Chancado");

			/*
				//Chancado
			lugar text,
			equipo text,
			fecha_inicio timestamp without time zone,
			fecha_termino timestamp without time zone,
			tipo_actividad text
			*/

			HashSet<DateTime> chancado1 = new HashSet<DateTime> ();
			HashSet<DateTime> chancado2 = new HashSet<DateTime> ();
			while (leido.Read())
			{
				DateTime fechaInicio = (DateTime)leido [2];
				DateTime fechaTermino = (DateTime)leido [3];
				TimeSpan resta = fechaTermino - fechaInicio;
				string equipo = (string)leido [1];
				if (resta.TotalDays < 10) {
					if (equipo.Equals("ch-01", StringComparison.OrdinalIgnoreCase) || equipo.Equals("ch-1", StringComparison.OrdinalIgnoreCase)) {
						agregarElemento (chancado1, fechaInicio, fechaTermino);
					} else if (equipo.Equals("ch-02", StringComparison.OrdinalIgnoreCase) || equipo.Equals("ch-2", StringComparison.OrdinalIgnoreCase)) {
						agregarElemento (chancado2, fechaInicio, fechaTermino);
					}

				}
			}
			Console.WriteLine ("Terminado");
			List<HashSet<DateTime>> tabla = new List<HashSet<DateTime>> ();
			tabla.Add (chancado1);
			tabla.Add (chancado2);
			return tabla;
		}

		private static void agregarElemento(HashSet<DateTime> tabla, DateTime fechaInicio, DateTime fechaTemino)
		{
			DateTime fechaActual = new DateTime (fechaInicio.Year, fechaInicio.Month, fechaInicio.Day, fechaInicio.Hour, 0,0);
			while (fechaTemino > fechaActual) {
				if (!tabla.Contains(fechaActual)) {
					tabla.Add (fechaActual);
				}
				fechaActual = fechaActual.AddHours (1);
			}
		}

		private static double valorHora (double dia, double noche, DateTime fecha)
		{
			//Para los camiones que esta dividido en dos columnas
			if (fecha.Hour >= 8 && fecha.Hour <= 18) {
				return dia;
			} else {
				return noche;
			}
		}

		private static double estado(HashSet<DateTime> elementos, DateTime fecha)
		{
			//El estado de las maquinarias si esta activa o inactiva
			if (elementos.Contains(fecha))
				return 1;
			else
				return 0;
		}
    }
}
