using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesNeuronalesArtificiales.RNA
{
    class BackPropagation
    {
        private RedNeuronas redMulticapa = new RedNeuronas();
        private Double alfa = 0.1; //taza de aprendizaje
        private int numeroEpocas = 10;//numero de veces que le presentas el conjunto a la red
         
        public BackPropagation()
        {
            int numeroNeuronas = 3;
            int numeroEntradas = 3;
            //(Func<Double, Double> funcionActivacion, int numeroNeuronas, int numeroEntrada, bool tipoConexion =true)
            redMulticapa.agregarNeuronasEntrada(FuncionesActivacion.Sigmoide, 
                numeroNeuronas, numeroEntradas, RedNeuronas.CONEXION_TODOCONTRATODO);
            redMulticapa.agregarCapaOculta(FuncionesActivacion.Sigmoide, 2);
            //redMulticapa.agregarCapaOculta(FuncionesActivacion.Sigmoide, 4);
            redMulticapa.agregarCapaSalida(FuncionesActivacion.Sigmoide, 1);
            inicializarPesosCapaEntrada();
            Double [] respuesta = redMulticapa.entrenandoLaRed(new Double[] { 760, 555, 800 });
            System.Console.WriteLine("respuesta: " + respuesta[0]);
        }

        public void inicializarPesosCapaEntrada()
        {
            NumeroRandom random = new NumeroRandom();
            foreach (Neurona neurona in redMulticapa.CapaEntrada)
            {
                for (int i = 0; i < neurona.Pesos.Length; i++)
                {
                    neurona.Pesos[i] = random.NextDouble(-1, 1);
                }
                neurona.setPolarizacion(random.NextDouble(-1, 1));
            }
        }

    }
}
