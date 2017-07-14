using RedesNeuronalesArtificiales.RNA;
using RedesNeuronalesArtificiales.BaseDeDatos;
using System;
using System.Collections.Generic;
using RedesNeuronalesArtificiales.AnalisisDeRNA;

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

            /*

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

			DateTime inicio = new DateTime(2010,01,01,00,00,00);
			DateTime fin = new DateTime(2017,03,01,00,00,00);

			List<double[]> datosMeteorologicos = Conexion.datosMeteorologicos (inicio, fin);
			Console.WriteLine ("Total: " + datosMeteorologicos.Count);
			List<double[,]> dr = Conexion.datosPorRangoMp10 (inicio, fin);
			double[,] sa = dr [0];//Sin Alerta
			double[,] a1 = dr [1];//Alerta 1
			double[,] a2 = dr [2];//Alerta 2
			double[,] a3 = dr [3];//Alerta 3
			double[,] a4 = dr [4];//Alerta 4

			Console.WriteLine ("Sin Alerta: " + sa.GetLength(0) + " " + sa.GetLength(1));
			Console.WriteLine ("Alerta 1: " + a1.GetLength(0) + " " + a1.GetLength(1));
			Console.WriteLine ("Alerta 2: " + a2.GetLength(0) + " " + a2.GetLength(1));
			Console.WriteLine ("Alerta 3: " + a3.GetLength(0) + " " + a3.GetLength(1));
			Console.WriteLine ("Alerta 4: " + a4.GetLength(0) + " " + a4.GetLength(1));

			/*Som redNeuronal = new Som (datosMeteorologicos[0].Length,1600, 40);
			redNeuronal.inicializarMatriz (0, 1);
			redNeuronal.Datos = datosMeteorologicos;

			//Conexion.datosDetencionPalas(inicio, fin);

			Console.WriteLine (redNeuronal);
			redNeuronal.entrenar (100);
			Console.WriteLine (redNeuronal);

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            */

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
			//Guardar.Serializar (redNeuronal, "Red Som Completa.mp10");
            //Lee Archivo

			//Som redNeuronalLeida = Guardar.Deserializar("Red Som.mp10");
			//Console.WriteLine (redNeuronalLeida);

        }
    }
}
