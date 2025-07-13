using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampDeudaPrev
{
    public int IdFactura { get; set; }

    public string? Rterc { get; set; }

    public string? Eterc { get; set; }

    public string? IdGuarani { get; set; }

    public string? Clientesap { get; set; }

    public string? Emp { get; set; }

    public DateTime? FechaFactura { get; set; }

    public string? TipoCbt { get; set; }

    public string? NroFactura { get; set; }

    public string? Leyenda { get; set; }

    public decimal? Importe { get; set; }

    public DateTime? FechaVencimiento1 { get; set; }

    public DateTime? FechaVencimiento2 { get; set; }

    public decimal? Recargo { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaProcesado { get; set; }

    public decimal? ImportePagado { get; set; }

    public DateTime? FechaPago { get; set; }

    public string? Opid { get; set; }
}
