using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampEncuestasPadresNov
{
    public string CepaIdAlumno { get; set; } = null!;

    public string CepaPadreMadre { get; set; } = null!;

    public int CepaIdNivelinstruccion { get; set; }

    public string CepaApellido { get; set; } = null!;

    public string CepaNombre { get; set; } = null!;

    public string? CepaDomicilio { get; set; }

    public string? CepaCodigopostal { get; set; }

    public int? CepaIdLocalidad { get; set; }

    public int? CepaIdProvincia { get; set; }

    public int? CepaIdPais { get; set; }

    public string? CepaTe { get; set; }

    public string? CepaCelular { get; set; }

    public string? CepaEmail { get; set; }
}
