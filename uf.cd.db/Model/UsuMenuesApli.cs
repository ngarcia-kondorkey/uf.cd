using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class UsuMenuesApli
{
    public string UsmaUsapApli { get; set; } = null!;

    public string UsmaItem { get; set; } = null!;

    public string UsmaNivel1 { get; set; } = null!;

    public string? UsmaNivel2 { get; set; }

    public string? UsmaDesItem { get; set; }

    public string? UsmaEnlace { get; set; }

    public string? UsmaHabilitado { get; set; }

    public DateOnly? UsmaFchAlta { get; set; }

    public string? UsmaUsrAlta { get; set; }

    public DateOnly? UsmaFchModi { get; set; }

    public string? UsmaUsrModi { get; set; }

    public short? UsmaOrden { get; set; }

    public string? UsmaIcono { get; set; }
}
