using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using RedesNeuronalesArtificiales.RNA;

namespace RedesNeuronalesArtificiales
{
	public class Guardar
	{
		public static void Serializar (Som datos, string archivo) {
			FileStream archivoGuardar = new FileStream (archivo, FileMode.Create);
			BinaryFormatter binario = new BinaryFormatter ();
			binario.Serialize (archivoGuardar, datos);
			archivoGuardar.Close ();
		}

		public static Som Deserializar(string archivo) {
			FileStream archivoAbrir = new FileStream (archivo, FileMode.Open);
			BinaryFormatter binario = new BinaryFormatter ();
			Som datos = binario.Deserialize (archivoAbrir) as Som;
			archivoAbrir.Close ();
			return datos;
		}
	}
}

