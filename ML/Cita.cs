using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Cita
    {
        public int IdCita { get; set; }
        public DateTime Fecha { get; set; }
        public ML.Candidato Candidato { get; set; }
        public ML.Reclutador Reclutador { get; set; }
        public ML.Status Status { get; set; }
        public string NombreCandidato { get; set; }
        public string NombreReclutador { get; set; }
    }
}
