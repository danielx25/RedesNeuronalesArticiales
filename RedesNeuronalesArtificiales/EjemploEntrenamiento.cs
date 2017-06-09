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
                                  { 1120, 2160, 800, 4080 },
                                    { 560, 720, 800, 2080 },
                                    { 560, 2160, 800, 3520 },
                                    { 560, 720, 0, 1280 },
                                    { 1120, 0, 800, 1920 },
                                    { 560, 2160, 800, 3520 },
                                    { 560, 0, 800, 1360 },
                                    { 1120, 0, 0, 1120 },
                                    { 1120, 720, 0, 1840 },
                                    { 0, 1440, 0, 1440 },
                                    { 560, 0, 800, 1360 },
                                    { 0, 720, 800, 1520 },
                                    { 1120, 0, 0, 1120 },
                                    { 0, 720, 800, 1520 },
                                    { 560, 1440, 800, 2800 },
                                    { 0, 2160, 800, 2960 },
                                    { 0, 2160, 800, 2960 },
                                    { 1120, 0, 0, 1120 },
                                    { 0, 2160, 800, 2960 },
                                    { 560, 1440, 800, 2800 },
                                    { 0, 1440, 0, 1440 },
                                    { 0, 0, 0, 0 },
                                    { 0, 720, 800, 1520 },
                                    { 1120, 1440, 800, 3360 },
                                    { 560, 720, 0, 1280 },
                                    { 0, 1440, 0, 1440 },
                                    { 560, 1440, 800, 2800 },
                                    { 560, 0, 0, 560 },
                                    { 560, 2160, 800, 3520 },
                                    { 0, 720, 800, 1520 }
                                   };
            return ejemplo;
        }



    }
}
