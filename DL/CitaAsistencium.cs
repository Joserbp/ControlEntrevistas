using System;
using System.Collections.Generic;

namespace DL;

public partial class CitaAsistencium
{
    public int IdCitaAsistencia { get; set; }

    public int IdCita { get; set; }

    public DateTime Fecha { get; set; }

    public string? Observaciones { get; set; }

    public virtual Citum IdCitaNavigation { get; set; } = null!;
}
