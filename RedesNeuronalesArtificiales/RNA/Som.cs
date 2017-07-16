using System;
using System.Collections.Generic;
using System.Collections;
using RedesNeuronalesArtificiales.Archivo;
using System.Drawing;

namespace RedesNeuronalesArtificiales.RNA
{
	[Serializable]
	public class Som
	{
		private int numeroVariablesEntradas = 0;
		private int numeroNeuronas = 0;
		private double[,] matrizPesos;
		private double[] distancia;
		private int[,] matriz;
		private int numeroColumnasMatriz = 2;
		private int numeroFilaMatriz = 2;
		private int[] indiceVecindad;
		private List<double[]> datos;
		private Hashtable colorMatriz;
		private Hashtable mp10Matriz;
		private Hashtable numeroDatos;

		private double alfa = 0.5;
		private double BETA = 0.001;

		public Som (int numeroVariablesEntrada, int numeroNeuronas, int numeroColumnasMatriz)
		{
			matrizPesos = new double[numeroVariablesEntrada,numeroNeuronas];
			distancia = new double[numeroNeuronas];
			if (numeroColumnasMatriz < 1)
				this.numeroColumnasMatriz = 2;
			if (numeroNeuronas % numeroColumnasMatriz != 0) {
				Console.WriteLine ("Error: No se puede formar una la matriz:" + numeroColumnasMatriz + "x" + (numeroNeuronas/(double)numeroColumnasMatriz));
				Console.WriteLine ("Codigo de error: " + Environment.ExitCode);
				Environment.Exit (Environment.ExitCode);
			}
			numeroFilaMatriz = (numeroNeuronas / numeroColumnasMatriz);
			this.matriz = new int[numeroFilaMatriz, numeroColumnasMatriz];
			this.numeroColumnasMatriz = numeroColumnasMatriz;
			this.numeroVariablesEntradas = numeroVariablesEntrada;
			this.numeroNeuronas = numeroNeuronas;
			this.colorMatriz = new Hashtable ();
			this.mp10Matriz = new Hashtable ();
			this.numeroDatos = new Hashtable ();
		}

		public void inicializarMatriz(double minimo, double maximo)
		{
			Random aleatorio = new Random ();

			for(int x=0; x<numeroVariablesEntradas; x++)
			{
				for (int y = 0; y < numeroNeuronas; y++) {
					matrizPesos [x, y] = aleatorio.NextDouble () * (maximo - minimo) + minimo;
				}
			}
			int indice = 0;
			for(int x=0; x<numeroFilaMatriz; x++)
			{
				for(int y=0; y<numeroColumnasMatriz; y++)
				{
					matriz [x, y] = indice;
					indice++;
				}
			}
		}

		public void inicializarMatriz(double[] minimos, double[] maximos)
		{
			Random aleatorio = new Random ();

			for(int x=0; x<numeroVariablesEntradas; x++)
			{
				for (int y = 0; y < numeroNeuronas; y++) {
					matrizPesos [x, y] = aleatorio.NextDouble () * (maximos [x] - minimos [x]) + minimos [x];
				}
			}

			int indice = 0;
			for(int x=0; x<numeroFilaMatriz; x++)
			{
				for(int y=0; y<numeroColumnasMatriz; y++)
				{
					matriz [x, y] = indice;
					indice++;
				}
			}
		}

