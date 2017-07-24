using RedesNeuronalesArtificiales.RNA;
using RedesNeuronalesArtificiales.BaseDeDatos;
using RedesNeuronalesArtificiales.Archivo;
using System;
using System.Collections.Generic;
using RedesNeuronalesArtificiales.AnalisisDeRNA;
using RedesNeuronalesArtificiales.Ventanas;
using RedesNeuronalesArtificiales.Reportes;
using System.Diagnostics;
using System.Windows.Forms;

namespace RedesNeuronalesArtificiales
{
    static class Inicio
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]

        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Entrenamiento());
            //Application.Run(new Ventana());
            //ReportePrediccion reporte1 = new ReportePrediccion();
            //reporte1.crearReporte();
            //ejemploPrediccionGrupo();
        }

        static void ejemploPrediccionGrupo()
        {
            Som redNeuronal = Guardar.Deserializar("Red Som Final.mp10");
            EscribirArchivo archivo = new EscribirArchivo("Pesos MP10.html", true);
			archivo.imprimir(Mp10.obtenerMP10HTML(redNeuronal.MatrizPesos, redNeuronal.NumeroFilas, redNeuronal.NumeroColumnas));
            archivo.cerrar();


            HashSet<int> hashtable = new HashSet<int>();
            hashtable.Add(870);
            hashtable.Add(720);
            hashtable.Add(950);
            hashtable.Add(717);
            hashtable.Add(726);
            hashtable.Add(818);
            hashtable.Add(699);

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
            System.Console.ReadKey();

            System.Console.WriteLine("termino!!!");
            System.Console.ReadKey();

            Double[,] vectoresG = { { .99, .2, .1},
                                  { 0, 0, 0},
                                  { .5, .5, .5},
                                  { .2, .1, 0} };

            Double[,] ejemplo = { { .1, .5, .6},
                                  { .3, .5, .1},
                                  { .1, .8, .7},
                                  { .9, .9, .5},
                                  { .6, .12, .3} };

            ConstruccionConjuntos conj = new ConstruccionConjuntos(7, 5);//numero de grupos, tam vector
            conj.tablaVectoresGrupos(pesosxgrupo);
            conj.calcularConjuntoClase(sa, 0);
            System.Console.WriteLine("---------------------->");
            conj.calcularConjuntoClase(a1, 1);
            System.Console.WriteLine("---------------------->");
            conj.calcularConjuntoClase(a2, 2);
            System.Console.WriteLine("---------------------->");
            conj.calcularConjuntoClase(a3, 3);
            System.Console.WriteLine("---------------------->");
            conj.calcularConjuntoClase(a4, 4);
            System.Console.ReadKey();
        }

		public static int calcularAlerta(double mp10)
		{
			if (mp10 <= 150)
				return 0;
			else if(mp10 > 150 && mp10 <= 250)
				return 1;
			else if(mp10 > 250 && mp10 <= 350)
				return 2;
			else if(mp10 > 350 && mp10 <= 500)
				return 3;
			else if(mp10 > 500)
				return 4;
			return -1;
		}
    }
}
