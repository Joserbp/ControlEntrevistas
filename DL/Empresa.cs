using System;
using System.Collections.Generic;

namespace DL;

public partial class Empresa
{
    public int IdEmpresa { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Vacante> Vacantes { get; set; } = new List<Vacante>();
}
