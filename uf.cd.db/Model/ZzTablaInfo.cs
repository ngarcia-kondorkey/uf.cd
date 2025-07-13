using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class ZzTablaInfo
{
    public string ZtaiColumn { get; set; } = null!;

    public string ZtaiTable { get; set; } = null!;

    public string ZtaiType { get; set; } = null!;

    public int? ZtaiLength { get; set; }

    public string ZtaiNull { get; set; } = null!;

    public string ZtaiLabel { get; set; } = null!;

    public string ZtaiMensaje { get; set; } = null!;

    public DateOnly ZtaiAltaF { get; set; }

    public DateOnly? ZtaiBajaF { get; set; }
}
