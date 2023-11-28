using System;
using System.Collections.Generic;

namespace DL;

public partial class Status
{
    public int IdStatus { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();
}
