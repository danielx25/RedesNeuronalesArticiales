﻿using System;
using System.Collections.Generic;
using System.Collections;
using RedesNeuronalesArtificiales.Archivo;

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

		private double alfa = 0.005;
		private double BETA = 0.0001;

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
							} else if (i > 0 && i <= 4) {
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
				EscribirArchivo archivo = new EscribirArchivo("Datos MP10 ciclo ("+cicloActual+").html");
				archivo.imprimir (obtenerMP10HTML());
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

		public string obtenerMP10HTML()
		{
			string texto = "<table>";
			double valorMP10Normalizado = 0;
			double valorMP10Real = 0;
			for(int x=0; x<numeroFilaMatriz; x++)
			{
				texto += "<tr>";
				for (int y = 0; y < numeroColumnasMatriz; y++) {
					if (mp10Matriz [matriz [x, y]] == null)
						texto += "<td style=' background: #ffffff; width: 10px; height: 10px;'></td>";
					else {
						valorMP10Normalizado = (((double)mp10Matriz [matriz [x, y]]) / ((int)numeroDatos [matriz [x, y]]));
						valorMP10Real = valorMP10Normalizado * 1400;
						if (valorMP10Real == 0)
							texto += "<td style=' background: #cbffcb; width: 10px; height: 10px;font-size: 10px;text-align: center;'>0</td>";
						else if (valorMP10Real > 0 && valorMP10Real <= 150) {//Sin Alerta
							if (valorMP10Real > 0 && valorMP10Real <= 50)
								texto += "<td style=' background: #92ff92; width: 10px; height: 10px;font-size: 10px;text-align: center;'>0</td>";
							else if (valorMP10Real > 50 && valorMP10Real <= 100)
								texto += "<td style=' background: #40e040; width: 10px; height: 10px;font-size: 10px;text-align: center;'>0</td>";
							else if (valorMP10Real > 100 && valorMP10Real <= 150)
								texto += "<td style=' background: #16a716; width: 10px; height: 10px;font-size: 10px;text-align: center;'>0</td>";
						} else if (valorMP10Real > 150 && valorMP10Real <= 250) {//Alerta 1
							if (valorMP10Real > 150 && valorMP10Real <= 200)
								texto += "<td style=' background: #00ffff; width: 10px; height: 10px;font-size: 10px;text-align: center;'>1</td>";
							else if (valorMP10Real > 200 && valorMP10Real <= 250)
								texto += "<td style=' background: #0090ff; width: 10px; height: 10px;font-size: 10px;text-align: center;'>1</td>";
						} else if (valorMP10Real > 250 && valorMP10Real <= 350) {//Alerta 2
							if (valorMP10Real > 250 && valorMP10Real <= 300)
								texto += "<td style=' background: #0055ff; width: 10px; height: 10px;font-size: 10px;text-align: center;color:#ffffff;'>2</td>";
							else if (valorMP10Real > 300 && valorMP10Real <= 350)
								texto += "<td style=' background: #0000ff; width: 10px; height: 10px;font-size: 10px;text-align: center;color:#ffffff;'>2</td>";
						} else if (valorMP10Real > 350 && valorMP10Real <= 500) {//Alerta 3
							if (valorMP10Real > 350 && valorMP10Real <= 400)
								texto += "<td style=' background: #ffff00; width: 10px; height: 10px;font-size: 10px;text-align: center;'>3</td>";
							else if (valorMP10Real > 400 && valorMP10Real <= 450)
								texto += "<td style=' background: #ff7f00; width: 10px; height: 10px;font-size: 10px;text-align: center;'>3</td>";
							else if (valorMP10Real > 450 && valorMP10Real <= 500)
								texto += "<td style=' background: #ff4600; width: 10px; height: 10px;font-size: 10px;text-align: center;'>3</td>";
						} else if (valorMP10Real > 500) {//Alerta 4
							if (valorMP10Real > 500 && valorMP10Real <= 600)
								texto += "<td style=' background: #ff0000; width: 10px; height: 10px;font-size: 10px;text-align: center;color:#ffffff;'>4</td>";
							else if (valorMP10Real > 600 && valorMP10Real <= 700)
								texto += "<td style=' background: #b20000; width: 10px; height: 10px;font-size: 10px;text-align: center;color:#ffffff;'>4</td>";
							else if (valorMP10Real > 700)
								texto += "<td style=' background: #480000; width: 10px; height: 10px;font-size: 10px;text-align: center;color:#ffffff;'>4</td>";
						}
					}
				}
				texto += "</tr>\n";
			}
			texto += "</table>";
			return texto;
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
			texto += "\nMP10\n";
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

