using RedesNeuronalesArtificiales.RNA;
using RedesNeuronalesArtificiales.BaseDeDatos;
using RedesNeuronalesArtificiales.Archivo;
using System;
using System.Collections.Generic;
using RedesNeuronalesArtificiales.AnalisisDeRNA;
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
			/*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Ventana());
*/
            ejemploPrediccionGrupo();
            /*
            Stopwatch tiempoEjecucion = new Stopwatch ();
			tiempoEjecucion.Start ();
			//Redes Som Entrenando

			DateTime inicio = new DateTime(2010,01,01,00,00,00);
			DateTime fin = new DateTime(2016,12,31,23,59,59);

			DateTime inicioPrueba = new DateTime(2017,01,01,00,00,00);
			DateTime finPrueba = new DateTime(2017,03,01,00,00,00);

			List<double[]> datosMeteorologicos = Conexion.datosMeteorologicos (inicio, fin, 100);
			List<double[]> datosPruebas = Conexion.datosMeteorologicos (inicioPrueba, finPrueba, 0);

			Som redNeuronal = new Som (datosMeteorologicos[0].Length,900, 30, 0.01, 0.0001);
			redNeuronal.inicializarMatriz (0, 1);
			redNeuronal.Datos = datosMeteorologicos;

			tiempoEjecucion.Stop ();
			Console.WriteLine("Tiempo al cargar la base de datos: " + tiempoEjecucion.Elapsed.ToString());
			tiempoEjecucion.Restart ();

			EscribirArchivo archivo = new EscribirArchivo("Pesos aleatorios.html", true);
			archivo.imprimir (Mp10.obtenerMP10HTML(redNeuronal.MatrizPesos, redNeuronal.NumeroFilas, redNeuronal.NumeroColumnas));
			archivo.cerrar ();

			redNeuronal.entrenar (100);

			//Guarda Archivo
			Guardar.Serializar (redNeuronal, "Red Som Final.mp10");
            //Lee Archivo
			//Som redNeuronal = Guardar.Deserializar("Red Som Final.mp10");
			tiempoEjecucion.Stop();
			Console.WriteLine("Tiempo al entrenar la red: " + tiempoEjecucion.Elapsed.ToString());

			int error = 0;
			int errorMayor = 0;
			int errorMenor = 0;
			for(int x=0; x<datosPruebas.Count; x++)
			{
				double[] fila = datosPruebas [x];
				double mp10Real = fila [7]*800;
				fila [7] = -1;
				int[] resultado = redNeuronal.calcularResultados (fila);
				//Console.WriteLine ("Neurona Ganadora: " + resultado[0] + " MP10: " + resultado[1] + " MP10 real: " + (mp10Real) + " Alerta: " + calcularAlerta(resultado[1]) + " Alerta Real: " + calcularAlerta(mp10Real));
				if (calcularAlerta (resultado [1]) != calcularAlerta (mp10Real))
					error++;
				if (Math.Abs (calcularAlerta (resultado [1]) - calcularAlerta (mp10Real)) > 1)
					errorMayor++;
				if (calcularAlerta (resultado [1]) < calcularAlerta (mp10Real))
					errorMenor++;
			}
			Console.WriteLine ("Porcentaje de error: " + ((error*100)/datosPruebas.Count) + "%");
			Console.WriteLine ("Porcentaje de error mayor a 1 Alerta: " + ((errorMayor*100)/datosPruebas.Count) + "%");
			Console.WriteLine ("Porcentaje de error menor de la alerta real: " + ((errorMenor*100)/error) + "%");*/
        }

        static void ejemploPrediccionGrupo()
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
