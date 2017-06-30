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
		private List<Fila> datos;

		public Som (int numeroVariablesEntrada, int numeroNeuronas)
		{
			matrizPesos = new double[numeroVariablesEntrada,numeroNeuronas];
			distancia = new double[numeroNeuronas];
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

		public void entrenar(int ciclos)
		{
			Console.WriteLine ("Entrenando...");
			double distanciaActual = 0;
			int cicloActual = 0;
			while (cicloActual < ciclos) {
				Console.WriteLine ("Ciclo Nº " + (cicloActual+1) + " de " + ciclos);
				for (int z = 0; z < datos.Count; z++) {
					for (int y = 0; y < numeroNeuronas; y++) {
						for (int x = 0; x < numeroVariablesEntradas; x++) {
							distanciaActual += Math.Pow (datos [z].procesarDato (x) - matrizPesos [x, y], 2);
						}
						distancia [y] = Math.Sqrt (distanciaActual);
						distanciaActual = 0;
					}
				}
				cicloActual++;
			}
			Console.WriteLine ("Entrenamiento terminado");
		}

		public List<Fila> Datos
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
			string matriz = "Matriz:\n";
			for(int x=0; x<numeroVariablesEntradas; x++)
			{
				for (int y = 0; y < numeroNeuronas; y++) {
					matriz += matrizPesos [x,y] + ", ";
				}
				matriz += "\n";
			}
			matriz += "\nDistancias:";
			for(int x=0; x<distancia.Length; x++)
			{
				matriz += distancia [x] + ", ";
			}
			matriz += "\n";
			return matriz;
		}
	}
}

