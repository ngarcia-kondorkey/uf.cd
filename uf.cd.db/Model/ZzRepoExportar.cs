using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class ZzRepoExportar
{
    public int Id { get; set; }

    public string Reporte { get; set; } = null!;

    public short Orden { get; set; }

    public string Col { get; set; } = null!;

    public string Lab { get; set; } = null!;

    public string Ancho { get; set; } = null!;

    public string Alineacion { get; set; } = null!;
}