		public void entrenar(int ciclos)
		{
			Console.WriteLine ("Entrenando...");
			//double distanciaActual = 0;
			double menorDistancia = double.MaxValue;
			int neuronaGanadora = -1;
			int cicloActual = 0;

			//Este ciclo se ejecuta hasta que llege al numero maximo de ciclos o
			//Hasta que la tasa de aprendizaje sea menor o igual a cero
			while (cicloActual < ciclos && alfa >= 0) {
				mp10Matriz.Clear ();//Prueba
				numeroDatos.Clear ();//Prueba
				Console.WriteLine ("Ciclo Nº " + (cicloActual+1) + " de " + ciclos + " Alfa: " + alfa);

				//Se recorre la tabla de datos
				for (int z = 0; z < datos.Count; z++) {

					//Se calcula la distancia y se selecciona la ganadora
					for (int y = 0; y < numeroNeuronas; y++) {

						//Se calcula distancia
						distancia [y] = calculoDistancia(datos[z], matrizPesos, y);

						//Se selecciona la ganadora
						if (distancia [y] < menorDistancia) {
							menorDistancia = distancia [y];
							neuronaGanadora = y;
						}
					}

					indiceVecindad = calcularVecindad (neuronaGanadora);

					//Se mueve la neurona ganadora y la vecindad
					int color = 0;
					for (int x = 0; x < numeroVariablesEntradas; x++) {
						for (int i = 0; i < indiceVecindad.Length; i++) {
							color = 0;
							if (i == 0) {
								color = 3;
								if(datos [z][x] >= 0 && datos[z][x] <= 1)
									matrizPesos [x, indiceVecindad [i]] += ((datos [z] [x] - matrizPesos [x, indiceVecindad [i]]) * alfa);
							} else if (i > 0 && i <= 8) {
								color = 2;
								if(datos [z][x] >= 0 && datos[z][x] <= 1)
									matrizPesos [x, indiceVecindad [i]] += ((datos [z] [x] - matrizPesos [x, indiceVecindad [i]]) * (alfa / 2));
							} else {
								color = 1;
								if(datos [z][x] >= 0 && datos[z][x] <= 1)
									matrizPesos [x, indiceVecindad [i]] += ((datos [z] [x] - matrizPesos [x, indiceVecindad [i]]) * (alfa / 3));
							}

							//Se almacena el "color"
							if (colorMatriz.ContainsKey (indiceVecindad[i])) {
								colorMatriz [indiceVecindad[i]] = (int)colorMatriz [indiceVecindad[i]] + color;
							} else {
								colorMatriz.Add (indiceVecindad[i], color);
							}

							if (x == 6) {
								//Mp10
								if (mp10Matriz.ContainsKey (indiceVecindad [i])) {
									mp10Matriz [indiceVecindad [i]] = (double)mp10Matriz [indiceVecindad [i]] + datos [z] [x];
									numeroDatos [indiceVecindad [i]] = (int)numeroDatos [indiceVecindad [i]] + 1;
								} else {
									mp10Matriz.Add (indiceVecindad [i], datos [z] [x]);
									numeroDatos.Add (indiceVecindad [i], 1);
								}
							}
						}
					}
					//Console.WriteLine (this);//Imprime la matriz
					//Console.WriteLine ("Ganadora " + neuronaGanadora);
					//Console.WriteLine ("Menor Distancia: " + menorDistancia);

					//Se restablecen los valores
					neuronaGanadora = -1;
					menorDistancia = double.MaxValue;
				}

				//Se disminuye la tasa de aprendizaje
				alfa -= BETA;
				cicloActual++;
				//Console.WriteLine (this);
				EscribirArchivo archivo = new EscribirArchivo("Datos ciclo ("+cicloActual+").txt");
				archivo.imprimir (this.ToString());
				archivo.cerrar ();
			}
			Console.WriteLine ("Entrenamiento terminado");
		}

		public double calculoDistancia(double[] datos, double[,] pesos, int neurona)
		{
			double distanciaActual = 0;

			//Sumatoria de la distancia
			for (int x = 0; x < datos.Length; x++) {
                if(datos [x] >= 0 && datos[x] <= 1)//Solo datos dentro de rango
                    distanciaActual += Math.Pow (datos [x] - pesos [x, neurona], 2);
				//else
				//	Console.WriteLine("Alerta dato fuera de rango en: " + datos[0] + " con valor: " + datos[x] + " Columna de datos: " + (x+1));
			}
			//Calculo de la distancia
			return Math.Sqrt (distanciaActual);
		}

