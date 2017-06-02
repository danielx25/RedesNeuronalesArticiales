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
        private List<List<Neurona>> capasOcultas = new List<List<Neurona>>();
        private List<Neurona> capaSalida = new List<Neurona>();

        //en la entrada a la capa cada neurona solo tiene derecho a un dato
        public const bool CONEXION_LINEAL = true;
        //esto el contrario a la conexion lineal todas las entradas van a todas las neuronas de la capa de entrada
        public const bool CONEXION_TODOCONTRATODO = false;

        //La ejecucion de cada neurona depende de esta lista
        //primeron van las neuronas de la capa de entrada despues capa oculta y despues la de salida
        List<Neurona> colaDeNeuronas;
        
        // el tipo de funcion que tendra las neuronas en la capa de entrada
        // numero de neuronas
        // numero de entradas
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

        public void iniciandoLaRNA()
        {
            //cargando la cola con todas las neuronas
            iniciandoCola();
            //uniniendoCapaEntradaCapaOculta();
            //uniendoLasCapasOcultas();
            //uniendoLaCapaSalidaCapaOculta();
        }

        public Double[] entrenandoLaRed(Double [] ejemploEntrenamiento)
        {
            this.ejemploEntrenamiento = ejemploEntrenamiento;
            List<Double> resultado = new List<double>();
            if (capaEntrada.Count>0)
            {
                int tamañoCola = capaEntrada.Count;
                Neurona neurona = null;
                for(int indiceCapa=0; indiceCapa < tamañoCola; indiceCapa++)
                {
                    neurona = capaEntrada[indiceCapa];
                    neurona.entradaxPesos();
                    neurona.humbralActivacion();
                }
                if(capasOcultas.Count ==0)
                {
                    foreach(Neurona neu in capaEntrada)
                    {
                        resultado.Add(neu.getSalidaAxon());
                    }
                    return resultado.ToArray();
                }
            }
            return new Double[0];
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

        protected void iniciandoCola()
        {
            colaDeNeuronas = new List<Neurona>();
            colaDeNeuronas.AddRange(capaEntrada);
            foreach (List<Neurona> capa in capasOcultas)
            {
                colaDeNeuronas.AddRange(capa);
            }
            colaDeNeuronas.AddRange(capaSalida);
        }

        protected void uniniendoCapaEntradaCapaOculta()
        {
            if(capasOcultas.Count > 0)
            {
                List<Neurona> capaOculta = capasOcultas[0];
                foreach (Neurona neuronaCapaOculta in capaOculta)
                {
                    foreach (Neurona neuronaCapaEntrada in capaEntrada)
                    {
                        neuronaCapaOculta.agregarNeuronaEnlace(neuronaCapaEntrada);
                    }
                }
            }
        }

        protected void uniendoLasCapasOcultas()
        {
            if (capasOcultas.Count > 2)
            {
                foreach (List<Neurona> capaOculta in capasOcultas)//accediendo a cada capa oculta
                {
                    foreach (Neurona neuronaCapaOculta in capaOculta)
                    {
                        foreach (Neurona neuronaCapaEntrada in capaSalida)
                        {

                        }
                    }
                }
            }
        }

        protected void uniendoLaCapaSalidaCapaOculta()
        {
            if (capaSalida.Count > 0 && capasOcultas.Count > 0)
            {
                List<Neurona> capaOculta = capasOcultas[capasOcultas.Count-1];
                if (capaOculta.Count > 0)
                    foreach (Neurona neuronaCapaOculta in capaOculta)
                    {
                        foreach (Neurona neuronaCapaSalida in capaSalida)
                        {
                            neuronaCapaSalida.agregarNeuronaEnlace(neuronaCapaOculta);
                        }
                    }
            }
        }
    }

    
}
