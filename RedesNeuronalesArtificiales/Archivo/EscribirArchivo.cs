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

		public void imprimir(String linea)
		{
			archivo.Write(linea);
		}

		public void cerrar()
		{
			if(!cerrado) archivo.Close();
			cerrado = true;
		}
	}
}

