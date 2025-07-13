using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class UsuAplicacione
{
    public string UsapApli { get; set; } = null!;

    public string? UsapHabilitado { get; set; }

    public DateOnly? UsapFchAlta { get; set; }

    public string? UsapUsrAlta { get; set; }

    public DateOnly? UsapFchModi { get; set; }

    public string? UsapUsrModi { get; set; }
}
