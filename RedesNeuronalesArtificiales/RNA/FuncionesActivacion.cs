using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesNeuronalesArtificiales.RNA
{
    static class FuncionesActivacion
    {
        //limitador fuerte
        static public double hardLim(double n)
        {
            if (n >= 0)
                return 1;
            return 0;

        }
        //Limitador fuerte simetrico
        static public double escalon(double n)
        {
            if (n >= 0)
                return 1;
            return -1;
        }

        static public double identidad(double n)
        {
            return n;
        }
        //Rectifier
        static public double rectificador(double n)
        {
            if (n < 0)
            {
                return 0;
            }
            return n;
        }
        /*a pendiente
          x0 centro
        */
        public static double Sigmoide(double n)
        {
            return 1 / (1 + Math.Exp(-n));
        }

    }
}
