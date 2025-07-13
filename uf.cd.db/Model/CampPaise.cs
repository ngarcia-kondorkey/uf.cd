using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampPaise
{
    public int CpaiIdPais { get; set; }

    public string CpaiPais { get; set; } = null!;

    public string? Iso31661 { get; set; }
}
