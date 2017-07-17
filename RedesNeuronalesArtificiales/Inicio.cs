using RedesNeuronalesArtificiales.RNA;
using RedesNeuronalesArtificiales.BaseDeDatos;
using RedesNeuronalesArtificiales.Archivo;
using System;
using System.Collections.Generic;
using RedesNeuronalesArtificiales.AnalisisDeRNA;
using System.Diagnostics;

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
			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Form1());

			//Redes Som Entrenando

			DateTime inicio = new DateTime(2013,08,08,22,00,00);
			DateTime fin = new DateTime(2017,03,01,00,00,00);

			List<double[]> datosMeteorologicos = Conexion.datosMeteorologicos (inicio, fin);
			Som redNeuronal = new Som (datosMeteorologicos[0].Length,1225, 35);
			redNeuronal.inicializarMatriz (0, 1);
			redNeuronal.Datos = datosMeteorologicos;
			tiempoEjecucion.Stop ();
			Console.WriteLine("Tiempo al cargar la base de datos: " + tiempoEjecucion.Elapsed.ToString());
			tiempoEjecucion.Restart ();
			//Console.WriteLine (redNeuronal);
			EscribirArchivo archivo = new EscribirArchivo("Pesos aleatorios.html");
			archivo.imprimir (redNeuronal.obtenerMP10HTML2());
			archivo.cerrar ();

			redNeuronal.entrenar (1000);
			//Console.WriteLine (redNeuronal);

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
*/

			//Guarda Archivo
			Guardar.Serializar (redNeuronal, "Red Som Final.mp10");
            //Lee Archivo

			//Som redNeuronalLeida = Guardar.Deserializar("Red Som Completa.mp10");
			//Console.WriteLine (redNeuronalLeida);
			tiempoEjecucion.Stop();
			Console.WriteLine("Tiempo al entrenar la red: " + tiempoEjecucion.Elapsed.ToString());
        }
    }
}
