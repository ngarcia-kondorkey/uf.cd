using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class UsuMenuesRol
{
    public string UsmrUsapApli { get; set; } = null!;

    public string UsmrUsroRol { get; set; } = null!;

    public string UsmrItem { get; set; } = null!;

    public string? UsmrAlta { get; set; }

    public string? UsmrBaja { get; set; }

    public string? UsmrModif { get; set; }

    public string? UsmrHabilitado { get; set; }

    public DateOnly? UsmrFchAlta { get; set; }

    public string? UsmrUsrAlta { get; set; }

    public DateOnly? UsmrFchModi { get; set; }

    public string? UsmrUsrModi { get; set; }
}
