using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampCursoAlumno
{
    public string CualIdAlumno { get; set; } = null!;

    public int CualIdCarrera { get; set; }

    public int CualLu { get; set; }

    public string CualClMateria { get; set; } = null!;

    public string CualAsignatura { get; set; } = null!;

    public int CualPl { get; set; }

    public int CualEstado { get; set; }

    public int? CualIdMateria { get; set; }

    public int? CualIdCurso { get; set; }

    public int? CualIdCiclo { get; set; }

    public int? CualIdComision { get; set; }
}
