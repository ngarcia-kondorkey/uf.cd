using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampCartelera
{
    public int CcarIdCarrera { get; set; }

    public int CcarIdMateria { get; set; }

    public DateTime CcarFecha { get; set; }

    public string CcarCohorte { get; set; } = null!;

    public string CcarAsignatura { get; set; } = null!;

    public string? CcarComision { get; set; }

    public string? CcarHoraInicio { get; set; }

    public string? CcarHoraFin { get; set; }

    public string? CcarAula { get; set; }

    public string? CcarDetalle { get; set; }

    public string? CcarDocentes { get; set; }
}
