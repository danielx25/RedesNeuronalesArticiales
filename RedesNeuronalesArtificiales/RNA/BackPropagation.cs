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
            //Double [] respuesta = redMulticapa.entrenandoLaRed(new Double[] { 0.0008928, 0.0004629, 0.00125});//4080
            //System.Console.WriteLine("respuesta: " + respuesta[0]);
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

            random = new NumeroRandom();
            foreach (List<Neurona> capaOculta in redMulticapa.CapasOcultas)
            {
                foreach(Neurona neurona in capaOculta)
                {
                    for (int i = 0; i < neurona.Pesos.Length; i++)
                    {
                        neurona.Pesos[i] =random.NextDouble(-1, 1);
                    }
                    neurona.setPolarizacion(random.NextDouble(-1, 1));
                }
            }

            foreach (Neurona neurona in redMulticapa.CapaSalida)
            {
                for (int i = 0; i < neurona.Pesos.Length; i++)
                {
                    neurona.Pesos[i] = random.NextDouble(-1, 1);
                }
                neurona.setPolarizacion(random.NextDouble(-1, 1));
            }
        }

        public void entrenamiento(Double[,] tabla)
        {

            int numeroFila = tabla.GetLength(0);
            int numeroColumna = tabla.GetLength(1);
            int numeroAciertos = 0;
            //int numeroRegistro = 0;
            Double[] fila = new Double[numeroColumna - 1];
            Double[] resultadoObtenido = new Double[redMulticapa.CapaSalida.Count];
            Double[] resultadoEsperado = new Double[redMulticapa.CapaSalida.Count];
            int contadorEpocas = 0;
            int numeroEpocas = 1;

            while (contadorEpocas < numeroEpocas)
            {
                for (int numeroRegistro = 0; numeroRegistro < numeroFila; numeroRegistro++)
                {
                    resultadoEsperado[0] = tabla[numeroRegistro, numeroColumna - 1];
                    for (int i = 0; i < numeroColumna - 1; i++)
                        fila[i] = tabla[numeroRegistro, i];
                    resultadoObtenido = redMulticapa.entrenandoLaRed(fila);
                    System.Console.WriteLine("salida: " + resultadoObtenido[0]);

                    if(resultadoObtenido[0] != resultadoEsperado[0])
                    {
                        propagacion_error(resultadoObtenido, resultadoEsperado);
                    }
                }
                System.Console.WriteLine();
                contadorEpocas += 1;
            }

        }

        public void propagacion_error(Double[] resultadoObtenido, Double[] resultadoEsperado)
        {

            Double[] gradienteSalida = new Double[resultadoEsperado.Length];
            //gradiente de salida de la red
            for (int i = 0; i < resultadoEsperado.Length; i++)
            {
                gradienteSalida[i] = (resultadoEsperado[i] - resultadoObtenido[i]) *
                    0.5 * (1+ resultadoObtenido[i])*(1- resultadoObtenido[i]);
            }

            List<Double>[] listaxCapadeGradientes = new List<Double>[redMulticapa.CapasOcultas.Count];
            //recorriendo las capas de atras para adelante
            for (int indice_capa=redMulticapa.CapasOcultas.Count-1; indice_capa >= 0; indice_capa--)
            {
                listaxCapadeGradientes[indice_capa] = new List<Double>();
                List<Neurona> capaOculta = redMulticapa.CapasOcultas[indice_capa];
                //el gradiente de la capa de salida
                if (indice_capa == redMulticapa.CapasOcultas.Count - 1)
                { 
                    //recorre las neurona de la capa oculta
                    for (int indice_NCaOcul = 0; indice_NCaOcul < capaOculta.Count; indice_NCaOcul++)
                    {
                        Double gradiente = 0;
                        //recorre cada neurona de la capa de la salida para buscar el peso que le corresponde
                        for (int indice_neurona = 0; indice_neurona < gradienteSalida.Length; indice_neurona++)
                        {
                            gradiente+= redMulticapa.CapaSalida[indice_neurona].Pesos[indice_NCaOcul] * gradienteSalida[indice_neurona];
                        }
                        Double y = capaOculta[indice_NCaOcul].getSalidaAxon();
                        gradiente = gradiente * 0.5 * (1 + y) * (1- y);
                        listaxCapadeGradientes[indice_capa].Add(gradiente);
                    }
                    
                }
                else
                {
                    //recorre las neurona de la capa oculta
                    for (int indice_NCaOcul = 0; indice_NCaOcul < capaOculta.Count; indice_NCaOcul++)
                    {
                        Double gradiente = 0;
                        //recorre la capa oculta que esta delan 
                        for (int indice_neurona = 0; indice_neurona<redMulticapa.CapasOcultas[indice_capa+1].Count;indice_neurona++)
                        {
                            gradiente += redMulticapa.CapaSalida[indice_neurona].Pesos[indice_NCaOcul] * listaxCapadeGradientes[indice_capa+1][indice_neurona];
                        }

                        Double y = capaOculta[indice_NCaOcul].getSalidaAxon();
                        gradiente = gradiente * 0.5 * (1 + y) * (1 - y);
                        listaxCapadeGradientes[indice_capa].Add(gradiente);
                    }
                }
            }
        }

    }
}
