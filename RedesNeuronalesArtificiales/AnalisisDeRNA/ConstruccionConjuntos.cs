using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesNeuronalesArtificiales.AnalisisDeRNA
{
    class ConstruccionConjuntos
    {
        // numero de grupos formados por la red Som
        private int numeroGrupos;
        private int numeroClases;
        private double[,] tablaDistancias;

        private Grupo[,] gruposxclases;

        public ConstruccionConjuntos(int numeroGrupos, int numeroClases)
        {
            this.numeroGrupos = numeroGrupos;
            this.numeroClases = numeroClases;
                                        // fila           columna
            gruposxclases = new Grupo[numeroClases,  numeroGrupos];

        }

        public void tablaVectoresGrupos(double[,] tabla)
        {
            for (int i = 0; i < tabla.GetLength(0); i++)
            {
                double[] vector = new double[tabla.GetLength(1)];
                for (int j= 0; j < tabla.GetLength(1); j++)
                {
                    vector[j] = tabla[i, j];
                    System.Console.Write(tabla[i, j] + "-");
                }
                System.Console.WriteLine();
                
                for(int indice_clase = 0; indice_clase<gruposxclases.GetLength(0); indice_clase++)
                {
                    gruposxclases[indice_clase, i] = new Grupo();
                    gruposxclases[indice_clase, i].vector = vector;
                }

            }
        }

        public void calcularConjuntoClase(double [,] conjuntoClase, int indice_clase)
        {
            double []vectorDisMedia = new double[gruposxclases.GetLength(1)];
            double[] vectorDisDVS = new double[gruposxclases.GetLength(1)];
            double[,] matrizDitancias = new double[conjuntoClase.GetLength(0), gruposxclases.GetLength(1)];

            for (int i = 0; i < conjuntoClase.GetLength(0); i++)
            {
                double[] vectorEntrada = new double[conjuntoClase.GetLength(1)];
                for (int j = 0; j < conjuntoClase.GetLength(1); j++)
                {
                    vectorEntrada[j] = conjuntoClase[i, j];
                    //System.Console.Write(conjuntoClase[i, j]+ "-");
                }
                double min = 999999999;
                int indice = 0;
                for (int indice_grupo = 0; indice_grupo<gruposxclases.GetLength(1); indice_grupo++)
                {
                    matrizDitancias[i, indice_grupo] = calculoDistancia(vectorEntrada, gruposxclases[indice_clase, indice_grupo].vector);
                    if (matrizDitancias[i, indice_grupo] < min)
                    {
                        indice = indice_grupo;
                        min = matrizDitancias[i, indice_grupo];
                    }
                        
                    System.Console.Write(matrizDitancias[i, indice_grupo] + "-");
                }
                gruposxclases[indice_clase, indice].numeroActivacion += 1;

                System.Console.WriteLine(indice);
            }
        }

        public double calculoDistancia(double[] datos, double[] pesos)
        {
            double distanciaActual = 0;

            //Sumatoria de la distancia
            for (int x = 0; x < datos.Length; x++)
            {
                if (datos[x] >= 0 && datos[x] <= 1)//Solo datos dentro de rango
                    distanciaActual += Math.Pow(datos[x] - pesos[x], 2);
                //else
                //	Console.WriteLine("Alerta dato fuera de rango en: " + datos[0] + " con valor: " + datos[x] + " Columna de datos: " + (x+1));
            }
            //Calculo de la distancia
            return Math.Sqrt(distanciaActual);
        }


    }
}
