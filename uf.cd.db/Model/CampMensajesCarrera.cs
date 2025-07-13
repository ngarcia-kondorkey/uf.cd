using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampMensajesCarrera
{
    public int CmecIdMensaje { get; set; }

    public int CmecIdCarrera { get; set; }

    public int? CmecIdPl { get; set; }

    public int? CmecIdMateria { get; set; }

    public int? CmecIdCurso { get; set; }

    public int? CmecIdCiclo { get; set; }

    public DateTime? CmecFechaDesde { get; set; }

    public DateTime? CmecFechaHasta { get; set; }

    public string? CmecTexto { get; set; }
}
