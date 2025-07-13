using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class UsuRole
{
    public string UsroRol { get; set; } = null!;

    public string? UsroObservaciones { get; set; }

    public string? UsroHabilitado { get; set; }

    public DateOnly? UsroFchAlta { get; set; }

    public string? UsroUsrAlta { get; set; }

    public DateOnly? UsroFchModi { get; set; }

    public string? UsroUsrModi { get; set; }

    public string? UsroPermPdf { get; set; }

    public string? UsroPermExcel { get; set; }

    public string? UsroPermHtml { get; set; }

    public string? UsroPermModi { get; set; }
}
