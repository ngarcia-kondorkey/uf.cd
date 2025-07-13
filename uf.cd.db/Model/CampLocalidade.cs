using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampLocalidade
{
    public int ClocIdPais { get; set; }

    public int ClocIdProvincia { get; set; }

    public int ClocIdLocalidad { get; set; }

    public string ClocLocalidad { get; set; } = null!;
}
