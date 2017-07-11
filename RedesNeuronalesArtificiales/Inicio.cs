using RedesNeuronalesArtificiales.RNA;
using RedesNeuronalesArtificiales.BaseDeDatos;
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

			Som redNeuronal = new Som (datosMeteorologicos[0].Length,1600, 40);
			redNeuronal.inicializarMatriz (0, 1);
			redNeuronal.Datos = datosMeteorologicos;

			//Conexion.datosDetencionPalas(inicio, fin);

			Console.WriteLine (redNeuronal);
			redNeuronal.entrenar (1);
			Console.WriteLine (redNeuronal);

			//Guarda Archivo
			Guardar.Serializar (redNeuronal, "Red Som.mp10");

            //Lee Archivo
			/*
			Som redNeuronalLeida = Guardar.Deserializar("Red Som.mp10");
			Console.WriteLine (redNeuronalLeida);
			*/
        }
    }
}
