//DTO para el Login (Crea una clase LoginModel en Models/LoginModel.cs o similar)
namespace uf.cd.api.Models {
    public class LoginModel {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "El nombre de usuario es requerido.")]
        public string Username { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "La contraseña es requerida.")]
        public string Password { get; set; }
    }

    public class ReporteCarteleraFicen{
        public DateTime Fecha { get; set;}
        public string Carrera { get; set; }
        public string Año { get; set; }
        public string Actividad { get; set; }
        public int Id_Comision { get; set; }
        public string Horario_Ini { get; set; }
        public string Horario_Fin { get; set; }
        public string Aula { get; set; }
        public string Detalle_Cartelera { get; set; }
        public string Mat_Doc  { get; set; }
        public int Id_Dia   { get; set; }
        public string Observacion_Inten { get; set; }
        public int Estimados { get; set; }
    }

    public class ReporteCartelera{
        public DateTime Fecha { get; set; }
        public string Año { get; set; }
        public string Carrera { get; set; }
        public string Actividad { get; set; }
        public int Id_Comision { get; set; }
        public string Horario_Ini { get; set; }
        public string Horario_Fin { get; set; }
        public string Aula { get; set; }
        public string Detalle_Cartelera { get; set; }
        public string Mat_Doc  { get; set; }
        public int Id_Carrera { get; set; }
        public int Id_Pl { get; set; }
        public int Id_Materia { get; set; }
        public int Id_Curso { get; set; }
        public int Id_Ciclo { get; set; }
       // public int Id_Comision { get; set; }
        public int Id_Dia   { get; set; }
        public int Id_Detalle { get; set; }
        public int Id_Clase { get; set; }
        public string Observacion_Inten { get; set; }
        public int Estimados { get; set; }
    }
}