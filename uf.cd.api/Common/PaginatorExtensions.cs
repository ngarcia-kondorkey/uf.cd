using System.Data;

namespace uf.cd.api.Common
{
    public static class PaginatorExtensions
    {
        public static PaginatedResult<Dictionary<string, object>> Paginate(this DataTable dataTable, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 1;

            var offset = (pageNumber - 1) * pageSize;
            var paginatedData = dataTable.Rows
                .OfType<DataRow>()
                .Skip(offset)
                .Take(pageSize)
                .ToSerializationDictionary();

            return new PaginatedResult<Dictionary<string, object>>(paginatedData, pageNumber, pageSize, dataTable.Rows.Count);
        }
    }
}
