using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesNeuronalesArtificiales.RNA
{
    class Neurona
    {
        // a :salida del axon
        private Double salida = 0;
        // intensidad de la relacion con la neurona W
        private Double[] pesos; 
        // valor de la salida de la neuna predesesora
        private Double[] entradas;
        // polarizacion o bias
        private Double b = 0;
        //entrada neta a la neurona
        private Double n = 0;
        List<Neurona> neuronasPredecesoras;

        private Func<Double, Double> funcionActivacion;

        public Neurona(Func<Double, Double> funcionAct)
        {
            funcionActivacion = funcionAct;
        }

        //esta funcion accede a los valores calculados por las neuronas predesesoras o la capa anterior a ella
        //se hizo de esta manera porque el metodo backpropagation desde la salida hacia atras
        protected void extrayendoMisValoresEntrada()
        {
            for(int i=0; i<neuronasPredecesoras.Count; i++)
            {

                entradas[i] = neuronasPredecesoras[i].getSalidaAxon(); 
            }
        }

        //calcula el valor nero del producto entre los pesos y la entrada y le suma el bias (polarizacion)
        private void entradaxPesos()
        {
            n = 0;
            for(int i=0; i<entradas.Length; i++)
            {
                n += entradas[i] * pesos[i]; 
            }
            n += b;//suma del vias
        }

        //calculo del valor que sale por el axon de la neurona
        private void humbralActivacion()
        {
            salida = funcionActivacion(n);
        }

        // salida neta de la neurona
        public Double getSalidaAxon()
        {
            return salida;
        }

    }
}
