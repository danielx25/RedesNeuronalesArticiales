﻿using RedesNeuronalesArtificiales.RNA;
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
            //Perceptron per = new Perceptron(7);
            //per.entrenamiento(EjemploEntrenamiento.LED7SEGMENTOS());
            //BackPropagation redmulticapa = new BackPropagation();
            //redmulticapa.entrenamiento(EjemploEntrenamiento.DESAYUNO_NORMALIZADO());

			//double[] minimos = {0, 1, 3, 5, 6, 7, 1, 0, 0.5, 0, 1, 1, 1, 1, 1, 1, 1, 1};
			//double[] maximos = {1, 2, 4, 6, 7, 8, 2, 1, 0.7, 3, 1, 1, 1, 1, 1, 1, 1, 1};
			DateTime inicio = new DateTime(2010,01,01,00,00,00);
			DateTime fin = new DateTime(2010,02,01,00,00,00);

			List<Fila> datosMeteorologicos = Conexion.datosMeteorologicos (inicio, fin);

			Som redNeuronal = new Som (18,40, 8);
			redNeuronal.inicializarMatriz (-0.5, 0.5);
			//redNeuronal.inicializarMatriz (minimos, maximos);
			redNeuronal.Datos = datosMeteorologicos;

			Console.WriteLine (redNeuronal);
			redNeuronal.entrenar (1);//Aun no tiene sentido dar mas ciclos
			Console.WriteLine (redNeuronal);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}
