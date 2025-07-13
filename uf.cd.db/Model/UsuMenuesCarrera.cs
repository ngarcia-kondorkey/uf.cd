using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class UsuMenuesCarrera
{
    public int CcaaIdCarrera { get; set; }

    public bool? MActividadEconom { get; set; }

    public bool? MCronoActi { get; set; }

    public bool? MCronoExam { get; set; }

    public bool? MCronoEncues { get; set; }

    public bool? MDatosPerso { get; set; }

    public bool? MDocAsubir { get; set; }

    public bool? MInfoProfPrevia { get; set; }

    public bool? MInfoPagos { get; set; }

    public bool? MInscriAsig { get; set; }
}
