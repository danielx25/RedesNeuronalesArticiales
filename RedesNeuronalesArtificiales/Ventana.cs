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
            resultado = new ConstruccionConjuntos(10, 5);//numero de grupos, numero de alertas
            
        }

        private void IngresarResultados()
        {
            Som redNeuronal = Guardar.Deserializar("Red Som Final.mp10");
            EscribirArchivo archivo = new EscribirArchivo("Pesos aleatorios.html", true);
            archivo.imprimir(Mp10.obtenerMP10HTML(redNeuronal.MatrizPesos, redNeuronal.NumeroFilas, redNeuronal.NumeroColumnas));
            archivo.cerrar();


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
            /*
            hashtable.Add(8706);
            hashtable.Add(7200);
            hashtable.Add(9510);
            hashtable.Add(7197);
            hashtable.Add(7926);
            hashtable.Add(8918);
            hashtable.Add(6899);
            */

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
            
            DateTime inicio = new DateTime(2017, 01, 01, 00, 00, 00);
            DateTime fin = new DateTime(2017, 02, 01, 23, 00, 00);

            List<double[]> datosMeteorologicos = Conexion.datosMeteorologicos(inicio, fin, 10);
            double[] fila = datosMeteorologicos[0];
            System.Console.WriteLine("---------------------->tipo alerta: " + fila[7] * 800);

            double mp10 = resultado.prediccionMP10(fila);
                /*(new double[]{
                0.165867009736995, 0.519046224692597, 0.273331067214742, 0.217533453585218, 0.208563547201633,
                0.330751708283526, 0.186680540697893, 0.216829888220607, 0.0447644907431001, 0.86081049966678,
                0.0625177019563619, 0.0158015103411543, 0.0398040585775627, 0.0153669505490552, 0.044929986919441,
                0.482442377359812, 0.592646121789071, 0.819033585870188, 0.0642792210282195, 0.718872113488089,
                0.0818010608358383, 0.23486132095433, 0.746469104721333, 0.212675689461812, 0.438268004260671,
                0.0154758472246046, 0.0432606489934153, 0.075923386555986, 0.359977093864478, 0.66537945890997,
                0.612634541406878, 0.354041483578386, 0.278723555756083, 0.100249147477755804, 0.00645568482134704,
                0.0153524804817408, 0.0138216209766464, 0.000301244981904876
            });*/


            graficoMP10.Titles.Add("nivel de mp10");
            var serieMp10 = new Series("mp10");
            serieMp10.ChartType = SeriesChartType.Line;
            serieMp10.Points.DataBindXY(resultado.rangoMP10, resultado.vectorNivelPertenencia);
            graficoMP10.Series.Add(serieMp10);
            textoNivelMp10.Text = mp10.ToString();

            double sinAlerta = 150 ;
            double alerta1 = 250 ;
            double alerta2 = 350 ;
            double alerta3 = 500 ;

            if (mp10 < sinAlerta)// sin alerta
                textoTipoAlerta.Text = "Sin alerta";
            if (sinAlerta <= mp10 && mp10 <= alerta1)
                textoTipoAlerta.Text = "Alerta 1";
            if (alerta1 < mp10 && mp10 <= alerta2)//alerta 2
                textoTipoAlerta.Text = "Alerta 2";
            if (alerta2 < mp10 && mp10 <= alerta3)//alerta 3
                textoTipoAlerta.Text = "Alerta 3";
            if (alerta3 < mp10)//alerta 4
                textoTipoAlerta.Text = "Alerta 4";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ((Control)prediccionMP10).Enabled = false;
        }
    }
}
