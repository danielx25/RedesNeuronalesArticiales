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

        private bool tipoConexion = false;
        private bool debug = true;
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
            this.tipoConexion = tipoConexion;
            for(int i=0; i<numeroNeuronas; i++)
            {
                if(tipoConexion == CONEXION_LINEAL)
                    capaEntrada.Add(new Neurona(funcionActivacion));
                if (tipoConexion == CONEXION_TODOCONTRATODO)
                    capaEntrada.Add(new Neurona(funcionActivacion, numeroEntrada));
            }
            ejemploEntrenamiento = new Double[numeroEntrada];
        }

        public void agregarCapaOculta(Func<Double, Double> funcionActivacion, int numeroNeuronas)
        {
            List<Neurona> capaOculta = new List<Neurona>();
            if (capasOcultas.Count == 0)
            {
                for (int indice = 0; indice < numeroNeuronas; indice++)//agregando las neurona a la lista
                {
                    Neurona neuronaCO = new Neurona(funcionActivacion, capaEntrada.Count);
                    foreach (Neurona neuronaCE in capaEntrada)//enlazando la nueva neurona 
                    {
                        neuronaCO.agregarNeuronaEnlace(neuronaCE);
                    }
                    capaOculta.Add(neuronaCO);

                }
            }
            else
            {
                for (int indice = 0; indice < numeroNeuronas; indice++)//agregando las neurona a la lista
                {
                    Neurona neuronaCO = new Neurona(funcionActivacion, capasOcultas[capasOcultas.Count - 1].Count);
                    foreach (Neurona neuronaC01 in capasOcultas[capasOcultas.Count-1])//enlazando la nueva neurona 
                    {
                        neuronaCO.agregarNeuronaEnlace(neuronaC01);
                    }
                    capaOculta.Add(neuronaCO);
                }
            }
            
            capasOcultas.Add(capaOculta);
        }

        public void agregarCapaSalida(Func<Double, Double> funcionActivacion, int numeroNeuronas)
        {
            if(capasOcultas.Count > 0)
            {
                for (int indice = 0; indice < numeroNeuronas; indice++)//agregando las neurona a la lista
                {
                    Neurona neuronaCS = new Neurona(funcionActivacion, capasOcultas[capasOcultas.Count-1].Count);
                    foreach (Neurona neuronaCE in capasOcultas[capasOcultas.Count - 1])//enlazando la nueva neurona 
                    {
                        neuronaCS.agregarNeuronaEnlace(neuronaCE);
                    }
                    capaSalida.Add(neuronaCS);

                }
            }
            else
            {
                if(capaEntrada.Count > 0)
                {
                    for (int indice = 0; indice < numeroNeuronas; indice++)//agregando las neurona a la lista
                    {
                        Neurona neuronaCS = new Neurona(funcionActivacion, capaEntrada.Count);
                        foreach (Neurona neuronaCE in capaEntrada)//enlazando la nueva neurona 
                        {
                            neuronaCS.agregarNeuronaEnlace(neuronaCE);
                        }
                        capaSalida.Add(neuronaCS);

                    }
                }
            }
        }

        public void enlazandoCapas(List<Neurona> capa0, List<Neurona> capa1)
        {
            foreach(Neurona neurona_1 in capa1)
            {
                foreach(Neurona neurona_0 in capa0)
                {
                    neurona_1.agregarNeuronaEnlace(neurona_0);
                }
            }
        }


        public void iniciandoLaRNA()
        {
            //cargando la cola con todas las neuronas
            //iniciandoCola();
            //uniniendoCapaEntradaCapaOculta();
            //uniendoLasCapasOcultas();
            //uniendoLaCapaSalidaCapaOculta();
        }

        public Double[] entrenandoLaRed(Double [] ejemploEntrenamiento)
        {
            this.ejemploEntrenamiento = ejemploEntrenamiento;
            List<Double> resultado = new List<double>();
            List<Double> resultadoxCapas = null;
            if (capaEntrada.Count>0)
            {
                Neurona neurona = null;
                resultadoxCapas = new List<Double>();
                for(int indiceCapa=0; indiceCapa < capaEntrada.Count; indiceCapa++)
                {
                    neurona = capaEntrada[indiceCapa];
                    if (tipoConexion == CONEXION_TODOCONTRATODO)
                        neurona.setEntrada(ejemploEntrenamiento);
                    if (tipoConexion == CONEXION_LINEAL)
                        neurona.setEntrada(new Double[] { ejemploEntrenamiento[indiceCapa] });

                    neurona.entradaxPesos();
                    neurona.humbralActivacion();
                    resultadoxCapas.Add(neurona.getSalidaAxon());
                    if(debug)
                    {
                        for (int i = 0; i < neurona.Pesos.Length; i++)
                        {
                            System.Console.WriteLine("W: " + neurona.Pesos[i] + " E: " + neurona.Entradas[i]);
                        }
                        System.Console.WriteLine("Neto (n): " + neurona.Neto);
                        System.Console.WriteLine("salida Capa Entrada: " + neurona.getSalidaAxon());
                    }
                    
                    
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
            System.Console.WriteLine();
            if(capasOcultas.Count > 0)
            {

                for(int indice_capa=0; indice_capa < capasOcultas.Count; indice_capa++) //(List < Neurona > capaOculta in capasOcultas)
                {
                    List<Double> resultado_aux = new List<Double>();
                    foreach(Neurona neuronaCO in capasOcultas[indice_capa])
                    {
                        neuronaCO.setEntrada(resultadoxCapas.ToArray());
                        neuronaCO.entradaxPesos();
                        neuronaCO.humbralActivacion();
                        resultado_aux.Add(neuronaCO.getSalidaAxon());
                        if (debug)
                        {
                            for (int i = 0; i < neuronaCO.Pesos.Length; i++)
                            {
                                System.Console.WriteLine("W: " + neuronaCO.Pesos[i] + " E: " + neuronaCO.Entradas[i]);
                            }
                            System.Console.WriteLine("Neto (n): " + neuronaCO.Neto);
                            System.Console.WriteLine("salida Capa Oculta: " + neuronaCO.getSalidaAxon());
                        }
                        
                        
                    }
                    resultadoxCapas = resultado_aux;
                }
            }

            if(capaSalida.Count > 0)
            {
                Neurona neurona = null;
                System.Console.WriteLine();
                for (int indiceCapa = 0; indiceCapa < capaSalida.Count; indiceCapa++)
                {
                    neurona = capaSalida[indiceCapa];
                    neurona.setEntrada(resultadoxCapas.ToArray());
                    neurona.entradaxPesos();
                    neurona.humbralActivacion();
                    resultado.Add(neurona.getSalidaAxon());
                    if (debug)
                    {
                        for (int i = 0; i < neurona.Pesos.Length; i++)
                        {
                            System.Console.WriteLine("W: " + neurona.Pesos[i] + " E: " + neurona.Entradas[i]);
                        }
                        System.Console.WriteLine("Neto (n): " + neurona.Neto);
                        System.Console.WriteLine("salida Capa salida: " + neurona.getSalidaAxon());
                    }
                    
                }


            }

            return resultado.ToArray();
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

        public Neurona getNeuronaCapaEntrada(int indice)
        {
            return capaEntrada[indice];
        }


        public List<Neurona> CapaEntrada
        {
            get
            {
                return capaEntrada;
            }
        }

        public List<List<Neurona>> CapasOcultas
        {
            get
            {
                return capasOcultas;
            }
        }

        public List<Neurona> CapaSalida
        {
            get
            {
                return capaSalida;
            }
        }
    }

    
}
