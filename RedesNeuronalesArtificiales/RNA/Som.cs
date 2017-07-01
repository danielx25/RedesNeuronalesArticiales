using System;
using System.Collections.Generic;

namespace RedesNeuronalesArtificiales.RNA
{
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

		private double alfa = 0.1;
		private double BETA = 0.05;

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
			alfa = 0.5;
			Console.WriteLine ("Entrenando...");
			//double distanciaActual = 0;
			double menorDistancia = double.MaxValue;
			int neuronaGanadora = -1;
			int cicloActual = 0;

			//Este ciclo se ejecuta hasta que llege al numero maximo de ciclos o
			//Hasta que la tasa de aprendizaje sea menor o igual a cero
			while (cicloActual < ciclos && alfa >= 0) {
				Console.WriteLine ("Ciclo Nº " + (cicloActual+1) + " de " + ciclos);

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
					for (int x = 0; x < numeroVariablesEntradas; x++) {
						for (int i = 0; i < indiceVecindad.Length; i++) {
							if(i == 0)
								matrizPesos [x, indiceVecindad[i]] += ((datos [z][x] - matrizPesos [x, indiceVecindad[i]]) * alfa);
							else if(i > 0 && i <= 8)
								matrizPesos [x, indiceVecindad[i]] += ((datos [z][x] - matrizPesos [x, indiceVecindad[i]]) * (alfa/2));
							else
								matrizPesos [x, indiceVecindad[i]] += ((datos [z][x] - matrizPesos [x, indiceVecindad[i]]) * (alfa/3));
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
			}
			Console.WriteLine ("Entrenamiento terminado");
		}

		public double calculoDistancia(double[] datos, double[,] pesos, int neurona)
		{
			double distanciaActual = 0;

			//Sumatoria de la distancia
			for (int x = 0; x < datos.Length; x++) {
				distanciaActual += Math.Pow (datos [x] - pesos [x, neurona], 2);
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
			for(int x=0; x<numeroVariablesEntradas; x++)
			{
				for (int y = 0; y < numeroNeuronas; y++) {
					texto += matrizPesos [x,y] + ", ";
				}
				texto += "\n";
			}
			texto += "Matriz Indice\n";
			for(int x=0; x<numeroFilaMatriz; x++)
			{
				for (int y = 0; y < numeroColumnasMatriz; y++) {
					texto += matriz [x,y] + "\t";
				}
				texto += "\n";
			}
			return texto;
		}
	}
}

