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
			/*
			DateTime inicio = new DateTime(2010,01,01,00,00,00);
			DateTime fin = new DateTime(2017,03,01,00,00,00);

			List<double[]> datosMeteorologicos = Conexion.datosMeteorologicos (inicio, fin);
			Som redNeuronal = new Som (datosMeteorologicos[0].Length,1600, 40);
			redNeuronal.inicializarMatriz (0, 1);
			redNeuronal.Datos = datosMeteorologicos;
			tiempoEjecucion.Stop ();
			Console.WriteLine("Tiempo al cargar la base de datos: " + tiempoEjecucion.Elapsed.ToString());
			tiempoEjecucion.Restart ();
			//Console.WriteLine (redNeuronal);
			redNeuronal.entrenar (100);
			Console.WriteLine (redNeuronal);
*/


			List<double[]> colores = new List<double[]> ();
			colores.Add (new double[] {n(29), n(196), n(72)});//RGB
			colores.Add (new double[] {n(4), n(131), n(37)});//RGB
			colores.Add (new double[] {n(94), n(109), n(224)});//RGB
			colores.Add (new double[] {n(0), n(30), n(255)});//RGB
			colores.Add (new double[] {n(255), n(210), n(0)});//RGB
			colores.Add (new double[] {n(210), n(29), n(157)});//RGB
			colores.Add (new double[] {n(255), n(0), n(0)});//RGB
			colores.Add (new double[] {n(0), n(255), n(0)});//RGB
			colores.Add (new double[] {n(0), n(0), n(255)});//RGB
			colores.Add (new double[] {n(255), n(255), n(255)});//RGB
			colores.Add (new double[] {n(0), n(0), n(0)});//RGB
			colores.Add (new double[] {n(216), n(152), n(149)});//RGB
			colores.Add (new double[] {n(232), n(141), n(52)});//RGB

			Som redColores = new Som (3, 400, 20);
			redColores.inicializarMatriz (0,1);
			redColores.Datos = colores;

			redColores.entrenar (5000);

			Console.WriteLine (redColores);
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
			//Guardar.Serializar (redNeuronal, "Red Som Final.mp10");
            //Lee Archivo

			//Som redNeuronalLeida = Guardar.Deserializar("Red Som Completa.mp10");
			//Console.WriteLine (redNeuronalLeida);
			tiempoEjecucion.Stop();
			Console.WriteLine("Tiempo al entrenar la red: " + tiempoEjecucion.Elapsed.ToString());
        }

		public static double n(int valor)
		{
			//(X_i - X.min) / (X.max - X.min)
			double valorNormalizado = ((double)(valor - 0))/((double)(255));
			if (valorNormalizado < 0 && valor != -1)
				Console.WriteLine ("Valor fuera de rango en: " + valor + " minimo: " + 0 + " maximo: " + 255);
			return valorNormalizado;
		}
    }
}
