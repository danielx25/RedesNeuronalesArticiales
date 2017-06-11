using Npgsql;
using System;
using System.Data;
using System.Collections.Generic;

namespace RedesNeuronalesArtificiales.BaseDeDatos
{
	class BaseDeDatos
    {
        public BaseDeDatos()
        {
			string datos_conexion = "Server=localhost;Port=5432;CommandTimeout=500000;User Id=postgres;Password=postgres;Database=meteorologico;";
            NpgsqlConnection conexion = new NpgsqlConnection(datos_conexion);
            conexion.Open();

			NpgsqlCommand leer = new NpgsqlCommand("SELECT * FROM meteorologico ORDER BY fecha", conexion);
			//WHERE fecha BETWEEN '2012-01-01 00:00:00' AND '2012-01-04 23:59:59'
            NpgsqlDataReader leido = leer.ExecuteReader();

            Console.WriteLine("Programas");
			Console.WriteLine ("Columnas: " + leido.FieldCount);

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
			/*
			int minutosFaltantes = 0;
			for(int x=0; x<tabla.Count; x++)
			{
				//if(x>0)
					//Console.WriteLine ((tabla [x].Fecha - tabla [x-1].Fecha).Minutes);
				if (x > 0 && (tabla [x].Fecha - tabla [x - 1].Fecha).Minutes > 1) {
					int faltan = 0;
					faltan = (tabla [x].Fecha - tabla [x - 1].Fecha).Minutes - 1;
					//Console.WriteLine ("Alerta Faltan minuto: " + faltan);
					//Console.WriteLine ("Entre: " + tabla [x - 1].Fecha + " y " + tabla [x].Fecha);
					DateTime fi = tabla [x - 1].Fecha;
					for(int y=1; y<faltan+1; y++)
					{
						tabla.Insert (x+y-1, new Fila(fi.AddMinutes(y),
							(tabla[x].VelocidadViento + tabla[x-1].VelocidadViento)/2,
							(Int16)((tabla[x].DireccionViento + tabla[x-1].DireccionViento)/2),
							(tabla[x].Temperatura + tabla[x-1].Temperatura)/2,
							(Int16)((tabla[x].HumedadRelativa + tabla[x-1].HumedadRelativa)/2),
							(tabla[x].Mp10 + tabla[x-1].Mp10)/2,
							(tabla[x].Precipitacion + tabla[x-1].Precipitacion)/2,
							(tabla[x].RadiacionSolar + tabla[x-1].RadiacionSolar)/2,
							(tabla[x].PrecionAtmosferica + tabla[x-1].PrecionAtmosferica)/2,
							(tabla[x].Evaporacion + tabla[x-1].Evaporacion)/2
							, true));
						minutosFaltantes++;
						//Console.WriteLine ("Insertando la fecha: " + fi.AddMinutes(y));
					}
				}
			}
			*/

			//Procesando e Insertando los datos a la tabla nueva
			mes = 0;
			int dia = 0;
			int numeroDatos = 2000;
			int datoProcesado = 0;
			string preConslta = "INSERT INTO meteorologicoprocesado2 (fecha, velocidad_viento, direccion_viento, temperatura, humedad_relativa, mp10, radiacion_solar, presion_atmosferica, precipitaciondia1, precipitaciondia2, precipitaciondia3, evaporaciondia1, evaporaciondia2, evaporaciondia3) Values ";
			string restoConsulta = "";
			Console.WriteLine ("Procesando e Insertando los datos a la tabla nueva");
			NpgsqlCommand insertar;
			for(int x=0; x<tabla.Count; x++)
			{
				if (mes != tabla [x].Fecha.Month) {
					mes = tabla [x].Fecha.Month;
					Console.WriteLine ("Año: " + tabla [x].Fecha.Year + " Mes: " + tabla [x].Fecha.Month + " Dia: " + tabla [x].Fecha.Day + " Hora: " + tabla [x].Fecha.Hour + " Minuto: " + tabla [x].Fecha.Minute);
				}
				if (dia != tabla [x].Fecha.Day) {
					dia = tabla [x].Fecha.Day;
					Console.WriteLine ("\tDia: " + dia);
				}
				if(x>0)
				{
					tabla [x].PrecipitacionAcumuladaDia1 = tabla [x - 1].PrecipitacionAcumuladaDia1 + tabla [x].Precipitacion;
					tabla [x].EvaporacionAcumuladaDia1 = tabla [x - 1].EvaporacionAcumuladaDia1 + tabla [x].Evaporacion;
					if (x >= 1440) {
						//Despues del primer dia
						tabla[x].PrecipitacionAcumuladaDia1 -= tabla[x-1440].Precipitacion;
						tabla[x].EvaporacionAcumuladaDia1 -= tabla[x-1440].Evaporacion;
					}
				}
				if ((tabla.Count - x) > 1439) {
					tabla [x + 1439].PrecipitacionAcumuladaDia2 = tabla [x].PrecipitacionAcumuladaDia1;
					tabla [x + 1439].EvaporacionAcumuladaDia2 = tabla [x].EvaporacionAcumuladaDia1;
				}
				if ((tabla.Count - x) > 2879) {
					tabla [x + 2879].PrecipitacionAcumuladaDia3 = tabla [x].PrecipitacionAcumuladaDia1;
					tabla [x + 2879].EvaporacionAcumuladaDia3 = tabla [x].EvaporacionAcumuladaDia1;
				}
				//Console.WriteLine ("INSERT INTO meteorologicoprocesado (fecha, velocidad_viento, direccion_viento, temperatura, humedad_relativa, mp10, radiacion_solar, presion_atmosferica, precipitaciondia1, precipitaciondia2, precipitaciondia3, evaporaciondia1, evaporaciondia2, evaporaciondia3) Values" +
				//	" ('"+tabla[x].Fecha+"', '"+tabla[x].VelocidadViento.ToString().Replace(",",".")+"', '"+tabla[x].DireccionViento+"', '"+tabla[x].Temperatura.ToString().Replace(",",".")+"', '"+tabla[x].HumedadRelativa+"', '"+tabla[x].Mp10+"', '"+tabla[x].RadiacionSolar+"', '"+tabla[x].PrecionAtmosferica+"', '"+tabla[x].PrecipitacionAcumuladaDia1.ToString().Replace(",",".")+"', '"+tabla[x].PrecipitacionAcumuladaDia2.ToString().Replace(",",".")+"', '"+tabla[x].PrecipitacionAcumuladaDia3.ToString().Replace(",",".")+"', '"+tabla[x].EvaporacionAcumuladaDia1.ToString().Replace(",",".")+"', '"+tabla[x].EvaporacionAcumuladaDia2.ToString().Replace(",",".")+"', '"+tabla[x].EvaporacionAcumuladaDia3.ToString().Replace(",",".")+"')");
				//NpgsqlCommand insertar = new NpgsqlCommand("INSERT INTO meteorologicoprocesado (fecha, velocidad_viento, direccion_viento, temperatura, humedad_relativa, mp10, radiacion_solar, presion_atmosferica, precipitaciondia1, precipitaciondia2, precipitaciondia3, evaporaciondia1, evaporaciondia2, evaporaciondia3) Values" +
				if (datoProcesado < numeroDatos) {
					if (datoProcesado == 0) {
						restoConsulta += " ('" + tabla [x].Fecha + "', '" + tabla [x].VelocidadViento.ToString ().Replace (",", ".") + "', '" + tabla [x].DireccionViento + "', '" + tabla [x].Temperatura.ToString ().Replace (",", ".") + "', '" + tabla [x].HumedadRelativa + "', '" + tabla [x].Mp10 + "', '" + tabla [x].RadiacionSolar + "', '" + tabla [x].PrecionAtmosferica + "', '" + tabla [x].PrecipitacionAcumuladaDia1.ToString ().Replace (",", ".") + "', '" + tabla [x].PrecipitacionAcumuladaDia2.ToString ().Replace (",", ".") + "', '" + tabla [x].PrecipitacionAcumuladaDia3.ToString ().Replace (",", ".") + "', '" + tabla [x].EvaporacionAcumuladaDia1.ToString ().Replace (",", ".") + "', '" + tabla [x].EvaporacionAcumuladaDia2.ToString ().Replace (",", ".") + "', '" + tabla [x].EvaporacionAcumuladaDia3.ToString ().Replace (",", ".") + "')";
					} else {
						restoConsulta += ", ('" + tabla [x].Fecha + "', '" + tabla [x].VelocidadViento.ToString ().Replace (",", ".") + "', '" + tabla [x].DireccionViento + "', '" + tabla [x].Temperatura.ToString ().Replace (",", ".") + "', '" + tabla [x].HumedadRelativa + "', '" + tabla [x].Mp10 + "', '" + tabla [x].RadiacionSolar + "', '" + tabla [x].PrecionAtmosferica + "', '" + tabla [x].PrecipitacionAcumuladaDia1.ToString ().Replace (",", ".") + "', '" + tabla [x].PrecipitacionAcumuladaDia2.ToString ().Replace (",", ".") + "', '" + tabla [x].PrecipitacionAcumuladaDia3.ToString ().Replace (",", ".") + "', '" + tabla [x].EvaporacionAcumuladaDia1.ToString ().Replace (",", ".") + "', '" + tabla [x].EvaporacionAcumuladaDia2.ToString ().Replace (",", ".") + "', '" + tabla [x].EvaporacionAcumuladaDia3.ToString ().Replace (",", ".") + "')";
					}
					datoProcesado++;
				} else {
					restoConsulta += ", ('" + tabla [x].Fecha + "', '" + tabla [x].VelocidadViento.ToString ().Replace (",", ".") + "', '" + tabla [x].DireccionViento + "', '" + tabla [x].Temperatura.ToString ().Replace (",", ".") + "', '" + tabla [x].HumedadRelativa + "', '" + tabla [x].Mp10 + "', '" + tabla [x].RadiacionSolar + "', '" + tabla [x].PrecionAtmosferica + "', '" + tabla [x].PrecipitacionAcumuladaDia1.ToString ().Replace (",", ".") + "', '" + tabla [x].PrecipitacionAcumuladaDia2.ToString ().Replace (",", ".") + "', '" + tabla [x].PrecipitacionAcumuladaDia3.ToString ().Replace (",", ".") + "', '" + tabla [x].EvaporacionAcumuladaDia1.ToString ().Replace (",", ".") + "', '" + tabla [x].EvaporacionAcumuladaDia2.ToString ().Replace (",", ".") + "', '" + tabla [x].EvaporacionAcumuladaDia3.ToString ().Replace (",", ".") + "')";
					insertar = new NpgsqlCommand (preConslta + restoConsulta, conexion);
					insertar.ExecuteNonQuery ();
					restoConsulta = "";
					datoProcesado = 0;
				}
				//insertar.ExecuteNonQuery ();
				//Console.WriteLine (tabla[x]);
			}
			insertar = new NpgsqlCommand (preConslta + restoConsulta, conexion);
			insertar.ExecuteNonQuery ();
			//Console.WriteLine ("Se rellenaron " + minutosFaltantes + " minutos");
			Console.WriteLine ("La lista contiene: " + tabla.Count + " elementos");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}