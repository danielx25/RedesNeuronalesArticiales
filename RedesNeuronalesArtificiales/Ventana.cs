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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RedesNeuronalesArtificiales
{
    public partial class Ventana : Form
    {
        //private void graficosPertenencia()
        //{}
        private delegate void delegadoCambioProgreso(int value);
        private delegate void graficosPertenencia1();
        graficosPertenencia1 delagadoGraficos;
        ConstruccionConjuntos resultado;//numero de grupos, numero de alertas
        double[] vectorEntrada = new double[38];
        public int numeroGrupos = 10;

        ConstruccionConjuntos[] resultados;
        int indiceActualResultado = 0;

        //ThreadStart delegado = new ThreadStart(analisisResultadosEntregados);

        public Ventana()
        {
            InitializeComponent();
            diasApredecir.SelectedIndex = 0;
            resultado = new ConstruccionConjuntos(numeroGrupos, 5);//numero de grupos, numero de alertas
            resultados = new ConstruccionConjuntos[1];
            resultados[0] = resultado;
        }

        private void IngresarResultados()
        {
            
            Som redNeuronal = Guardar.Deserializar("Red Som Final.mp10");
            EscribirArchivo archivo = new EscribirArchivo("Pesos aleatorios.html", true);
            archivo.imprimir(Mp10.obtenerMP10HTML(redNeuronal.MatrizPesos, redNeuronal.NumeroFilas, redNeuronal.NumeroColumnas));
            archivo.cerrar();
            cambiarBarraProgreso(22);

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
            Console.WriteLine("Total: " + datosMeteorologicos.Count);
            List<double[,]> dr = Conexion.datosPorRangoMp10(inicio, fin, 10);
            cambiarBarraProgreso(55);
            double[,] sa = dr[0];//Sin Alerta
            double[,] a1 = dr[1];//Alerta 1
            double[,] a2 = dr[2];//Alerta 2
            double[,] a3 = dr[3];//Alerta 3
            double[,] a4 = dr[4];//Alerta 4

            resultado.tablaVectoresGrupos(pesosxgrupo);
            resultado.calcularConjuntoClase(sa, 0);
            cambiarBarraProgreso(66);
            resultado.calcularConjuntoClase(a1, 1);
            resultado.calcularConjuntoClase(a2, 2);
            cambiarBarraProgreso(71);
            resultado.calcularConjuntoClase(a3, 3);
            resultado.calcularConjuntoClase(a4, 4);
            resultado.etiquetadoDelosGrupos();
            cambiarBarraProgreso(80);

            DateTime inicio1 = new DateTime(2017, 01, 01, 00, 00, 00);
            DateTime fin1 = new DateTime(2017, 02, 01, 23, 00, 00);

            List<double[]> datosMeteorologicos1 = Conexion.datosMeteorologicos(inicio1, fin1, 10);
            double[] fila = datosMeteorologicos1[0];
            System.Console.WriteLine("---------------------->tipo alerta: " + fila[7] * 800);
            resultado.prediccionMP10(fila);
            cambiarBarraProgreso(100);
        }

        private void contruirGraficoMp10()
        {
            

            double mp10 = resultado.Mp10predecido;

            for (int indice_alerta = 0; indice_alerta < resultado.gruposGanadores.GetLength(0); indice_alerta++)
            {
                double[] vectorDistancia = new double[resultado.vectorDistanciaMP10.GetLength(1)];
                for (int indice_distancia = 0; indice_distancia < vectorDistancia.Length; indice_distancia++)
                {
                    vectorDistancia[indice_distancia] = resultado.vectorDistanciaMP10[indice_alerta, indice_distancia];
                }

                var serieMp10 = new Series(resultado.gruposGanadores[indice_alerta].clase);
                serieMp10.ChartType = SeriesChartType.Line;
                serieMp10.Points.DataBindXY(resultado.rangoMP10, vectorDistancia);

                if (String.Compare(resultado.gruposGanadores[indice_alerta].clase, "sin alerta") == 0)
                    serieMp10.Color = Color.Green;

                if (String.Compare(resultado.gruposGanadores[indice_alerta].clase, "alerta 1") == 0)
                    serieMp10.Color = Color.Yellow;

                if (String.Compare(resultado.gruposGanadores[indice_alerta].clase, "alerta 2") == 0)
                    serieMp10.Color = Color.Orange;

                if (String.Compare(resultado.gruposGanadores[indice_alerta].clase, "alerta 3") == 0)
                    serieMp10.Color = Color.Purple;

                if (String.Compare(resultado.gruposGanadores[indice_alerta].clase, "alerta 4") == 0)
                    serieMp10.Color = Color.Red;

                graficoMP10.Series.Add(serieMp10);
            }
            var seriePuntoMinimo = new Series("punto minimo");
            seriePuntoMinimo.ChartType = SeriesChartType.Line;
            seriePuntoMinimo.Points.DataBindXY(new double[]{mp10, mp10}, new double[]{0, resultado.distanciaMinima});
            graficoMP10.Series.Add(seriePuntoMinimo);

            textoNivelMp10.Text = mp10.ToString();


            double sinAlerta = 150;
            double alerta1 = 250;
            double alerta2 = 350;
            double alerta3 = 500;

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

        private void analisisResultadosEntregados()
        { 
            IngresarResultados();
            contruirGraficoMp10();
            graficosPertenencia();
        }

        private void cambiarBarraProgreso(int valor)
        {
            if (this.InvokeRequired)
            {
                delegadoCambioProgreso delegado = new delegadoCambioProgreso(cambiarBarraProgreso);
                object[] parametros = new object[] { valor };
                this.Invoke(delegado, parametros);
            }
            else
            {
                barraProgreso.Value = valor;
            }
        }

        private void graficosPertenencia()
        {
            if(this.InvokeRequired)
            {
                delagadoGraficos = new graficosPertenencia1(graficosPertenencia);
                this.Invoke(delagadoGraficos);
            }
            else
            {
                Console.WriteLine("Bueno no entro aca cierto");
                graficoSinAlerta.Titles.Add("Sin alerta");
                Grupo grupon = resultado.gruposxclases[0, 6];
                double rangox0 = 0.0;
                double rangox1 = 2.3;
                double x = 0;
                int densidad = 40;
                double tasa = (rangox1 - rangox0) / (double)densidad;

                string cadenaLeyenda;

                for (int indice_grupo = 0; indice_grupo < resultado.gruposxclases.GetLength(1); indice_grupo++)
                {
                    grupon = resultado.gruposxclases[0, indice_grupo];
                    double[] ejex = new double[densidad];
                    double[] ejey = new double[densidad];

                    cadenaLeyenda = "grupo" + indice_grupo;
                    if (grupon.etiquetada)
                    {
                        cadenaLeyenda = grupon.clase;
                    }
                    else
                        continue;

                    var series = new Series(cadenaLeyenda);
                    series.ChartType = SeriesChartType.Line;
                    if (String.Compare(cadenaLeyenda, "sin alerta") == 0)
                        series.Color = Color.Green;

                    if (String.Compare(cadenaLeyenda, "alerta 1") == 0)
                        series.Color = Color.Yellow;

                    if (String.Compare(cadenaLeyenda, "alerta 2") == 0)
                        series.Color = Color.Orange;

                    if (String.Compare(cadenaLeyenda, "alerta 3") == 0)
                        series.Color = Color.Purple;

                    if (String.Compare(cadenaLeyenda, "alerta 4") == 0)
                        series.Color = Color.Red;

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

                    grupon = resultado.gruposxclases[indiceClase, indice_grupo];
                    double[] ejex = new double[densidad];
                    double[] ejey = new double[densidad];

                    cadenaLeyenda = "grupo" + indice_grupo;
                    if (grupon.etiquetada)
                    {
                        cadenaLeyenda = grupon.clase;
                    }
                    else
                        continue;

                    var series = new Series(cadenaLeyenda);
                    series.ChartType = SeriesChartType.Line;
                    if (String.Compare(cadenaLeyenda, "sin alerta") == 0)
                        series.Color = Color.Green;

                    if (String.Compare(cadenaLeyenda, "alerta 1") == 0)
                        series.Color = Color.Yellow;

                    if (String.Compare(cadenaLeyenda, "alerta 2") == 0)
                        series.Color = Color.Orange;

                    if (String.Compare(cadenaLeyenda, "alerta 3") == 0)
                        series.Color = Color.Purple;

                    if (String.Compare(cadenaLeyenda, "alerta 4") == 0)
                        series.Color = Color.Red;


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
                    grupon = resultado.gruposxclases[indiceClase, indice_grupo];
                    double[] ejex = new double[densidad];
                    double[] ejey = new double[densidad];

                    cadenaLeyenda = "grupo" + indice_grupo;
                    if (grupon.etiquetada)
                    {
                        cadenaLeyenda = grupon.clase;
                    }
                    else
                        continue;

                    var series = new Series(cadenaLeyenda);
                    series.ChartType = SeriesChartType.Line;
                    if (String.Compare(cadenaLeyenda, "sin alerta") == 0)
                        series.Color = Color.Green;

                    if (String.Compare(cadenaLeyenda, "alerta 1") == 0)
                        series.Color = Color.Yellow;

                    if (String.Compare(cadenaLeyenda, "alerta 2") == 0)
                        series.Color = Color.Orange;

                    if (String.Compare(cadenaLeyenda, "alerta 3") == 0)
                        series.Color = Color.Purple;

                    if (String.Compare(cadenaLeyenda, "alerta 4") == 0)
                        series.Color = Color.Red;

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
                //rangox1 = 2.3
                x = 0;
                //densidad = 40;
                //double tasa = (rangox1 - rangox0) / (double)densidad;
                for (int indice_grupo = 0; indice_grupo < resultado.gruposxclases.GetLength(1); indice_grupo++)
                {

                    grupon = resultado.gruposxclases[indiceClase, indice_grupo];
                    double[] ejex = new double[densidad];
                    double[] ejey = new double[densidad];

                    cadenaLeyenda = "grupo" + indice_grupo;
                    if (grupon.etiquetada)
                    {
                        cadenaLeyenda = grupon.clase;
                    }
                    else
                        continue;

                    var series = new Series(cadenaLeyenda);
                    series.ChartType = SeriesChartType.Line;
                    if (String.Compare(cadenaLeyenda, "sin alerta") == 0)
                        series.Color = Color.Green;

                    if (String.Compare(cadenaLeyenda, "alerta 1") == 0)
                        series.Color = Color.Yellow;

                    if (String.Compare(cadenaLeyenda, "alerta 2") == 0)
                        series.Color = Color.Orange;

                    if (String.Compare(cadenaLeyenda, "alerta 3") == 0)
                        series.Color = Color.Purple;

                    if (String.Compare(cadenaLeyenda, "alerta 4") == 0)
                        series.Color = Color.Red;

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

                    grupon = resultado.gruposxclases[indiceClase, indice_grupo];
                    double[] ejex = new double[densidad];
                    double[] ejey = new double[densidad];

                    cadenaLeyenda = "grupo" + indice_grupo;
                    if (grupon.etiquetada)
                    {
                        cadenaLeyenda = grupon.clase;
                    }
                    else
                        continue;

                    var series = new Series(cadenaLeyenda);
                    series.ChartType = SeriesChartType.Line;
                    if (String.Compare(cadenaLeyenda, "sin alerta") == 0)
                        series.Color = Color.Green;

                    if (String.Compare(cadenaLeyenda, "alerta 1") == 0)
                        series.Color = Color.Yellow;

                    if (String.Compare(cadenaLeyenda, "alerta 2") == 0)
                        series.Color = Color.Orange;

                    if (String.Compare(cadenaLeyenda, "alerta 3") == 0)
                        series.Color = Color.Purple;

                    if (String.Compare(cadenaLeyenda, "alerta 4") == 0)
                        series.Color = Color.Red;


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
            }
            
        }

        private void crearGrafico_Click(object sender, EventArgs e)
        {
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            rellenoFormulario();
            /*
            //Creamos el delegado 
            ThreadStart proceso = new ThreadStart(analisisResultadosEntregados);
            //Creamos la instancia del hilo 
            Thread hilo = new Thread(proceso);
            //Iniciamos el hilo 
            hilo.Start();
            */
        }

        double [] rellenoFormulario()
        {
            double []vectorEntrada = new double[38];

            int mes = datoMes1.SelectedIndex+1;
            double velocidadViento = Convert.ToDouble( datoVeloViento1.Text);
            double direccionViento = Convert.ToDouble(datoDirecViento1.Text);
            double temperatura = Convert.ToDouble(datoTemp1.Text);
            double humedad = Convert.ToDouble(datoHumedad1.Text);
            double radiacion = Convert.ToDouble(datoRadiaSolar1.Text);
            double presion = Convert.ToDouble(datoPresion1.Text);

            double preci1 = Convert.ToDouble(datoPreciMañana1.Text);
            double preci2 = Convert.ToDouble(datoPrecihoy1.Text);
            double preci3 = Convert.ToDouble(datoPreci1_1.Text);
            double preci4 = Convert.ToDouble(datoPreci2_1.Text);
            double preci5 = Convert.ToDouble(datoPreci3_1.Text);

            double evapo1 = Convert.ToDouble(datoEvapomañana1.Text);
            double evapo2 = Convert.ToDouble(datoEvapohoy1.Text);
            double evapo3 = Convert.ToDouble(datoEvapo1_1.Text);
            double evapo4 = Convert.ToDouble(datoEvapo2_1.Text);
            double evapo5 = Convert.ToDouble(datoEvapo3_1.Text);

            Console.WriteLine("mes: " + mes);

            return vectorEntrada;
        }

        private void diasApredecir_SelectedIndexChanged(object sender, EventArgs e)
        {
            prediccionDiaComboBox.Items.Clear();
            String opcion = (string)diasApredecir.SelectedItem;

            this.diasPrediccion.Controls.Clear();

            if (String.Compare(opcion, "1 Dia") == 0)
            {
                prediccionDiaComboBox.Items.Add("dia 1");
                resultados = new ConstruccionConjuntos[1];
                resultados[0] = resultado;
                this.diasPrediccion.Controls.Add(tabDia1);
            }

            if (String.Compare(opcion, "2 Dias") == 0)
            {
                prediccionDiaComboBox.Items.Add("dia 1");
                prediccionDiaComboBox.Items.Add("dia 2");
                resultados = new ConstruccionConjuntos[2];
                resultados[0] = resultado;
                this.diasPrediccion.Controls.Add(tabDia1);
                this.diasPrediccion.Controls.Add(tabDia2);
            }

            if (String.Compare(opcion, "3 Dias") == 0)
            {
                prediccionDiaComboBox.Items.Add("dia 1");
                prediccionDiaComboBox.Items.Add("dia 2");
                prediccionDiaComboBox.Items.Add("dia 3");
                resultados = new ConstruccionConjuntos[3];
                resultados[0] = resultado;
                this.diasPrediccion.Controls.Add(tabDia1);
                this.diasPrediccion.Controls.Add(tabDia2);
                this.diasPrediccion.Controls.Add(tabDia3);

            }

            if (String.Compare(opcion, "4 Dias") == 0)
            {
                prediccionDiaComboBox.Items.Add("dia 1");
                prediccionDiaComboBox.Items.Add("dia 2");
                prediccionDiaComboBox.Items.Add("dia 3");
                prediccionDiaComboBox.Items.Add("dia 4");
                resultados = new ConstruccionConjuntos[4];
                resultados[0] = resultado;
                this.diasPrediccion.Controls.Add(tabDia1);
                this.diasPrediccion.Controls.Add(tabDia2);
                this.diasPrediccion.Controls.Add(tabDia3);
                this.diasPrediccion.Controls.Add(tabDia4);

            }

            if (String.Compare(opcion, "5 Dias") == 0)
            {
                prediccionDiaComboBox.Items.Add("dia 1");
                prediccionDiaComboBox.Items.Add("dia 2");
                prediccionDiaComboBox.Items.Add("dia 3");
                prediccionDiaComboBox.Items.Add("dia 4");
                prediccionDiaComboBox.Items.Add("dia 5");
                resultados = new ConstruccionConjuntos[5];
                resultados[0] = resultado;
                this.diasPrediccion.Controls.Add(tabDia1);
                this.diasPrediccion.Controls.Add(tabDia2);
                this.diasPrediccion.Controls.Add(tabDia3);
                this.diasPrediccion.Controls.Add(tabDia4);
                this.diasPrediccion.Controls.Add(tabDia5);
            }
            prediccionDiaComboBox.SelectedIndex = 0;
        }

        private void agregarDia(string dia)
        {
            System.Windows.Forms.TabPage otroDia = new System.Windows.Forms.TabPage();
            //otroDia.Controls.Add(this.datosEntrada);
            otroDia.Location = new System.Drawing.Point(4, 25);
            otroDia.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            otroDia.Name = "dia_n";
            otroDia.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            otroDia.Size = new System.Drawing.Size(1139, 481);
            otroDia.TabIndex = 1;
            otroDia.Text = dia;
            otroDia.UseVisualStyleBackColor = true;
            diasPrediccion.Controls.Add(otroDia);
        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }
    }
}
