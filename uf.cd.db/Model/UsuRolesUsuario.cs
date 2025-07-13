using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class UsuRolesUsuario
{
    public string UsruUsroRol { get; set; } = null!;

    public string UsruUsuaNombre { get; set; } = null!;

    public string? UsruHabilitado { get; set; }

    public DateOnly? UsruFchAlta { get; set; }

    public string? UsruUsrAlta { get; set; }

    public DateOnly? UsruFchModi { get; set; }

    public string? UsruUsrModi { get; set; }
}
