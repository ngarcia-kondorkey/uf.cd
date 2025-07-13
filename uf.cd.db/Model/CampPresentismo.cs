using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampPresentismo
{
    public string CpreIdAlumno { get; set; } = null!;

    public int CpreIdCarrera { get; set; }

    public int CpreLu { get; set; }

    public string CpreAsignatura { get; set; } = null!;

    public string CpreCiclo { get; set; } = null!;

    public int CprePresentes { get; set; }

    public int CpreAusentes { get; set; }

    public int CpreJustificadas { get; set; }

    public int CpreAusentesPermitidos { get; set; }

    public int CpreDatosNoValidados { get; set; }

    public DateTime? CpreFecha { get; set; }

    public int? CpreClases { get; set; }
}
