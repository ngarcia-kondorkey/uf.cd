using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampCiclo
{
    public int? CcicIdCarrera { get; set; }

    public int? CcicIdCurso { get; set; }

    public int? CcicIdCiclo { get; set; }

    public int? CcicIdMateria { get; set; }

    public string? CcicDescCiclo { get; set; }

    public string? CcicDescMateria { get; set; }
}
