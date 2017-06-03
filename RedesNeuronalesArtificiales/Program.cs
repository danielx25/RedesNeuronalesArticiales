using RedesNeuronalesArtificiales.RNA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedesNeuronalesArtificiales
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
      
        static void Main()
        {
            Perceptron per = new Perceptron();
            per.entrenamiento(EjemploEntrenamiento.AND());
            System.Console.WriteLine("fila: 0 "+ EjemploEntrenamiento.AND().GetLength(0));
            System.Console.WriteLine("colu: 1 "+ EjemploEntrenamiento.AND().GetLength(1));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
