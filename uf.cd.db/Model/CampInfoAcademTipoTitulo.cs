using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampInfoAcademTipoTitulo
{
    public int CittIdTipoTitulo { get; set; }

    public string CittTipoTitulo { get; set; } = null!;

    public int? CittOrden { get; set; }
}
