using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class DayVisitor
{
    public int Id { get; set; }

    public int NVisit { get; set; }

    public DateTime? DateVisit { get; set; }
}
