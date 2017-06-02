using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesNeuronalesArtificiales
{
    class ejemplo
    {
        Func<Double, Double> mifuncion;
        public ejemplo(Func<Double, Double> transfirMetodo)
        {
            mifuncion = transfirMetodo;
        }




        void miMetodo()
        {
            mifuncion(2.8);
        }
    }

    class ejempl0
    {
    }
}
