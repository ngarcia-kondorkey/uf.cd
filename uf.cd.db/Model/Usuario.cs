using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class Usuario
{
    public string UsuaNombre { get; set; } = null!;

    public string UsuaPwd { get; set; } = null!;

    public string? UsuaNota { get; set; }

    public string UsuaHabilitado { get; set; } = null!;

    public DateOnly UsuaFchAlta { get; set; }

    public string UsuaUsrAlta { get; set; } = null!;

    public DateOnly? UsuaFchModi { get; set; }

    public string? UsuaUsrModi { get; set; }

    public int? UsuaFilasPag { get; set; }

    public int? UsuaClieId { get; set; }

    public string? UsuaPwd2 { get; set; }

    public string? UsuaSha1 { get; set; }
}
