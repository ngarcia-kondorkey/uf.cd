using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampMateriaRegular
{
    public string CmarIdAlumno { get; set; } = null!;

    public int CmarIdCarrera { get; set; }

    public int CmarLu { get; set; }

    public string CmarClMateria { get; set; } = null!;

    public string CmarAsignatura { get; set; } = null!;

    public int? CmarPlRegular { get; set; }

    public int? CmarEstado { get; set; }

    public int? CmarExamenesRendidos { get; set; }

    public int? CmarIdMateria { get; set; }
}
