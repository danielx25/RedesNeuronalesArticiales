using System;

namespace RedesNeuronalesArtificiales.BaseDeDatos
{
	public class Entrada
	{
		public static double normalizar(double valor, double minimo, double maximo)
		{
			//(X_i - X.min) / (X.max - X.min)
			double valorNormalizado = (valor - minimo)/(maximo-minimo);
			if (valorNormalizado < 0 && valor != -1)
				Console.WriteLine ("Valor fuera de rango en: " + valor + " minimo: " + minimo + " maximo: " + maximo);
			return valorNormalizado;
		}

		public static double normalizar(int valor, int minimo, int maximo)
		{
			//(X_i - X.min) / (X.max - X.min)
			double valorNormalizado = ((double)(valor - minimo))/((double)(maximo-minimo));
			if (valorNormalizado < 0 && valor != -1)
				Console.WriteLine ("Valor fuera de rango en: " + valor + " minimo: " + minimo + " maximo: " + maximo);
			return valorNormalizado;
		}
	}
}

