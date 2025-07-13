using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class Admisione
{
    public int Id { get; set; }

    public string Dni { get; set; } = null!;

    public string? Url { get; set; }
}
