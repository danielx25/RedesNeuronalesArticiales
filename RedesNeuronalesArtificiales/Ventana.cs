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
        List<double[]> listaEntrada;
        double[] vectorEntrada = new double[38];
        public int numeroGrupos = 10;

        ConstruccionConjuntos[] resultados;
        int indiceActualResultado = 0;
        string nombreArchivoMp10 = "Red Som Final.mp10";
        string nombreArchivoHtml = "Pesos aleatorios.html";
        HashSet<int> listaGruposNeurona;

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
            
            Som redNeuronal = Guardar.Deserializar(nombreArchivoMp10);
            EscribirArchivo archivo = new EscribirArchivo(nombreArchivoHtml, true);
            archivo.imprimir(Mp10.obtenerMP10HTML(redNeuronal.MatrizPesos, redNeuronal.NumeroFilas, redNeuronal.NumeroColumnas));
            archivo.cerrar();
            cambiarBarraProgreso(22);

            listaGruposNeurona = new HashSet<int>();
            listaGruposNeurona.Add(555);
            listaGruposNeurona.Add(290);
            listaGruposNeurona.Add(1373);
            listaGruposNeurona.Add(2471);
            listaGruposNeurona.Add(2352);
            listaGruposNeurona.Add(868);
            listaGruposNeurona.Add(1664);
            listaGruposNeurona.Add(1300);
            listaGruposNeurona.Add(1650);
            listaGruposNeurona.Add(943);

            double[,] pesosxgrupo = redNeuronal.obtenerPesosNeuronas(listaGruposNeurona);

            DateTime inicio = new DateTime(2010, 01, 01, 00, 00, 00);
            DateTime fin = new DateTime(2017, 03, 01, 00, 00, 00);

            //List<double[]> datosMeteorologicos = Conexion.datosMeteorologicos(inicio, fin, 10);
            //Console.WriteLine("Total: " + datosMeteorologicos.Count);
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
            if(resultados.Length == 1)
                listaEntrada.Add(obtenerFormulario1());

            if (resultados.Length == 2)
            {
                listaEntrada.Add(obtenerFormulario1());
                listaEntrada.Add(obtenerFormulario2());
            }

            if (resultados.Length == 3)
            {
                listaEntrada.Add(obtenerFormulario1());
                listaEntrada.Add(obtenerFormulario2());
                listaEntrada.Add(obtenerFormulario3());
            }

            if (resultados.Length == 4)
            {
                listaEntrada.Add(obtenerFormulario1());
                listaEntrada.Add(obtenerFormulario2());
                listaEntrada.Add(obtenerFormulario3());
                listaEntrada.Add(obtenerFormulario4());
            }

            if (resultados.Length == 5)
            {
                listaEntrada.Add(obtenerFormulario1());
                listaEntrada.Add(obtenerFormulario2());
                listaEntrada.Add(obtenerFormulario3());
                listaEntrada.Add(obtenerFormulario4());
                listaEntrada.Add(obtenerFormulario5());
            }



            /*
            //Creamos el delegado 
            ThreadStart proceso = new ThreadStart(analisisResultadosEntregados);
            //Creamos la instancia del hilo 
            Thread hilo = new Thread(proceso);
            //Iniciamos el hilo 
            hilo.Start();
            */
        }

        double [] obtenerFormulario1()
        {
            double []vectorEntrada;
            double verano = Difuso.verano(datoMes1.Value);
            double invierno = Difuso.invierno(datoMes1.Value);
            double hora = datoMes1.Value.Hour;//24 horas
            double velocidadViento = Convert.ToDouble( datoVeloViento1.Text);
            double direccionViento = Convert.ToDouble(datoDirecViento1.Text);
            double temperatura = Convert.ToDouble(datoTemp1.Text);
            double humedad = Convert.ToDouble(datoHumedad1.Text);
            double mp10 = 0;
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

            double datoChancadoDia1_ = Convert.ToDouble(datoChancadoDia1.Text);
            double datoMovitecDia1_ = Convert.ToDouble(datoMovitecDia1.Text);
            double datoGerencia1_ = Convert.ToDouble(datoGerencia1.Text);
            double datosDasdia1_ = Convert.ToDouble(datosDasdia1.Text);
            double datosCmovil1_ = Convert.ToDouble(datosCmovil1.Text);
            double datosCnorte1_ = Convert.ToDouble(datosCnorte1.Text);
            double datosCachimba1_1_ = Convert.ToDouble(datosCachimba1_1.Text);
            double datosCachimba2_1_ = Convert.ToDouble(datosCachimba2_1.Text);

            double palas1 = (double)datosDetencionPalas1.GetItemCheckState(0);
            double palas2 = (double)datosDetencionPalas1.GetItemCheckState(1);
            double palas3 = (double)datosDetencionPalas1.GetItemCheckState(2);
            double palas4 = (double)datosDetencionPalas1.GetItemCheckState(3);
            double palas5 = (double)datosDetencionPalas1.GetItemCheckState(4);
            double palas6 = (double)datosDetencionPalas1.GetItemCheckState(5);
            double palas7 = (double)datosDetencionPalas1.GetItemCheckState(6);
            double palas8 = (double)datosDetencionPalas1.GetItemCheckState(7);

            double chancado1 = (double)datosDetencionChancadores_1.GetItemCheckState(0);
            double chancado2 = (double)datosDetencionChancadores_1.GetItemCheckState(1);

            hora = Entrada.normalizar(datoMes1.Value.Hour, 0, 24);
            velocidadViento = Entrada.normalizar(velocidadViento, 0, 30);//velocidad_viento
            direccionViento = Entrada.normalizar(direccionViento, 0, 360);//direccion_viento
            temperatura = Entrada.normalizar(temperatura, -10, 55);//temperatura
            humedad = Entrada.normalizar(humedad, 0, 100);//humedad_relativa
            mp10 = Entrada.normalizar(mp10, 0, 800);//mp10
            radiacion = Entrada.normalizar(radiacion, 0, 1700);//radiacion_solar
            presion = Entrada.normalizar(presion, 440, 600);//presion_atmosferica
            preci1 = Entrada.normalizar(preci1, 0, 47);//precipitaciondia1
            preci2 = Entrada.normalizar(preci2, 0, 47);//precipitaciondia2
            preci3 = Entrada.normalizar(preci3, 0, 47);//precipitaciondia3
            preci4 = Entrada.normalizar(preci4, 0, 47);//precipitaciondia4
            preci5 = Entrada.normalizar(preci5, 0, 47);//precipitaciondia5
            evapo1 = Entrada.normalizar(evapo1, 0, 363000);//evaporaciondia1
            evapo2 = Entrada.normalizar(evapo2, 0, 363000);//evaporaciondia2
            evapo3 = Entrada.normalizar(evapo3, 0, 363000);//evaporaciondia3
            evapo4 = Entrada.normalizar(evapo4, 0, 363000);//evaporaciondia4
            evapo5 = Entrada.normalizar(evapo5, 0, 363000);//evaporaciondia5
            datoChancadoDia1_ = Entrada.normalizar(datoChancadoDia1_, 0, 7);//chaxa camion dia y noche
            datoMovitecDia1_ = Entrada.normalizar(datoMovitecDia1_, 0, 8);//movitec camion dia y noche
            datosDasdia1_ = Entrada.normalizar(datosDasdia1_, 0, 4);//das camion dia
            datosCnorte1_ = Entrada.normalizar(Math.Round((datosCnorte1_ / 24), 1), 0, 90240);//cnorte consumo de agua
            datosCmovil1_ = Entrada.normalizar(Math.Round((datosCmovil1_ / 24), 1), 0, 4480);//cmovil consumo de agua
            datosCachimba1_1_ = Entrada.normalizar(Math.Round((datosCachimba1_1_ / 24), 1), 0, 1500);//cachimba1 consumo de agua
            datosCachimba2_1_ = Entrada.normalizar(Math.Round((datosCachimba2_1_ / 24), 1), 0, 2270);//cachimba2 consumo de agua
            datoGerencia1_ = Entrada.normalizar(Math.Round((datoGerencia1_ / 24), 1), 0, 27000);//gerencia consumo de agua

            vectorEntrada = new double[] { verano, invierno, hora, velocidadViento, direccionViento,
                temperatura, humedad, mp10, radiacion, presion, preci1, preci2, preci3, preci4, preci5,
                evapo1, evapo2, evapo3, evapo4, evapo5, datoChancadoDia1_, datoMovitecDia1_, datoGerencia1_,
                datosDasdia1_, datosCmovil1_, datosCnorte1_, datosCachimba1_1_, datosCachimba2_1_, palas1,
                palas2, palas3, palas4, palas5, palas6, palas7, palas8, chancado1, chancado2};
            for(int i=0; i<vectorEntrada.Length; i++)
            {
                Console.WriteLine(vectorEntrada[i]);
            }
            return vectorEntrada;
        }

        double[] obtenerFormulario2()
        {
            double[] vectorEntrada;
            double verano = Difuso.verano(datoMes2.Value);
            double invierno = Difuso.invierno(datoMes2.Value);
            double hora = datoMes2.Value.Hour;//24 horas
            double velocidadViento = Convert.ToDouble(datoVeloViento2.Text);
            double direccionViento = Convert.ToDouble(datoDirecViento2.Text);
            double temperatura = Convert.ToDouble(datoTemp2.Text);
            double humedad = Convert.ToDouble(datoHumedad2.Text);
            double mp10 = 0;
            double radiacion = Convert.ToDouble(datoRadiaSolar2.Text);
            double presion = Convert.ToDouble(datoPresion2.Text);

            double preci1 = Convert.ToDouble(datoPreciMañana2.Text);
            double preci2 = Convert.ToDouble(datoPrecihoy2.Text);
            double preci3 = Convert.ToDouble(datoPreci1_2.Text);
            double preci4 = Convert.ToDouble(datoPreci2_2.Text);
            double preci5 = Convert.ToDouble(datoPreci3_2.Text);

            double evapo1 = Convert.ToDouble(datoEvapomañana2.Text);
            double evapo2 = Convert.ToDouble(datoEvapohoy2.Text);
            double evapo3 = Convert.ToDouble(datoEvapo1_2.Text);
            double evapo4 = Convert.ToDouble(datoEvapo2_2.Text);
            double evapo5 = Convert.ToDouble(datoEvapo3_2.Text);

            double datoChancadoDia1_ = Convert.ToDouble(datoChancadoDia2.Text);
            double datoMovitecDia1_ = Convert.ToDouble(datoMovitecDia2.Text);
            double datoGerencia1_ = Convert.ToDouble(datoGerencia2.Text);
            double datosDasdia1_ = Convert.ToDouble(datosDasdia2.Text);
            double datosCmovil1_ = Convert.ToDouble(datosCmovil2.Text);
            double datosCnorte1_ = Convert.ToDouble(datosCnorte2.Text);
            double datosCachimba1_1_ = Convert.ToDouble(datosCachimba1_2.Text);
            double datosCachimba2_1_ = Convert.ToDouble(datosCachimba2_2.Text);

            double palas1 = (double)datosDetencionPalas2.GetItemCheckState(0);
            double palas2 = (double)datosDetencionPalas2.GetItemCheckState(1);
            double palas3 = (double)datosDetencionPalas2.GetItemCheckState(2);
            double palas4 = (double)datosDetencionPalas2.GetItemCheckState(3);
            double palas5 = (double)datosDetencionPalas2.GetItemCheckState(4);
            double palas6 = (double)datosDetencionPalas2.GetItemCheckState(5);
            double palas7 = (double)datosDetencionPalas2.GetItemCheckState(6);
            double palas8 = (double)datosDetencionPalas2.GetItemCheckState(7);

            double chancado1 = (double)datosDetencionChancadores_2.GetItemCheckState(0);
            double chancado2 = (double)datosDetencionChancadores_2.GetItemCheckState(1);

            hora = Entrada.normalizar(datoMes2.Value.Hour, 0, 24);
            velocidadViento = Entrada.normalizar(velocidadViento, 0, 30);//velocidad_viento
            direccionViento = Entrada.normalizar(direccionViento, 0, 360);//direccion_viento
            temperatura = Entrada.normalizar(temperatura, -10, 55);//temperatura
            humedad = Entrada.normalizar(humedad, 0, 100);//humedad_relativa
            mp10 = Entrada.normalizar(mp10, 0, 800);//mp10
            radiacion = Entrada.normalizar(radiacion, 0, 1700);//radiacion_solar
            presion = Entrada.normalizar(presion, 440, 600);//presion_atmosferica
            preci1 = Entrada.normalizar(preci1, 0, 47);//precipitaciondia1
            preci2 = Entrada.normalizar(preci2, 0, 47);//precipitaciondia2
            preci3 = Entrada.normalizar(preci3, 0, 47);//precipitaciondia3
            preci4 = Entrada.normalizar(preci4, 0, 47);//precipitaciondia4
            preci5 = Entrada.normalizar(preci5, 0, 47);//precipitaciondia5
            evapo1 = Entrada.normalizar(evapo1, 0, 363000);//evaporaciondia1
            evapo2 = Entrada.normalizar(evapo2, 0, 363000);//evaporaciondia2
            evapo3 = Entrada.normalizar(evapo3, 0, 363000);//evaporaciondia3
            evapo4 = Entrada.normalizar(evapo4, 0, 363000);//evaporaciondia4
            evapo5 = Entrada.normalizar(evapo5, 0, 363000);//evaporaciondia5
            datoChancadoDia1_ = Entrada.normalizar(datoChancadoDia1_, 0, 7);//chaxa camion dia y noche
            datoMovitecDia1_ = Entrada.normalizar(datoMovitecDia1_, 0, 8);//movitec camion dia y noche
            datosDasdia1_ = Entrada.normalizar(datosDasdia1_, 0, 4);//das camion dia
            datosCnorte1_ = Entrada.normalizar(Math.Round((datosCnorte1_ / 24), 1), 0, 90240);//cnorte consumo de agua
            datosCmovil1_ = Entrada.normalizar(Math.Round((datosCmovil1_ / 24), 1), 0, 4480);//cmovil consumo de agua
            datosCachimba1_1_ = Entrada.normalizar(Math.Round((datosCachimba1_1_ / 24), 1), 0, 1500);//cachimba1 consumo de agua
            datosCachimba2_1_ = Entrada.normalizar(Math.Round((datosCachimba2_1_ / 24), 1), 0, 2270);//cachimba2 consumo de agua
            datoGerencia1_ = Entrada.normalizar(Math.Round((datoGerencia1_ / 24), 1), 0, 27000);//gerencia consumo de agua

            vectorEntrada = new double[] { verano, invierno, hora, velocidadViento, direccionViento,
                temperatura, humedad, mp10, radiacion, presion, preci1, preci2, preci3, preci4, preci5,
                evapo1, evapo2, evapo3, evapo4, evapo5, datoChancadoDia1_, datoMovitecDia1_, datoGerencia1_,
                datosDasdia1_, datosCmovil1_, datosCnorte1_, datosCachimba1_1_, datosCachimba2_1_, palas1,
                palas2, palas3, palas4, palas5, palas6, palas7, palas8, chancado1, chancado2};
            for (int i = 0; i < vectorEntrada.Length; i++)
            {
                Console.WriteLine(vectorEntrada[i]);
            }
            return vectorEntrada;
        }

        double[] obtenerFormulario3()
        {
            double[] vectorEntrada;
            double verano = Difuso.verano(datoMes3.Value);
            double invierno = Difuso.invierno(datoMes3.Value);
            double hora = datoMes3.Value.Hour;//34 horas
            double velocidadViento = Convert.ToDouble(datoVeloViento3.Text);
            double direccionViento = Convert.ToDouble(datoDirecViento3.Text);
            double temperatura = Convert.ToDouble(datoTemp3.Text);
            double humedad = Convert.ToDouble(datoHumedad3.Text);
            double mp10 = 0;
            double radiacion = Convert.ToDouble(datoRadiaSolar3.Text);
            double presion = Convert.ToDouble(datoPresion3.Text);

            double preci1 = Convert.ToDouble(datoPreciMañana3.Text);
            double preci2 = Convert.ToDouble(datoPrecihoy3.Text);
            double preci3 = Convert.ToDouble(datoPreci1_3.Text);
            double preci4 = Convert.ToDouble(datoPreci2_3.Text);
            double preci5 = Convert.ToDouble(datoPreci3_3.Text);

            double evapo1 = Convert.ToDouble(datoEvapomañana3.Text);
            double evapo2 = Convert.ToDouble(datoEvapohoy3.Text);
            double evapo3 = Convert.ToDouble(datoEvapo1_3.Text);
            double evapo4 = Convert.ToDouble(datoEvapo2_3.Text);
            double evapo5 = Convert.ToDouble(datoEvapo3_3.Text);

            double datoChancadoDia1_ = Convert.ToDouble(datoChancadoDia3.Text);
            double datoMovitecDia1_ = Convert.ToDouble(datoMovitecDia3.Text);
            double datoGerencia1_ = Convert.ToDouble(datoGerencia3.Text);
            double datosDasdia1_ = Convert.ToDouble(datosDasdia3.Text);
            double datosCmovil1_ = Convert.ToDouble(datosCmovil3.Text);
            double datosCnorte1_ = Convert.ToDouble(datosCnorte3.Text);
            double datosCachimba1_1_ = Convert.ToDouble(datosCachimba1_3.Text);
            double datosCachimba2_1_ = Convert.ToDouble(datosCachimba2_3.Text);

            double palas1 = (double)datosDetencionPalas3.GetItemCheckState(0);
            double palas2 = (double)datosDetencionPalas3.GetItemCheckState(1);
            double palas3 = (double)datosDetencionPalas3.GetItemCheckState(2);
            double palas4 = (double)datosDetencionPalas3.GetItemCheckState(3);
            double palas5 = (double)datosDetencionPalas3.GetItemCheckState(4);
            double palas6 = (double)datosDetencionPalas3.GetItemCheckState(5);
            double palas7 = (double)datosDetencionPalas3.GetItemCheckState(6);
            double palas8 = (double)datosDetencionPalas3.GetItemCheckState(7);

            double chancado1 = (double)datosDetencionChancadores_3.GetItemCheckState(0);
            double chancado2 = (double)datosDetencionChancadores_3.GetItemCheckState(1);

            hora = Entrada.normalizar(datoMes3.Value.Hour, 0, 24);
            velocidadViento = Entrada.normalizar(velocidadViento, 0, 30);//velocidad_viento
            direccionViento = Entrada.normalizar(direccionViento, 0, 360);//direccion_viento
            temperatura = Entrada.normalizar(temperatura, -10, 55);//temperatura
            humedad = Entrada.normalizar(humedad, 0, 100);//humedad_relativa
            mp10 = Entrada.normalizar(mp10, 0, 800);//mp10
            radiacion = Entrada.normalizar(radiacion, 0, 1700);//radiacion_solar
            presion = Entrada.normalizar(presion, 440, 600);//presion_atmosferica
            preci1 = Entrada.normalizar(preci1, 0, 47);//precipitaciondia1
            preci2 = Entrada.normalizar(preci2, 0, 47);//precipitaciondia2
            preci3 = Entrada.normalizar(preci3, 0, 47);//precipitaciondia3
            preci4 = Entrada.normalizar(preci4, 0, 47);//precipitaciondia4
            preci5 = Entrada.normalizar(preci5, 0, 47);//precipitaciondia5
            evapo1 = Entrada.normalizar(evapo1, 0, 363000);//evaporaciondia1
            evapo2 = Entrada.normalizar(evapo2, 0, 363000);//evaporaciondia2
            evapo3 = Entrada.normalizar(evapo3, 0, 363000);//evaporaciondia3
            evapo4 = Entrada.normalizar(evapo4, 0, 363000);//evaporaciondia4
            evapo5 = Entrada.normalizar(evapo5, 0, 363000);//evaporaciondia5
            datoChancadoDia1_ = Entrada.normalizar(datoChancadoDia1_, 0, 7);//chaxa camion dia y noche
            datoMovitecDia1_ = Entrada.normalizar(datoMovitecDia1_, 0, 8);//movitec camion dia y noche
            datosDasdia1_ = Entrada.normalizar(datosDasdia1_, 0, 4);//das camion dia
            datosCnorte1_ = Entrada.normalizar(Math.Round((datosCnorte1_ / 24), 1), 0, 90240);//cnorte consumo de agua
            datosCmovil1_ = Entrada.normalizar(Math.Round((datosCmovil1_ / 24), 1), 0, 4480);//cmovil consumo de agua
            datosCachimba1_1_ = Entrada.normalizar(Math.Round((datosCachimba1_1_ / 24), 1), 0, 1500);//cachimba1 consumo de agua
            datosCachimba2_1_ = Entrada.normalizar(Math.Round((datosCachimba2_1_ / 24), 1), 0, 2270);//cachimba2 consumo de agua
            datoGerencia1_ = Entrada.normalizar(Math.Round((datoGerencia1_ / 24), 1), 0, 27000);//gerencia consumo de agua

            vectorEntrada = new double[] { verano, invierno, hora, velocidadViento, direccionViento,
                temperatura, humedad, mp10, radiacion, presion, preci1, preci2, preci3, preci4, preci5,
                evapo1, evapo2, evapo3, evapo4, evapo5, datoChancadoDia1_, datoMovitecDia1_, datoGerencia1_,
                datosDasdia1_, datosCmovil1_, datosCnorte1_, datosCachimba1_1_, datosCachimba2_1_, palas1,
                palas2, palas3, palas4, palas5, palas6, palas7, palas8, chancado1, chancado2};
            for (int i = 0; i < vectorEntrada.Length; i++)
            {
                Console.WriteLine(vectorEntrada[i]);
            }
            return vectorEntrada;
        }

        double[] obtenerFormulario4()
        {
            double[] vectorEntrada;
            double verano = Difuso.verano(datoMes4.Value);
            double invierno = Difuso.invierno(datoMes4.Value);
            double hora = datoMes4.Value.Hour;//34 horas
            double velocidadViento = Convert.ToDouble(datoVeloViento4.Text);
            double direccionViento = Convert.ToDouble(datoDirecViento4.Text);
            double temperatura = Convert.ToDouble(datoTemp4.Text);
            double humedad = Convert.ToDouble(datoHumedad4.Text);
            double mp10 = 0;
            double radiacion = Convert.ToDouble(datoRadiaSolar4.Text);
            double presion = Convert.ToDouble(datoPresion4.Text);

            double preci1 = Convert.ToDouble(datoPreciMañana4.Text);
            double preci2 = Convert.ToDouble(datoPrecihoy4.Text);
            double preci3 = Convert.ToDouble(datoPreci1_4.Text);
            double preci4 = Convert.ToDouble(datoPreci2_4.Text);
            double preci5 = Convert.ToDouble(datoPreci3_4.Text);

            double evapo1 = Convert.ToDouble(datoEvapomañana4.Text);
            double evapo2 = Convert.ToDouble(datoEvapohoy4.Text);
            double evapo3 = Convert.ToDouble(datoEvapo1_4.Text);
            double evapo4 = Convert.ToDouble(datoEvapo2_4.Text);
            double evapo5 = Convert.ToDouble(datoEvapo3_4.Text);

            double datoChancadoDia1_ = Convert.ToDouble(datoChancadoDia4.Text);
            double datoMovitecDia1_ = Convert.ToDouble(datoMovitecDia4.Text);
            double datoGerencia1_ = Convert.ToDouble(datoGerencia4.Text);
            double datosDasdia1_ = Convert.ToDouble(datosDasdia4.Text);
            double datosCmovil1_ = Convert.ToDouble(datosCmovil4.Text);
            double datosCnorte1_ = Convert.ToDouble(datosCnorte4.Text);
            double datosCachimba1_1_ = Convert.ToDouble(datosCachimba1_4.Text);
            double datosCachimba2_1_ = Convert.ToDouble(datosCachimba2_4.Text);

            double palas1 = (double)datosDetencionPalas4.GetItemCheckState(0);
            double palas2 = (double)datosDetencionPalas4.GetItemCheckState(1);
            double palas3 = (double)datosDetencionPalas4.GetItemCheckState(2);
            double palas4 = (double)datosDetencionPalas4.GetItemCheckState(3);
            double palas5 = (double)datosDetencionPalas4.GetItemCheckState(4);
            double palas6 = (double)datosDetencionPalas4.GetItemCheckState(5);
            double palas7 = (double)datosDetencionPalas4.GetItemCheckState(6);
            double palas8 = (double)datosDetencionPalas4.GetItemCheckState(7);

            double chancado1 = (double)datosDetencionChancadores_4.GetItemCheckState(0);
            double chancado2 = (double)datosDetencionChancadores_4.GetItemCheckState(1);

            hora = Entrada.normalizar(datoMes4.Value.Hour, 0, 24);
            velocidadViento = Entrada.normalizar(velocidadViento, 0, 30);//velocidad_viento
            direccionViento = Entrada.normalizar(direccionViento, 0, 360);//direccion_viento
            temperatura = Entrada.normalizar(temperatura, -10, 55);//temperatura
            humedad = Entrada.normalizar(humedad, 0, 100);//humedad_relativa
            mp10 = Entrada.normalizar(mp10, 0, 800);//mp10
            radiacion = Entrada.normalizar(radiacion, 0, 1700);//radiacion_solar
            presion = Entrada.normalizar(presion, 440, 600);//presion_atmosferica
            preci1 = Entrada.normalizar(preci1, 0, 47);//precipitaciondia1
            preci2 = Entrada.normalizar(preci2, 0, 47);//precipitaciondia2
            preci3 = Entrada.normalizar(preci3, 0, 47);//precipitaciondia3
            preci4 = Entrada.normalizar(preci4, 0, 47);//precipitaciondia4
            preci5 = Entrada.normalizar(preci5, 0, 47);//precipitaciondia5
            evapo1 = Entrada.normalizar(evapo1, 0, 363000);//evaporaciondia1
            evapo2 = Entrada.normalizar(evapo2, 0, 363000);//evaporaciondia2
            evapo3 = Entrada.normalizar(evapo3, 0, 363000);//evaporaciondia3
            evapo4 = Entrada.normalizar(evapo4, 0, 363000);//evaporaciondia4
            evapo5 = Entrada.normalizar(evapo5, 0, 363000);//evaporaciondia5
            datoChancadoDia1_ = Entrada.normalizar(datoChancadoDia1_, 0, 7);//chaxa camion dia y noche
            datoMovitecDia1_ = Entrada.normalizar(datoMovitecDia1_, 0, 8);//movitec camion dia y noche
            datosDasdia1_ = Entrada.normalizar(datosDasdia1_, 0, 4);//das camion dia
            datosCnorte1_ = Entrada.normalizar(Math.Round((datosCnorte1_ / 24), 1), 0, 90240);//cnorte consumo de agua
            datosCmovil1_ = Entrada.normalizar(Math.Round((datosCmovil1_ / 24), 1), 0, 4480);//cmovil consumo de agua
            datosCachimba1_1_ = Entrada.normalizar(Math.Round((datosCachimba1_1_ / 24), 1), 0, 1500);//cachimba1 consumo de agua
            datosCachimba2_1_ = Entrada.normalizar(Math.Round((datosCachimba2_1_ / 24), 1), 0, 2270);//cachimba2 consumo de agua
            datoGerencia1_ = Entrada.normalizar(Math.Round((datoGerencia1_ / 24), 1), 0, 27000);//gerencia consumo de agua

            vectorEntrada = new double[] { verano, invierno, hora, velocidadViento, direccionViento,
                temperatura, humedad, mp10, radiacion, presion, preci1, preci2, preci3, preci4, preci5,
                evapo1, evapo2, evapo3, evapo4, evapo5, datoChancadoDia1_, datoMovitecDia1_, datoGerencia1_,
                datosDasdia1_, datosCmovil1_, datosCnorte1_, datosCachimba1_1_, datosCachimba2_1_, palas1,
                palas2, palas3, palas4, palas5, palas6, palas7, palas8, chancado1, chancado2};
            for (int i = 0; i < vectorEntrada.Length; i++)
            {
                Console.WriteLine(vectorEntrada[i]);
            }
            return vectorEntrada;
        }

        double[] obtenerFormulario5()
        {
            double[] vectorEntrada;
            double verano = Difuso.verano(datoMes5.Value);
            double invierno = Difuso.invierno(datoMes5.Value);
            double hora = datoMes5.Value.Hour;//34 horas
            double velocidadViento = Convert.ToDouble(datoVeloViento5.Text);
            double direccionViento = Convert.ToDouble(datoDirecViento5.Text);
            double temperatura = Convert.ToDouble(datoTemp5.Text);
            double humedad = Convert.ToDouble(datoHumedad5.Text);
            double mp10 = 0;
            double radiacion = Convert.ToDouble(datoRadiaSolar5.Text);
            double presion = Convert.ToDouble(datoPresion5.Text);

            double preci1 = Convert.ToDouble(datoPreciMañana5.Text);
            double preci2 = Convert.ToDouble(datoPrecihoy5.Text);
            double preci3 = Convert.ToDouble(datoPreci1_5.Text);
            double preci4 = Convert.ToDouble(datoPreci2_5.Text);
            double preci5 = Convert.ToDouble(datoPreci3_5.Text);

            double evapo1 = Convert.ToDouble(datoEvapomañana5.Text);
            double evapo2 = Convert.ToDouble(datoEvapohoy5.Text);
            double evapo3 = Convert.ToDouble(datoEvapo1_5.Text);
            double evapo4 = Convert.ToDouble(datoEvapo2_5.Text);
            double evapo5 = Convert.ToDouble(datoEvapo3_5.Text);

            double datoChancadoDia1_ = Convert.ToDouble(datoChancadoDia5.Text);
            double datoMovitecDia1_ = Convert.ToDouble(datoMovitecDia5.Text);
            double datoGerencia1_ = Convert.ToDouble(datoGerencia5.Text);
            double datosDasdia1_ = Convert.ToDouble(datosDasdia5.Text);
            double datosCmovil1_ = Convert.ToDouble(datosCmovil5.Text);
            double datosCnorte1_ = Convert.ToDouble(datosCnorte5.Text);
            double datosCachimba1_1_ = Convert.ToDouble(datosCachimba1_5.Text);
            double datosCachimba2_1_ = Convert.ToDouble(datosCachimba2_5.Text);

            double palas1 = (double)datosDetencionPalas5.GetItemCheckState(0);
            double palas2 = (double)datosDetencionPalas5.GetItemCheckState(1);
            double palas3 = (double)datosDetencionPalas5.GetItemCheckState(2);
            double palas4 = (double)datosDetencionPalas5.GetItemCheckState(3);
            double palas5 = (double)datosDetencionPalas5.GetItemCheckState(4);
            double palas6 = (double)datosDetencionPalas5.GetItemCheckState(5);
            double palas7 = (double)datosDetencionPalas5.GetItemCheckState(6);
            double palas8 = (double)datosDetencionPalas5.GetItemCheckState(7);

            double chancado1 = (double)datosDetencionChancadores_5.GetItemCheckState(0);
            double chancado2 = (double)datosDetencionChancadores_5.GetItemCheckState(1);

            hora = Entrada.normalizar(datoMes5.Value.Hour, 0, 24);
            velocidadViento = Entrada.normalizar(velocidadViento, 0, 30);//velocidad_viento
            direccionViento = Entrada.normalizar(direccionViento, 0, 360);//direccion_viento
            temperatura = Entrada.normalizar(temperatura, -10, 55);//temperatura
            humedad = Entrada.normalizar(humedad, 0, 100);//humedad_relativa
            mp10 = Entrada.normalizar(mp10, 0, 800);//mp10
            radiacion = Entrada.normalizar(radiacion, 0, 1700);//radiacion_solar
            presion = Entrada.normalizar(presion, 440, 600);//presion_atmosferica
            preci1 = Entrada.normalizar(preci1, 0, 47);//precipitaciondia1
            preci2 = Entrada.normalizar(preci2, 0, 47);//precipitaciondia2
            preci3 = Entrada.normalizar(preci3, 0, 47);//precipitaciondia3
            preci4 = Entrada.normalizar(preci4, 0, 47);//precipitaciondia4
            preci5 = Entrada.normalizar(preci5, 0, 47);//precipitaciondia5
            evapo1 = Entrada.normalizar(evapo1, 0, 363000);//evaporaciondia1
            evapo2 = Entrada.normalizar(evapo2, 0, 363000);//evaporaciondia2
            evapo3 = Entrada.normalizar(evapo3, 0, 363000);//evaporaciondia3
            evapo4 = Entrada.normalizar(evapo4, 0, 363000);//evaporaciondia4
            evapo5 = Entrada.normalizar(evapo5, 0, 363000);//evaporaciondia5
            datoChancadoDia1_ = Entrada.normalizar(datoChancadoDia1_, 0, 7);//chaxa camion dia y noche
            datoMovitecDia1_ = Entrada.normalizar(datoMovitecDia1_, 0, 8);//movitec camion dia y noche
            datosDasdia1_ = Entrada.normalizar(datosDasdia1_, 0, 4);//das camion dia
            datosCnorte1_ = Entrada.normalizar(Math.Round((datosCnorte1_ / 24), 1), 0, 90240);//cnorte consumo de agua
            datosCmovil1_ = Entrada.normalizar(Math.Round((datosCmovil1_ / 24), 1), 0, 4480);//cmovil consumo de agua
            datosCachimba1_1_ = Entrada.normalizar(Math.Round((datosCachimba1_1_ / 24), 1), 0, 1500);//cachimba1 consumo de agua
            datosCachimba2_1_ = Entrada.normalizar(Math.Round((datosCachimba2_1_ / 24), 1), 0, 2270);//cachimba2 consumo de agua
            datoGerencia1_ = Entrada.normalizar(Math.Round((datoGerencia1_ / 24), 1), 0, 27000);//gerencia consumo de agua

            vectorEntrada = new double[] { verano, invierno, hora, velocidadViento, direccionViento,
                temperatura, humedad, mp10, radiacion, presion, preci1, preci2, preci3, preci4, preci5,
                evapo1, evapo2, evapo3, evapo4, evapo5, datoChancadoDia1_, datoMovitecDia1_, datoGerencia1_,
                datosDasdia1_, datosCmovil1_, datosCnorte1_, datosCachimba1_1_, datosCachimba2_1_, palas1,
                palas2, palas3, palas4, palas5, palas6, palas7, palas8, chancado1, chancado2};
            for (int i = 0; i < vectorEntrada.Length; i++)
            {
                Console.WriteLine(vectorEntrada[i]);
            }
            return vectorEntrada;
        }

        private void diasApredecir_SelectedIndexChanged(object sender, EventArgs e)
        {
            prediccionDiaComboBox.Items.Clear();
            String opcion = (string)diasApredecir.SelectedItem;
            listaEntrada = new List<double[]>();

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
