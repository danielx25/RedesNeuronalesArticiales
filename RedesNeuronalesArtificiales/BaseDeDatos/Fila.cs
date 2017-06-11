using System;

namespace RedesNeuronalesArtificiales.BaseDeDatos
{
	public class Fila
	{
		private DateTime fecha;
		private Double velocidadViento;
		private Int16 direccionViento;
		private Double temperatura;
		private Int16 humedadRelativa;
		private Int32 mp10;
		private Double precipitacion;//Acumulada
		private Int32 radiacionSolar;
		private Int32 precionAtmosferica;
		private Double evaporacion;//Acumulada

		private Double precipitacionAcumuladaDia1;
		private Double precipitacionAcumuladaDia2;
		private Double precipitacionAcumuladaDia3;

		private Double evaporacionAcumuladaDia1;
		private Double evaporacionAcumuladaDia2;
		private Double evaporacionAcumuladaDia3;

		private bool estimada = false;

		public Fila (DateTime fecha, Double velocidadViento, Int16 direccionViento, Double temperatura, Int16 humedadRelativa,
			Int32 mp10,	Double precipitacion, Int32 radiacionSolar,	Int32 precionAtmosferica, Double evaporacion)
		{
			this.fecha = fecha;
			this.velocidadViento = velocidadViento;
			this.direccionViento = direccionViento;
			this.temperatura = temperatura;
			this.humedadRelativa = humedadRelativa;
			this.mp10 = mp10;
			this.precipitacion = precipitacion;
			this.radiacionSolar = radiacionSolar;
			this.precionAtmosferica = precionAtmosferica;
			this.evaporacion = evaporacion;
		}

		public Fila (DateTime fecha, Double velocidadViento, Int16 direccionViento, Double temperatura, Int16 humedadRelativa,
			Int32 mp10,	Double precipitacion, Int32 radiacionSolar,	Int32 precionAtmosferica, Double evaporacion, bool estimada)
		{
			this.fecha = fecha;
			this.velocidadViento = velocidadViento;
			this.direccionViento = direccionViento;
			this.temperatura = temperatura;
			this.humedadRelativa = humedadRelativa;
			this.mp10 = mp10;
			this.precipitacion = precipitacion;
			this.radiacionSolar = radiacionSolar;
			this.precionAtmosferica = precionAtmosferica;
			this.evaporacion = evaporacion;
			this.estimada = estimada;
		}

		public DateTime Fecha
		{
			get {
				return fecha;
			}
		}

		public Double VelocidadViento
		{
			get {
				return velocidadViento;
			}
		}

		public Int16 DireccionViento
		{
			get {
				return direccionViento;
			}
		}

		public Double Temperatura
		{
			get {
				return temperatura;
			}
		}

		public Int16 HumedadRelativa
		{
			get {
				return humedadRelativa;
			}
		}

		public Int32 Mp10
		{
			get {
				return mp10;
			}
		}

		public Double Precipitacion
		{
			get {
				return precipitacion;
			}
			set {
				this.precipitacion = value;
			}
		}

		public Int32 RadiacionSolar
		{
			get {
				return radiacionSolar;
			}
		}

		public Int32 PrecionAtmosferica
		{
			get {
				return precionAtmosferica;
			}
		}

		public Double Evaporacion
		{
			get {
				return evaporacion;
			}
			set {
				this.evaporacion = value;
			}
		}

		public Double PrecipitacionAcumuladaDia1
		{
			get {
				return precipitacionAcumuladaDia1;
			}
			set {
				this.precipitacionAcumuladaDia1 = value;
			}
		}

		public Double PrecipitacionAcumuladaDia2
		{
			get {
				return precipitacionAcumuladaDia2;
			}
			set {
				this.precipitacionAcumuladaDia2 = value;
			}
		}

		public Double PrecipitacionAcumuladaDia3
		{
			get {
				return precipitacionAcumuladaDia3;
			}
			set {
				this.precipitacionAcumuladaDia3 = value;
			}
		}

		public Double EvaporacionAcumuladaDia1
		{
			get {
				return evaporacionAcumuladaDia1;
			}
			set {
				this.evaporacionAcumuladaDia1 = value;
			}
		}

		public Double EvaporacionAcumuladaDia2
		{
			get {
				return evaporacionAcumuladaDia2;
			}
			set {
				this.evaporacionAcumuladaDia2 = value;
			}
		}

		public Double EvaporacionAcumuladaDia3
		{
			get {
				return evaporacionAcumuladaDia3;
			}
			set {
				this.evaporacionAcumuladaDia3 = value;
			}
		}

		public override string ToString ()
		{
			return string.Format ("[Fila: {0} \t {1} \t {2} \t {3} \t {4} \t {5} \t {6} \t {7} \t {8} \t {9} \t {10} \t {11} \t {12} \t {13} \t {14} \t {15} \t {16}]", Fecha, VelocidadViento, DireccionViento, Temperatura, HumedadRelativa, Mp10, Precipitacion, RadiacionSolar, PrecionAtmosferica, Evaporacion, PrecipitacionAcumuladaDia1, PrecipitacionAcumuladaDia2, PrecipitacionAcumuladaDia3, EvaporacionAcumuladaDia1, EvaporacionAcumuladaDia2, EvaporacionAcumuladaDia3, estimada);
		}
	}
}