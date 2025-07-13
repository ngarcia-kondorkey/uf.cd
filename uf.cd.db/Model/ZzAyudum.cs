using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class ZzAyudum
{
    public int ZhlpId { get; set; }

    public string UhlpEnlace { get; set; } = null!;

    public string UhlpTitulo { get; set; } = null!;

    public string UhlpTexto { get; set; } = null!;

    public string? UhlpRelac { get; set; }

    public string? UhlpLocImagen { get; set; }

    public string? UhlpImgAncho { get; set; }

    public DateTime UhlpAltaF { get; set; }

    public DateTime? UhlpBajaF { get; set; }
}
