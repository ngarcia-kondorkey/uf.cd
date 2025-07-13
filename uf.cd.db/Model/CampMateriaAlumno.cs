using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampMateriaAlumno
{
    public string CmalIdAlumno { get; set; } = null!;

    public int CmalIdCarrera { get; set; }

    public int CmalLu { get; set; }

    public int CmalIdMateria { get; set; }

    public string CmalAsignatura { get; set; } = null!;

    public DateOnly? CmalFecha { get; set; }

    public float? CmalCalificacion { get; set; }

    public string? CmalResultado { get; set; }

    public string? CmalClMateria { get; set; }
}
