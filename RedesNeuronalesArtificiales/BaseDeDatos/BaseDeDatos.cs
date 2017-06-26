using Npgsql;
using System;
using System.Data;
using System.Collections.Generic;

namespace RedesNeuronalesArtificiales.BaseDeDatos
{
	class BaseDeDatos
    {
		public List<Fila> datosMeteorologicos()
        {
			string datos_conexion = "Server="+Configuracion.SERVIDOR +";" +
									"Port="+Configuracion.PUERTO+";" +
									"CommandTimeout=500000;" +
									"User Id="+Configuracion.USUARIO+";" +
									"Password="+Configuracion.CONTRASEÑA+";" +
									"Database=meteorologico;";

            NpgsqlConnection conexion = new NpgsqlConnection(datos_conexion);
            conexion.Open();

			NpgsqlCommand leer = new NpgsqlCommand("SELECT * FROM meteorologico ORDER BY fecha", conexion);
			//WHERE fecha BETWEEN '2012-01-01 00:00:00' AND '2012-01-04 23:59:59'
            NpgsqlDataReader leido = leer.ExecuteReader();

			List<Fila> tabla = new List<Fila> ();
			Console.WriteLine ("Creando Lista");
			int mes = 0;
            while (leido.Read())
            {
				if (mes != ((DateTime)leido [0]).Month) {
					mes = ((DateTime)leido [0]).Month;
					Console.WriteLine ("Año: " + ((DateTime)leido [0]).Year + " Mes: " + ((DateTime)leido [0]).Month + " Dia: " + ((DateTime)leido [0]).Day + " Hora: " + ((DateTime)leido [0]).Hour + " Minuto: " + ((DateTime)leido [0]).Minute);
				}
				tabla.Add (new Fila((DateTime)leido[0],(Double)leido[1],(Int16)leido[2],(Double)leido[3],(Int16)leido[4],(Int32)leido[5],(Double)leido[6],(Int32)leido[7],(Int32)leido[8],(Double)leido[9]));

			}

			return tabla;
        }
    }
}