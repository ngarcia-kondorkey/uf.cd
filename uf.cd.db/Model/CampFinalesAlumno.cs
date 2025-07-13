using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampFinalesAlumno
{
    public string? CfalIdAlumno { get; set; }

    public int? CfalIdCarrera { get; set; }

    public int? CfalLu { get; set; }

    public int? CfalIdMateria { get; set; }
}
