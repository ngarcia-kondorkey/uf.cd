using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampParcialNota
{
    public int CpnoIdExamen { get; set; }

    public int CpnoLu { get; set; }

    public int CpnoIdCarrera { get; set; }

    public string CpnoCarrera { get; set; } = null!;

    public string CpnoAsignatura { get; set; } = null!;

    public DateTime? CpnoFecha { get; set; }

    public float? CpnoNota { get; set; }

    public string CpnoExamen { get; set; } = null!;

    public string CpnoEstado { get; set; } = null!;

    public int? CpnoIdMateria { get; set; }

    public int? CpnoIdCurso { get; set; }

    public int? CpnoIdCiclo { get; set; }

    public string? CpnoResultado { get; set; }
}
