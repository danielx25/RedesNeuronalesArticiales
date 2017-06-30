using Npgsql;
using System;
using System.Data;
using System.Collections.Generic;

namespace RedesNeuronalesArtificiales.BaseDeDatos
{
	public class Conexion
    {
		
		public static List<Fila> datosMeteorologicos(DateTime inicio, DateTime fin)
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
            NpgsqlDataReader leido = leer.ExecuteReader();

			List<Fila> tabla = new List<Fila> ();
			Console.WriteLine ("Creando Lista");
			Fila filaActual;
            while (leido.Read())
            {
				filaActual = new Fila ((DateTime)leido [0], (Double)leido [1], (Int16)leido [2], (Double)leido [3], (Int16)leido [4], (Int32)leido [5], (Int32)leido [6], (Int32)leido [7], (Double)leido [8], (Double)leido [9], (Double)leido [10], (Double)leido [11], (Double)leido [12], (Double)leido [13], (Double)leido [14], (Double)leido [15], (Double)leido [16], (Double)leido [17]);
				tabla.Add (filaActual);

			}
			return tabla;
        }
    }
}