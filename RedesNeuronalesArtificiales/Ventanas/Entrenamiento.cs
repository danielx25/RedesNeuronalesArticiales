using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using RedesNeuronalesArtificiales.RNA;
using RedesNeuronalesArtificiales.Archivo;
using RedesNeuronalesArtificiales.BaseDeDatos;
using RedesNeuronalesArtificiales.AnalisisDeRNA;

namespace RedesNeuronalesArtificiales.Ventanas
{
    public partial class Entrenamiento : Form
    {
        private Som redNeuronal;
        private string nombre1, nombre2, nombre3;
        private Som archivo1, archivo2, archivo3;
        private Thread hilo;
        private Thread hiloProgreso;
        private bool entrenando = false;

        public Entrenamiento()
        {
            InitializeComponent();
        }

        private void agregarNeurona_Click(object sender, EventArgs e)
        {
            if (!entradaNeurona.Text.Equals(""))
            {
                gruposDeNeuronas.Items.Add("Neurona " + entradaNeurona.Text);
                string neuronas = "";
                for(int x=0; x < gruposDeNeuronas.Items.Count; x++)
                {
                    if(x < gruposDeNeuronas.Items.Count-1)
                        neuronas += gruposDeNeuronas.Items[x].ToString().Split(' ')[1] + ",";
                    else
                        neuronas += gruposDeNeuronas.Items[x].ToString().Split(' ')[1];
                }
                string nombre = entradaNombreArchivo.Text;
                if (nombre.Equals(""))
                    nombre = "Archivo sin nombre";
                EscribirArchivo archivo = new EscribirArchivo(nombre+".txt",true);
                archivo.imprimir(neuronas);
                archivo.cerrar();
            }
            entradaNeurona.Text = "";
        }

        private void entradaNeurona_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void entradaLimiteCiclos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void botonEntrenar_Click(object sender, EventArgs e)
        {
            int totalNeuronas = int.Parse(entradaNumeroNeuronas.Text);
            int neuronasPorFila = int.Parse(entradaNumeroNeuronasPorFila.Text);
            if (neuronasPorFila > 1 && (totalNeuronas % neuronasPorFila == 0))
            {
                double alfa = double.Parse(entradaAlfa.Text.Replace('.', ','));
                double beta = double.Parse(entradaBeta.Text.Replace('.', ','));
                int numeroDeCiclos = int.Parse(entradaLimiteCiclos.Text);
                DateTime inicio = fechaDeInicio.Value;
                DateTime fin = fechaDeTermino.Value;

                List<double[]> datosMeteorologicos = Conexion.datosMeteorologicos(inicio, fin, 0);
                redNeuronal = new Som(datosMeteorologicos[0].Length, totalNeuronas, neuronasPorFila, alfa, beta);
                redNeuronal.inicializarMatriz(0, 1);
                redNeuronal.Datos = datosMeteorologicos;
                
				mapaResultado.DocumentText = Mp10.obtenerMP10HTML(redNeuronal.MatrizPesos, redNeuronal.NumeroFilas, redNeuronal.NumeroColumnas);
                
                entrenando = true;
                hilo = new Thread(entrenar);
                hilo.Priority = ThreadPriority.Highest;
                hiloProgreso = new Thread(progreso);
                hilo.Start(numeroDeCiclos);
                hiloProgreso.Start();
                botonEntrenar.Enabled = false;
                botonDetener.Enabled = true;
            }
        }

        public void entrenar(object numeroDeCiclos)
        {
            redNeuronal.entrenar((int)numeroDeCiclos);
            string nombre = entradaNombreArchivo.Text;
            if (nombre.Equals(""))
                nombre = "Archivo sin nombre";
			Guardar.Serializar (redNeuronal, nombre+".mp10");
			EscribirArchivo imagen = new EscribirArchivo (nombre+".html", true);
			imagen.imprimir (Mp10.obtenerMP10HTML(redNeuronal.MatrizPesos, redNeuronal.NumeroFilas, redNeuronal.NumeroColumnas));
			imagen.cerrar ();
            entrenando = false;
        }

