﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesNeuronalesArtificiales.RNA
{
    class Perceptron
    {
        RedNeuronas percepron;
        NumeroRandom random = new NumeroRandom();
        Double alfa = 0.3;
        Double polarizacion = 0;
        Double[] salida;
        public Perceptron()
        {
            percepron = new RedNeuronas();
            percepron.agregarNeuronasEntrada(FuncionesActivacion.hardLim, 1, 2, RedNeuronas.CONEXION_TODOCONTRATODO);
            percepron.iniciandoLaRNA();
            Neurona neurona = percepron.getNeuronaCapaEntrada(0);
            neurona.Pesos = new Double[] {random.NextDouble(-1 ,1), random.NextDouble(-1, 1)};
        }

        public void entrenamiento(Double[,] tabla)
        {
            Neurona neurona = percepron.getNeuronaCapaEntrada(0);

            int numeroFila = tabla.GetLength(0);
            int numeroColumna = tabla.GetLength(1);
            int numeroAciertos = 0;
            //int numeroRegistro = 0;
            Double[] fila = new Double[numeroColumna];
            Double esperado = 0;
            while(numeroAciertos < numeroFila)
            {
                numeroAciertos = 0;
                for(int numeroRegistro=0; numeroRegistro<numeroFila; numeroRegistro++)
                {
                    for (int i = 0; i < numeroColumna - 1; i++)
                        fila[i] = tabla[numeroRegistro, i];

                    
                    salida = percepron.entrenandoLaRed(fila);
                    esperado = tabla[numeroRegistro, numeroColumna - 1];
                    System.Console.Write("salida: " + salida + "esperado: " + esperado);
                    if (esperado != salida[0])
                    {
                        Double[] pesos = neurona.Pesos;
                        Double nuevoPeso = 0;
                        for(int indice=0; indice<pesos.Length; indice++)
                        {
                            nuevoPeso = ajustePeso(pesos[indice], alfa, salida[0], esperado, tabla[numeroRegistro, indice]);
                            neurona.Pesos[indice] = nuevoPeso;
                            System.Console.Write("w" + indice + ": " + nuevoPeso);
                        }
                        System.Console.WriteLine();
                    }
                    else
                        numeroAciertos += 1;
                }
            }
    }

        private Double ajustePeso(Double peso_, Double alfa_, Double obtenido_, Double esperado_, Double entrada_)
        {
            return peso_ + alfa_ * (esperado_ - obtenido_) * entrada_;
        }

    }
}
