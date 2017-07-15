using System;
using System.IO;

namespace RedesNeuronalesArtificiales.Archivo
{
	public class EscribirArchivo
	{

		private StreamWriter archivo = null;
		private bool cerrado = false;

		public EscribirArchivo() : this("sinTitulo")
		{
		}

		public EscribirArchivo(String nombreArchivo) : this(nombreArchivo,false)
		{
		}

		public EscribirArchivo(String nombreArchivo, bool reescribir)
		{
			try
			{
				archivo = new StreamWriter(nombreArchivo,!reescribir);
			}
			catch(IOException)
			{
				Console.WriteLine("No se puede crear el archivo \""+nombreArchivo+"\"");
			}
		}

		public void imprimir(int dato)
		{
			archivo.Write(dato);
		}

		public void imprimir(String linea)
		{
			archivo.Write(linea);
		}

		public void imprimirln()
		{
			archivo.Write("\n");
		}

		public void imprimirln(int dato)
		{
			archivo.Write(dato+"\n");
		}

		public void imprimirln(String linea)
		{
			archivo.Write(linea+"\n");
		}

		public void cerrar()
		{
			if(!cerrado) archivo.Close();
			cerrado = true;
		}
	}
}

