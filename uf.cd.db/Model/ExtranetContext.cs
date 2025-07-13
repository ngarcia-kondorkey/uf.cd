using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace uf.cd.db.Model;

public partial class ExtranetContext : DbContext
{
    public ExtranetContext()
    {
    }

    public ExtranetContext(DbContextOptions<ExtranetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admisione> Admisiones { get; set; }

    public virtual DbSet<CampAlumno> CampAlumnos { get; set; }

    public virtual DbSet<CampAlumnoMaterium> CampAlumnoMateria { get; set; }

    public virtual DbSet<CampAlumnosNov> CampAlumnosNovs { get; set; }

    public virtual DbSet<CampAlumnosNovLog> CampAlumnosNovLogs { get; set; }

    public virtual DbSet<CampAula> CampAulas { get; set; }

    public virtual DbSet<CampAusencia> CampAusencias { get; set; }

    public virtual DbSet<CampCarrera> CampCarreras { get; set; }

    public virtual DbSet<CampCartelera> CampCarteleras { get; set; }

    public virtual DbSet<CampCbu> CampCbus { get; set; }

    public virtual DbSet<CampCiclo> CampCiclos { get; set; }

    public virtual DbSet<CampCorrelativa> CampCorrelativas { get; set; }

    public virtual DbSet<CampCronogramaFecha> CampCronogramaFechas { get; set; }

    public virtual DbSet<CampCursoAlumno> CampCursoAlumnos { get; set; }

    public virtual DbSet<CampDeudaAux> CampDeudaAuxes { get; set; }

    public virtual DbSet<CampDeudaHist> CampDeudaHists { get; set; }

    public virtual DbSet<CampDeudaPrev> CampDeudaPrevs { get; set; }

    public virtual DbSet<CampDeudum> CampDeuda { get; set; }

    public virtual DbSet<CampDocumenTipoASubir> CampDocumenTipoASubirs { get; set; }

    public virtual DbSet<CampDocumentacionSubidaNov> CampDocumentacionSubidaNovs { get; set; }

    public virtual DbSet<CampEncuestasEstudioItem> CampEncuestasEstudioItems { get; set; }

    public virtual DbSet<CampEncuestasLaboral> CampEncuestasLaborals { get; set; }

    public virtual DbSet<CampEncuestasLaborale> CampEncuestasLaborales { get; set; }

    public virtual DbSet<CampEncuestasPadresNov> CampEncuestasPadresNovs { get; set; }

    public virtual DbSet<CampEncuestasPeriodo> CampEncuestasPeriodos { get; set; }

    public virtual DbSet<CampEstadoAcademico> CampEstadoAcademicos { get; set; }

    public virtual DbSet<CampEstadoAdministra> CampEstadoAdministras { get; set; }

    public virtual DbSet<CampEstadoAsignatura> CampEstadoAsignaturas { get; set; }

    public virtual DbSet<CampExamenNov> CampExamenNovs { get; set; }

    public virtual DbSet<CampFinalesAlumno> CampFinalesAlumnos { get; set; }

    public virtual DbSet<CampFinalesFecha> CampFinalesFechas { get; set; }

    public virtual DbSet<CampFinalesIncripcion> CampFinalesIncripcions { get; set; }

    public virtual DbSet<CampInfoAcademNivele> CampInfoAcademNiveles { get; set; }

    public virtual DbSet<CampInfoAcademTipoTitulo> CampInfoAcademTipoTitulos { get; set; }

    public virtual DbSet<CampInfoAcademicaPreviaNov> CampInfoAcademicaPreviaNovs { get; set; }

    public virtual DbSet<CampInfoAcademicaPrevium> CampInfoAcademicaPrevia { get; set; }

    public virtual DbSet<CampInfoProfesionalPreviaNov> CampInfoProfesionalPreviaNovs { get; set; }

    public virtual DbSet<CampInfoProfesionalPrevium> CampInfoProfesionalPrevia { get; set; }

    public virtual DbSet<CampInscripMateriaNov> CampInscripMateriaNovs { get; set; }

    public virtual DbSet<CampLocalidade> CampLocalidades { get; set; }

    public virtual DbSet<CampMateriaAlumno> CampMateriaAlumnos { get; set; }

    public virtual DbSet<CampMateriaRegular> CampMateriaRegulars { get; set; }

    public virtual DbSet<CampMateriasACursar> CampMateriasACursars { get; set; }

    public virtual DbSet<CampMensajesCarrera> CampMensajesCarreras { get; set; }

    public virtual DbSet<CampNacionalidade> CampNacionalidades { get; set; }

    public virtual DbSet<CampOtro> CampOtros { get; set; }

    public virtual DbSet<CampPagosLog> CampPagosLogs { get; set; }

    public virtual DbSet<CampPagosLogSponsor> CampPagosLogSponsors { get; set; }

    public virtual DbSet<CampPaise> CampPaises { get; set; }

    public virtual DbSet<CampParcialNota> CampParcialNotas { get; set; }

    public virtual DbSet<CampPlanAlumno> CampPlanAlumnos { get; set; }

    public virtual DbSet<CampPresentismo> CampPresentismos { get; set; }

    public virtual DbSet<CampProvincia> CampProvincias { get; set; }

    public virtual DbSet<CampSaldo> CampSaldos { get; set; }

    public virtual DbSet<CampSexo> CampSexos { get; set; }

    public virtual DbSet<CampSponsor> CampSponsors { get; set; }

    public virtual DbSet<CampTipodoc> CampTipodocs { get; set; }

    public virtual DbSet<CompensacionesAgosto> CompensacionesAgostos { get; set; }

    public virtual DbSet<DayVisitor> DayVisitors { get; set; }

    public virtual DbSet<EntidadesBancaria> EntidadesBancarias { get; set; }

    public virtual DbSet<Parametro> Parametros { get; set; }

    public virtual DbSet<TotalVisitor> TotalVisitors { get; set; }

    public virtual DbSet<UsuAccesosLog> UsuAccesosLogs { get; set; }

    public virtual DbSet<UsuAplicacione> UsuAplicaciones { get; set; }

    public virtual DbSet<UsuIncidentesLog> UsuIncidentesLogs { get; set; }

    public virtual DbSet<UsuMenuesApli> UsuMenuesAplis { get; set; }

    public virtual DbSet<UsuMenuesCarrera> UsuMenuesCarreras { get; set; }

    public virtual DbSet<UsuMenuesRol> UsuMenuesRols { get; set; }

    public virtual DbSet<UsuRole> UsuRoles { get; set; }

    public virtual DbSet<UsuRolesUsuario> UsuRolesUsuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<ZzAyudum> ZzAyuda { get; set; }

    public virtual DbSet<ZzRepoExportar> ZzRepoExportars { get; set; }

    public virtual DbSet<ZzSistema> ZzSistemas { get; set; }

    public virtual DbSet<ZzTablaInfo> ZzTablaInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=10.150.2.238;database=extranet;uid=root;pwd=Cambio12", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.1.61-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Admisione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("admisiones");

            entity.HasIndex(e => e.Dni, "fk_dni");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Dni)
                .HasMaxLength(50)
                .HasColumnName("dni");
            entity.Property(e => e.Url)
                .HasMaxLength(500)
                .HasColumnName("url");
        });

        modelBuilder.Entity<CampAlumno>(entity =>
        {
            entity.HasKey(e => new { e.CaluIdAlumno, e.CaluIdCarrera })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("camp_alumnos")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.CaluClientesap, "ClienteSap");

            entity.Property(e => e.CaluIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("calu_id_alumno");
            entity.Property(e => e.CaluIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("calu_id_carrera");
            entity.Property(e => e.CaluAnioinicio)
                .HasColumnType("int(11)")
                .HasColumnName("calu_anioinicio");
            entity.Property(e => e.CaluApellido)
                .HasMaxLength(50)
                .HasColumnName("calu_apellido");
            entity.Property(e => e.CaluCalle)
                .HasMaxLength(100)
                .HasColumnName("calu_calle");
            entity.Property(e => e.CaluCallePro)
                .HasMaxLength(100)
                .HasColumnName("calu_calle_pro");
            entity.Property(e => e.CaluCelular)
                .HasMaxLength(50)
                .HasColumnName("calu_celular");
            entity.Property(e => e.CaluClientesap)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("calu_clientesap");
            entity.Property(e => e.CaluCodigopostal)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("calu_codigopostal");
            entity.Property(e => e.CaluCodigopostalPro)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("calu_codigopostal_pro");
            entity.Property(e => e.CaluCohorte)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("calu_cohorte");
            entity.Property(e => e.CaluCuil)
                .HasMaxLength(15)
                .HasColumnName("calu_cuil");
            entity.Property(e => e.CaluDistritonacimiento)
                .HasMaxLength(100)
                .HasColumnName("calu_distritonacimiento");
            entity.Property(e => e.CaluDto)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("calu_dto");
            entity.Property(e => e.CaluDtoPro)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("calu_dto_pro");
            entity.Property(e => e.CaluEmail)
                .HasMaxLength(300)
                .HasColumnName("calu_email");
            entity.Property(e => e.CaluEmail2)
                .HasMaxLength(300)
                .HasColumnName("calu_email2");
            entity.Property(e => e.CaluEsAlumno)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("calu_es_alumno");
            entity.Property(e => e.CaluEterc)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("calu_eterc");
            entity.Property(e => e.CaluExtranjero)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("calu_extranjero");
            entity.Property(e => e.CaluFechabaja).HasColumnName("calu_fechabaja");
            entity.Property(e => e.CaluFechagraduacion).HasColumnName("calu_fechagraduacion");
            entity.Property(e => e.CaluFechanac).HasColumnName("calu_fechanac");
            entity.Property(e => e.CaluIdEstadoacademico)
                .HasColumnType("int(11)")
                .HasColumnName("calu_id_estadoacademico");
            entity.Property(e => e.CaluIdEstadoadministrativo)
                .HasColumnType("int(11)")
                .HasColumnName("calu_id_estadoadministrativo");
            entity.Property(e => e.CaluIdLocalidad)
                .HasColumnType("int(11)")
                .HasColumnName("calu_id_localidad");
            entity.Property(e => e.CaluIdLocalidadPro)
                .HasColumnType("int(11)")
                .HasColumnName("calu_id_localidad_pro");
            entity.Property(e => e.CaluIdNacionalidad)
                .HasColumnType("int(11)")
                .HasColumnName("calu_id_nacionalidad");
            entity.Property(e => e.CaluIdPais)
                .HasColumnType("int(11)")
                .HasColumnName("calu_id_pais");
            entity.Property(e => e.CaluIdPaisPro)
                .HasColumnType("int(11)")
                .HasColumnName("calu_id_pais_pro");
            entity.Property(e => e.CaluIdPaisemision)
                .HasColumnType("int(11)")
                .HasColumnName("calu_id_paisemision");
            entity.Property(e => e.CaluIdPaisnac)
                .HasColumnType("int(11)")
                .HasColumnName("calu_id_paisnac");
            entity.Property(e => e.CaluIdProvincia)
                .HasColumnType("int(11)")
                .HasColumnName("calu_id_provincia");
            entity.Property(e => e.CaluIdProvinciaPro)
                .HasColumnType("int(11)")
                .HasColumnName("calu_id_provincia_pro");
            entity.Property(e => e.CaluIdSexo)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("calu_id_sexo");
            entity.Property(e => e.CaluIdTipodoc)
                .HasColumnType("int(11)")
                .HasColumnName("calu_id_tipodoc");
            entity.Property(e => e.CaluLu)
                .HasColumnType("int(11)")
                .HasColumnName("calu_lu");
            entity.Property(e => e.CaluLugarnacimiento)
                .HasMaxLength(100)
                .HasColumnName("calu_lugarnacimiento");
            entity.Property(e => e.CaluMatricula)
                .HasMaxLength(30)
                .HasColumnName("calu_matricula");
            entity.Property(e => e.CaluNombre)
                .HasMaxLength(50)
                .HasColumnName("calu_nombre");
            entity.Property(e => e.CaluNro)
                .HasMaxLength(5)
                .HasColumnName("calu_nro");
            entity.Property(e => e.CaluNroPro)
                .HasMaxLength(5)
                .HasColumnName("calu_nro_pro");
            entity.Property(e => e.CaluNrodoc)
                .HasMaxLength(20)
                .HasColumnName("calu_nrodoc");
            entity.Property(e => e.CaluPiso)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("calu_piso");
            entity.Property(e => e.CaluPisoPro)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("calu_piso_pro");
            entity.Property(e => e.CaluRterc)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("calu_rterc");
            entity.Property(e => e.CaluTe)
                .HasMaxLength(100)
                .HasColumnName("calu_te");
        });

        modelBuilder.Entity<CampAlumnoMaterium>(entity =>
        {
            entity.HasKey(e => new { e.CamaIdAlumno, e.CamaIdCarrera, e.CamaLu, e.CamaIdMateria })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0 });

            entity
                .ToTable("camp_alumno_materia")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.Property(e => e.CamaIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cama_id_alumno");
            entity.Property(e => e.CamaIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("cama_id_carrera");
            entity.Property(e => e.CamaLu)
                .HasColumnType("int(11)")
                .HasColumnName("cama_lu");
            entity.Property(e => e.CamaIdMateria)
                .HasColumnType("int(4)")
                .HasColumnName("cama_id_materia");
            entity.Property(e => e.CamaFinal)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("cama_final");
            entity.Property(e => e.CamaRegular)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("cama_regular");
        });

        modelBuilder.Entity<CampAlumnosNov>(entity =>
        {
            entity.HasKey(e => new { e.CanvIdAlumno, e.CanvIdCarrera })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("camp_alumnos_nov")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CanvIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("canv_id_alumno");
            entity.Property(e => e.CanvIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_carrera");
            entity.Property(e => e.CanvAnioinicio)
                .HasColumnType("int(11)")
                .HasColumnName("canv_anioinicio");
            entity.Property(e => e.CanvApellido)
                .HasMaxLength(50)
                .HasColumnName("canv_apellido");
            entity.Property(e => e.CanvCalle)
                .HasMaxLength(100)
                .HasColumnName("canv_calle");
            entity.Property(e => e.CanvCallePro)
                .HasMaxLength(100)
                .HasColumnName("canv_calle_pro");
            entity.Property(e => e.CanvCelular)
                .HasMaxLength(50)
                .HasColumnName("canv_celular");
            entity.Property(e => e.CanvCodigopostal)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("canv_codigopostal");
            entity.Property(e => e.CanvCodigopostalPro)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("canv_codigopostal_pro");
            entity.Property(e => e.CanvCuil)
                .HasMaxLength(15)
                .HasColumnName("canv_cuil");
            entity.Property(e => e.CanvDistritonacimiento)
                .HasMaxLength(100)
                .HasColumnName("canv_distritonacimiento");
            entity.Property(e => e.CanvDto)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("canv_dto");
            entity.Property(e => e.CanvDtoPro)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("canv_dto_pro");
            entity.Property(e => e.CanvEmail)
                .HasMaxLength(300)
                .HasColumnName("canv_email");
            entity.Property(e => e.CanvEmail2)
                .HasMaxLength(300)
                .HasColumnName("canv_email2");
            entity.Property(e => e.CanvExtranjero)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("canv_extranjero");
            entity.Property(e => e.CanvFechabaja).HasColumnName("canv_fechabaja");
            entity.Property(e => e.CanvFechagraduacion).HasColumnName("canv_fechagraduacion");
            entity.Property(e => e.CanvFechanac).HasColumnName("canv_fechanac");
            entity.Property(e => e.CanvIdEstadoacademico)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_estadoacademico");
            entity.Property(e => e.CanvIdEstadoadministrativo)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_estadoadministrativo");
            entity.Property(e => e.CanvIdLocalidad)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_localidad");
            entity.Property(e => e.CanvIdLocalidadPro)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_localidad_pro");
            entity.Property(e => e.CanvIdNacionalidad)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_nacionalidad");
            entity.Property(e => e.CanvIdPais)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_pais");
            entity.Property(e => e.CanvIdPaisPro)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_pais_pro");
            entity.Property(e => e.CanvIdPaisemision)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_paisemision");
            entity.Property(e => e.CanvIdPaisnac)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_paisnac");
            entity.Property(e => e.CanvIdProvincia)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_provincia");
            entity.Property(e => e.CanvIdProvinciaPro)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_provincia_pro");
            entity.Property(e => e.CanvIdSexo)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("canv_id_sexo");
            entity.Property(e => e.CanvIdTipodoc)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_tipodoc");
            entity.Property(e => e.CanvLu)
                .HasColumnType("int(11)")
                .HasColumnName("canv_lu");
            entity.Property(e => e.CanvLugarnacimiento)
                .HasMaxLength(100)
                .HasColumnName("canv_lugarnacimiento");
            entity.Property(e => e.CanvMatricula)
                .HasMaxLength(30)
                .HasColumnName("canv_matricula");
            entity.Property(e => e.CanvNombre)
                .HasMaxLength(50)
                .HasColumnName("canv_nombre");
            entity.Property(e => e.CanvNro)
                .HasMaxLength(5)
                .HasColumnName("canv_nro");
            entity.Property(e => e.CanvNroPro)
                .HasMaxLength(5)
                .HasColumnName("canv_nro_pro");
            entity.Property(e => e.CanvNrodoc)
                .HasMaxLength(20)
                .HasColumnName("canv_nrodoc");
            entity.Property(e => e.CanvPiso)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("canv_piso");
            entity.Property(e => e.CanvPisoPro)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("canv_piso_pro");
            entity.Property(e => e.CanvTe)
                .HasMaxLength(100)
                .HasColumnName("canv_te");
            entity.Property(e => e.Estado)
                .HasColumnType("smallint(6)")
                .HasColumnName("estado");
        });

        modelBuilder.Entity<CampAlumnosNovLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_alumnos_nov_log")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CanvAnioinicio)
                .HasColumnType("int(11)")
                .HasColumnName("canv_anioinicio");
            entity.Property(e => e.CanvApellido)
                .HasMaxLength(50)
                .HasColumnName("canv_apellido");
            entity.Property(e => e.CanvCalle)
                .HasMaxLength(100)
                .HasColumnName("canv_calle");
            entity.Property(e => e.CanvCallePro)
                .HasMaxLength(100)
                .HasColumnName("canv_calle_pro");
            entity.Property(e => e.CanvCelular)
                .HasMaxLength(50)
                .HasColumnName("canv_celular");
            entity.Property(e => e.CanvCodigopostal)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("canv_codigopostal");
            entity.Property(e => e.CanvCodigopostalPro)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("canv_codigopostal_pro");
            entity.Property(e => e.CanvCuil)
                .HasMaxLength(15)
                .HasColumnName("canv_cuil");
            entity.Property(e => e.CanvDistritonacimiento)
                .HasMaxLength(100)
                .HasColumnName("canv_distritonacimiento");
            entity.Property(e => e.CanvDto)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("canv_dto");
            entity.Property(e => e.CanvDtoPro)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("canv_dto_pro");
            entity.Property(e => e.CanvEmail)
                .HasMaxLength(300)
                .HasColumnName("canv_email");
            entity.Property(e => e.CanvEmail2)
                .HasMaxLength(300)
                .HasColumnName("canv_email2");
            entity.Property(e => e.CanvExtranjero)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("canv_extranjero");
            entity.Property(e => e.CanvFechabaja).HasColumnName("canv_fechabaja");
            entity.Property(e => e.CanvFechagraduacion).HasColumnName("canv_fechagraduacion");
            entity.Property(e => e.CanvFechanac).HasColumnName("canv_fechanac");
            entity.Property(e => e.CanvIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("canv_id_alumno");
            entity.Property(e => e.CanvIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_carrera");
            entity.Property(e => e.CanvIdEstadoacademico)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_estadoacademico");
            entity.Property(e => e.CanvIdEstadoadministrativo)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_estadoadministrativo");
            entity.Property(e => e.CanvIdLocalidad)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_localidad");
            entity.Property(e => e.CanvIdLocalidadPro)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_localidad_pro");
            entity.Property(e => e.CanvIdNacionalidad)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_nacionalidad");
            entity.Property(e => e.CanvIdPais)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_pais");
            entity.Property(e => e.CanvIdPaisPro)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_pais_pro");
            entity.Property(e => e.CanvIdPaisemision)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_paisemision");
            entity.Property(e => e.CanvIdPaisnac)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_paisnac");
            entity.Property(e => e.CanvIdProvincia)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_provincia");
            entity.Property(e => e.CanvIdProvinciaPro)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_provincia_pro");
            entity.Property(e => e.CanvIdSexo)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("canv_id_sexo");
            entity.Property(e => e.CanvIdTipodoc)
                .HasColumnType("int(11)")
                .HasColumnName("canv_id_tipodoc");
            entity.Property(e => e.CanvLu)
                .HasColumnType("int(11)")
                .HasColumnName("canv_lu");
            entity.Property(e => e.CanvLugarnacimiento)
                .HasMaxLength(100)
                .HasColumnName("canv_lugarnacimiento");
            entity.Property(e => e.CanvMatricula)
                .HasMaxLength(30)
                .HasColumnName("canv_matricula");
            entity.Property(e => e.CanvNombre)
                .HasMaxLength(50)
                .HasColumnName("canv_nombre");
            entity.Property(e => e.CanvNro)
                .HasMaxLength(5)
                .HasColumnName("canv_nro");
            entity.Property(e => e.CanvNroPro)
                .HasMaxLength(5)
                .HasColumnName("canv_nro_pro");
            entity.Property(e => e.CanvNrodoc)
                .HasMaxLength(20)
                .HasColumnName("canv_nrodoc");
            entity.Property(e => e.CanvPiso)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("canv_piso");
            entity.Property(e => e.CanvPisoPro)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("canv_piso_pro");
            entity.Property(e => e.CanvTe)
                .HasMaxLength(100)
                .HasColumnName("canv_te");
            entity.Property(e => e.Estado)
                .HasColumnType("smallint(6)")
                .HasColumnName("estado");
            entity.Property(e => e.FechaProcesado).HasColumnName("fecha_procesado");
        });

        modelBuilder.Entity<CampAula>(entity =>
        {
            entity.HasKey(e => e.CaulIdAula).HasName("PRIMARY");

            entity
                .ToTable("camp_aulas")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CaulIdAula)
                .HasMaxLength(20)
                .HasColumnName("caul_id_aula");
            entity.Property(e => e.CaulDireccion)
                .HasMaxLength(50)
                .HasColumnName("caul_direccion");
            entity.Property(e => e.CaulEdificio)
                .HasMaxLength(50)
                .HasColumnName("caul_edificio");
        });

        modelBuilder.Entity<CampAusencia>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_ausencias")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CausAsignatura)
                .HasMaxLength(200)
                .HasColumnName("caus_asignatura");
            entity.Property(e => e.CausCiclo)
                .HasMaxLength(200)
                .HasColumnName("caus_ciclo");
            entity.Property(e => e.CausFecha).HasColumnName("caus_fecha");
            entity.Property(e => e.CausIdAlumno)
                .HasMaxLength(50)
                .HasColumnName("caus_id_alumno");
            entity.Property(e => e.CausIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("caus_id_carrera");
            entity.Property(e => e.CausLu)
                .HasColumnType("int(11)")
                .HasColumnName("caus_lu");
            entity.Property(e => e.CausTipo)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("caus_tipo");
            entity.Property(e => e.CausValido)
                .HasColumnType("smallint(6)")
                .HasColumnName("caus_valido");
        });

        modelBuilder.Entity<CampCarrera>(entity =>
        {
            entity.HasKey(e => e.CcaaIdCarrera).HasName("PRIMARY");

            entity
                .ToTable("camp_carrera")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CcaaIdCarrera)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("ccaa_id_carrera");
            entity.Property(e => e.CcaaCarrera)
                .HasMaxLength(200)
                .HasColumnName("ccaa_carrera");
            entity.Property(e => e.CcaaClCarrera)
                .HasMaxLength(12)
                .IsFixedLength()
                .HasColumnName("ccaa_cl_carrera");
        });

        modelBuilder.Entity<CampCartelera>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_cartelera")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CcarAsignatura)
                .HasMaxLength(255)
                .HasColumnName("ccar_asignatura");
            entity.Property(e => e.CcarAula)
                .HasMaxLength(20)
                .HasColumnName("ccar_aula");
            entity.Property(e => e.CcarCohorte)
                .HasMaxLength(50)
                .HasColumnName("ccar_cohorte");
            entity.Property(e => e.CcarComision)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("ccar_comision");
            entity.Property(e => e.CcarDetalle)
                .HasMaxLength(200)
                .HasColumnName("ccar_detalle");
            entity.Property(e => e.CcarDocentes)
                .HasMaxLength(200)
                .HasColumnName("ccar_docentes");
            entity.Property(e => e.CcarFecha).HasColumnName("ccar_fecha");
            entity.Property(e => e.CcarHoraFin)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("ccar_hora_fin");
            entity.Property(e => e.CcarHoraInicio)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("ccar_hora_inicio");
            entity.Property(e => e.CcarIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("ccar_id_carrera");
            entity.Property(e => e.CcarIdMateria)
                .HasColumnType("int(11)")
                .HasColumnName("ccar_id_materia");
        });

        modelBuilder.Entity<CampCbu>(entity =>
        {
            entity.HasKey(e => new { e.Idcarrera, e.Idalumno })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("camp_cbu")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.Property(e => e.Idcarrera)
                .HasColumnType("int(11)")
                .HasColumnName("IDCarrera");
            entity.Property(e => e.Idalumno)
                .HasMaxLength(16)
                .HasColumnName("IDAlumno");
            entity.Property(e => e.Acepta)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("ACEPTA");
            entity.Property(e => e.Actividadacademica)
                .HasMaxLength(50)
                .HasColumnName("ACTIVIDADACADEMICA");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .HasColumnName("APELLIDO");
            entity.Property(e => e.Banco)
                .HasMaxLength(100)
                .HasColumnName("BANCO");
            entity.Property(e => e.Cbu)
                .HasMaxLength(22)
                .HasColumnName("CBU");
            entity.Property(e => e.Celular)
                .HasMaxLength(50)
                .HasColumnName("CELULAR");
            entity.Property(e => e.ClCarrera)
                .HasMaxLength(12)
                .IsFixedLength()
                .HasColumnName("CL_CARRERA");
            entity.Property(e => e.Cuit)
                .HasMaxLength(15)
                .HasColumnName("CUIT");
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .HasColumnName("DIRECCION");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("EMAIL");
            entity.Property(e => e.FechaAlta).HasColumnName("FECHA_ALTA");
            entity.Property(e => e.FechaBaja).HasColumnName("FECHA_BAJA");
            entity.Property(e => e.FechaNacimiento).HasColumnName("FECHA_NACIMIENTO");
            entity.Property(e => e.Idguarani)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("IDGUARANI");
            entity.Property(e => e.Idsap)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("IDSAP");
            entity.Property(e => e.Localidad)
                .HasMaxLength(100)
                .HasColumnName("LOCALIDAD");
            entity.Property(e => e.Lu)
                .HasColumnType("int(11)")
                .HasColumnName("LU");
            entity.Property(e => e.Nacionalidad)
                .HasMaxLength(30)
                .HasColumnName("NACIONALIDAD");
            entity.Property(e => e.Nombres)
                .HasMaxLength(50)
                .HasColumnName("NOMBRES");
            entity.Property(e => e.NroCuenta)
                .HasMaxLength(50)
                .HasColumnName("NRO_CUENTA");
            entity.Property(e => e.Nrodocumento)
                .HasMaxLength(30)
                .HasColumnName("NRODOCUMENTO");
            entity.Property(e => e.PostalCp)
                .HasMaxLength(20)
                .HasColumnName("POSTAL_CP");
            entity.Property(e => e.Provincia)
                .HasMaxLength(100)
                .HasColumnName("PROVINCIA");
            entity.Property(e => e.Telefono)
                .HasMaxLength(100)
                .HasColumnName("TELEFONO");
            entity.Property(e => e.Tipodocumento)
                .HasMaxLength(10)
                .HasColumnName("TIPODOCUMENTO");
        });

        modelBuilder.Entity<CampCiclo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_ciclo")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CcicDescCiclo)
                .HasMaxLength(200)
                .HasColumnName("ccic_desc_ciclo");
            entity.Property(e => e.CcicDescMateria)
                .HasMaxLength(200)
                .HasColumnName("ccic_desc_materia");
            entity.Property(e => e.CcicIdCarrera)
                .HasColumnType("int(4)")
                .HasColumnName("ccic_id_carrera");
            entity.Property(e => e.CcicIdCiclo)
                .HasColumnType("int(4)")
                .HasColumnName("ccic_id_ciclo");
            entity.Property(e => e.CcicIdCurso)
                .HasColumnType("int(4)")
                .HasColumnName("ccic_id_curso");
            entity.Property(e => e.CcicIdMateria)
                .HasColumnType("int(4)")
                .HasColumnName("ccic_id_materia");
        });

        modelBuilder.Entity<CampCorrelativa>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_correlativas")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CcorAsignaturaCorrelativa)
                .HasMaxLength(200)
                .HasColumnName("ccor_asignatura_correlativa");
            entity.Property(e => e.CcorClMateriaCorrelativa)
                .HasMaxLength(12)
                .IsFixedLength()
                .HasColumnName("ccor_cl_materia_correlativa");
            entity.Property(e => e.CcorCondicion)
                .HasMaxLength(200)
                .HasColumnName("ccor_condicion");
            entity.Property(e => e.CcorIdCarrera)
                .HasColumnType("int(4)")
                .HasColumnName("ccor_id_carrera");
            entity.Property(e => e.CcorIdMateria)
                .HasColumnType("int(4)")
                .HasColumnName("ccor_id_materia");
            entity.Property(e => e.CcorIdMateriaCorrelativa)
                .HasColumnType("int(4)")
                .HasColumnName("ccor_id_materia_correlativa");
            entity.Property(e => e.CcorIdPlan)
                .HasColumnType("int(4)")
                .HasColumnName("ccor_id_plan");
        });

        modelBuilder.Entity<CampCronogramaFecha>(entity =>
        {
            entity.HasKey(e => new { e.CcroTipoExamen, e.CcroIdExamen })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("camp_cronograma_fechas")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CcroTipoExamen)
                .HasMaxLength(50)
                .HasColumnName("ccro_tipo_examen");
            entity.Property(e => e.CcroIdExamen)
                .HasColumnType("int(11)")
                .HasColumnName("ccro_id_examen");
            entity.Property(e => e.CcroAsignatura)
                .HasMaxLength(200)
                .HasColumnName("ccro_asignatura");
            entity.Property(e => e.CcroCarrera)
                .HasMaxLength(100)
                .HasColumnName("ccro_carrera");
            entity.Property(e => e.CcroCiclo)
                .HasMaxLength(50)
                .HasColumnName("ccro_ciclo");
            entity.Property(e => e.CcroComision)
                .HasColumnType("int(11)")
                .HasColumnName("ccro_comision");
            entity.Property(e => e.CcroExamen)
                .HasMaxLength(50)
                .HasColumnName("ccro_examen");
            entity.Property(e => e.CcroFecha).HasColumnName("ccro_fecha");
            entity.Property(e => e.CcroIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("ccro_id_carrera");
            entity.Property(e => e.CcroIdMateria)
                .HasColumnType("int(11)")
                .HasColumnName("ccro_id_materia");
            entity.Property(e => e.CcroRecuperatorio)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("ccro_recuperatorio");
        });

        modelBuilder.Entity<CampCursoAlumno>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_curso_alumno")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CualAsignatura)
                .HasMaxLength(200)
                .HasColumnName("cual_asignatura");
            entity.Property(e => e.CualClMateria)
                .HasMaxLength(12)
                .IsFixedLength()
                .HasColumnName("cual_cl_materia");
            entity.Property(e => e.CualEstado)
                .HasColumnType("int(11)")
                .HasColumnName("cual_estado");
            entity.Property(e => e.CualIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cual_id_alumno");
            entity.Property(e => e.CualIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("cual_id_carrera");
            entity.Property(e => e.CualIdCiclo)
                .HasColumnType("int(11)")
                .HasColumnName("cual_id_ciclo");
            entity.Property(e => e.CualIdComision)
                .HasColumnType("int(11)")
                .HasColumnName("cual_id_comision");
            entity.Property(e => e.CualIdCurso)
                .HasColumnType("int(11)")
                .HasColumnName("cual_id_curso");
            entity.Property(e => e.CualIdMateria)
                .HasColumnType("int(11)")
                .HasColumnName("cual_id_materia");
            entity.Property(e => e.CualLu)
                .HasColumnType("int(11)")
                .HasColumnName("cual_lu");
            entity.Property(e => e.CualPl)
                .HasColumnType("int(11)")
                .HasColumnName("cual_pl");
        });

        modelBuilder.Entity<CampDeudaAux>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PRIMARY");

            entity
                .ToTable("camp_deuda_aux")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.HasIndex(e => e.NroFactura, "ix_nro_factura");

            entity.Property(e => e.IdFactura)
                .HasColumnType("int(11)")
                .HasColumnName("id_factura");
            entity.Property(e => e.Clientesap)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("clientesap");
            entity.Property(e => e.Emp)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("emp");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("estado");
            entity.Property(e => e.Eterc)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("eterc");
            entity.Property(e => e.FechaFactura).HasColumnName("fecha_factura");
            entity.Property(e => e.FechaPago).HasColumnName("fecha_pago");
            entity.Property(e => e.FechaProcesado).HasColumnName("fecha_procesado");
            entity.Property(e => e.FechaVencimiento1).HasColumnName("fecha_vencimiento1");
            entity.Property(e => e.FechaVencimiento2).HasColumnName("fecha_vencimiento2");
            entity.Property(e => e.IdGuarani)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("id_guarani");
            entity.Property(e => e.Importe)
                .HasPrecision(10, 2)
                .HasColumnName("importe");
            entity.Property(e => e.ImportePagado)
                .HasPrecision(10, 2)
                .HasColumnName("importe_pagado");
            entity.Property(e => e.Leyenda)
                .HasMaxLength(255)
                .HasColumnName("leyenda");
            entity.Property(e => e.NroFactura)
                .HasMaxLength(20)
                .HasColumnName("nro_factura")
                .UseCollation("utf8_general_ci");
            entity.Property(e => e.Opid)
                .HasMaxLength(45)
                .HasColumnName("opid");
            entity.Property(e => e.Recargo)
                .HasPrecision(10, 2)
                .HasColumnName("recargo");
            entity.Property(e => e.Rterc)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("rterc");
            entity.Property(e => e.TipoCbt)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("tipo_cbt");
        });

        modelBuilder.Entity<CampDeudaHist>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_deuda_hist")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.HasIndex(e => e.NroFactura, "ix_nro_factura");

            entity.Property(e => e.Clientesap)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("clientesap");
            entity.Property(e => e.Emp)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("emp");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("estado");
            entity.Property(e => e.Eterc)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("eterc");
            entity.Property(e => e.FechaFactura).HasColumnName("fecha_factura");
            entity.Property(e => e.FechaPago).HasColumnName("fecha_pago");
            entity.Property(e => e.FechaProcesado).HasColumnName("fecha_procesado");
            entity.Property(e => e.FechaVencimiento1).HasColumnName("fecha_vencimiento1");
            entity.Property(e => e.FechaVencimiento2).HasColumnName("fecha_vencimiento2");
            entity.Property(e => e.IdFactura)
                .HasColumnType("int(11)")
                .HasColumnName("id_factura");
            entity.Property(e => e.IdGuarani)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("id_guarani");
            entity.Property(e => e.Importe)
                .HasPrecision(10, 2)
                .HasColumnName("importe");
            entity.Property(e => e.ImportePagado)
                .HasPrecision(10, 2)
                .HasColumnName("importe_pagado");
            entity.Property(e => e.Leyenda)
                .HasMaxLength(255)
                .HasColumnName("leyenda");
            entity.Property(e => e.NroFactura)
                .HasMaxLength(20)
                .HasColumnName("nro_factura")
                .UseCollation("utf8_general_ci");
            entity.Property(e => e.Opid)
                .HasMaxLength(45)
                .HasColumnName("opid");
            entity.Property(e => e.Recargo)
                .HasPrecision(10, 2)
                .HasColumnName("recargo");
            entity.Property(e => e.Rterc)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("rterc");
            entity.Property(e => e.TipoCbt)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("tipo_cbt");
        });

        modelBuilder.Entity<CampDeudaPrev>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PRIMARY");

            entity
                .ToTable("camp_deuda_prev")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.HasIndex(e => e.NroFactura, "ix_nro_factura");

            entity.Property(e => e.IdFactura)
                .HasColumnType("int(11)")
                .HasColumnName("id_factura");
            entity.Property(e => e.Clientesap)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("clientesap");
            entity.Property(e => e.Emp)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("emp");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("estado");
            entity.Property(e => e.Eterc)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("eterc");
            entity.Property(e => e.FechaFactura).HasColumnName("fecha_factura");
            entity.Property(e => e.FechaPago).HasColumnName("fecha_pago");
            entity.Property(e => e.FechaProcesado).HasColumnName("fecha_procesado");
            entity.Property(e => e.FechaVencimiento1).HasColumnName("fecha_vencimiento1");
            entity.Property(e => e.FechaVencimiento2).HasColumnName("fecha_vencimiento2");
            entity.Property(e => e.IdGuarani)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("id_guarani");
            entity.Property(e => e.Importe)
                .HasPrecision(10, 2)
                .HasColumnName("importe");
            entity.Property(e => e.ImportePagado)
                .HasPrecision(10, 2)
                .HasColumnName("importe_pagado");
            entity.Property(e => e.Leyenda)
                .HasMaxLength(255)
                .HasColumnName("leyenda");
            entity.Property(e => e.NroFactura)
                .HasMaxLength(20)
                .HasColumnName("nro_factura")
                .UseCollation("utf8_general_ci");
            entity.Property(e => e.Opid)
                .HasMaxLength(45)
                .HasColumnName("opid");
            entity.Property(e => e.Recargo)
                .HasPrecision(10, 2)
                .HasColumnName("recargo");
            entity.Property(e => e.Rterc)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("rterc");
            entity.Property(e => e.TipoCbt)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("tipo_cbt");
        });

        modelBuilder.Entity<CampDeudum>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PRIMARY");

            entity
                .ToTable("camp_deuda")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.HasIndex(e => new { e.Estado, e.FechaProcesado }, "IX_Estado");

            entity.HasIndex(e => e.NroFactura, "IX_NRO_FACT");

            entity.HasIndex(e => e.Eterc, "camp_deuda_eterc_IDX");

            entity.Property(e => e.IdFactura)
                .HasColumnType("int(11)")
                .HasColumnName("id_factura");
            entity.Property(e => e.Clientesap)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("clientesap");
            entity.Property(e => e.Emp)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("emp");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("estado");
            entity.Property(e => e.Eterc)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("eterc");
            entity.Property(e => e.FechaFactura).HasColumnName("fecha_factura");
            entity.Property(e => e.FechaPago).HasColumnName("fecha_pago");
            entity.Property(e => e.FechaProcesado).HasColumnName("fecha_procesado");
            entity.Property(e => e.FechaVencimiento1).HasColumnName("fecha_vencimiento1");
            entity.Property(e => e.FechaVencimiento2).HasColumnName("fecha_vencimiento2");
            entity.Property(e => e.IdGuarani)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("id_guarani");
            entity.Property(e => e.Importe)
                .HasPrecision(10, 2)
                .HasColumnName("importe");
            entity.Property(e => e.ImportePagado)
                .HasPrecision(10, 2)
                .HasColumnName("importe_pagado");
            entity.Property(e => e.Leyenda)
                .HasMaxLength(255)
                .HasColumnName("leyenda");
            entity.Property(e => e.ModoPago)
                .HasMaxLength(2)
                .HasColumnName("modo_pago");
            entity.Property(e => e.NroFactura)
                .HasMaxLength(20)
                .HasColumnName("nro_factura")
                .UseCollation("utf8_general_ci");
            entity.Property(e => e.Opid)
                .HasMaxLength(45)
                .HasColumnName("opid");
            entity.Property(e => e.Recargo)
                .HasPrecision(10, 2)
                .HasColumnName("recargo");
            entity.Property(e => e.Rterc)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("rterc");
            entity.Property(e => e.TipoCbt)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("tipo_cbt");
        });

        modelBuilder.Entity<CampDocumenTipoASubir>(entity =>
        {
            entity.HasKey(e => e.CdocIdTipoadjunto).HasName("PRIMARY");

            entity
                .ToTable("camp_documen_tipo_a_subir")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CdocIdTipoadjunto)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("cdoc_id_tipoadjunto");
            entity.Property(e => e.CdocDenoTipoadjunto)
                .HasMaxLength(50)
                .HasColumnName("cdoc_deno_tipoadjunto");
        });

        modelBuilder.Entity<CampDocumentacionSubidaNov>(entity =>
        {
            entity.HasKey(e => new { e.CdsuIdAlumno, e.CdsuId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("camp_documentacion_subida_nov")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CdsuIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cdsu_id_alumno");
            entity.Property(e => e.CdsuId)
                .HasColumnType("int(11)")
                .HasColumnName("cdsu_id");
            entity.Property(e => e.CdsuDoc).HasColumnName("cdsu_doc");
            entity.Property(e => e.CdsuFecharecibida).HasColumnName("cdsu_fecharecibida");
            entity.Property(e => e.CdsuFechasubida).HasColumnName("cdsu_fechasubida");
            entity.Property(e => e.CdsuIdTipoadjunto)
                .HasColumnType("int(11)")
                .HasColumnName("cdsu_id_tipoadjunto");
            entity.Property(e => e.CdsuNombre)
                .HasMaxLength(50)
                .HasColumnName("cdsu_nombre");
            entity.Property(e => e.CdsuNota)
                .HasMaxLength(100)
                .HasColumnName("cdsu_nota");
            entity.Property(e => e.CdsuTipo)
                .HasMaxLength(50)
                .HasColumnName("cdsu_tipo");
        });

        modelBuilder.Entity<CampEncuestasEstudioItem>(entity =>
        {
            entity.HasKey(e => e.CeeiIdNivelinstruccion).HasName("PRIMARY");

            entity
                .ToTable("camp_encuestas_estudio_items")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CeeiIdNivelinstruccion)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("ceei_id_nivelinstruccion");
            entity.Property(e => e.CeeiDeno)
                .HasMaxLength(50)
                .HasColumnName("ceei_deno");
        });

        modelBuilder.Entity<CampEncuestasLaboral>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("camp_encuestas_laboral")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CelAno)
                .HasMaxLength(20)
                .HasColumnName("cel_ano");
            entity.Property(e => e.CelCaluLu)
                .HasColumnType("int(11)")
                .HasColumnName("cel_calu_lu");
            entity.Property(e => e.CelIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cel_id_alumno");
            entity.Property(e => e.CelIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("cel_id_carrera");
            entity.Property(e => e.CelTexto)
                .HasMaxLength(500)
                .HasColumnName("cel_texto");
            entity.Property(e => e.Estado)
                .HasColumnType("smallint(6)")
                .HasColumnName("estado");
        });

        modelBuilder.Entity<CampEncuestasLaborale>(entity =>
        {
            entity.HasKey(e => e.ElId).HasName("PRIMARY");

            entity
                .ToTable("camp_encuestas_laborales")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.Property(e => e.ElId)
                .HasColumnType("int(11)")
                .HasColumnName("el_id");
            entity.Property(e => e.ElIdsso)
                .HasMaxLength(20)
                .HasColumnName("el_idsso");
            entity.Property(e => e.ElTexto)
                .HasMaxLength(100)
                .HasColumnName("el_texto");
        });

        modelBuilder.Entity<CampEncuestasPadresNov>(entity =>
        {
            entity.HasKey(e => new { e.CepaIdAlumno, e.CepaPadreMadre })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("camp_encuestas_padres_nov")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CepaIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cepa_id_alumno");
            entity.Property(e => e.CepaPadreMadre)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("cepa_padre_madre");
            entity.Property(e => e.CepaApellido)
                .HasMaxLength(50)
                .HasColumnName("cepa_apellido");
            entity.Property(e => e.CepaCelular)
                .HasMaxLength(50)
                .HasColumnName("cepa_celular");
            entity.Property(e => e.CepaCodigopostal)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("cepa_codigopostal");
            entity.Property(e => e.CepaDomicilio)
                .HasMaxLength(100)
                .HasColumnName("cepa_domicilio");
            entity.Property(e => e.CepaEmail)
                .HasMaxLength(300)
                .HasColumnName("cepa_email");
            entity.Property(e => e.CepaIdLocalidad)
                .HasColumnType("int(11)")
                .HasColumnName("cepa_id_localidad");
            entity.Property(e => e.CepaIdNivelinstruccion)
                .HasColumnType("int(11)")
                .HasColumnName("cepa_id_nivelinstruccion");
            entity.Property(e => e.CepaIdPais)
                .HasColumnType("int(11)")
                .HasColumnName("cepa_id_pais");
            entity.Property(e => e.CepaIdProvincia)
                .HasColumnType("int(11)")
                .HasColumnName("cepa_id_provincia");
            entity.Property(e => e.CepaNombre)
                .HasMaxLength(50)
                .HasColumnName("cepa_nombre");
            entity.Property(e => e.CepaTe)
                .HasMaxLength(100)
                .HasColumnName("cepa_te");
        });

        modelBuilder.Entity<CampEncuestasPeriodo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("camp_encuestas_periodo")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Desde).HasColumnName("desde");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("estado");
            entity.Property(e => e.Hasta).HasColumnName("hasta");
        });

        modelBuilder.Entity<CampEstadoAcademico>(entity =>
        {
            entity.HasKey(e => e.CeacIdEstadoacademico).HasName("PRIMARY");

            entity
                .ToTable("camp_estado_academico")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CeacIdEstadoacademico)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("ceac_id_estadoacademico");
            entity.Property(e => e.CeacEstadoacademico)
                .HasMaxLength(100)
                .HasColumnName("ceac_estadoacademico");
            entity.Property(e => e.CeacSemaforo)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("ceac_semaforo");
        });

        modelBuilder.Entity<CampEstadoAdministra>(entity =>
        {
            entity.HasKey(e => e.CeadIdEstadoadministrativo).HasName("PRIMARY");

            entity
                .ToTable("camp_estado_administra")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CeadIdEstadoadministrativo)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("cead_id_estadoadministrativo");
            entity.Property(e => e.CeadEstadoadministrativo)
                .HasMaxLength(100)
                .HasColumnName("cead_estadoadministrativo");
            entity.Property(e => e.CeadSemaforo)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("cead_semaforo");
        });

        modelBuilder.Entity<CampEstadoAsignatura>(entity =>
        {
            entity.HasKey(e => e.CeasIdEstadoasignatura).HasName("PRIMARY");

            entity
                .ToTable("camp_estado_asignatura")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CeasIdEstadoasignatura)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("ceas_id_estadoasignatura");
            entity.Property(e => e.CeasEstadoasignatura)
                .HasMaxLength(50)
                .HasColumnName("ceas_estadoasignatura");
        });

        modelBuilder.Entity<CampExamenNov>(entity =>
        {
            entity.HasKey(e => new { e.CexnIdExamen, e.CexnParcialFinal, e.CexnFecha, e.CexnIdAlumno })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0 });

            entity
                .ToTable("camp_examen_nov")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CexnIdExamen)
                .HasColumnType("int(11)")
                .HasColumnName("cexn_id_examen");
            entity.Property(e => e.CexnParcialFinal)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("cexn_parcial_final");
            entity.Property(e => e.CexnFecha).HasColumnName("cexn_fecha");
            entity.Property(e => e.CexnIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cexn_id_alumno");
            entity.Property(e => e.CexnFechaconfirma).HasColumnName("cexn_fechaconfirma");
            entity.Property(e => e.CexnSiNo)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("cexn_si_no");
            entity.Property(e => e.CexnUsuario)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("cexn_usuario");
        });

        modelBuilder.Entity<CampFinalesAlumno>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_finales_alumnos")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CfalIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cfal_id_alumno");
            entity.Property(e => e.CfalIdCarrera)
                .HasColumnType("int(4)")
                .HasColumnName("cfal_id_carrera");
            entity.Property(e => e.CfalIdMateria)
                .HasColumnType("int(4)")
                .HasColumnName("cfal_id_materia");
            entity.Property(e => e.CfalLu)
                .HasColumnType("int(4)")
                .HasColumnName("cfal_lu");
        });

        modelBuilder.Entity<CampFinalesFecha>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_finales_fechas")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CfifAsignatura)
                .HasMaxLength(200)
                .HasColumnName("cfif_asignatura");
            entity.Property(e => e.CfifFecha).HasColumnName("cfif_fecha");
            entity.Property(e => e.CfifIdExamen)
                .HasColumnType("int(11)")
                .HasColumnName("cfif_id_examen");
            entity.Property(e => e.CfifIdLlamado)
                .HasColumnType("int(11)")
                .HasColumnName("cfif_id_llamado");
            entity.Property(e => e.CfifIdMateria)
                .HasColumnType("int(11)")
                .HasColumnName("cfif_id_materia");
            entity.Property(e => e.CfifIdPeriodo)
                .HasColumnType("int(11)")
                .HasColumnName("cfif_id_periodo");
            entity.Property(e => e.CfifObservacion)
                .HasMaxLength(50)
                .HasColumnName("cfif_observacion");
        });

        modelBuilder.Entity<CampFinalesIncripcion>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_finales_incripcion")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CfiiFechaIncripcion).HasColumnName("cfii_fecha_incripcion");
            entity.Property(e => e.CfiiIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cfii_id_alumno");
            entity.Property(e => e.CfiiIdEstadoadministrativo)
                .HasColumnType("int(11)")
                .HasColumnName("cfii_id_estadoadministrativo");
            entity.Property(e => e.CfiiIdExamen)
                .HasColumnType("int(11)")
                .HasColumnName("cfii_id_examen");
            entity.Property(e => e.CfiiIdLlamado)
                .HasColumnType("int(11)")
                .HasColumnName("cfii_id_llamado");
            entity.Property(e => e.CfiiIdLu)
                .HasColumnType("int(11)")
                .HasColumnName("cfii_id_lu");
            entity.Property(e => e.CfiiIdPeriodo)
                .HasColumnType("int(11)")
                .HasColumnName("cfii_id_periodo");
            entity.Property(e => e.CfiiInscripto)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("cfii_inscripto");
        });

        modelBuilder.Entity<CampInfoAcademNivele>(entity =>
        {
            entity.HasKey(e => e.CialIdNivel).HasName("PRIMARY");

            entity
                .ToTable("camp_info_academ_niveles")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CialIdNivel)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("cial_id_nivel");
            entity.Property(e => e.CialNivel)
                .HasMaxLength(50)
                .HasColumnName("cial_nivel");
            entity.Property(e => e.CialOrden)
                .HasColumnType("int(11)")
                .HasColumnName("cial_orden");
        });

        modelBuilder.Entity<CampInfoAcademTipoTitulo>(entity =>
        {
            entity.HasKey(e => e.CittIdTipoTitulo).HasName("PRIMARY");

            entity
                .ToTable("camp_info_academ_tipo_titulo")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CittIdTipoTitulo)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("citt_id_tipo_titulo");
            entity.Property(e => e.CittOrden)
                .HasColumnType("int(11)")
                .HasColumnName("citt_orden");
            entity.Property(e => e.CittTipoTitulo)
                .HasMaxLength(50)
                .HasColumnName("citt_tipo_titulo");
        });

        modelBuilder.Entity<CampInfoAcademicaPreviaNov>(entity =>
        {
            entity.HasKey(e => new { e.CianIdAlumno, e.CianIdCarrera, e.CianNroTitulo })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("camp_info_academica_previa_nov")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CianIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cian_id_alumno");
            entity.Property(e => e.CianIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("cian_id_carrera");
            entity.Property(e => e.CianNroTitulo)
                .HasColumnType("smallint(6)")
                .HasColumnName("cian_nro_titulo");
            entity.Property(e => e.CianConvaReva)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("cian_conva_reva");
            entity.Property(e => e.CianFechaemision).HasColumnName("cian_fechaemision");
            entity.Property(e => e.CianFechagraduacion).HasColumnName("cian_fechagraduacion");
            entity.Property(e => e.CianIdLocalidad)
                .HasColumnType("int(11)")
                .HasColumnName("cian_id_localidad");
            entity.Property(e => e.CianIdNivel)
                .HasMaxLength(100)
                .HasColumnName("cian_id_nivel");
            entity.Property(e => e.CianIdPais)
                .HasColumnType("int(11)")
                .HasColumnName("cian_id_pais");
            entity.Property(e => e.CianIdProvincia)
                .HasColumnType("int(11)")
                .HasColumnName("cian_id_provincia");
            entity.Property(e => e.CianIdTipoTitulo)
                .HasMaxLength(100)
                .HasColumnName("cian_id_tipo_titulo");
            entity.Property(e => e.CianInstituto)
                .HasMaxLength(100)
                .HasColumnName("cian_instituto");
            entity.Property(e => e.CianResolucion)
                .HasMaxLength(100)
                .HasColumnName("cian_resolucion");
            entity.Property(e => e.CianTitulo)
                .HasMaxLength(100)
                .HasColumnName("cian_titulo");
            entity.Property(e => e.CianUniversidad)
                .HasMaxLength(100)
                .HasColumnName("cian_universidad");
        });

        modelBuilder.Entity<CampInfoAcademicaPrevium>(entity =>
        {
            entity.HasKey(e => new { e.CiapIdAlumno, e.CiapIdCarrera, e.CiapNroTitulo })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("camp_info_academica_previa")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CiapIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("ciap_id_alumno");
            entity.Property(e => e.CiapIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("ciap_id_carrera");
            entity.Property(e => e.CiapNroTitulo)
                .HasColumnType("smallint(6)")
                .HasColumnName("ciap_nro_titulo");
            entity.Property(e => e.CiapConvaReva)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("ciap_conva_reva");
            entity.Property(e => e.CiapFechaemision).HasColumnName("ciap_fechaemision");
            entity.Property(e => e.CiapFechagraduacion).HasColumnName("ciap_fechagraduacion");
            entity.Property(e => e.CiapIdLocalidad)
                .HasColumnType("int(11)")
                .HasColumnName("ciap_id_localidad");
            entity.Property(e => e.CiapIdNivel)
                .HasColumnType("int(11)")
                .HasColumnName("ciap_id_nivel");
            entity.Property(e => e.CiapIdPais)
                .HasColumnType("int(11)")
                .HasColumnName("ciap_id_pais");
            entity.Property(e => e.CiapIdProvincia)
                .HasColumnType("int(11)")
                .HasColumnName("ciap_id_provincia");
            entity.Property(e => e.CiapIdTipoTitulo)
                .HasColumnType("int(11)")
                .HasColumnName("ciap_id_tipo_titulo");
            entity.Property(e => e.CiapInstituto)
                .HasMaxLength(100)
                .HasColumnName("ciap_instituto");
            entity.Property(e => e.CiapOrigenDato)
                .HasMaxLength(3)
                .IsFixedLength()
                .HasColumnName("ciap_origen_dato");
            entity.Property(e => e.CiapResolucion)
                .HasMaxLength(100)
                .HasColumnName("ciap_resolucion");
            entity.Property(e => e.CiapTitulo)
                .HasMaxLength(100)
                .HasColumnName("ciap_titulo");
            entity.Property(e => e.CiapUniversidad)
                .HasMaxLength(100)
                .HasColumnName("ciap_universidad");
        });

        modelBuilder.Entity<CampInfoProfesionalPreviaNov>(entity =>
        {
            entity.HasKey(e => new { e.CipnIdAlumno, e.CipnIdCarrera, e.CipnNroProfesion })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("camp_info_profesional_previa_nov")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CipnIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cipn_id_alumno");
            entity.Property(e => e.CipnIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("cipn_id_carrera");
            entity.Property(e => e.CipnNroProfesion)
                .HasColumnType("smallint(6)")
                .HasColumnName("cipn_nro_profesion");
            entity.Property(e => e.CipnEntidad)
                .HasMaxLength(100)
                .HasColumnName("cipn_entidad");
            entity.Property(e => e.CipnFechavto).HasColumnName("cipn_fechavto");
            entity.Property(e => e.CipnIdPais)
                .HasColumnType("int(11)")
                .HasColumnName("cipn_id_pais");
            entity.Property(e => e.CipnIdProvincia)
                .HasColumnType("int(11)")
                .HasColumnName("cipn_id_provincia");
            entity.Property(e => e.CipnLugar)
                .HasMaxLength(100)
                .HasColumnName("cipn_lugar");
            entity.Property(e => e.CipnNroMatricula)
                .HasMaxLength(50)
                .HasColumnName("cipn_nro_matricula");
        });

        modelBuilder.Entity<CampInfoProfesionalPrevium>(entity =>
        {
            entity.HasKey(e => new { e.CippIdAlumno, e.CippIdCarrera, e.CippNroProfesion })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("camp_info_profesional_previa")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CippIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cipp_id_alumno");
            entity.Property(e => e.CippIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("cipp_id_carrera");
            entity.Property(e => e.CippNroProfesion)
                .HasColumnType("smallint(6)")
                .HasColumnName("cipp_nro_profesion");
            entity.Property(e => e.CippEntidad)
                .HasMaxLength(100)
                .HasColumnName("cipp_entidad");
            entity.Property(e => e.CippFechavto).HasColumnName("cipp_fechavto");
            entity.Property(e => e.CippIdPais)
                .HasColumnType("int(11)")
                .HasColumnName("cipp_id_pais");
            entity.Property(e => e.CippIdProvincia)
                .HasColumnType("int(11)")
                .HasColumnName("cipp_id_provincia");
            entity.Property(e => e.CippLugar)
                .HasMaxLength(100)
                .HasColumnName("cipp_lugar");
            entity.Property(e => e.CippNroMatricula)
                .HasMaxLength(50)
                .HasColumnName("cipp_nro_matricula");
            entity.Property(e => e.CippOrigenDato)
                .HasMaxLength(3)
                .IsFixedLength()
                .HasColumnName("cipp_origen_dato");
        });

        modelBuilder.Entity<CampInscripMateriaNov>(entity =>
        {
            entity.HasKey(e => new { e.CimnIdMateria, e.CimnIdAlumno })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("camp_inscrip_materia_nov")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CimnIdMateria)
                .HasColumnType("int(11)")
                .HasColumnName("cimn_id_materia");
            entity.Property(e => e.CimnIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cimn_id_alumno");
            entity.Property(e => e.CimnFecha).HasColumnName("cimn_fecha");
            entity.Property(e => e.CimnFechaconfirma).HasColumnName("cimn_fechaconfirma");
            entity.Property(e => e.CimnUsuario)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("cimn_usuario");
            entity.Property(e => e.Estado)
                .HasColumnType("int(11)")
                .HasColumnName("estado");
        });

        modelBuilder.Entity<CampLocalidade>(entity =>
        {
            entity.HasKey(e => new { e.ClocIdPais, e.ClocIdProvincia, e.ClocIdLocalidad })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("camp_localidades")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.ClocIdPais)
                .HasColumnType("int(11)")
                .HasColumnName("cloc_id_pais");
            entity.Property(e => e.ClocIdProvincia)
                .HasColumnType("int(11)")
                .HasColumnName("cloc_id_provincia");
            entity.Property(e => e.ClocIdLocalidad)
                .HasColumnType("int(11)")
                .HasColumnName("cloc_id_localidad");
            entity.Property(e => e.ClocLocalidad)
                .HasMaxLength(100)
                .HasColumnName("cloc_localidad");
        });

        modelBuilder.Entity<CampMateriaAlumno>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_materia_alumno")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CmalAsignatura)
                .HasMaxLength(200)
                .HasColumnName("cmal_asignatura");
            entity.Property(e => e.CmalCalificacion).HasColumnName("cmal_calificacion");
            entity.Property(e => e.CmalClMateria)
                .HasMaxLength(12)
                .IsFixedLength()
                .HasColumnName("cmal_cl_materia");
            entity.Property(e => e.CmalFecha).HasColumnName("cmal_fecha");
            entity.Property(e => e.CmalIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cmal_id_alumno");
            entity.Property(e => e.CmalIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("cmal_id_carrera");
            entity.Property(e => e.CmalIdMateria)
                .HasColumnType("int(11)")
                .HasColumnName("cmal_id_materia");
            entity.Property(e => e.CmalLu)
                .HasColumnType("int(11)")
                .HasColumnName("cmal_lu");
            entity.Property(e => e.CmalResultado)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("cmal_resultado");
        });

        modelBuilder.Entity<CampMateriaRegular>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_materia_regular")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CmarAsignatura)
                .HasMaxLength(200)
                .HasColumnName("cmar_asignatura");
            entity.Property(e => e.CmarClMateria)
                .HasMaxLength(12)
                .IsFixedLength()
                .HasColumnName("cmar_cl_materia");
            entity.Property(e => e.CmarEstado)
                .HasColumnType("int(11)")
                .HasColumnName("cmar_estado");
            entity.Property(e => e.CmarExamenesRendidos)
                .HasColumnType("int(11)")
                .HasColumnName("cmar_examenes_rendidos");
            entity.Property(e => e.CmarIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cmar_id_alumno");
            entity.Property(e => e.CmarIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("cmar_id_carrera");
            entity.Property(e => e.CmarIdMateria)
                .HasColumnType("int(11)")
                .HasColumnName("cmar_id_materia");
            entity.Property(e => e.CmarLu)
                .HasColumnType("int(11)")
                .HasColumnName("cmar_lu");
            entity.Property(e => e.CmarPlRegular)
                .HasColumnType("int(11)")
                .HasColumnName("cmar_pl_regular");
        });

        modelBuilder.Entity<CampMateriasACursar>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_materias_a_cursar")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CmcuAnio)
                .HasColumnType("smallint(2)")
                .HasColumnName("cmcu_anio");
            entity.Property(e => e.CmcuAsignatura)
                .HasMaxLength(200)
                .HasColumnName("cmcu_asignatura");
            entity.Property(e => e.CmcuClMateria)
                .HasMaxLength(12)
                .IsFixedLength()
                .HasColumnName("cmcu_cl_materia");
            entity.Property(e => e.CmcuIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cmcu_id_alumno");
            entity.Property(e => e.CmcuIdCarrera)
                .HasColumnType("int(4)")
                .HasColumnName("cmcu_id_carrera");
            entity.Property(e => e.CmcuIdMateria)
                .HasColumnType("int(4)")
                .HasColumnName("cmcu_id_materia");
            entity.Property(e => e.CmcuIdPlan)
                .HasColumnType("int(4)")
                .HasColumnName("cmcu_id_plan");
            entity.Property(e => e.CmcuLu)
                .HasColumnType("int(4)")
                .HasColumnName("cmcu_lu");
        });

        modelBuilder.Entity<CampMensajesCarrera>(entity =>
        {
            entity.HasKey(e => e.CmecIdMensaje).HasName("PRIMARY");

            entity
                .ToTable("camp_mensajes_carrera")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CmecIdMensaje)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("cmec_id_mensaje");
            entity.Property(e => e.CmecFechaDesde).HasColumnName("cmec_fecha_desde");
            entity.Property(e => e.CmecFechaHasta).HasColumnName("cmec_fecha_hasta");
            entity.Property(e => e.CmecIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("cmec_id_carrera");
            entity.Property(e => e.CmecIdCiclo)
                .HasColumnType("int(11)")
                .HasColumnName("cmec_id_ciclo");
            entity.Property(e => e.CmecIdCurso)
                .HasColumnType("int(11)")
                .HasColumnName("cmec_id_curso");
            entity.Property(e => e.CmecIdMateria)
                .HasColumnType("int(11)")
                .HasColumnName("cmec_id_materia");
            entity.Property(e => e.CmecIdPl)
                .HasColumnType("int(11)")
                .HasColumnName("cmec_id_pl");
            entity.Property(e => e.CmecTexto)
                .HasMaxLength(500)
                .HasColumnName("cmec_texto");
        });

        modelBuilder.Entity<CampNacionalidade>(entity =>
        {
            entity.HasKey(e => e.CnacIdNacionalidad).HasName("PRIMARY");

            entity
                .ToTable("camp_nacionalidades")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CnacIdNacionalidad)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("cnac_id_nacionalidad");
            entity.Property(e => e.CnacNacionalidad)
                .HasMaxLength(50)
                .HasColumnName("cnac_nacionalidad");
        });

        modelBuilder.Entity<CampOtro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("camp_otros")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Clientesap)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("clientesap");
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .HasColumnName("email");
            entity.Property(e => e.Eterc)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("eterc");
            entity.Property(e => e.Nombres)
                .HasMaxLength(60)
                .HasColumnName("nombres");
            entity.Property(e => e.Pass)
                .HasMaxLength(100)
                .HasColumnName("pass");
            entity.Property(e => e.Rterc)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("rterc");
        });

        modelBuilder.Entity<CampPagosLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("camp_pagos_log")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Alumno)
                .HasMaxLength(30)
                .HasColumnName("alumno");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .HasColumnName("estado");
            entity.Property(e => e.Factura)
                .HasMaxLength(50)
                .HasColumnName("factura");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.ModoPago)
                .HasMaxLength(10)
                .HasColumnName("modo_pago");
            entity.Property(e => e.Monto)
                .HasMaxLength(25)
                .HasColumnName("monto");
            entity.Property(e => e.NumRefMp)
                .HasMaxLength(30)
                .HasColumnName("num_ref_mp");
            entity.Property(e => e.Variablex)
                .HasMaxLength(255)
                .HasColumnName("variablex");
        });

        modelBuilder.Entity<CampPagosLogSponsor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("camp_pagos_log_sponsors")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Alumno)
                .HasMaxLength(30)
                .HasColumnName("alumno");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .HasColumnName("estado");
            entity.Property(e => e.Factura)
                .HasMaxLength(50)
                .HasColumnName("factura");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.Monto)
                .HasMaxLength(10)
                .HasColumnName("monto");
        });

        modelBuilder.Entity<CampPaise>(entity =>
        {
            entity.HasKey(e => e.CpaiIdPais).HasName("PRIMARY");

            entity
                .ToTable("camp_paises")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CpaiIdPais)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("cpai_id_pais");
            entity.Property(e => e.CpaiPais)
                .HasMaxLength(50)
                .HasColumnName("cpai_pais");
            entity.Property(e => e.Iso31661)
                .HasMaxLength(3)
                .HasColumnName("ISO_3166_1");
        });

        modelBuilder.Entity<CampParcialNota>(entity =>
        {
            entity.HasKey(e => new { e.CpnoIdExamen, e.CpnoLu })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("camp_parcial_notas")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CpnoIdExamen)
                .HasColumnType("int(11)")
                .HasColumnName("cpno_id_examen");
            entity.Property(e => e.CpnoLu)
                .HasColumnType("int(11)")
                .HasColumnName("cpno_lu");
            entity.Property(e => e.CpnoAsignatura)
                .HasMaxLength(200)
                .HasColumnName("cpno_asignatura");
            entity.Property(e => e.CpnoCarrera)
                .HasMaxLength(100)
                .HasColumnName("cpno_carrera");
            entity.Property(e => e.CpnoEstado)
                .HasMaxLength(15)
                .HasColumnName("cpno_estado");
            entity.Property(e => e.CpnoExamen)
                .HasMaxLength(50)
                .HasColumnName("cpno_examen");
            entity.Property(e => e.CpnoFecha).HasColumnName("cpno_fecha");
            entity.Property(e => e.CpnoIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("cpno_id_carrera");
            entity.Property(e => e.CpnoIdCiclo)
                .HasColumnType("int(11)")
                .HasColumnName("cpno_id_ciclo");
            entity.Property(e => e.CpnoIdCurso)
                .HasColumnType("int(11)")
                .HasColumnName("cpno_id_curso");
            entity.Property(e => e.CpnoIdMateria)
                .HasColumnType("int(11)")
                .HasColumnName("cpno_id_materia");
            entity.Property(e => e.CpnoNota).HasColumnName("cpno_nota");
            entity.Property(e => e.CpnoResultado)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("cpno_resultado");
        });

        modelBuilder.Entity<CampPlanAlumno>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_plan_alumno")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CpalAnio)
                .HasMaxLength(10)
                .HasColumnName("cpal_anio");
            entity.Property(e => e.CpalAsignatura)
                .HasMaxLength(200)
                .HasColumnName("cpal_asignatura");
            entity.Property(e => e.CpalCifras)
                .HasMaxLength(30)
                .HasColumnName("cpal_cifras");
            entity.Property(e => e.CpalCodigoasignatura)
                .HasMaxLength(12)
                .HasColumnName("cpal_codigoasignatura");
            entity.Property(e => e.CpalFecha).HasColumnName("cpal_fecha");
            entity.Property(e => e.CpalFolio)
                .HasColumnType("int(11)")
                .HasColumnName("cpal_folio");
            entity.Property(e => e.CpalIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cpal_id_alumno");
            entity.Property(e => e.CpalIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("cpal_id_carrera");
            entity.Property(e => e.CpalIdEstadoacademico)
                .HasColumnType("int(11)")
                .HasColumnName("cpal_id_estadoacademico");
            entity.Property(e => e.CpalIdEstadoasignatura)
                .HasColumnType("int(11)")
                .HasColumnName("cpal_id_estadoasignatura");
            entity.Property(e => e.CpalIdMateria)
                .HasColumnType("int(11)")
                .HasColumnName("cpal_id_materia");
            entity.Property(e => e.CpalIdPl)
                .HasColumnType("int(11)")
                .HasColumnName("cpal_id_pl");
            entity.Property(e => e.CpalLetras)
                .HasMaxLength(50)
                .HasColumnName("cpal_letras");
            entity.Property(e => e.CpalLibro)
                .HasMaxLength(20)
                .HasColumnName("cpal_libro");
            entity.Property(e => e.CpalLu)
                .HasColumnType("int(11)")
                .HasColumnName("cpal_lu");
            entity.Property(e => e.CpalOrden)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("cpal_orden");
            entity.Property(e => e.CpalPlanalumno)
                .HasMaxLength(20)
                .HasColumnName("cpal_planalumno");
        });

        modelBuilder.Entity<CampPresentismo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_presentismo")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CpreAsignatura)
                .HasMaxLength(200)
                .HasColumnName("cpre_asignatura");
            entity.Property(e => e.CpreAusentes)
                .HasColumnType("int(11)")
                .HasColumnName("cpre_ausentes");
            entity.Property(e => e.CpreAusentesPermitidos)
                .HasColumnType("int(11)")
                .HasColumnName("cpre_ausentes_permitidos");
            entity.Property(e => e.CpreCiclo)
                .HasMaxLength(200)
                .HasColumnName("cpre_ciclo");
            entity.Property(e => e.CpreClases)
                .HasColumnType("int(11)")
                .HasColumnName("cpre_clases");
            entity.Property(e => e.CpreDatosNoValidados)
                .HasColumnType("int(11)")
                .HasColumnName("cpre_datos_no_validados");
            entity.Property(e => e.CpreFecha).HasColumnName("cpre_fecha");
            entity.Property(e => e.CpreIdAlumno)
                .HasMaxLength(20)
                .HasColumnName("cpre_id_alumno");
            entity.Property(e => e.CpreIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("cpre_id_carrera");
            entity.Property(e => e.CpreJustificadas)
                .HasColumnType("int(11)")
                .HasColumnName("cpre_justificadas");
            entity.Property(e => e.CpreLu)
                .HasColumnType("int(11)")
                .HasColumnName("cpre_lu");
            entity.Property(e => e.CprePresentes)
                .HasColumnType("int(11)")
                .HasColumnName("cpre_presentes");
        });

        modelBuilder.Entity<CampProvincia>(entity =>
        {
            entity.HasKey(e => new { e.CprvIdPais, e.CprvIdProvincia })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("camp_provincias")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CprvIdPais)
                .HasColumnType("int(11)")
                .HasColumnName("cprv_id_pais");
            entity.Property(e => e.CprvIdProvincia)
                .HasColumnType("int(11)")
                .HasColumnName("cprv_id_provincia");
            entity.Property(e => e.CprvIdProvincia2)
                .HasMaxLength(3)
                .HasColumnName("cprv_id_provincia2");
            entity.Property(e => e.CprvProvincia)
                .HasMaxLength(100)
                .HasColumnName("cprv_provincia");
        });

        modelBuilder.Entity<CampSaldo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("camp_saldos")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CsalAlumno)
                .HasMaxLength(120)
                .HasColumnName("csal_alumno");
            entity.Property(e => e.CsalEstadoAdminis)
                .HasMaxLength(120)
                .HasColumnName("csal_estado_adminis");
            entity.Property(e => e.CsalFechaFactura)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("csal_fecha_factura");
            entity.Property(e => e.CsalIdAlumnos)
                .HasMaxLength(20)
                .HasColumnName("csal_id_alumnos");
            entity.Property(e => e.CsalIdCarrera)
                .HasColumnType("int(11)")
                .HasColumnName("csal_id_carrera");
            entity.Property(e => e.CsalLu)
                .HasColumnType("int(11)")
                .HasColumnName("csal_lu");
            entity.Property(e => e.CsalNroFactura)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("csal_nro_factura");
            entity.Property(e => e.CsalRterc)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("csal_rterc");
            entity.Property(e => e.CsalSaldo).HasColumnName("csal_saldo");
            entity.Property(e => e.CsalTotalFactura).HasColumnName("csal_total_factura");
        });

        modelBuilder.Entity<CampSexo>(entity =>
        {
            entity.HasKey(e => e.CsexIdSexo).HasName("PRIMARY");

            entity
                .ToTable("camp_sexo")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CsexIdSexo)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("csex_id_sexo");
            entity.Property(e => e.CsexSexo)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("csex_sexo");
        });

        modelBuilder.Entity<CampSponsor>(entity =>
        {
            entity.HasKey(e => e.IdSponsor).HasName("PRIMARY");

            entity
                .ToTable("camp_sponsors")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.Property(e => e.IdSponsor)
                .HasColumnType("int(11)")
                .HasColumnName("id_sponsor");
            entity.Property(e => e.Apellido)
                .HasMaxLength(60)
                .HasColumnName("apellido");
            entity.Property(e => e.Clientesap)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("clientesap");
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(45)
                .HasColumnName("codigo_postal");
            entity.Property(e => e.Domicilio)
                .HasMaxLength(60)
                .HasColumnName("domicilio");
            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .HasColumnName("email");
            entity.Property(e => e.Eterc)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("eterc");
            entity.Property(e => e.Localidad)
                .HasMaxLength(60)
                .HasColumnName("localidad");
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .HasColumnName("nombre");
            entity.Property(e => e.NumDoc)
                .HasMaxLength(45)
                .HasColumnName("num_doc");
            entity.Property(e => e.Pais)
                .HasMaxLength(3)
                .HasColumnName("pais");
            entity.Property(e => e.Provincia)
                .HasMaxLength(3)
                .HasColumnName("provincia");
            entity.Property(e => e.Rterc)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("rterc");
            entity.Property(e => e.TelefonoFijo)
                .HasMaxLength(45)
                .HasColumnName("telefono_fijo");
            entity.Property(e => e.TelefonoMovil)
                .HasMaxLength(45)
                .HasColumnName("telefono_movil");
            entity.Property(e => e.TipoDoc)
                .HasColumnType("int(11)")
                .HasColumnName("tipo_doc");
        });

        modelBuilder.Entity<CampTipodoc>(entity =>
        {
            entity.HasKey(e => e.CtdcIdTipodoc).HasName("PRIMARY");

            entity
                .ToTable("camp_tipodoc")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.CtdcIdTipodoc)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("ctdc_id_tipodoc");
            entity.Property(e => e.CtdcTipodoc)
                .HasMaxLength(50)
                .HasColumnName("ctdc_tipodoc");
        });

        modelBuilder.Entity<CompensacionesAgosto>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("COMPENSACIONES AGOSTO")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.Property(e => e.Fact)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        modelBuilder.Entity<DayVisitor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("day_visitors")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.DateVisit).HasColumnName("date_visit");
            entity.Property(e => e.NVisit)
                .HasColumnType("int(11)")
                .HasColumnName("n_visit");
        });

        modelBuilder.Entity<EntidadesBancaria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("entidades_bancarias")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Codigo)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("codigo");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Entidad)
                .HasMaxLength(255)
                .HasColumnName("entidad");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Tipo).HasColumnName("tipo");
        });

        modelBuilder.Entity<Parametro>(entity =>
        {
            entity.HasKey(e => new { e.ParaEmpre, e.ParaClave })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("parametros")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.ParaEmpre)
                .HasColumnType("int(11)")
                .HasColumnName("para_empre");
            entity.Property(e => e.ParaClave)
                .HasMaxLength(30)
                .HasColumnName("para_clave");
            entity.Property(e => e.ParaAtributo)
                .HasMaxLength(30)
                .HasColumnName("para_atributo");
            entity.Property(e => e.ParaDescripcion)
                .HasMaxLength(100)
                .HasColumnName("para_descripcion");
            entity.Property(e => e.ParaFchAlta).HasColumnName("para_fch_alta");
            entity.Property(e => e.ParaFchModi).HasColumnName("para_fch_modi");
            entity.Property(e => e.ParaHabilitado)
                .HasMaxLength(1)
                .HasColumnName("para_habilitado");
            entity.Property(e => e.ParaTexto)
                .HasColumnType("text")
                .HasColumnName("para_texto");
            entity.Property(e => e.ParaUsrAlta)
                .HasMaxLength(30)
                .HasColumnName("para_usr_alta");
            entity.Property(e => e.ParaUsrModi)
                .HasMaxLength(30)
                .HasColumnName("para_usr_modi");
            entity.Property(e => e.ParaValorC)
                .HasMaxLength(30)
                .HasColumnName("para_valor_c");
            entity.Property(e => e.ParaValorN)
                .HasColumnType("int(11)")
                .HasColumnName("para_valor_n");
        });

        modelBuilder.Entity<TotalVisitor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("total_visitors")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.DateVisit).HasColumnName("date_visit");
            entity.Property(e => e.NVisit)
                .HasColumnType("int(11)")
                .HasColumnName("n_visit");
            entity.Property(e => e.User)
                .HasMaxLength(30)
                .HasColumnName("user");
        });

        modelBuilder.Entity<UsuAccesosLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("usu_accesos_log")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.UsalAltaF).HasColumnName("usal_alta_f");
            entity.Property(e => e.UsalClave)
                .HasMaxLength(50)
                .HasColumnName("usal_clave");
            entity.Property(e => e.UsalCodigoChar)
                .HasMaxLength(50)
                .HasColumnName("usal_codigo_char");
            entity.Property(e => e.UsalCodigoNumber)
                .HasColumnType("int(11)")
                .HasColumnName("usal_codigo_number");
            entity.Property(e => e.UsalEmpre)
                .HasColumnType("int(11)")
                .HasColumnName("usal_empre");
            entity.Property(e => e.UsalFechaHora)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("usal_fecha_hora");
            entity.Property(e => e.UsalHabilitado)
                .HasMaxLength(1)
                .HasColumnName("usal_habilitado");
            entity.Property(e => e.UsalMofiF).HasColumnName("usal_mofi_f");
            entity.Property(e => e.UsalOrden)
                .HasColumnType("int(11)")
                .HasColumnName("usal_orden");
            entity.Property(e => e.UsalUsrAlta)
                .HasMaxLength(50)
                .HasColumnName("usal_usr_alta");
            entity.Property(e => e.UsalUsrModi)
                .HasMaxLength(50)
                .HasColumnName("usal_usr_modi");
            entity.Property(e => e.UsalUsuaNombre)
                .HasMaxLength(50)
                .HasColumnName("usal_usua_nombre");
            entity.Property(e => e.UsalVistas)
                .HasMaxLength(300)
                .HasColumnName("usal_vistas");
        });

        modelBuilder.Entity<UsuAplicacione>(entity =>
        {
            entity.HasKey(e => e.UsapApli).HasName("PRIMARY");

            entity
                .ToTable("usu_aplicaciones")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.UsapApli)
                .HasMaxLength(10)
                .HasDefaultValueSql("''")
                .HasColumnName("usap_apli");
            entity.Property(e => e.UsapFchAlta).HasColumnName("usap_fch_alta");
            entity.Property(e => e.UsapFchModi).HasColumnName("usap_fch_modi");
            entity.Property(e => e.UsapHabilitado)
                .HasMaxLength(1)
                .HasColumnName("usap_habilitado");
            entity.Property(e => e.UsapUsrAlta)
                .HasMaxLength(50)
                .HasColumnName("usap_usr_alta");
            entity.Property(e => e.UsapUsrModi)
                .HasMaxLength(50)
                .HasColumnName("usap_usr_modi");
        });

        modelBuilder.Entity<UsuIncidentesLog>(entity =>
        {
            entity.HasKey(e => e.UsilOrden).HasName("PRIMARY");

            entity
                .ToTable("usu_incidentes_log")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.UsilOrden)
                .HasColumnType("int(11)")
                .HasColumnName("usil_orden");
            entity.Property(e => e.UsilEmpre)
                .HasColumnType("int(11)")
                .HasColumnName("usil_empre");
            entity.Property(e => e.UsilError)
                .HasMaxLength(350)
                .HasColumnName("usil_error");
            entity.Property(e => e.UsilErrorTest)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("usil_error_test");
            entity.Property(e => e.UsilFechaHora)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("usil_fecha_hora");
            entity.Property(e => e.UsilProceso)
                .HasMaxLength(500)
                .HasColumnName("usil_proceso");
            entity.Property(e => e.UsilUsuaNombre)
                .HasMaxLength(50)
                .HasColumnName("usil_usua_nombre");
        });

        modelBuilder.Entity<UsuMenuesApli>(entity =>
        {
            entity.HasKey(e => new { e.UsmaUsapApli, e.UsmaItem })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("usu_menues_apli")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.UsmaUsapApli)
                .HasMaxLength(10)
                .HasColumnName("usma_usap_apli");
            entity.Property(e => e.UsmaItem)
                .HasMaxLength(50)
                .HasColumnName("usma_item");
            entity.Property(e => e.UsmaDesItem)
                .HasMaxLength(50)
                .HasColumnName("usma_des_item");
            entity.Property(e => e.UsmaEnlace)
                .HasMaxLength(200)
                .HasColumnName("usma_enlace");
            entity.Property(e => e.UsmaFchAlta).HasColumnName("usma_fch_alta");
            entity.Property(e => e.UsmaFchModi).HasColumnName("usma_fch_modi");
            entity.Property(e => e.UsmaHabilitado)
                .HasMaxLength(1)
                .HasColumnName("usma_habilitado");
            entity.Property(e => e.UsmaIcono)
                .HasMaxLength(50)
                .HasColumnName("usma_icono");
            entity.Property(e => e.UsmaNivel1)
                .HasMaxLength(50)
                .HasColumnName("usma_nivel1");
            entity.Property(e => e.UsmaNivel2)
                .HasMaxLength(50)
                .HasColumnName("usma_nivel2");
            entity.Property(e => e.UsmaOrden)
                .HasColumnType("smallint(6)")
                .HasColumnName("usma_orden");
            entity.Property(e => e.UsmaUsrAlta)
                .HasMaxLength(50)
                .HasColumnName("usma_usr_alta");
            entity.Property(e => e.UsmaUsrModi)
                .HasMaxLength(50)
                .HasColumnName("usma_usr_modi");
        });

        modelBuilder.Entity<UsuMenuesCarrera>(entity =>
        {
            entity.HasKey(e => e.CcaaIdCarrera).HasName("PRIMARY");

            entity
                .ToTable("usu_menues_carrera")
                .HasCharSet("utf8")
                .UseCollation("utf8_bin");

            entity.Property(e => e.CcaaIdCarrera)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("ccaa_id_carrera");
            entity.Property(e => e.MActividadEconom).HasColumnName("m_actividad_econom");
            entity.Property(e => e.MCronoActi).HasColumnName("m_crono_acti");
            entity.Property(e => e.MCronoEncues).HasColumnName("m_crono_encues");
            entity.Property(e => e.MCronoExam).HasColumnName("m_crono_exam");
            entity.Property(e => e.MDatosPerso).HasColumnName("m_datos_perso");
            entity.Property(e => e.MDocAsubir).HasColumnName("m_doc_asubir");
            entity.Property(e => e.MInfoPagos).HasColumnName("m_info_pagos");
            entity.Property(e => e.MInfoProfPrevia).HasColumnName("m_info_prof_previa");
            entity.Property(e => e.MInscriAsig).HasColumnName("m_inscri_asig");
        });

        modelBuilder.Entity<UsuMenuesRol>(entity =>
        {
            entity.HasKey(e => new { e.UsmrUsapApli, e.UsmrUsroRol, e.UsmrItem })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("usu_menues_rol")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.UsmrUsapApli)
                .HasMaxLength(10)
                .HasDefaultValueSql("''")
                .HasColumnName("usmr_usap_apli");
            entity.Property(e => e.UsmrUsroRol)
                .HasMaxLength(30)
                .HasDefaultValueSql("''")
                .HasColumnName("usmr_usro_rol");
            entity.Property(e => e.UsmrItem)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .HasColumnName("usmr_item");
            entity.Property(e => e.UsmrAlta)
                .HasMaxLength(1)
                .HasColumnName("usmr_alta");
            entity.Property(e => e.UsmrBaja)
                .HasMaxLength(1)
                .HasColumnName("usmr_baja");
            entity.Property(e => e.UsmrFchAlta).HasColumnName("usmr_fch_alta");
            entity.Property(e => e.UsmrFchModi).HasColumnName("usmr_fch_modi");
            entity.Property(e => e.UsmrHabilitado)
                .HasMaxLength(1)
                .HasColumnName("usmr_habilitado");
            entity.Property(e => e.UsmrModif)
                .HasMaxLength(1)
                .HasColumnName("usmr_modif");
            entity.Property(e => e.UsmrUsrAlta)
                .HasMaxLength(50)
                .HasColumnName("usmr_usr_alta");
            entity.Property(e => e.UsmrUsrModi)
                .HasMaxLength(50)
                .HasColumnName("usmr_usr_modi");
        });

        modelBuilder.Entity<UsuRole>(entity =>
        {
            entity.HasKey(e => e.UsroRol).HasName("PRIMARY");

            entity
                .ToTable("usu_roles")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.UsroRol)
                .HasMaxLength(30)
                .HasDefaultValueSql("''")
                .HasColumnName("usro_rol");
            entity.Property(e => e.UsroFchAlta).HasColumnName("usro_fch_alta");
            entity.Property(e => e.UsroFchModi).HasColumnName("usro_fch_modi");
            entity.Property(e => e.UsroHabilitado)
                .HasMaxLength(1)
                .HasColumnName("usro_habilitado");
            entity.Property(e => e.UsroObservaciones)
                .HasMaxLength(200)
                .HasColumnName("usro_observaciones");
            entity.Property(e => e.UsroPermExcel)
                .HasMaxLength(1)
                .HasColumnName("usro_perm_excel");
            entity.Property(e => e.UsroPermHtml)
                .HasMaxLength(1)
                .HasColumnName("usro_perm_html");
            entity.Property(e => e.UsroPermModi)
                .HasMaxLength(1)
                .HasColumnName("usro_perm_modi");
            entity.Property(e => e.UsroPermPdf)
                .HasMaxLength(1)
                .HasColumnName("usro_perm_pdf");
            entity.Property(e => e.UsroUsrAlta)
                .HasMaxLength(50)
                .HasColumnName("usro_usr_alta");
            entity.Property(e => e.UsroUsrModi)
                .HasMaxLength(50)
                .HasColumnName("usro_usr_modi");
        });

        modelBuilder.Entity<UsuRolesUsuario>(entity =>
        {
            entity.HasKey(e => new { e.UsruUsroRol, e.UsruUsuaNombre })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("usu_roles_usuario")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => new { e.UsruHabilitado, e.UsruUsuaNombre }, "usru_habilitado");

            entity.Property(e => e.UsruUsroRol)
                .HasMaxLength(30)
                .HasDefaultValueSql("''")
                .HasColumnName("usru_usro_rol");
            entity.Property(e => e.UsruUsuaNombre)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .HasColumnName("usru_usua_nombre");
            entity.Property(e => e.UsruFchAlta).HasColumnName("usru_fch_alta");
            entity.Property(e => e.UsruFchModi).HasColumnName("usru_fch_modi");
            entity.Property(e => e.UsruHabilitado)
                .HasMaxLength(1)
                .HasColumnName("usru_habilitado");
            entity.Property(e => e.UsruUsrAlta)
                .HasMaxLength(50)
                .HasColumnName("usru_usr_alta");
            entity.Property(e => e.UsruUsrModi)
                .HasMaxLength(50)
                .HasColumnName("usru_usr_modi");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuaNombre).HasName("PRIMARY");

            entity
                .ToTable("usuarios")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.UsuaHabilitado, "Habilitado");

            entity.Property(e => e.UsuaNombre)
                .HasMaxLength(50)
                .HasColumnName("usua_nombre");
            entity.Property(e => e.UsuaClieId)
                .HasColumnType("int(11)")
                .HasColumnName("usua_clie_id");
            entity.Property(e => e.UsuaFchAlta).HasColumnName("usua_fch_alta");
            entity.Property(e => e.UsuaFchModi).HasColumnName("usua_fch_modi");
            entity.Property(e => e.UsuaFilasPag)
                .HasColumnType("int(11)")
                .HasColumnName("usua_filas_pag");
            entity.Property(e => e.UsuaHabilitado)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("usua_habilitado");
            entity.Property(e => e.UsuaNota)
                .HasMaxLength(100)
                .HasColumnName("usua_nota");
            entity.Property(e => e.UsuaPwd)
                .HasMaxLength(50)
                .HasColumnName("usua_pwd");
            entity.Property(e => e.UsuaPwd2)
                .HasMaxLength(50)
                .HasColumnName("usua_pwd_2");
            entity.Property(e => e.UsuaSha1)
                .HasMaxLength(50)
                .HasColumnName("usua_sha1");
            entity.Property(e => e.UsuaUsrAlta)
                .HasMaxLength(50)
                .HasColumnName("usua_usr_alta");
            entity.Property(e => e.UsuaUsrModi)
                .HasMaxLength(50)
                .HasColumnName("usua_usr_modi");
        });

        modelBuilder.Entity<ZzAyudum>(entity =>
        {
            entity.HasKey(e => e.ZhlpId).HasName("PRIMARY");

            entity
                .ToTable("zz_ayuda")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.UhlpEnlace, "zz_ayuda_ik").IsUnique();

            entity.Property(e => e.ZhlpId)
                .HasColumnType("int(11)")
                .HasColumnName("zhlp_id");
            entity.Property(e => e.UhlpAltaF)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("uhlp_alta_f");
            entity.Property(e => e.UhlpBajaF)
                .HasColumnType("timestamp")
                .HasColumnName("uhlp_baja_f");
            entity.Property(e => e.UhlpEnlace)
                .HasMaxLength(250)
                .HasColumnName("uhlp_enlace");
            entity.Property(e => e.UhlpImgAncho)
                .HasMaxLength(20)
                .HasColumnName("uhlp_img_ancho");
            entity.Property(e => e.UhlpLocImagen)
                .HasMaxLength(100)
                .HasColumnName("uhlp_loc_imagen");
            entity.Property(e => e.UhlpRelac)
                .HasMaxLength(200)
                .HasColumnName("uhlp_relac");
            entity.Property(e => e.UhlpTexto)
                .HasColumnType("text")
                .HasColumnName("uhlp_texto");
            entity.Property(e => e.UhlpTitulo)
                .HasMaxLength(100)
                .HasColumnName("uhlp_titulo");
        });

        modelBuilder.Entity<ZzRepoExportar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("zz_repo_exportar")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => new { e.Reporte, e.Col }, "zz_repo_exportar_ik").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Alineacion)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("alineacion");
            entity.Property(e => e.Ancho)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("ancho");
            entity.Property(e => e.Col)
                .HasMaxLength(40)
                .IsFixedLength()
                .HasColumnName("col");
            entity.Property(e => e.Lab)
                .HasMaxLength(40)
                .IsFixedLength()
                .HasColumnName("lab");
            entity.Property(e => e.Orden)
                .HasColumnType("smallint(6)")
                .HasColumnName("orden");
            entity.Property(e => e.Reporte)
                .HasMaxLength(40)
                .IsFixedLength()
                .HasColumnName("reporte");
        });

        modelBuilder.Entity<ZzSistema>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("zz_sistema")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.Texto)
                .HasMaxLength(200)
                .HasColumnName("texto");
        });

        modelBuilder.Entity<ZzTablaInfo>(entity =>
        {
            entity.HasKey(e => e.ZtaiColumn).HasName("PRIMARY");

            entity
                .ToTable("zz_tabla_info")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.ZtaiColumn)
                .HasMaxLength(30)
                .HasColumnName("ztai_column");
            entity.Property(e => e.ZtaiAltaF).HasColumnName("ztai_alta_f");
            entity.Property(e => e.ZtaiBajaF).HasColumnName("ztai_baja_f");
            entity.Property(e => e.ZtaiLabel)
                .HasMaxLength(50)
                .HasColumnName("ztai_label");
            entity.Property(e => e.ZtaiLength)
                .HasColumnType("int(5)")
                .HasColumnName("ztai_length");
            entity.Property(e => e.ZtaiMensaje)
                .HasMaxLength(50)
                .HasColumnName("ztai_mensaje");
            entity.Property(e => e.ZtaiNull)
                .HasMaxLength(3)
                .HasColumnName("ztai_null");
            entity.Property(e => e.ZtaiTable)
                .HasMaxLength(30)
                .HasColumnName("ztai_table");
            entity.Property(e => e.ZtaiType)
                .HasMaxLength(15)
                .HasColumnName("ztai_type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
