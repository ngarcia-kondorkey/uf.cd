using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampInfoAcademNivele
{
    public int CialIdNivel { get; set; }

    public string CialNivel { get; set; } = null!;

    public int? CialOrden { get; set; }
}
