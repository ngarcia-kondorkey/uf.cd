using Newtonsoft.Json;

namespace uf.cd.api.Common
{
    public class PaginatedResult<T>
    {
        public PaginatedResult(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            Items = data;
        }

        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("totalRecords")]
        public int TotalRecords { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        [JsonProperty("items")]
        public IEnumerable<T> Items { get; set; }
    }

}
