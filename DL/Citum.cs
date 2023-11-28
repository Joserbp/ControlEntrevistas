using System;
using System.Collections.Generic;

namespace DL;

public partial class Citum
{
    public int IdCita { get; set; }

    public DateTime Fecha { get; set; }

    public string? IdCandidato { get; set; }

    public int? IdReclutador { get; set; }

    public int? IdStatus { get; set; }

    public string? NombreCandidato { get; set; }

    public string? NombreReclutador { get; set; }

    public virtual ICollection<CitaAsistencium> CitaAsistencia { get; set; } = new List<CitaAsistencium>();

    public virtual Candidato? IdCandidatoNavigation { get; set; }

    public virtual Reclutador? IdReclutadorNavigation { get; set; }

    public virtual Status? IdStatusNavigation { get; set; }
}
