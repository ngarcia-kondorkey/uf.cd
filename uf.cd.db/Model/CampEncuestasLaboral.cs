using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampEncuestasLaboral
{
    public int Id { get; set; }

    public string? CelIdAlumno { get; set; }

    public int? CelIdCarrera { get; set; }

    public int? CelCaluLu { get; set; }

    public string? CelAno { get; set; }

    public string CelTexto { get; set; } = null!;

    public short? Estado { get; set; }
}
