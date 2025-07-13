
namespace uf.cd.api.Models {
    public class CorsPolicyInfo
    {
        public string Name { get; set; }
        public bool AllowAnyOrigin { get; set; }
        public IReadOnlyList<string> AllowedSpecificOrigins { get; set; }
        public bool IsOriginAllowedCustomFunction { get; set; }
        public bool AllowAnyMethod { get; set; }
        public IReadOnlyList<string> AllowedSpecificMethods { get; set; }
        public bool AllowAnyHeader { get; set; }
        public IReadOnlyList<string> AllowedSpecificHeaders { get; set; }
        public bool SupportsCredentials { get; set; }

        public string OriginsDescription
        {
            get
            {
                if (AllowAnyOrigin) return "Cualquier origen (*)";
                if (IsOriginAllowedCustomFunction) return "Función personalizada (SetIsOriginAllowed)";
                if (AllowedSpecificOrigins != null && AllowedSpecificOrigins.Count > 0) return string.Join(", ", AllowedSpecificOrigins);
                return "Ningún origen específico permitido";
            }
        }

        public string MethodsDescription
        {
            get
            {
                if (AllowAnyMethod) return "Cualquier método (*)";
                if (AllowedSpecificMethods != null && AllowedSpecificMethods.Count > 0) return string.Join(", ", AllowedSpecificMethods);
                return "Ningún método específico permitido";
            }
        }

        public string HeadersDescription
        {
            get
            {
                if (AllowAnyHeader) return "Cualquier cabecera (*)";
                if (AllowedSpecificHeaders != null && AllowedSpecificHeaders.Count > 0) return string.Join(", ", AllowedSpecificHeaders);
                return "Ninguna cabecera específica permitida";
            }
        }
    }
}
