using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using RedesNeuronalesArtificiales.RNA;
using RedesNeuronalesArtificiales.Archivo;
using RedesNeuronalesArtificiales.BaseDeDatos;

namespace RedesNeuronalesArtificiales.Ventanas
{
    public partial class Entrenamiento : Form
    {
        private Som redNeuronal;
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
                EscribirArchivo archivo = new EscribirArchivo("Grupo de neuronas.txt",true);
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
                
                mapaResultado.Navigate("about:black");
                mapaResultado.Document.OpenNew(false);
				mapaResultado.DocumentText = Mp10.obtenerMP10HTML(redNeuronal.MatrizPesos, redNeuronal.NumeroFilas, redNeuronal.NumeroColumnas);
                mapaResultado.Refresh();
                
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
			Guardar.Serializar (redNeuronal, "Red entrenada.mp10");
			EscribirArchivo imagen = new EscribirArchivo ("Mapa del mp10.html", true);
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
                        valor = redNeuronal.CicloActual;
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
    }
}
