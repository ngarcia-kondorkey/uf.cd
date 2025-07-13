using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampEncuestasPeriodo
{
    public int Id { get; set; }

    public DateTime Desde { get; set; }

    public DateTime Hasta { get; set; }

    public string Estado { get; set; } = null!;
}
