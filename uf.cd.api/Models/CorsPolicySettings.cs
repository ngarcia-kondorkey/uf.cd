namespace uf.cd.api.Models {
    public class CorsPolicySettings
    {
        public string[] AllowedOrigins { get; set; }
        public string[] AllowedMethods { get; set; }
        public string[] AllowedHeaders { get; set; }
        // Opcional: si necesitas AllowCredentials, añádelo aquí
        // public bool AllowCredentials { get; set; } = false;
    }
}