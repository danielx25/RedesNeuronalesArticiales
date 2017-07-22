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
		private double[] distancia;//Distancia entre neuronas
		private int[,] matriz;//Matriz de indices
		private int numeroColumnasMatriz = 2;
		private int numeroFilaMatriz = 2;
		private int[] indiceVecindad;
		private List<double[]> datos;//Datos de entrada
		private Hashtable colorMatriz;//Matriz de colores

		private double alfa = 0.05;
		private double BETA = 0.004;
		private int ciclos = 0;
		private int cicloActual = 0;

		public Som (int numeroVariablesEntrada, int numeroNeuronas, int numeroColumnasMatriz, double alfa, double beta)
		{
			this.alfa = alfa;
			this.BETA = beta;
			matrizPesos = new double[numeroVariablesEntrada,numeroNeuronas];
			distancia = new double[numeroNeuronas];
			if (numeroColumnasMatriz < 2)
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
			if (numeroColumnasMatriz < 2)
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
            double alfaActual = alfa;
            this.ciclos = ciclos;
			Console.WriteLine ("Entrenando...");
			double menorDistancia = double.MaxValue;
			int neuronaGanadora = -1;
			cicloActual = 0;

            double sinAlerta = 1.0 / (58401-228);
            double alerta1 = 1.0 / (2258-228);
            double alerta2 = 1.0 / (974-228);
            double alerta3 = 1.0 / (524-228);
            double alerta4 = 1.0;
            double mp10 = 0;
            double peso = 1;

			//Este ciclo se ejecuta hasta que llege al numero maximo de ciclos o
			//Hasta que la tasa de aprendizaje sea menor o igual a cero
			while (cicloActual < ciclos && alfaActual >= 0) {
				Console.WriteLine ("Ciclo Nº " + CicloActual + " de " + TotalCiclos + " Limite, Alfa actual: " + alfaActual + " Beta: " + BETA);

				//Se recorre la tabla de datos
				for (int z = 0; z < datos.Count && cicloActual < ciclos; z++) {

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
                            mp10 = matrizPesos[7, indiceVecindad[i]]*800;
                            if (mp10 <= 150)
                                peso = sinAlerta;
                            else if (mp10 > 150 && mp10 <= 250)
                                peso = alerta1;
                            else if (mp10 > 250 && mp10 <= 350)
                                peso = alerta2;
                            else if (mp10 > 350 && mp10 <= 500)
                                peso = alerta3;
                            else if (mp10 > 500)
                                peso = alerta4;
                            if (i == 0) {//Ganadora
								color = 5;
								if(datos [z][x] >= 0 && datos[z][x] <= 1)
									matrizPesos [x, indiceVecindad [i]] += ((datos [z] [x] - matrizPesos [x, indiceVecindad [i]]) * alfaActual*peso);
							} else if (i > 0 && i <= 4) {//Distancia 1
								color = 4;
								if(datos [z][x] >= 0 && datos[z][x] <= 1)
									matrizPesos [x, indiceVecindad [i]] += ((datos [z] [x] - matrizPesos [x, indiceVecindad [i]]) * (alfaActual / 2) * peso);
							} else if (i > 4 && i <= 12) {//Distancia 2
								color = 3;
								if(datos [z][x] >= 0 && datos[z][x] <= 1)
									matrizPesos [x, indiceVecindad [i]] += ((datos [z] [x] - matrizPesos [x, indiceVecindad [i]]) * (alfaActual / 3) * peso);
							} else if (i > 12 && i <= 24) {//Distancia 3
								color = 2;
								if(datos [z][x] >= 0 && datos[z][x] <= 1)
									matrizPesos [x, indiceVecindad [i]] += ((datos [z] [x] - matrizPesos [x, indiceVecindad [i]]) * (alfaActual / 4) * peso);
							} else if (i > 24 && i <= 40) {//Distancia 4
								color = 1;
								if(datos [z][x] >= 0 && datos[z][x] <= 1)
									matrizPesos [x, indiceVecindad [i]] += ((datos [z] [x] - matrizPesos [x, indiceVecindad [i]]) * (alfaActual / 5) * peso);
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
                alfaActual -= BETA;
				cicloActual++;
				//EscribirArchivo archivo = new EscribirArchivo("Datos MP10 ciclo ("+cicloActual+").html", true);
				//archivo.imprimir(Mp10.obtenerMP10HTML(MatrizPesos, NumeroFilas, NumeroColumnas));
				//archivo.cerrar ();
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

		private int verificar(int x, int y)
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

		public double[,] obtenerPesosNeuronas(HashSet<int> neuronas)
		{
			List<double[]> neuronasCentrales = new List<double[]>();
			int contadorEncontrado = 0;
			for (int x = 0; x < numeroNeuronas && contadorEncontrado < neuronas.Count; x++)
			{
				double[] neuronaActual = new double[numeroVariablesEntradas];
				if (neuronas.Contains(x))
				{
					for (int y = 0; y < numeroVariablesEntradas; y++)
					{
						neuronaActual[y] = matrizPesos[y, x];
					}
					neuronasCentrales.Add(neuronaActual);
					contadorEncontrado++;
				}
			}

			int numGrupo = neuronasCentrales.Count;
			int numColumna = neuronasCentrales[0].Length;
			double[,] pesosGrupos = new double[numGrupo, numColumna];
			for (int fila = 0; fila < numGrupo; fila++)
			{
				for (int columna = 0; columna < numColumna; columna++)
				{
					pesosGrupos[fila, columna] = neuronasCentrales[fila][columna];
					System.Console.Write("| " + pesosGrupos[fila, columna]);
				}
				System.Console.WriteLine();
			}


			return pesosGrupos;
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

		public double[,] MatrizPesos
		{
			get {
				return matrizPesos;
			}
		}

		public int NumeroFilas
		{
			get {
				return numeroFilaMatriz;
			}
		}

		public int NumeroColumnas
		{
			get {
				return numeroColumnasMatriz;
			}
		}

		public int TotalCiclos
		{
			get {
				if (Math.Ceiling (alfa / BETA) < ciclos)
					return (int)Math.Ceiling (alfa / BETA);
				else
					return ciclos;
			}
		}

		public int CicloActual
		{
			get {
				return cicloActual+1;
			}
            set
            {
                cicloActual = value;
            }
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

