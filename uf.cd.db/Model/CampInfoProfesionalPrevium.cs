using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampInfoProfesionalPrevium
{
    public string CippIdAlumno { get; set; } = null!;

    public int CippIdCarrera { get; set; }

    public short CippNroProfesion { get; set; }

    public string? CippEntidad { get; set; }

    public string? CippLugar { get; set; }

    public string? CippNroMatricula { get; set; }

    public DateOnly? CippFechavto { get; set; }

    public int? CippIdPais { get; set; }

    public int? CippIdProvincia { get; set; }

    public string? CippOrigenDato { get; set; }
}
