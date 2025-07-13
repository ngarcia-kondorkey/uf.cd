using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class TotalVisitor
{
    public int Id { get; set; }

    public int NVisit { get; set; }

    public DateTime DateVisit { get; set; }

    public string? User { get; set; }
}
