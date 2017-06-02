using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesNeuronalesArtificiales.RNA
{
    class RedNeuronas
    {
        private List<Neurona> capaEntrada = new List<Neurona>();
        private List<List<Neurona>> capaOculta = new List<List<Neurona>>();
        private List<Neurona> capaSalida = new List<Neurona>();

        public void agregarNeuronaEntrada(Func<Double, Double> funcionActivacion)
        {
            capaEntrada.Add(new Neurona(funcionActivacion));
        }

        public void agregarNeuronasEntrada(Func<Double, Double> funcionActivacion, int numero)
        {
            for(int i=0; i<numero; i++)
                capaEntrada.Add(new Neurona(funcionActivacion));
        }
    }

    
}
