using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesNeuronalesArtificiales.AnalisisDeRNA
{
    class Grupo
    {
        public int numeroActivacion;
        public double[] vector;
        public double media;
        public double desviacionEstandar;
        public double nivelPertenencia;
        public string clase ="SinEtiqueta";//tipo de clase en la SOM
        public bool etiquetada = false;
    }
}
