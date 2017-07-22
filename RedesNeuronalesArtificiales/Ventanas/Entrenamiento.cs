using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using RedesNeuronalesArtificiales.RNA;
using RedesNeuronalesArtificiales.Archivo;

namespace RedesNeuronalesArtificiales.Ventanas
{
    public partial class Entrenamiento : Form
    {
        public Entrenamiento()
        {
            InitializeComponent();
        }

        private void agregarNeurona_Click(object sender, EventArgs e)
        {
            gruposDeNeuronas.Items.Add("Neurona " + entradaNeurona.Text);
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
            //mapaResultado.DocumentText = "<h1>Aqui se mostrara el mapa de resultados</h1>";
            Som redNeuronal = Guardar.Deserializar("Red Som Final.mp10");
            mapaResultado.Navigate("about:black");
            mapaResultado.Document.OpenNew(false);
            mapaResultado.Document.Write(Mp10.obtenerMP10HTML(redNeuronal.MatrizPesos, redNeuronal.NumeroFilas, redNeuronal.NumeroColumnas));
            mapaResultado.Refresh();
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
    }
}
