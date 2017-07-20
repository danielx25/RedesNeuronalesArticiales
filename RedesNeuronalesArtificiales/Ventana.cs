using RedesNeuronalesArtificiales.AnalisisDeRNA;
using RedesNeuronalesArtificiales.Archivo;
using RedesNeuronalesArtificiales.BaseDeDatos;
using RedesNeuronalesArtificiales.RNA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RedesNeuronalesArtificiales
{
    public partial class Ventana : Form
    {
        ConstruccionConjuntos resultado;//numero de grupos, numero de alertas
        public Ventana()
        {
            InitializeComponent();
            resultado = new ConstruccionConjuntos(7, 5);//numero de grupos, numero de alertas
        }

        private void IngresarResultados()
        {
            Som redNeuronal = Guardar.Deserializar("Red Som Final.mp10");
            EscribirArchivo archivo = new EscribirArchivo("Pesos aleatorios.html", true);
            archivo.imprimir(Mp10.obtenerMP10HTML(redNeuronal.MatrizPesos, redNeuronal.NumeroFilas, redNeuronal.NumeroColumnas));
            archivo.cerrar();


            HashSet<int> hashtable = new HashSet<int>();
            hashtable.Add(8706);
            hashtable.Add(7200);
            hashtable.Add(9510);
            hashtable.Add(7197);
            hashtable.Add(7926);
            hashtable.Add(8918);
            hashtable.Add(6899);

            double[,] pesosxgrupo = redNeuronal.obtenerPesosNeuronas(hashtable);

            DateTime inicio = new DateTime(2010, 01, 01, 00, 00, 00);
            DateTime fin = new DateTime(2017, 03, 01, 00, 00, 00);

            List<double[]> datosMeteorologicos = Conexion.datosMeteorologicos(inicio, fin, 10);
            Console.WriteLine("Total: " + datosMeteorologicos.Count);
            List<double[,]> dr = Conexion.datosPorRangoMp10(inicio, fin, 10);
            double[,] sa = dr[0];//Sin Alerta
            double[,] a1 = dr[1];//Alerta 1
            double[,] a2 = dr[2];//Alerta 2
            double[,] a3 = dr[3];//Alerta 3
            double[,] a4 = dr[4];//Alerta 4

            Console.WriteLine("Sin Alerta: " + sa.GetLength(0) + " " + sa.GetLength(1));
            Console.WriteLine("Alerta 1: " + a1.GetLength(0) + " " + a1.GetLength(1));
            Console.WriteLine("Alerta 2: " + a2.GetLength(0) + " " + a2.GetLength(1));
            Console.WriteLine("Alerta 3: " + a3.GetLength(0) + " " + a3.GetLength(1));
            Console.WriteLine("Alerta 4: " + a4.GetLength(0) + " " + a4.GetLength(1));

            System.Console.WriteLine("termino de cargar los grupos y los ejemplos");

            Double[,] vectoresG = { { .99, .2, .1},
                                  { 0, 0, 0},
                                  { .5, .5, .5},
                                  { .2, .1, 0} };

            Double[,] ejemplo = { { .1, .5, .6},
                                  { .3, .5, .1},
                                  { .1, .8, .7},
                                  { .9, .9, .5},
                                  { .6, .12, .3} };

            
            resultado.tablaVectoresGrupos(pesosxgrupo);
            resultado.calcularConjuntoClase(sa, 0);
            System.Console.WriteLine("---------------------->");
            resultado.calcularConjuntoClase(a1, 1);
            System.Console.WriteLine("---------------------->");
            resultado.calcularConjuntoClase(a2, 2);
            System.Console.WriteLine("---------------------->");
            resultado.calcularConjuntoClase(a3, 3);
            System.Console.WriteLine("---------------------->");
            resultado.calcularConjuntoClase(a4, 4);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void crearGrafico_Click(object sender, EventArgs e)
        {
            IngresarResultados();
            Series grupo1 = new Series("grupo1");
            System.Console.WriteLine("creando grafico");
            graficoSinAlerta.Titles.Add("Sin alerta");
            
            Grupo grupo6 = resultado.gruposxclases[0, 6];
            double [] ejex = new double[20];
            double [] ejey = new double[20];
            double x = 0;
            int densidad = 20;
            for (int i=0; i< densidad; i++ )
            {
                ejex[i] = x;
                ejey[i] = resultado.funcion_gaussiana(x, grupo6.media, grupo6.desviacionEstandar, grupo6.nivelPertenencia);
                x += 1 / (double)densidad;
            }

            var series = new Series("grupo 6");
            series.ChartType = SeriesChartType.Line;
            series.Points.DataBindXY(ejex, ejey);
            graficoSinAlerta.Series.Add(series);


        }
    }
}
