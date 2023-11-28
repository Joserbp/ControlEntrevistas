using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class CitaAsistencia
    {
        public int IdCitaAsistencia { get; set; } 
        public Cita Cita { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
    }
}
