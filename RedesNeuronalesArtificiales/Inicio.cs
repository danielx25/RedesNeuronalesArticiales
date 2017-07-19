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
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Ventana());
            /*
            Stopwatch tiempoEjecucion = new Stopwatch ();
			tiempoEjecucion.Start ();

			//Atntiguo
            //Perceptron per = new Perceptron(7);
            //per.entrenamiento(EjemploEntrenamiento.LED7SEGMENTOS());
            //BackPropagation redmulticapa = new BackPropagation();
            //redmulticapa.entrenamiento(EjemploEntrenamiento.DESAYUNO_NORMALIZADO());
			//var z = new int[x.Length + y.Length];
			//x.CopyTo(z, 0);
			//y.CopyTo(z, x.Length);
			

			//Redes Som Entrenando

			DateTime inicio = new DateTime(2013,08,08,22,00,00);
			DateTime fin = new DateTime(2016,12,31,23,59,59);

			DateTime inicioPrueba = new DateTime(2017,01,01,00,00,00);
			DateTime finPrueba = new DateTime(2017,03,01,00,00,00);

			List<double[]> datosMeteorologicos = Conexion.datosMeteorologicos (inicio, fin);
			List<double[]> datosPruebas = Conexion.datosMeteorologicos (inicioPrueba, finPrueba);

			Som redNeuronal = new Som (datosMeteorologicos[0].Length,1600, 40);
			redNeuronal.inicializarMatriz (0, 1);
			redNeuronal.Datos = datosMeteorologicos;

			tiempoEjecucion.Stop ();
			Console.WriteLine("Tiempo al cargar la base de datos: " + tiempoEjecucion.Elapsed.ToString());
			tiempoEjecucion.Restart ();

			EscribirArchivo archivo = new EscribirArchivo("Pesos aleatorios.html", true);
			archivo.imprimir (redNeuronal.obtenerMP10HTML());
			archivo.cerrar ();

			redNeuronal.entrenar (200);

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            /*

            Double[,] vectoresG = { { 99, 2, 1},
                                  { 0, 0, 0},
                                  { 5, 5, 5},
                                  { 2, 1, 0} };

            Double[,] ejemplo = { { 0, 0, 0},
                                  { 0, 1, 0},
                                  { 1, 0, 0},
                                  { 1, 1, 1} };

            ConstruccionConjuntos conj = new ConstruccionConjuntos(4, 3);
            conj.tablaVectoresGrupos(vectoresG);
            conj.calcularConjuntoClase(ejemplo, 0);
            System.Console.ReadKey();


			//Guarda Archivo
			Guardar.Serializar (redNeuronal, "Red Som Final.mp10");
            //Lee Archivo
			//Som redNeuronalLeida = Guardar.Deserializar("Red Som Completa.mp10");
			//Console.WriteLine (redNeuronalLeida);
			tiempoEjecucion.Stop();
			Console.WriteLine("Tiempo al entrenar la red: " + tiempoEjecucion.Elapsed.ToString());


			for(int x=0; x<datosPruebas.Count; x++)
			{
				double[] fila = datosPruebas [x];
				double mp10Real = fila [7];
				fila [7] = -1;
				int[] resultado = redNeuronal.calcularResultados (fila);
				Console.WriteLine ("Neurona Ganadora: " + resultado[0] + " MP10: " + resultado[1] + " MP10 real: " + mp10Real + " Alerta: " + calcularAlerta(resultado[1]) + "Alerta Real: " + calcularAlerta(mp10Real));
			}
            */
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
