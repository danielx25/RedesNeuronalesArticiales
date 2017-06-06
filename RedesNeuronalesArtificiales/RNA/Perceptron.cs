using System;
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
        Double alfa = 0.1;
        Double polarizacion = 0;
        Double[] salida;
        public Perceptron(int numeroEntradas)
        {
            percepron = new RedNeuronas();
            //1  una neurona, 3 entradas
            percepron.agregarNeuronasEntrada(FuncionesActivacion.hardLim, 1, numeroEntradas, RedNeuronas.CONEXION_TODOCONTRATODO);
            percepron.iniciandoLaRNA();
            Neurona neurona = percepron.getNeuronaCapaEntrada(0);
            for(int i=0; i<neurona.Pesos.Length; i++)
            {
                neurona.Pesos[i] = random.NextDouble(-1, 1);
            }
            
            //neurona.Pesos = new Double[] { -0.7, 0.2 };
            neurona.setPolarizacion(random.NextDouble(-1, 1));
        }

        public void entrenamiento(Double[,] tabla)
        {
            Neurona neurona = percepron.getNeuronaCapaEntrada(0);

            int numeroFila = tabla.GetLength(0);
            int numeroColumna = tabla.GetLength(1);
            int numeroAciertos = 0;
            //int numeroRegistro = 0;
            Double[] fila = new Double[numeroColumna-1];
            Double esperado = 0;

            int contador = 0;
            while(numeroAciertos < numeroFila)
            {
                numeroAciertos = 0;
                if (contador > 100000)
                    break;
                contador += 1;
                
                for(int numeroRegistro=0; numeroRegistro<numeroFila; numeroRegistro++)
                {
                    for (int i = 0; i < numeroColumna - 1; i++)
                        fila[i] = tabla[numeroRegistro, i];

                    
                    salida = percepron.entrenandoLaRed(fila);
                    esperado = tabla[numeroRegistro, numeroColumna - 1];
                    System.Console.WriteLine(" salida: " + salida[0] + " esperado: " + esperado);
                    if (esperado != salida[0])
                    {
                        Double[] pesos = neurona.Pesos;
                        Double nuevoPeso = 0;
                        Double error = 0;
                        for(int indice=0; indice<pesos.Length; indice++)
                        {
                            //System.Console.Write(" antesW" + indice + ": " + pesos[indice]);
                            //System.Console.Write(" p: " + tabla[numeroRegistro, indice] + " ");
                            nuevoPeso = ajustePeso(pesos[indice], 1, salida[0], esperado, tabla[numeroRegistro, indice]);
                            neurona.Pesos[indice] = nuevoPeso;
                            //System.Console.Write(" w" + indice + ": " + nuevoPeso);
                        }
                        //System.Console.Write(" b: " + neurona.Polarizacion);
                        //System.Console.WriteLine();
                        error = (esperado - salida[0]);
                        neurona.Polarizacion += error;
                    }
                    else
                    {
                        //System.Console.WriteLine();
                        numeroAciertos += 1;
                    }

                    //System.Console.Write(" Aciertos: " + numeroAciertos);
                        
                }
                System.Console.WriteLine("numero Aciertos: " + numeroAciertos);
                System.Console.WriteLine();
            }
            System.Console.WriteLine("contador: " + contador);
    }

        private Double ajustePeso(Double peso_, Double alfa_, Double obtenido_, Double esperado_, Double entrada_)
        {
            return peso_ + alfa_ * (esperado_ - obtenido_) * entrada_;
        }

    }
}
