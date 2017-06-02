using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesNeuronalesArtificiales.RNA
{
    class RedNeuronas
    {
        //datos de entrenamiento
        private Double [] ejemploEntrenamiento;
        //salida de la red
        private Double[] salidaRed;

        private List<Neurona> capaEntrada = new List<Neurona>();
        private List<List<Neurona>> capaOculta = new List<List<Neurona>>();
        private List<Neurona> capaSalida = new List<Neurona>();

        //en la entrada a la capa cada neurona solo tiene derecho a un dato
        public const bool CONEXION_LINEAL = true;
        //esto el contrario a la conexion lineal todas las entradas van a todas las neuronas de la capa de entrada
        public const bool CONEXION_TODOCONTRATODO = false;
        // el tipo de funcion que tendra las neuronas en la capa de entrada
        // numero de neuronas
        // numero de entrado
        public void agregarNeuronasEntrada(Func<Double, Double> funcionActivacion, int numeroNeuronas, int numeroEntrada, bool tipoConexion =true)
        {
            for(int i=0; i<numeroNeuronas; i++)
            {
                if(tipoConexion == CONEXION_LINEAL)
                    capaEntrada.Add(new Neurona(funcionActivacion));
                if (tipoConexion == CONEXION_TODOCONTRATODO)
                    capaEntrada.Add(new Neurona(funcionActivacion, numeroEntrada));
            }
            ejemploEntrenamiento = new Double[numeroEntrada];
        }

        public void ejecutarUnaNeurona()
        {
            Neurona neurona = capaEntrada[0];
            neurona.setPesos(new Double[] { 1, 1 });
            neurona.setPolarizacion(-1.5);

            neurona.setEntrada(new Double[] {0, 0});
            neurona.entradaxPesos();
            neurona.humbralActivacion();
            System.Console.WriteLine("0, 0 = 0: "+neurona.getSalidaAxon());

            neurona.setEntrada(new Double[] { 0, 1 });
            neurona.entradaxPesos();
            neurona.humbralActivacion();
            System.Console.WriteLine("0, 1 = 0: " + neurona.getSalidaAxon());

            neurona.setEntrada(new Double[] { 1, 0 });
            neurona.entradaxPesos();
            neurona.humbralActivacion();
            System.Console.WriteLine("1, 0 = 0: " + neurona.getSalidaAxon());

            neurona.setEntrada(new Double[] { 1, 1 });
            neurona.entradaxPesos();
            neurona.humbralActivacion();
            System.Console.WriteLine("1, 1 = 1: " + neurona.getSalidaAxon());

        }



    }

    
}