        public void progreso()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => barraDeProgreso.Maximum = redNeuronal.TotalCiclos));
                Invoke(new Action(() => botonEntrenar.Enabled = false));
                Invoke(new Action(() => botonDetener.Enabled = true));
            }
            int cicloAnterior = redNeuronal.CicloActual;
            int valor;
            while (entrenando)
            {
                if (InvokeRequired)
                {
                    valor = redNeuronal.CicloActual;
                    if (valor > redNeuronal.TotalCiclos)
						valor = redNeuronal.TotalCiclos;
                    Invoke(new Action(() => barraDeProgreso.Value = valor));
                }
                if(cicloAnterior != redNeuronal.CicloActual)
                {
                    cicloAnterior = redNeuronal.CicloActual;
                    mapaResultado.DocumentText = Mp10.obtenerMP10HTML(redNeuronal.MatrizPesos, redNeuronal.NumeroFilas, redNeuronal.NumeroColumnas);
                }
                Thread.Sleep(100);
            }
            if (InvokeRequired)
            {
                Invoke(new Action(() => barraDeProgreso.Value = 0));
                Invoke(new Action(() => botonEntrenar.Enabled = true));
                Invoke(new Action(() => botonDetener.Enabled = false));
            }
        }

        private void entradaBeta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsPunctuation(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void entradaAlfa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsPunctuation(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void pestannaEntrenamiento_Enter(object sender, EventArgs e)
        {
            mapaResultado.DocumentText = "<center>Aqui se mostrara el mapa del MP10 cuando comience el entrenamiento de la red</center>";
        }

        private void botonLimpiar_Click(object sender, EventArgs e)
        {
            gruposDeNeuronas.Items.Clear();
            entradaNeurona.Text = "";
        }

        private void entradaNumeroNeuronas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void entradaNumeroNeuronasPorFila_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void detener_Click(object sender, EventArgs e)
        {
            if (redNeuronal != null)
            {
                redNeuronal.CicloActual = 900000;
                entrenando = false;
                botonEntrenar.Enabled = true;
                botonDetener.Enabled = false;
            }
        }

        private void Entrenamiento_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(entrenando)
            {
                entrenando = false;
                redNeuronal.CicloActual = 900000;
            }
            Thread.Sleep(110);
            Application.Exit();
        }

        private void abrirArchivo_Click(object sender, EventArgs e)
        {
            dialogoAbrirArchivo.Filter = "Archivos de red neuronal (*.mp10)|*.mp10";
            dialogoAbrirArchivo.FilterIndex = 2;
            dialogoAbrirArchivo.RestoreDirectory = true;
            if (dialogoAbrirArchivo.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    nombreArchivo1.Text = dialogoAbrirArchivo.SafeFileName;
                    nombre1 = dialogoAbrirArchivo.FileName;
                    archivo1 = Guardar.Deserializar(dialogoAbrirArchivo.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al leer el archivo: " + ex.Message);
                }
            }
        }

        private void abrirArchivo2_Click(object sender, EventArgs e)
        {
            dialogoAbrirArchivo.Filter = "Archivos de red neuronal (*.mp10)|*.mp10";
            dialogoAbrirArchivo.FilterIndex = 2;
            dialogoAbrirArchivo.RestoreDirectory = true;
            if (dialogoAbrirArchivo.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    nombreArchivo2.Text = dialogoAbrirArchivo.SafeFileName;
                    nombre2 = dialogoAbrirArchivo.FileName;
                    archivo2 = Guardar.Deserializar(dialogoAbrirArchivo.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al leer el archivo: " + ex.Message);
                }
            }
        }

        private void abrirArchivo3_Click(object sender, EventArgs e)
        {
            dialogoAbrirArchivo.Filter = "Archivos de red neuronal (*.mp10)|*.mp10";
            dialogoAbrirArchivo.FilterIndex = 2;
            dialogoAbrirArchivo.RestoreDirectory = true;
            if (dialogoAbrirArchivo.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    nombreArchivo3.Text = dialogoAbrirArchivo.SafeFileName;
                    nombre3 = dialogoAbrirArchivo.FileName;
                    archivo3 = Guardar.Deserializar(dialogoAbrirArchivo.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al leer el archivo: " + ex.Message);
                }
            }
        }

        private void botonIniciarValidacion_Click(object sender, EventArgs e)
        {
            if(archivo1 != null && archivo2 != null && archivo3 != null)
            {
                barraDeProgresoValidacion.Maximum = 100;
                List<double[]> datos1 = Conexion.datosMeteorologicos(entradaFechaInicio.Value, entradaFechaTermino.Value,0);
                barraDeProgresoValidacion.Value = 3;
                List<double[]> datos2 = Conexion.datosMeteorologicos(entradaFechaInicio2.Value, entradaFechaTermino2.Value, 0);
                barraDeProgresoValidacion.Value = 7;
                List<double[]> datos3 = Conexion.datosMeteorologicos(entradaFechaInicio3.Value, entradaFechaTermino3.Value, 0);
                barraDeProgresoValidacion.Value = 10;
                List<double[,]> datosRango = Conexion.datosPorRangoMp10(new DateTime(2010,01,01), new DateTime(2017,03,01), 0);
                barraDeProgresoValidacion.Value = 30;

                Console.WriteLine("Creando tablas hash");
                HashSet<int> tabla1 = obtenerTabla(nombre1);
                barraDeProgresoValidacion.Value = 32;
                HashSet<int> tabla2 = obtenerTabla(nombre2);
                barraDeProgresoValidacion.Value = 34;
                HashSet<int> tabla3 = obtenerTabla(nombre3);
                barraDeProgresoValidacion.Value = 36;

                Console.WriteLine("Leyendo archivos");
                Som redNeuronal1 = Guardar.Deserializar(nombre1);
                barraDeProgresoValidacion.Value = 38;
                Som redNeuronal2 = Guardar.Deserializar(nombre2);
                barraDeProgresoValidacion.Value = 40;
                Som redNeuronal3 = Guardar.Deserializar(nombre3);
                barraDeProgresoValidacion.Value = 42;

                Console.WriteLine("Obteniendo Neuronas");
                double[,] pesosPorGrupo1 = redNeuronal1.obtenerPesosNeuronas(tabla1);
                double[,] pesosPorGrupo2 = redNeuronal2.obtenerPesosNeuronas(tabla2);
                double[,] pesosPorGrupo3 = redNeuronal3.obtenerPesosNeuronas(tabla3);
                barraDeProgresoValidacion.Value = 44;

                //Primer Archivo
                double[,] sa = datosRango[0];//Sin Alerta
                double[,] a1 = datosRango[1];//Alerta 1
                double[,] a2 = datosRango[2];//Alerta 2
                double[,] a3 = datosRango[3];//Alerta 3
                double[,] a4 = datosRango[4];//Alerta 4

                Console.WriteLine("Creando conjuntos");
                ConstruccionConjuntos resultado1 = new ConstruccionConjuntos(tabla1.Count, 5);//numero de grupos y los tipos de alerta
                ConstruccionConjuntos resultado2 = new ConstruccionConjuntos(tabla2.Count, 5);
                ConstruccionConjuntos resultado3 = new ConstruccionConjuntos(tabla3.Count, 5);
                barraDeProgresoValidacion.Value = 45;

                Console.WriteLine("Procesando conjuntos");
                //Primer Archivo
                resultado1.tablaVectoresGrupos(pesosPorGrupo1);
                resultado1.calcularConjuntoClase(sa, 0);
                resultado1.calcularConjuntoClase(a1, 1);
                resultado1.calcularConjuntoClase(a2, 2);
                resultado1.calcularConjuntoClase(a3, 3);
                resultado1.calcularConjuntoClase(a4, 4);
                resultado1.etiquetadoDelosGrupos();
                barraDeProgresoValidacion.Value = 50;
                //Segundo Archivo
                resultado2.tablaVectoresGrupos(pesosPorGrupo2);
                resultado2.calcularConjuntoClase(sa, 0);
                resultado2.calcularConjuntoClase(a1, 1);
                resultado2.calcularConjuntoClase(a2, 2);
                resultado2.calcularConjuntoClase(a3, 3);
                resultado2.calcularConjuntoClase(a4, 4);
                resultado2.etiquetadoDelosGrupos();
                barraDeProgresoValidacion.Value = 55;
                //Tercer Archivo
                resultado3.tablaVectoresGrupos(pesosPorGrupo3);
                resultado3.calcularConjuntoClase(sa, 0);
                resultado3.calcularConjuntoClase(a1, 1);
                resultado3.calcularConjuntoClase(a2, 2);
                resultado3.calcularConjuntoClase(a3, 3);
                resultado3.calcularConjuntoClase(a4, 4);
                resultado3.etiquetadoDelosGrupos();
                barraDeProgresoValidacion.Value = 60;

                Console.WriteLine("Calculando precision");
                double[] resultadoArchivo1 = calcularPrecision(datos1, resultado1);
                barraDeProgresoValidacion.Value = 70;
                double[] resultadoArchivo2 = calcularPrecision(datos2, resultado2);
                barraDeProgresoValidacion.Value = 80;
                double[] resultadoArchivo3 = calcularPrecision(datos3, resultado3);
                barraDeProgresoValidacion.Value = 99;

                Console.WriteLine("Mostrando resultados");
                resultadoArchivo1P.Text = resultadoArchivo1[0] + "";
                resultadoArchivo2P.Text = resultadoArchivo2[0] + "";
                resultadoArchivo3P.Text = resultadoArchivo3[0] + "";
                resultadoTotalP.Text = ((resultadoArchivo1[0]+ resultadoArchivo2[0]+ resultadoArchivo3[0])/3) + "";

                resultadoArchivo1E1.Text = (100-resultadoArchivo1[0]) + "";
                resultadoArchivo2E1.Text = (100-resultadoArchivo2[0]) + "";
                resultadoArchivo3E1.Text = (100-resultadoArchivo3[0]) + "";
                resultadoTotalE1.Text = (100-((resultadoArchivo1[0] + resultadoArchivo2[0] + resultadoArchivo3[0]) / 3)) + "";

                resultadoArchivo1E2.Text = resultadoArchivo1[1] + "";
                resultadoArchivo2E2.Text = resultadoArchivo2[1] + "";
                resultadoArchivo3E2.Text = resultadoArchivo3[1] + "";
                resultadoTotalE2.Text = ((resultadoArchivo1[1] + resultadoArchivo2[1] + resultadoArchivo3[1]) / 3) + "";
                barraDeProgresoValidacion.Value = 100;
                panelResultados.Visible = true;
            }
            else
            {
                MessageBox.Show("Seleccione los archivos faltantes antes de continuar");
            }
        }

        private double[] calcularPrecision(List<double[]> datos, ConstruccionConjuntos resultado)
        {
            int errores = 0;
            int errorMayor = 0;
            int nivelAlertaPredecido = 0;
            int nivelAlertaReal = 0;
            for (int x = 0; x < datos.Count; x++)
            {
                nivelAlertaReal = obtenerNivelAlerta((int)(datos[x][7] * 800));
                nivelAlertaPredecido = obtenerNivelAlerta((int)resultado.prediccionMP10(datos[x]));
                if (nivelAlertaReal != nivelAlertaPredecido)
                {
                    errores++;
                    if (Math.Abs(nivelAlertaReal - nivelAlertaPredecido) > 1)
                        errorMayor++;
                }
            }
            double porcentajeAcertado = 100 - ((errores * 100.0) / datos.Count);
            double porcentajeAcertadoMenor = 100 - ((errorMayor * 100.0) / datos.Count);
            return new double[] {porcentajeAcertado, porcentajeAcertadoMenor};
        }

        private int obtenerNivelAlerta(double mp10)
        {
            if (mp10 <= 150)
                return 0;
            else if (mp10 > 150 && mp10 <= 250)
                return 1;
            else if (mp10 > 250 && mp10 <= 350)
                return 2;
            else if (mp10 > 350 && mp10 <= 500)
                return 3;
            else
                return 4;
        }

        private HashSet<int> obtenerTabla(string nombre)
        {
            StreamReader archivo = new StreamReader(nombre.Substring(0, nombre.Length-5)+".txt");
            string linea = archivo.ReadLine();
            string[] neuronas = linea.Split(',');

            HashSet<int> tabla = new HashSet<int>();
            for (int x = 0; x < neuronas.Length; x++)
            {
                tabla.Add(int.Parse(neuronas[x]));
            }
            return tabla;
        }
    }
}
