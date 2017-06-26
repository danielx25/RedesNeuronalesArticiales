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
        private Double alfa = 0.5; //taza de aprendizaje
        private int numeroEpocas = 10;//numero de veces que le presentas el conjunto a la red

        private bool debug = false;
         
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
            double[] ejemplo = new double[] { 0.5, 0.7, 0.2};
            int indice = 0;
            foreach (Neurona neurona in redMulticapa.CapaEntrada)
            {
                for (int i = 0; i < neurona.Pesos.Length; i++)
                {
                    neurona.Pesos[i] = ejemplo[indice];//random.NextDouble(-1, 1);
                }
                neurona.setPolarizacion(ejemplo[indice]);//(random.NextDouble(-1, 1));
                indice += 1;
            }
            double[] ejemplo1 = new double[] { 0.3, 0.8};
            indice = 0;
            random = new NumeroRandom();
            foreach (List<Neurona> capaOculta in redMulticapa.CapasOcultas)
            {
                foreach(Neurona neurona in capaOculta)
                {
                    for (int i = 0; i < neurona.Pesos.Length; i++)
                    {
                        neurona.Pesos[i] = ejemplo1[indice];//random.NextDouble(-1, 1);
                    }
                    neurona.setPolarizacion(ejemplo1[indice]);//(random.NextDouble(-1, 1));
                    indice += 1;
                }
            }

            foreach (Neurona neurona in redMulticapa.CapaSalida)
            {
                for (int i = 0; i < neurona.Pesos.Length; i++)
                {
                    neurona.Pesos[i] = 2.2;//random.NextDouble(-1, 1);
                }
                neurona.setPolarizacion(2.2);//random.NextDouble(-1, 1));
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
                    {
                        fila[i] = tabla[numeroRegistro, i];
                        /*
                        if (tabla[numeroRegistro, i] != 0)
                            fila[i] = 1 / tabla[numeroRegistro, i];
                        else
                            fila[i] = 0;

                        if(tabla[numeroRegistro, numeroColumna - 1] !=0)
                            resultadoEsperado[0] = 1/tabla[numeroRegistro, numeroColumna - 1];
                        */
                    }
                        
                    resultadoObtenido = redMulticapa.entrenandoLaRed(fila);
                    System.Console.WriteLine("re Obtenido: " + resultadoObtenido[0]+" re Esperado: "+resultadoEsperado[0]);

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
            System.Console.WriteLine("gradiente salida: " + gradienteSalida[0]);
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
                        gradiente = gradiente * 0.5 *  (1- y)*(1+y);
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
                        gradiente = gradiente * 0.5 *  (1 - y) * (1 + y);
                        listaxCapadeGradientes[indice_capa].Add(gradiente);
                    }
                }
            }

            for(int indiceCapa=0; indiceCapa<listaxCapadeGradientes.Length; indiceCapa++)
            {
                foreach(Double l in listaxCapadeGradientes[indiceCapa])
                {
                    System.Console.WriteLine("gradiente CapaOculta: " + l);
                }
            }

            Double[] gradienteEntrada = new Double[redMulticapa.CapaEntrada.Count];
            List<Neurona> capaOculta1 = redMulticapa.CapasOcultas[0];
            //gradiente de la capa de entrada
            for (int indice_NeuronaEntrada = 0; indice_NeuronaEntrada < gradienteEntrada.Length; indice_NeuronaEntrada++)
            {
                Double gradiente = 0;
                for(int indice_neurona=0; indice_neurona<capaOculta1.Count; indice_neurona++)
                {
                    gradiente += capaOculta1[indice_neurona].Pesos[indice_NeuronaEntrada]* listaxCapadeGradientes[0][indice_neurona];
                    System.Console.WriteLine("W: " + capaOculta1[indice_neurona].Pesos[indice_NeuronaEntrada] + " G: " + listaxCapadeGradientes[0][indice_neurona]);
                }
                Double y = redMulticapa.CapaEntrada[indice_NeuronaEntrada].getSalidaAxon();
                gradiente = gradiente * 0.5 *  (1 - y) * (1 + y);
                System.Console.WriteLine("y: " + y);
                gradienteEntrada[indice_NeuronaEntrada] = gradiente;
                System.Console.WriteLine("gradiente capa entrada: " + gradiente);
            }

            //actualizando los pesos de la capa de entrada
            for(int indice_neurona = 0; indice_neurona<redMulticapa.CapaEntrada.Count; indice_neurona++)
            {
                Double deltaW = 0;
                for(int indice_pesos = 0; indice_pesos< redMulticapa.CapaEntrada[indice_neurona].Pesos.Length; indice_pesos++)
                {
                    
                    deltaW = alfa * gradienteEntrada[indice_neurona] *
                        redMulticapa.CapaEntrada[indice_neurona].Entradas[indice_pesos];
                    redMulticapa.CapaEntrada[indice_neurona].Pesos[indice_pesos] += deltaW;
                    System.Console.WriteLine("alfa: " + alfa + " G: " + gradienteEntrada[indice_neurona] +" En: "+
                        redMulticapa.CapaEntrada[indice_neurona].Entradas[indice_pesos]+" = "+deltaW);
                }
                redMulticapa.CapaEntrada[indice_neurona].Polarizacion+= alfa * gradienteEntrada[indice_neurona]*-1;
            }
            
            //actualizando los pesos de las capas ocultas
            for(int indice_capa =0; indice_capa<redMulticapa.CapasOcultas.Count; indice_capa++)
            {
                List<Neurona> capa = redMulticapa.CapasOcultas[indice_capa];
                Double gradiente = 0;
                for(int indice_neurona=0; indice_neurona < capa.Count; indice_neurona++)
                {
                    gradiente = listaxCapadeGradientes[indice_capa][indice_neurona];
                    for (int indice_pesos = 0; indice_pesos < capa[indice_neurona].Pesos.Length; indice_pesos++)
                    {
                        capa[indice_neurona].Pesos[indice_pesos]+= alfa * gradiente * capa[indice_neurona].Entradas[indice_pesos];
                    }
                    capa[indice_neurona].Polarizacion+= alfa * gradiente * -1;
                }
            }
            //acualizando los pesos de la capa de salida

            for (int indice_neurona = 0; indice_neurona < redMulticapa.CapaSalida.Count; indice_neurona++)
            {
                for (int indice_pesos = 0; indice_pesos < redMulticapa.CapaSalida[indice_neurona].Pesos.Length; indice_pesos++)
                {
                    redMulticapa.CapaSalida[indice_neurona].Pesos[indice_pesos]+= alfa * gradienteSalida[indice_neurona] *
                        redMulticapa.CapaSalida[indice_neurona].Entradas[indice_pesos];
                }
                redMulticapa.CapaSalida[indice_neurona].Polarizacion+= alfa * gradienteSalida[indice_neurona] * -1;
            }


        }

    }
}
