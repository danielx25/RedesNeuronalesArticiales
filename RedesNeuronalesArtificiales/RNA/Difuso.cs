using System;

namespace RedesNeuronalesArtificiales
{
	public class Difuso
	{
		public static double verano(DateTime fecha)
		{
			double mes = fecha.Month + (((99.99999*fecha.Day)/DateTime.DaysInMonth(fecha.Year, fecha.Month))/100);
			double valor1 = funcionGaussiana (0.25, 0.5, mes);
			double valor2 = funcionGaussiana (0.25, 12.5, mes);
			if (valor1 > valor2)
				return valor1;
			else
				return valor2;
		}

		public static double invierno(DateTime fecha)
		{
			double mes = fecha.Month + (((99.99999*fecha.Day)/DateTime.DaysInMonth(fecha.Year, fecha.Month))/100);
			return funcionGaussiana (0.25, 6.5, mes);
		}

		public static double verano(DateTime fecha, double ancho, double centro)
		{
			double mes = fecha.Month + (((99.99999*fecha.Day)/DateTime.DaysInMonth(fecha.Year, fecha.Month))/100);
			double valor1 = funcionGaussiana (ancho, centro-12, mes);
			double valor2 = funcionGaussiana (ancho, centro, mes);
			if (valor1 > valor2)
				return valor1;
			else
				return valor2;
		}

		public static double invierno(DateTime fecha, double ancho, double centro)
		{
			double mes = fecha.Month + (((99.99999*fecha.Day)/DateTime.DaysInMonth(fecha.Year, fecha.Month))/100);
			return funcionGaussiana (ancho, centro, mes);
		}

		public static double funcionGaussiana(double k, double m, double x)
		{
			return Math.Exp (-k*Math.Pow((x-m),2));
		}
	}
}

