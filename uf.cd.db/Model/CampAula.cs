using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampAula
{
    public string CaulIdAula { get; set; } = null!;

    public string CaulEdificio { get; set; } = null!;

    public string? CaulDireccion { get; set; }
}
