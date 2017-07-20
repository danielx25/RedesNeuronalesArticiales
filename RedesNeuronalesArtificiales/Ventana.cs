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
        public Ventana()
        {
            InitializeComponent();
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
            Series grupo1 = new Series("grupo1");
            var series = new Series("Finance");
            series.ChartType = SeriesChartType.Line;
            // Frist parameter is X-Axis and Second is Collection of Y- Axis
            series.Points.DataBindXY(new[] { 2001, 2002, 2003, 2004 }, new[] { 100, 200, 90, 150 });
            System.Console.WriteLine("creando grafico");
            graficoSinAlerta.Titles.Add("Sin alerta");
            graficoSinAlerta.Series.Add(series);
        }
    }
}
