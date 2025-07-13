using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampPagosLog
{
    public int Id { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Alumno { get; set; }

    public string? Factura { get; set; }

    public string? Monto { get; set; }

    public string? ModoPago { get; set; }

    public string? Estado { get; set; }

    public string? Variablex { get; set; }

    public string? NumRefMp { get; set; }
}
