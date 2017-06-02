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

        public Perceptron()
        {
            percepron = new RedNeuronas();
            percepron.agregarNeuronasEntrada(FuncionesActivacion.hardLim, 1, 2, RedNeuronas.CONEXION_TODOCONTRATODO);
            percepron.ejecutarUnaNeurona();
        }

    }
}
