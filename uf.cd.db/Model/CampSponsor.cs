using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampSponsor
{
    public int IdSponsor { get; set; }

    public string? Rterc { get; set; }

    public string? Eterc { get; set; }

    public string? Clientesap { get; set; }

    public string? Email { get; set; }

    public int? TipoDoc { get; set; }

    public string? NumDoc { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? TelefonoFijo { get; set; }

    public string? TelefonoMovil { get; set; }

    public string? Pais { get; set; }

    public string? Provincia { get; set; }

    public string? Localidad { get; set; }

    public string? Domicilio { get; set; }

    public string? CodigoPostal { get; set; }
}
