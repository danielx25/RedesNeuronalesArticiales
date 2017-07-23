using RedesNeuronalesArtificiales.AnalisisDeRNA;
using RedesNeuronalesArtificiales.Archivo;
using RedesNeuronalesArtificiales.BaseDeDatos;
using RedesNeuronalesArtificiales.RNA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesNeuronalesArtificiales.ValidacionRNA
{
    class Validacion
    {

        public void validando()
        {
            Som redNeuronal = Guardar.Deserializar("Red Som Final.mp10");
            EscribirArchivo archivo = new EscribirArchivo("Pesos aleatorios.html", true);
            archivo.imprimir(Mp10.obtenerMP10HTML(redNeuronal.MatrizPesos, redNeuronal.NumeroFilas, redNeuronal.NumeroColumnas));
            archivo.cerrar();


            /*eligiendo las neuronas ganadoras de cada grupo formado  por la red SOM*/
            HashSet<int> hashtable = new HashSet<int>();
            hashtable.Add(555);
            hashtable.Add(290);
            hashtable.Add(1373);
            hashtable.Add(2471);
            hashtable.Add(2352);
            hashtable.Add(868);
            hashtable.Add(1664);
            hashtable.Add(1300);
            hashtable.Add(1650);
            hashtable.Add(943);

            double[,] pesosxgrupo = redNeuronal.obtenerPesosNeuronas(hashtable);

            DateTime inicio = new DateTime(2010, 01, 01, 00, 00, 00);
            DateTime fin = new DateTime(2017, 03, 01, 00, 00, 00);

            List<double[]> datosMeteorologicos = Conexion.datosMeteorologicos(inicio, fin, 10);
            List<double[,]> dr = Conexion.datosPorRangoMp10(inicio, fin, 10);

            double[,] sa = dr[0];//Sin Alerta
            double[,] a1 = dr[1];//Alerta 1
            double[,] a2 = dr[2];//Alerta 2
            double[,] a3 = dr[3];//Alerta 3
            double[,] a4 = dr[4];//Alerta 4

            int numeroGrupos = hashtable.Count;
            ConstruccionConjuntos resultado = new ConstruccionConjuntos(numeroGrupos, 5);//numero de grupos y los tipos de alerta

            resultado.tablaVectoresGrupos(pesosxgrupo);
            resultado.calcularConjuntoClase(sa, 0);
            resultado.calcularConjuntoClase(a1, 1);
            resultado.calcularConjuntoClase(a2, 2);
            resultado.calcularConjuntoClase(a3, 3);
            resultado.calcularConjuntoClase(a4, 4);
            resultado.etiquetadoDelosGrupos();

            /* esta parte se itera con todos lo casos con los que no se ha entrenado la red*/
            DateTime inicio1 = new DateTime(2017, 01, 01, 00, 00, 00);
            DateTime fin1 = new DateTime(2017, 02, 01, 23, 00, 00);

            List<double[]> datosMeteorologicos1 = Conexion.datosMeteorologicos(inicio1, fin1, 10);
            double[] fila = datosMeteorologicos1[0];
            System.Console.WriteLine("---------------------->tipo alerta: " + fila[7] * 800);
            double mp10 = resultado.prediccionMP10(fila);
        }
    }
}
