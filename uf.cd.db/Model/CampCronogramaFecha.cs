using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampCronogramaFecha
{
    public string CcroTipoExamen { get; set; } = null!;

    public int CcroIdExamen { get; set; }

    public int CcroIdCarrera { get; set; }

    public string CcroCarrera { get; set; } = null!;

    public string CcroAsignatura { get; set; } = null!;

    public DateTime? CcroFecha { get; set; }

    public string CcroExamen { get; set; } = null!;

    public string? CcroCiclo { get; set; }

    public int? CcroComision { get; set; }

    public int? CcroIdMateria { get; set; }

    public string? CcroRecuperatorio { get; set; }
}
