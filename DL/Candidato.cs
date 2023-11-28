using System;
using System.Collections.Generic;

namespace DL;

public partial class Candidato
{
    public string IdCandidato { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public string Correo { get; set; } = null!;

    public string Celular { get; set; } = null!;

    public int? IdVacante { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual Vacante? IdVacanteNavigation { get; set; }
}
