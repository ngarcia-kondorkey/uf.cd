using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampExamenNov
{
    public string CexnIdAlumno { get; set; } = null!;

    public int CexnIdExamen { get; set; }

    public string CexnParcialFinal { get; set; } = null!;

    public DateOnly CexnFecha { get; set; }

    public string CexnSiNo { get; set; } = null!;

    public string CexnUsuario { get; set; } = null!;

    public DateOnly? CexnFechaconfirma { get; set; }
}
