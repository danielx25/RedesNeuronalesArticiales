using System;
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

		private double alfa = 0.05;
		private double BETA = 0.004;

		public Som (int numeroVariablesEntrada, int numeroNeuronas, int numeroColumnasMatriz, double alfa, double beta)
		{
			this.alfa = alfa;
			this.BETA = beta;
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
		}

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

		public int[] calcularResultados(double[] fila)
		{
			int neuronaGanadora = -1;
			double mp10Normalizado = 0;
			double distanciaActual = 0;
			double menorDistancia = double.MaxValue;
			//int[] vecindad;
			for (int y = 0; y < numeroNeuronas; y++) {

				//Se calcula distancia
				distanciaActual = calculoDistancia(fila, matrizPesos, y);

				//Se selecciona la ganadora
				if (distanciaActual < menorDistancia) {
					menorDistancia = distanciaActual;
					neuronaGanadora = y;
					mp10Normalizado = matrizPesos [7, y];
				}
			}
			return new int[] {neuronaGanadora, (int)(mp10Normalizado * 800)};
		}

		public void entrenar(int ciclos)
		{
			Console.WriteLine ("Entrenando...");
			double menorDistancia = double.MaxValue;
			int neuronaGanadora = -1;
			int cicloActual = 0;

			//Este ciclo se ejecuta hasta que llege al numero maximo de ciclos o
			//Hasta que la tasa de aprendizaje sea menor o igual a cero
			while (cicloActual < ciclos && alfa >= 0) {
				Console.WriteLine ("Ciclo Nº " + (cicloActual+1) + " de " + ciclos + " Limite, Alfa actual: " + alfa);

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
							if (i == 0) {//Ganadora
								color = 5;
								if(datos [z][x] >= 0 && datos[z][x] <= 1)
									matrizPesos [x, indiceVecindad [i]] += ((datos [z] [x] - matrizPesos [x, indiceVecindad [i]]) * alfa);
							} else if (i > 0 && i <= 4) {//Distancia 1
								color = 4;
								if(datos [z][x] >= 0 && datos[z][x] <= 1)
									matrizPesos [x, indiceVecindad [i]] += ((datos [z] [x] - matrizPesos [x, indiceVecindad [i]]) * (alfa / 2));
							} else if (i > 4 && i <= 12) {//Distancia 2
								color = 3;
								if(datos [z][x] >= 0 && datos[z][x] <= 1)
									matrizPesos [x, indiceVecindad [i]] += ((datos [z] [x] - matrizPesos [x, indiceVecindad [i]]) * (alfa / 3));
							} else if (i > 12 && i <= 24) {//Distancia 3
								color = 2;
								if(datos [z][x] >= 0 && datos[z][x] <= 1)
									matrizPesos [x, indiceVecindad [i]] += ((datos [z] [x] - matrizPesos [x, indiceVecindad [i]]) * (alfa / 4));
							} else if (i > 24 && i <= 40) {//Distancia 4
								color = 1;
								if(datos [z][x] >= 0 && datos[z][x] <= 1)
									matrizPesos [x, indiceVecindad [i]] += ((datos [z] [x] - matrizPesos [x, indiceVecindad [i]]) * (alfa / 5));
							}

							//Se almacena el "color"
							if (colorMatriz.ContainsKey (indiceVecindad[i])) {
								colorMatriz [indiceVecindad[i]] = (int)colorMatriz [indiceVecindad[i]] + color;
							} else {
								colorMatriz.Add (indiceVecindad[i], color);
							}
						}
					}

					//Se restablecen los valores
					neuronaGanadora = -1;
					menorDistancia = double.MaxValue;
				}

				//Se disminuye la tasa de aprendizaje
				alfa -= BETA;
				cicloActual++;
				EscribirArchivo archivo = new EscribirArchivo("Datos MP10 ciclo ("+cicloActual+").html", true);
				archivo.imprimir(obtenerMP10HTML());
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
			int[] vecindad = new int[41];//Math.Pow(tamañoVecindad,2)+Math.Pow(tamañoVecindad-1)
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

						//Distancia 2
						vecindad [5] = verificar (x+1,y+1);
						vecindad [6] = verificar (x-1,y-1);
						vecindad [7] = verificar (x+1,y-1);
						vecindad [8] = verificar (x-1,y+1);
						vecindad [9] = verificar (x,y-2);
						vecindad [10] = verificar (x,y+2);
						vecindad [11] = verificar (x-2,y);
						vecindad [12] = verificar (x+2,y);

						//Distancia 3
						vecindad [13] = verificar (x,y-3);
						vecindad [14] = verificar (x+1,y-2);
						vecindad [15] = verificar (x+2,y-1);
						vecindad [16] = verificar (x+3,y);
						vecindad [17] = verificar (x+2,y+1);
						vecindad [18] = verificar (x+1,y+2);
						vecindad [19] = verificar (x,y+3);
						vecindad [20] = verificar (x-1,y+2);
						vecindad [21] = verificar (x-2,y+1);
						vecindad [22] = verificar (x-3,y);
						vecindad [23] = verificar (x-2,y-1);
						vecindad [24] = verificar (x-1,y-2);

						//Distancia 4
						vecindad [25] = verificar (x,y-4);
						vecindad [26] = verificar (x+1,y-3);
						vecindad [27] = verificar (x+2,y-2);
						vecindad [28] = verificar (x+3,y-1);
						vecindad [29] = verificar (x+4,y);
						vecindad [30] = verificar (x+3,y+1);
						vecindad [31] = verificar (x+2,y+2);
						vecindad [32] = verificar (x+1,y+3);
						vecindad [33] = verificar (x,y+4);
						vecindad [34] = verificar (x-1,y+3);
						vecindad [35] = verificar (x-2,y+2);
						vecindad [36] = verificar (x-3,y+1);
						vecindad [37] = verificar (x-4,y);
						vecindad [38] = verificar (x-3,y-1);
						vecindad [39] = verificar (x-2,y-2);
						vecindad [40] = verificar (x-1,y-3);

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

		public List<double[]> obtenerPesosNeuronas(HashSet<int> neuronas)
		{
			List<double[]> neuronasCentrales = new List<double[]> ();
			int contadorEncontrado = 0;
			for (int x = 0; x < numeroNeuronas && contadorEncontrado < neuronas.Count; x++) {
				double[] neuronaActual = new double[numeroVariablesEntradas];
				if (neuronas.Contains(x)) {
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
			int neuronaActual = 0;
			for(int x=0; x<numeroFilaMatriz; x++)
			{
				texto += "<tr>";
				for (int y = 0; y < numeroColumnasMatriz; y++) {
					valorMP10Normalizado = matrizPesos[7,neuronaActual];
					valorMP10Real = valorMP10Normalizado * 800;
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
					neuronaActual++;
				}
				texto += "</tr>\n";
			}
			texto += "</table>";
			return texto;
		}

		public override string ToString ()
		{
			string texto = "Matriz Color:\n";
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
			return texto;
		}
	}
}

