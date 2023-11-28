using System;
using System.Collections.Generic;

namespace DL;

public partial class Vacante
{
    public int IdVacante { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdEmpresa { get; set; }

    public virtual ICollection<Candidato> Candidatos { get; set; } = new List<Candidato>();

    public virtual Empresa? IdEmpresaNavigation { get; set; }
}
