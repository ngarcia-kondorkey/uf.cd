using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampInfoProfesionalPreviaNov
{
    public string CipnIdAlumno { get; set; } = null!;

    public int CipnIdCarrera { get; set; }

    public short CipnNroProfesion { get; set; }

    public string? CipnEntidad { get; set; }

    public string? CipnLugar { get; set; }

    public string? CipnNroMatricula { get; set; }

    public DateOnly? CipnFechavto { get; set; }

    public int? CipnIdPais { get; set; }

    public int? CipnIdProvincia { get; set; }
}
