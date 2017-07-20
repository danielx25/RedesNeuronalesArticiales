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
        double[] vectorEntrada = new double[38];
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
            System.Console.WriteLine("creando grafico");
            graficoSinAlerta.Titles.Add("Sin alerta");

            

            Grupo grupon = resultado.gruposxclases[0, 6];
            double rangox0 = 0.5;
            double rangox1 = 2.3;
            double x = 0;
            int densidad = 40;
            double tasa = (rangox1 - rangox0) / (double)densidad;
            for (int indice_grupo = 0;  indice_grupo < resultado.gruposxclases.GetLength(1); indice_grupo++)
            {
                var series = new Series("grupo"+indice_grupo);
                series.ChartType = SeriesChartType.Line;

                grupon = resultado.gruposxclases[0, indice_grupo];
                double[] ejex = new double[densidad];
                double[] ejey = new double[densidad];
                x = rangox0;
                for (int i = 0; i < densidad; i++)
                {
                    ejex[i] = x;
                    ejey[i] = resultado.funcion_gaussiana(x, grupon.media, grupon.desviacionEstandar, grupon.nivelPertenencia);
                    x += tasa;
                }
                series.Points.DataBindXY(ejex, ejey);
                graficoSinAlerta.Series.Add(series);

            }

            graficoAlerta1.Titles.Add("Alerta 1");

            int indiceClase = 1;
            grupon = resultado.gruposxclases[indiceClase, 6];
            //rangox0 = 0.5;
            //rangox1 = 2.3;
            x = 0;
            //densidad = 40;
            //double tasa = (rangox1 - rangox0) / (double)densidad;
            for (int indice_grupo = 0; indice_grupo < resultado.gruposxclases.GetLength(1); indice_grupo++)
            {
                var series = new Series("grupo" + indice_grupo);
                series.ChartType = SeriesChartType.Line;

                grupon = resultado.gruposxclases[indiceClase, indice_grupo];
                double[] ejex = new double[densidad];
                double[] ejey = new double[densidad];
                x = rangox0;
                for (int i = 0; i < densidad; i++)
                {
                    ejex[i] = x;
                    ejey[i] = resultado.funcion_gaussiana(x, grupon.media, grupon.desviacionEstandar, grupon.nivelPertenencia);
                    x += tasa;
                }
                series.Points.DataBindXY(ejex, ejey);
                graficoAlerta1.Series.Add(series);

            }


            graficoAlerta2.Titles.Add("Alerta 2");

            indiceClase = 2;
            grupon = resultado.gruposxclases[indiceClase, 6];
            //rangox0 = 0.5;
            //rangox1 = 2.3;
            x = 0;
            //densidad = 40;
            //double tasa = (rangox1 - rangox0) / (double)densidad;
            for (int indice_grupo = 0; indice_grupo < resultado.gruposxclases.GetLength(1); indice_grupo++)
            {
                var series = new Series("grupo" + indice_grupo);
                series.ChartType = SeriesChartType.Line;

                grupon = resultado.gruposxclases[indiceClase, indice_grupo];
                double[] ejex = new double[densidad];
                double[] ejey = new double[densidad];
                x = rangox0;
                for (int i = 0; i < densidad; i++)
                {
                    ejex[i] = x;
                    ejey[i] = resultado.funcion_gaussiana(x, grupon.media, grupon.desviacionEstandar, grupon.nivelPertenencia);
                    x += tasa;
                }
                series.Points.DataBindXY(ejex, ejey);
                graficoAlerta2.Series.Add(series);

            }


            graficoAlerta3.Titles.Add("Alerta 3");

            indiceClase = 3;
            grupon = resultado.gruposxclases[indiceClase, 6];
            //rangox0 = 0.5;
            //rangox1 = 2.3;
            x = 0;
            //densidad = 40;
            //double tasa = (rangox1 - rangox0) / (double)densidad;
            for (int indice_grupo = 0; indice_grupo < resultado.gruposxclases.GetLength(1); indice_grupo++)
            {
                var series = new Series("grupo" + indice_grupo);
                series.ChartType = SeriesChartType.Line;

                grupon = resultado.gruposxclases[indiceClase, indice_grupo];
                double[] ejex = new double[densidad];
                double[] ejey = new double[densidad];
                x = rangox0;
                for (int i = 0; i < densidad; i++)
                {
                    ejex[i] = x;
                    ejey[i] = resultado.funcion_gaussiana(x, grupon.media, grupon.desviacionEstandar, grupon.nivelPertenencia);
                    x += tasa;
                }
                series.Points.DataBindXY(ejex, ejey);
                graficoAlerta3.Series.Add(series);

            }


            graficoAlerta4.Titles.Add("Alerta 4");

            indiceClase = 4;
            grupon = resultado.gruposxclases[indiceClase, 6];
            //rangox0 = 0.5;
            //rangox1 = 2.3;
            x = 0;
            //densidad = 40;
            //double tasa = (rangox1 - rangox0) / (double)densidad;
            for (int indice_grupo = 0; indice_grupo < resultado.gruposxclases.GetLength(1); indice_grupo++)
            {
                var series = new Series("grupo" + indice_grupo);
                series.ChartType = SeriesChartType.Line;

                grupon = resultado.gruposxclases[indiceClase, indice_grupo];
                double[] ejex = new double[densidad];
                double[] ejey = new double[densidad];
                x = rangox0;
                for (int i = 0; i < densidad; i++)
                {
                    ejex[i] = x;
                    ejey[i] = resultado.funcion_gaussiana(x, grupon.media, grupon.desviacionEstandar, grupon.nivelPertenencia);
                    x += tasa;
                }
                series.Points.DataBindXY(ejex, ejey);
                graficoAlerta4.Series.Add(series);

            }


            double mp10 = resultado.prediccionMP10(new double[]{ });


            graficoMP10.Titles.Add("nivel de mp10");
            var serieMp10 = new Series("mp10");
            serieMp10.ChartType = SeriesChartType.Line;
            serieMp10.Points.DataBindXY(resultado.rangoMP10, resultado.vectorNivelPertenencia);
            textoNivelMp10.Text = mp10.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ((Control)prediccionMP10).Enabled = false;
        }
    }
}
