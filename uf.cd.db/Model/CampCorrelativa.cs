using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampCorrelativa
{
    public int? CcorIdCarrera { get; set; }

    public int? CcorIdMateria { get; set; }

    public int? CcorIdPlan { get; set; }

    public int? CcorIdMateriaCorrelativa { get; set; }

    public string? CcorCondicion { get; set; }

    public string? CcorClMateriaCorrelativa { get; set; }

    public string? CcorAsignaturaCorrelativa { get; set; }
}
