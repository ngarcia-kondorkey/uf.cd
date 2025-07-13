using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class Parametro
{
    public int ParaEmpre { get; set; }

    public string ParaClave { get; set; } = null!;

    public string? ParaAtributo { get; set; }

    public int? ParaValorN { get; set; }

    public string? ParaValorC { get; set; }

    public string? ParaDescripcion { get; set; }

    public string? ParaHabilitado { get; set; }

    public DateOnly? ParaFchAlta { get; set; }

    public string? ParaUsrAlta { get; set; }

    public DateOnly? ParaFchModi { get; set; }

    public string? ParaUsrModi { get; set; }

    public string? ParaTexto { get; set; }
}
