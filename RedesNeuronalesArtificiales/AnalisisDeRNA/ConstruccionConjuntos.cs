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
            gruposxclases = new Grupo[numeroClases, numeroGrupos];
            // matriz de grupos, cada fila representa la activacion de los n grupos respecto a la z clase

        }
        /***
         * ingresa los vectores de cada grupo y repite el mismo por todas las z clases que hayan
         */
        public void tablaVectoresGrupos(double[,] tabla)
        {
            for (int i = 0; i < tabla.GetLength(0); i++)
            {
                double[] vector = new double[tabla.GetLength(1)];
                for (int j = 0; j < tabla.GetLength(1); j++)
                {
                    vector[j] = tabla[i, j];
                    //System.Console.Write(tabla[i, j] + "-");
                }
                //System.Console.WriteLine();

                for (int indice_clase = 0; indice_clase < gruposxclases.GetLength(0); indice_clase++)
                {
                    gruposxclases[indice_clase, i] = new Grupo();
                    gruposxclases[indice_clase, i].vector = vector;
                }

            }
        }

        // calcula la distancia media que existe entre grupo i a toda los ejemplos de la clase
        private double MediaDistianciaGrupo(double[,] matrizDitancias, int indiceGrupo)
        {
            double suma = 0;
            for (int i = 0; i < matrizDitancias.GetLength(0); i++)
            {
                suma += matrizDitancias[i, indiceGrupo];
            }
            return suma / matrizDitancias.GetLength(0);
        }
        // calcula la desviacion estandar que existe entre grupo i a toda los ejemplos de la clase
        private double DesviacionDistianciaGrupo(double[,] matrizDitancias, double mediaDistancia, int indiceGrupo)
        {
            double suma = 0;
            int n = matrizDitancias.GetLength(0);
            for (int i = 0; i < matrizDitancias.GetLength(0); i++)
            {
                suma += Math.Pow((matrizDitancias[i, indiceGrupo] - mediaDistancia), 2);
            }

            suma = Math.Sqrt(suma / n);

            return suma;
        }


        //calcula los conjuntos de cada grupo segun la clase y se guarda en la matriz grupoxclases
        public void calcularConjuntoClase(double[,] conjuntoClase, int indice_clase)
        {
            double[] vectorDisMedia = new double[gruposxclases.GetLength(1)];
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


                double min = Double.PositiveInfinity;
                int indice_min = 0;
                for (int indice_grupo = 0; indice_grupo < gruposxclases.GetLength(1); indice_grupo++)
                {
                    matrizDitancias[i, indice_grupo] = calculoDistancia(vectorEntrada, gruposxclases[indice_clase, indice_grupo].vector);
                    if (matrizDitancias[i, indice_grupo] < min)
                    {
                        indice_min = indice_grupo;
                        min = matrizDitancias[i, indice_grupo];
                    }

                    //System.Console.Write(matrizDitancias[i, indice_grupo] + "-");
                }
                gruposxclases[indice_clase, indice_min].numeroActivacion += 1;

                //System.Console.WriteLine(indice_min);
            }

            System.Console.WriteLine("distancias medias: ");

            for (int d = 0; d < matrizDitancias.GetLength(1); d++)
            {
                vectorDisMedia[d] = MediaDistianciaGrupo(matrizDitancias, d);
                vectorDisDVS[d] = DesviacionDistianciaGrupo(matrizDitancias, vectorDisMedia[d], d);
                gruposxclases[indice_clase, d].media = vectorDisMedia[d];
                gruposxclases[indice_clase, d].desviacionEstandar = vectorDisDVS[d];
                gruposxclases[indice_clase, d].nivelPertenencia = gruposxclases[indice_clase, d].numeroActivacion / (double)conjuntoClase.GetLength(0);
                System.Console.WriteLine();
                System.Console.WriteLine("numero Activacion  : " + gruposxclases[indice_clase, d].numeroActivacion);
                System.Console.WriteLine("media              : " + gruposxclases[indice_clase, d].media);
                System.Console.WriteLine("desviacion estandar: " + gruposxclases[indice_clase, d].desviacionEstandar);
                System.Console.WriteLine("nivel pertencia    : " + gruposxclases[indice_clase, d].nivelPertenencia);


                //System.Console.Write("["+vectorDisMedia[d] + ";"+ vectorDisDVS[d]+"] - ");
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

        /*
         * def gaussian(x, mu, sig, a):
            return a*np.exp(-np.power(x - mu, 2.) / (2 * np.power(sig, 2.)))
         */


        private double funcion_gaussiana(double x, double media, double desviasion, double pertenencia)
        {
            return pertenencia * Math.Exp(-Math.Pow(x - media, 2) / (2 * Math.Pow(desviasion, 2)));
        }

        public double prediccionMP10(double[] entradaNormalizada)
        {
            double minMP10 = 0;
            double maxMP10 = 800;
            double[] vectorNivelPertenencia = new double[801];
            double maxPertenencia = Double.NegativeInfinity;
            double clasePredecir;
            double mp10Predecir = 0;

            List<double> l = entradaNormalizada.ToList();
            l.Add(0);

            double[] vectorEntrada = l.ToArray();

            double sinAlerta = 150 / maxMP10;
            double alerta1 = 250 / maxMP10;
            double alerta2 = 350 / maxMP10;
            double alerta3 = 500 / maxMP10;

            int indice_mp10 = vectorEntrada.Length - 1;
            double distanciaMin;
            for (int mp10 = 0; mp10 <= 800; mp10++)
            {
                vectorEntrada[indice_mp10] = mp10 / maxMP10;
                if (mp10 < sinAlerta)// sin alerta
                {
                    distanciaMin = Double.PositiveInfinity;
                    Grupo grupoMenorDistancia = null;
                    // calcula cual es el grupo mas cercano  a la clase = tipo de alerta
                    for (int indice_grupo = 0; indice_grupo < gruposxclases.GetLength(1); indice_grupo++)
                    {
                        if (distanciaMin > gruposxclases[0, indice_grupo].media)
                        {
                            distanciaMin = gruposxclases[0, indice_grupo].media;
                            grupoMenorDistancia = gruposxclases[0, indice_grupo];
                        }

                    }
                    double distancia = calculoDistancia(vectorEntrada, grupoMenorDistancia.vector);
                    vectorNivelPertenencia[mp10] = funcion_gaussiana(distancia, grupoMenorDistancia.media,
                        grupoMenorDistancia.desviacionEstandar, grupoMenorDistancia.nivelPertenencia);
                }

                if (sinAlerta <= mp10 && mp10 <= alerta1)//alerta 1
                {
                    distanciaMin = Double.PositiveInfinity;
                    Grupo grupoMenorDistancia = null;
                    // calcula cual es el grupo mas cercano  a la clase = tipo de alerta
                    for (int indice_grupo = 0; indice_grupo < gruposxclases.GetLength(1); indice_grupo++)
                    {
                        if (distanciaMin > gruposxclases[1, indice_grupo].media)
                        {
                            distanciaMin = gruposxclases[1, indice_grupo].media;
                            grupoMenorDistancia = gruposxclases[1, indice_grupo];
                        }

                    }
                    double distancia = calculoDistancia(vectorEntrada, grupoMenorDistancia.vector);
                    vectorNivelPertenencia[mp10] = funcion_gaussiana(distancia, grupoMenorDistancia.media,
                        grupoMenorDistancia.desviacionEstandar, grupoMenorDistancia.nivelPertenencia);
                }

                if (alerta1 < mp10 && mp10 <= alerta2)//alerta 2
                {
                    distanciaMin = Double.PositiveInfinity;
                    Grupo grupoMenorDistancia = null;
                    // calcula cual es el grupo mas cercano  a la clase = tipo de alerta
                    for (int indice_grupo = 0; indice_grupo < gruposxclases.GetLength(1); indice_grupo++)
                    {
                        if (distanciaMin > gruposxclases[2, indice_grupo].media)
                        {
                            distanciaMin = gruposxclases[2, indice_grupo].media;
                            grupoMenorDistancia = gruposxclases[2, indice_grupo];
                        }

                    }
                    double distancia = calculoDistancia(vectorEntrada, grupoMenorDistancia.vector);
                    vectorNivelPertenencia[mp10] = funcion_gaussiana(distancia, grupoMenorDistancia.media,
                        grupoMenorDistancia.desviacionEstandar, grupoMenorDistancia.nivelPertenencia);

                }

                if (alerta2 < mp10 && mp10 <= alerta3)//alerta 3
                {
                    distanciaMin = Double.PositiveInfinity;
                    Grupo grupoMenorDistancia = null;
                    // calcula cual es el grupo mas cercano  a la clase = tipo de alerta
                    for (int indice_grupo = 0; indice_grupo < gruposxclases.GetLength(1); indice_grupo++)
                    {
                        if (distanciaMin > gruposxclases[3, indice_grupo].media)
                        {
                            distanciaMin = gruposxclases[3, indice_grupo].media;
                            grupoMenorDistancia = gruposxclases[3, indice_grupo];
                        }

                    }
                    double distancia = calculoDistancia(vectorEntrada, grupoMenorDistancia.vector);
                    vectorNivelPertenencia[mp10] = funcion_gaussiana(distancia, grupoMenorDistancia.media,
                        grupoMenorDistancia.desviacionEstandar, grupoMenorDistancia.nivelPertenencia);
                }

                if (alerta3 < mp10)//alerta 4
                {
                    distanciaMin = Double.PositiveInfinity;
                    Grupo grupoMenorDistancia = null;
                    // calcula cual es el grupo mas cercano  a la clase = tipo de alerta
                    for (int indice_grupo = 0; indice_grupo < gruposxclases.GetLength(1); indice_grupo++)
                    {
                        if (distanciaMin > gruposxclases[4, indice_grupo].media)
                        {
                            distanciaMin = gruposxclases[4, indice_grupo].media;
                            grupoMenorDistancia = gruposxclases[4, indice_grupo];
                        }

                    }
                    double distancia = calculoDistancia(vectorEntrada, grupoMenorDistancia.vector);
                    vectorNivelPertenencia[mp10] = funcion_gaussiana(distancia, grupoMenorDistancia.media,
                        grupoMenorDistancia.desviacionEstandar, grupoMenorDistancia.nivelPertenencia);
                }

                if (maxPertenencia < vectorNivelPertenencia[mp10])
                {
                    maxPertenencia = vectorNivelPertenencia[mp10];
                    mp10Predecir = mp10;
                }
            }

            return mp10Predecir;
        }
    }
}
