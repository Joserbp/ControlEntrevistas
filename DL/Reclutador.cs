using System;
using System.Collections.Generic;

namespace DL;

public partial class Reclutador
{
    public int IdReclutador { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();
}
