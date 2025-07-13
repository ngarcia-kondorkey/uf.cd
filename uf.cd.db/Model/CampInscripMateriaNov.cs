using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampInscripMateriaNov
{
    public string CimnIdAlumno { get; set; } = null!;

    public int CimnIdMateria { get; set; }

    public DateOnly CimnFecha { get; set; }

    public string CimnUsuario { get; set; } = null!;

    public DateOnly? CimnFechaconfirma { get; set; }

    public int? Estado { get; set; }
}