		public int[] calcularVecindad(int ganadora)
		{
			int[] vecindad = new int[13];//Math.Pow(tamañoVecindad,2)+Math.Pow(tamañoVecindad-1)
			vecindad[0] = ganadora;

			for(int x=0; x<numeroFilaMatriz; x++)
			{
				for(int y=0; y<numeroColumnasMatriz; y++)
				{
					if (matriz [x, y] == ganadora) {

						vecindad [1] = verificar (x,y-1);
						vecindad [2] = verificar (x,y+1);
						vecindad [3] = verificar (x-1,y);
						vecindad [4] = verificar (x+1,y);

						vecindad [5] = verificar (x+1,y+1);
						vecindad [6] = verificar (x-1,y-1);
						vecindad [7] = verificar (x+1,y-1);
						vecindad [8] = verificar (x-1,y+1);

						vecindad [9] = verificar (x,y-2);
						vecindad [10] = verificar (x,y+2);
						vecindad [11] = verificar (x-2,y);
						vecindad [12] = verificar (x+2,y);

						y = numeroColumnasMatriz;
						x = numeroFilaMatriz;
					}
				}
			}
			return vecindad;
		}

		public int verificar(int x, int y)
		{
			if(x >= 0 && x<numeroFilaMatriz && y>=0 && y<numeroColumnasMatriz)
				return matriz[x,y];
			int nuevaX = x;
			int nuevaY = y;

			if (x < 0)
				nuevaX = numeroFilaMatriz + x;
			if (x >= numeroFilaMatriz)
				nuevaX = x - numeroFilaMatriz;

			if (y < 0)
				nuevaY = numeroColumnasMatriz + y;
			if (y >= numeroColumnasMatriz)
				nuevaY = y - numeroColumnasMatriz;
			
			return matriz[nuevaX, nuevaY];
		}

		public List<double[]> obtenerPesosNeuronas(Hashtable neuronas)
		{
			List<double[]> neuronasCentrales = new List<double[]> ();
			int contadorEncontrado = 0;
			for (int x = 0; x < numeroNeuronas && contadorEncontrado < neuronas.Count; x++) {
				double[] neuronaActual = new double[numeroVariablesEntradas];
				if (neuronas [x] != null) {
					for(int y=0; y<numeroVariablesEntradas; y++)
					{
						neuronaActual[y] = matrizPesos[y,x];
					}
					neuronasCentrales.Add (neuronaActual);
					contadorEncontrado++;
				}
			}
			return neuronasCentrales;
		}

		public List<double[]> Datos
		{
			get {
				return datos;
			}
			set {
				this.datos = value;
			}
		}

		public override string ToString ()
		{
			string texto = "Matriz:\n";
			if (numeroNeuronas <= 500) {
				for (int x = 0; x < numeroVariablesEntradas; x++) {
					for (int y = 0; y < numeroNeuronas; y++) {
						texto += matrizPesos [x, y] + ", ";
					}
					texto += "\n";
				}
			}
			texto += "\nMatriz Color\n";
			for(int x=0; x<numeroFilaMatriz; x++)
			{
				for (int y = 0; y < numeroColumnasMatriz; y++) {
					if(colorMatriz[matriz [x,y]] == null)
						texto += "0\t";
					else
						texto += colorMatriz[matriz [x,y]] + "\t";
				}
				texto += "\n";
			}
			texto += "\nMatriz Color RGB\n<table>";
			bool cicloTerminado = false;
			int neurona = 0;
			while(!cicloTerminado)
			{
				texto +="<tr>";
				for (int y=0; y < numeroColumnasMatriz; y++) {
					texto +="<td style=' background: rgb(";
					for (int x = 0; x < numeroVariablesEntradas; x++) {
						int valor = (int)(matrizPesos [x, neurona] * 255);
						if(x<numeroVariablesEntradas-1)
							texto += valor + ",";
						else
							texto += valor;
					}
					neurona++;
					texto +=");width: 10px; height: 10px;'></td>";
				}
				texto += "\n";
				if (neurona >= numeroNeuronas)
					cicloTerminado = true;
				texto +="</tr>";
			}
			texto += "</table>\nMP10\n";
			for(int x=0; x<numeroFilaMatriz; x++)
			{
				for (int y = 0; y < numeroColumnasMatriz; y++) {
					if(mp10Matriz[matriz [x,y]] == null)
						texto += "0\t";
					else
						texto += (((double)mp10Matriz[matriz [x,y]])/((int)numeroDatos[matriz [x,y]])) + "\t";
				}
				texto += "\n";
			}
			return texto;
		}
	}
}

