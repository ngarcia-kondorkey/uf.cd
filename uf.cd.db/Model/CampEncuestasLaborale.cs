using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampEncuestasLaborale
{
    public int ElId { get; set; }

    public string ElTexto { get; set; } = null!;

    public string ElIdsso { get; set; } = null!;
}
