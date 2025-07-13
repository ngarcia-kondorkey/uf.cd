using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampProvincia
{
    public int CprvIdPais { get; set; }

    public int CprvIdProvincia { get; set; }

    public string CprvProvincia { get; set; } = null!;

    public string? CprvIdProvincia2 { get; set; }
}
