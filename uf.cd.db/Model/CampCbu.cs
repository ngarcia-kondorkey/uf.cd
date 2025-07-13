using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampCbu
{
    public int Idcarrera { get; set; }

    public int Lu { get; set; }

    public string? Idsap { get; set; }

    public string? Idguarani { get; set; }

    public string Idalumno { get; set; } = null!;

    public string? Apellido { get; set; }

    public string? Nombres { get; set; }

    public string? Tipodocumento { get; set; }

    public string? Nrodocumento { get; set; }

    public string? Cuit { get; set; }

    public string? Nacionalidad { get; set; }

    public string? Actividadacademica { get; set; }

    public string? Direccion { get; set; }

    public string? Localidad { get; set; }

    public string? Provincia { get; set; }

    public string? PostalCp { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public string? Telefono { get; set; }

    public string? Celular { get; set; }

    public string? Cbu { get; set; }

    public string? NroCuenta { get; set; }

    public string? Banco { get; set; }

    public DateTime? FechaAlta { get; set; }

    public DateTime? FechaBaja { get; set; }

    public string? Email { get; set; }

    public string? Acepta { get; set; }

    public string? ClCarrera { get; set; }
}
