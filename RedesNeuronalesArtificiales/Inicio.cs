using RedesNeuronalesArtificiales.RNA;
using RedesNeuronalesArtificiales.BaseDeDatos;
using RedesNeuronalesArtificiales.Archivo;
using System;
using System.Collections.Generic;
using RedesNeuronalesArtificiales.AnalisisDeRNA;
using System.Diagnostics;
using System.Windows.Forms;

namespace RedesNeuronalesArtificiales
{
    static class Inicio
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]

        static void Main()
        {/*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Ventana());
*/
            Stopwatch tiempoEjecucion = new Stopwatch ();
			tiempoEjecucion.Start ();
			//Redes Som Entrenando

			DateTime inicio = new DateTime(2010,01,01,00,00,00);
			DateTime fin = new DateTime(2016,12,31,23,59,59);

			DateTime inicioPrueba = new DateTime(2017,01,01,00,00,00);
			DateTime finPrueba = new DateTime(2017,03,01,00,00,00);

			List<double[]> datosMeteorologicos = Conexion.datosMeteorologicos (inicio, fin, 100);
			List<double[]> datosPruebas = Conexion.datosMeteorologicos (inicioPrueba, finPrueba, 0);

			Som redNeuronal = new Som (datosMeteorologicos[0].Length,2500, 50, 0.01, 0.0001);
			redNeuronal.inicializarMatriz (0, 1);
			redNeuronal.Datos = datosMeteorologicos;

			tiempoEjecucion.Stop ();
			Console.WriteLine("Tiempo al cargar la base de datos: " + tiempoEjecucion.Elapsed.ToString());
			tiempoEjecucion.Restart ();

			EscribirArchivo archivo = new EscribirArchivo("Pesos aleatorios.html", true);
			archivo.imprimir (redNeuronal.obtenerMP10HTML());
			archivo.cerrar ();

			redNeuronal.entrenar (100);

			//Guarda Archivo
			Guardar.Serializar (redNeuronal, "Red Som Final.mp10");
            //Lee Archivo
			//Som redNeuronal = Guardar.Deserializar("Red Som Final.mp10");
			tiempoEjecucion.Stop();
			Console.WriteLine("Tiempo al entrenar la red: " + tiempoEjecucion.Elapsed.ToString());

			int error = 0;
			int errorMayor = 0;
			int errorMenor = 0;
			for(int x=0; x<datosPruebas.Count; x++)
			{
				double[] fila = datosPruebas [x];
				double mp10Real = fila [7]*800;
				fila [7] = -1;
				int[] resultado = redNeuronal.calcularResultados (fila);
				//Console.WriteLine ("Neurona Ganadora: " + resultado[0] + " MP10: " + resultado[1] + " MP10 real: " + (mp10Real) + " Alerta: " + calcularAlerta(resultado[1]) + " Alerta Real: " + calcularAlerta(mp10Real));
				if (calcularAlerta (resultado [1]) != calcularAlerta (mp10Real))
					error++;
				if (Math.Abs (calcularAlerta (resultado [1]) - calcularAlerta (mp10Real)) > 1)
					error++;
				if (calcularAlerta (resultado [1]) < calcularAlerta (mp10Real))
					errorMenor++;
			}
        }
		public static int calcularAlerta(double mp10)
		{
			if (mp10 <= 150)
				return 0;
			else if(mp10 > 150 && mp10 <= 250)
				return 1;
			else if(mp10 > 250 && mp10 <= 350)
				return 2;
			else if(mp10 > 350 && mp10 <= 500)
				return 3;
			else if(mp10 > 500)
				return 4;
			return -1;
		}
    }
}
