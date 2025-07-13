using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampCarrera
{
    public int CcaaIdCarrera { get; set; }

    public string CcaaCarrera { get; set; } = null!;

    public string? CcaaClCarrera { get; set; }
}
