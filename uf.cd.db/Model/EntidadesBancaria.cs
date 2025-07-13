using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class EntidadesBancaria
{
    public int Id { get; set; }

    public string? Codigo { get; set; }

    public string? Entidad { get; set; }

    public bool? Tipo { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedAt { get; set; }
}
