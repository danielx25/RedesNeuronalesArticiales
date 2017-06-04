using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesNeuronalesArtificiales
{
    static class EjemploEntrenamiento
    {
        static public Double[,] AND()
        {
            Double[,] ejemplo = { { 0, 0, 0},
                                  { 0, 1, 0},
                                  { 1, 0, 0},
                                  { 1, 1, 1} };
            return ejemplo;
        }

        static public Double[,] OR()
        {
            Double[,] ejemplo = { { 0, 0, 0},
                                  { 0, 1, 1},
                                  { 1, 0, 1},
                                  { 1, 1, 1} };
            return ejemplo;
        }

        static public Double[,] ejemplo1()
        {
            Double[,] ejemplo = { { 2, 1, 1},
                                  { 0, -1, 1},
                                  { -2, 1, -1},
                                  { 0, 2, -1} };
            return ejemplo;
        }

        static public Double[,] XOR()
        {
            Double[,] ejemplo = { { 0, 0, 1},
                                  { 0, 1, 0},
                                  { 1, 0, 0},
                                  { 1, 1, 1} };
            return ejemplo;
        }

        static public Double[,] XORDIMENSION()
        {
            Double[,] ejemplo = { {1, 0, 0, 1},
                                  {0, 0, 1, 0},
                                  {0, 1, 0, 0},
                                  {1, 1, 1, 1} };
            return ejemplo;
        }

    }
}
