using System;
using System.Collections.Generic;

namespace RedesNeuronalesArtificiales.RNA
{
	public class Som
	{
		private int numeroVariablesEntradas = 0;
		private int numeroNeuronas = 0;
		private double[,] matrizPesos;
		//private List<int> distancia;

		public Som (int numeroVariablesEntrada, int numeroNeuronas)
		{
			matrizPesos = new double[numeroVariablesEntrada,numeroNeuronas];
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
		}

		public override string ToString ()
		{
			string matriz = "Matriz:\n";
			for(int x=0; x<numeroVariablesEntradas; x++)
			{
				for (int y = 0; y < numeroNeuronas; y++) {
					matriz += matrizPesos [x,y] + ", ";
				}
				matriz += "\n";
			}
			return matriz;
		}
	}
}

