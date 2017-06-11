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

        static public Double[,] LED7SEGMENTOS()
        {                        //a  b  c  d  e  f  g   Object 
            Double[,] ejemplo = { {1, 1, 1, 1, 1, 1, 0, 1},//0
                                  {0, 1, 1, 0, 0, 0, 0, 0},//1
                                  {1, 1, 0, 1, 1, 0, 1, 1},//2
                                  {1, 1, 1, 1, 0, 0, 1, 0},//3
                                  {0, 1, 1, 0, 0, 1, 1, 1},//4
                                  {1, 0, 1, 1, 0, 1, 1, 0},//5
                                  {1, 0, 1, 1, 1, 1, 1, 1},//6
                                  {1, 1, 1, 0, 0, 0, 0, 0},//7
                                  {1, 1, 1, 1, 1, 1, 1, 1},//8
                                  {1, 1, 1, 1, 1, 0, 1, 0},//9
                                   };
            return ejemplo;
        }

        static public Double[,] DESAYUNO()
        {
            Double[,] ejemplo = {
                                  { 2, 3, 0, 3280 },
                                     { 2, 1, 0, 1840 },
                                     /*{ 0, 0, 1, 800 },
                                     { 1, 1, 0, 1280 },
                                     { 0, 3, 0, 2160 },
                                     { 2, 2, 1, 3360 },
                                     { 2, 0, 0, 1120 },
                                     { 0, 0, 0, 0 },
                                     { 0, 0, 1, 800 },
                                     { 0, 2, 1, 2240 },
                                     { 1, 2, 1, 2800 },
                                     { 2, 1, 0, 1840 },
                                     { 2, 2, 0, 2560 },
                                     { 1, 3, 0, 2720 },
                                     { 1, 3, 1, 3520 },
                                     { 0, 3, 1, 2960 },
                                     { 1, 0, 1, 1360 },
                                     { 2, 3, 0, 3280 },
                                     { 0, 1, 0, 720 },
                                     { 0, 0, 0, 0 },
                                     { 0, 0, 1, 800 },
                                     { 1, 2, 1, 2800 },
                                     { 0, 3, 1, 2960 },
                                     { 0, 3, 1, 2960 },
                                     { 2, 0, 0, 1120 },
                                     { 2, 0, 0, 1120 },
                                     { 0, 2, 1, 2240 },
                                     { 1, 1, 0, 1280 },
                                     { 0, 0, 0, 0 },
                                     { 0, 2, 0, 1440 }*/
                                   };
            return ejemplo;
        }



    }
}
