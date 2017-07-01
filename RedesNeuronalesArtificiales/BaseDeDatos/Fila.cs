using System;

namespace RedesNeuronalesArtificiales
{
	public class Fila
	{
		private DateTime fecha;
		private Double velocidadViento;
		private Int16 direccionViento;
		private Double temperatura;
		private Int16 humedadRelativa;
		private Int32 mp10;
		private Int32 radiacionSolar;
		private Int32 precionAtmosferica;

		private Double precipitacionAcumuladaDia1;
		private Double precipitacionAcumuladaDia2;
		private Double precipitacionAcumuladaDia3;
		private Double precipitacionAcumuladaDia4;
		private Double precipitacionAcumuladaDia5;

		private Double evaporacionAcumuladaDia1;
		private Double evaporacionAcumuladaDia2;
		private Double evaporacionAcumuladaDia3;
		private Double evaporacionAcumuladaDia4;
		private Double evaporacionAcumuladaDia5;

		public Fila (DateTime fecha, Double velocidadViento, Int16 direccionViento, Double temperatura, Int16 humedadRelativa,
			Int32 mp10,	Int32 radiacionSolar, Int32 precionAtmosferica, Double precipitacionAcumuladaDia1, Double precipitacionAcumuladaDia2, 
			Double precipitacionAcumuladaDia3, Double precipitacionAcumuladaDia4, Double precipitacionAcumuladaDia5,
			Double evaporacionAcumuladaDia1, Double evaporacionAcumuladaDia2, Double evaporacionAcumuladaDia3, Double evaporacionAcumuladaDia4,
			Double evaporacionAcumuladaDia5)
		{
			this.fecha = fecha;
			this.velocidadViento = velocidadViento;
			this.direccionViento = direccionViento;
			this.temperatura = temperatura;
			this.humedadRelativa = humedadRelativa;
			this.mp10 = mp10;
			this.radiacionSolar = radiacionSolar;
			this.precionAtmosferica = precionAtmosferica;
			this.precipitacionAcumuladaDia1 = precipitacionAcumuladaDia1;
			this.precipitacionAcumuladaDia2 = precipitacionAcumuladaDia2;
			this.precipitacionAcumuladaDia3 = precipitacionAcumuladaDia3;
			this.precipitacionAcumuladaDia4 = precipitacionAcumuladaDia4;
			this.precipitacionAcumuladaDia5 = precipitacionAcumuladaDia5;
			this.evaporacionAcumuladaDia1 = evaporacionAcumuladaDia1;
			this.evaporacionAcumuladaDia2 = evaporacionAcumuladaDia2;
			this.evaporacionAcumuladaDia3 = evaporacionAcumuladaDia3;
			this.evaporacionAcumuladaDia4 = evaporacionAcumuladaDia4;
			this.evaporacionAcumuladaDia5 = evaporacionAcumuladaDia5;
		}

		public double procesarDato(int x)
		{
			double rango = 1; //negativo y positivo
			//(X_i - X.min) / (X.max - X.min)
			if (x == 0) {
				//Fecha
				//Console.WriteLine ("Error: Fecha no procesadas aun");
				return 0;
			}
			else if (x == 1) {
				//velocidadViento
				return (velocidadViento/30.0)*rango;
			}
			else if (x == 2) {
				//direccionViento
				return (direccionViento/360)*rango;
			}
			else if (x == 3) {
				//temperatura
				return (temperatura/50)*rango;//Impresiso
			}
			else if (x == 4) {
				//humedadRelativa
				return (humedadRelativa/100.0)*rango;
			}
			else if (x == 5) {
				//mp10
				return (mp10/1800.0)*rango;//Impresiso
			}
			else if (x == 6) {
				//radiacionSolar
				return (radiacionSolar/1700.0)*rango;//Impresiso
			}
			else if (x == 7) {
				//precionAtmosferica
				return (precionAtmosferica/600.0)*rango;
			}
			else if (x == 8) {
				//precipitacionAcumuladaDia1
				return (PrecipitacionAcumuladaDia1/200)*rango;//Impresiso
			}
			else if (x == 9) {
				//precipitacionAcumuladaDia2
				return (PrecipitacionAcumuladaDia2/200)*rango;//Impresiso
			}
			else if (x == 10) {
				//precipitacionAcumuladaDia3
				return (PrecipitacionAcumuladaDia3/200)*rango;//Impresiso
			}
			else if (x == 11) {
				//precipitacionAcumuladaDia4
				return (PrecipitacionAcumuladaDia4/200)*rango;//Impresiso
			}
			else if (x == 12) {
				//precipitacionAcumuladaDia5
				return (PrecipitacionAcumuladaDia5/200)*rango;//Impresiso
			}
			else if (x == 13) {
				//evaporacionAcumuladaDia1
				return (evaporacionAcumuladaDia1/300)*rango;//Impresiso
			}
			else if (x == 14) {
				//evaporacionAcumuladaDia2
				return (evaporacionAcumuladaDia2/300)*rango;//Impresiso
			}
			else if (x == 15) {
				//evaporacionAcumuladaDia3
				return (evaporacionAcumuladaDia3/300)*rango;//Impresiso
			}
			else if (x == 16) {
				//evaporacionAcumuladaDia4
				return (evaporacionAcumuladaDia4/300)*rango;//Impresiso
			}
			else if (x == 17) {
				//evaporacionAcumuladaDia5
				return (evaporacionAcumuladaDia5/300)*rango;//Impresiso
			}
			Console.WriteLine ("Error: No hay tantas columnas en la fila");
			return 0;
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

		public Double PrecipitacionAcumuladaDia4
		{
			get {
				return precipitacionAcumuladaDia4;
			}
			set {
				this.precipitacionAcumuladaDia4 = value;
			}
		}

		public Double PrecipitacionAcumuladaDia5
		{
			get {
				return precipitacionAcumuladaDia5;
			}
			set {
				this.precipitacionAcumuladaDia5 = value;
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

		public Double EvaporacionAcumuladaDia4
		{
			get {
				return evaporacionAcumuladaDia4;
			}
			set {
				this.evaporacionAcumuladaDia4 = value;
			}
		}

		public Double EvaporacionAcumuladaDia5
		{
			get {
				return evaporacionAcumuladaDia5;
			}
			set {
				this.evaporacionAcumuladaDia5 = value;
			}
		}

		public override string ToString ()
		{
			return string.Format ("[Fila: {0} \t {1} \t {2} \t {3} \t {4} \t {5} \t {6} \t {7} \t {8} \t {9} \t {10} \t {11} \t {12} \t {13} \t {14} \t {15} \t {16} \t {17}]", Fecha, VelocidadViento, DireccionViento, Temperatura, HumedadRelativa, Mp10, RadiacionSolar, PrecionAtmosferica, PrecipitacionAcumuladaDia1, PrecipitacionAcumuladaDia2, PrecipitacionAcumuladaDia3, PrecipitacionAcumuladaDia4, PrecipitacionAcumuladaDia5, EvaporacionAcumuladaDia1, EvaporacionAcumuladaDia2, EvaporacionAcumuladaDia3, EvaporacionAcumuladaDia4, EvaporacionAcumuladaDia5);
		}
	}
}