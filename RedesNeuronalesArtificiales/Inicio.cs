using RedesNeuronalesArtificiales.RNA;
using System;
using System.Collections.Generic;

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
            //Perceptron per = new Perceptron(7);
            //per.entrenamiento(EjemploEntrenamiento.LED7SEGMENTOS());
            //BackPropagation redmulticapa = new BackPropagation();
            //redmulticapa.entrenamiento(EjemploEntrenamiento.DESAYUNO());

			//double[] minimos = {0, 0.5, 0.3, -0.5, -0.5, -1, 1, 0, 0.5, 0};
			//double[] maximos = {1, 1, 0.3, 0.5, 0.5, 1, 2, 1, 0.7, 3 };

			Som redNeuronal = new Som (10,20);
			//redNeuronal.inicializarMatriz (minimos, maximos);
			redNeuronal.inicializarMatriz (-0.5, 0.5);
			Console.WriteLine (redNeuronal);


            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}
