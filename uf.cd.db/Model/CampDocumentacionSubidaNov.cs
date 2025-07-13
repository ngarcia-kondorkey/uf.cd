using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampDocumentacionSubidaNov
{
    public string CdsuIdAlumno { get; set; } = null!;

    public int CdsuId { get; set; }

    public int CdsuIdTipoadjunto { get; set; }

    public DateOnly CdsuFechasubida { get; set; }

    public string? CdsuNota { get; set; }

    public byte[]? CdsuDoc { get; set; }

    public string CdsuNombre { get; set; } = null!;

    public string CdsuTipo { get; set; } = null!;

    public DateOnly? CdsuFecharecibida { get; set; }
}
