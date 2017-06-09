using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesNeuronalesArtificiales.RNA
{
    class Neurona
    {
        int numeroCapa = 0;
        int numeroNeurona = 0;

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
        List<Neurona> neuronasPredecesoras = new List<Neurona>();

        private Func<Double, Double> funcionActivacion;

        public Neurona(Func<Double, Double> funcionAct)
        {
            funcionActivacion = funcionAct;
            pesos = new Double[1];
            entradas = new Double[1];
        }

        public Neurona(Func<Double, Double> funcionAct, int numeroEntrada)
        {
            funcionActivacion = funcionAct;
            pesos = new Double[numeroEntrada];
            entradas = new Double[numeroEntrada];
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
        public void entradaxPesos()
        {
            n = 0;
            for(int i=0; i<entradas.Length; i++)
            {
                //System.Console.Write(" w_: " + pesos[i] + " p_: " + entradas[i]+" ");
                n += entradas[i] * pesos[i];
                
            }
            n += b;//suma del vias
            //System.Console.Write(" W*N: " + n + " ");
        }

        //calculo del valor que sale por el axon de la neurona
        public void humbralActivacion()
        {
            salida = funcionActivacion(n);
        }

        // salida neta de la neurona
        public Double getSalidaAxon()
        {
            return salida;
        }

        public void setEntrada(Double[] entradas)
        {
            this.entradas = entradas;
        }

        public Double[] Pesos
        {
            get
            {
                return pesos;
            }
            set
            {
                pesos = value;
            }


        }

        public Double Polarizacion
        {
            get
            {
                return b;
            }
            set
            {
                b = value;
            }
        }

        public void setPolarizacion(Double p)
        {
            b = p;
        }

        public void agregarNeuronaEnlace(Neurona neurona)
        {
            neuronasPredecesoras.Add(neurona);
        }
    }
}
