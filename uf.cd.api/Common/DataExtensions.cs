using System.Data;

namespace uf.cd.api.Common
{
    public static class DataExtensions
    {
        public static List<Dictionary<string, object>> ToDictionary(this DataTable dataTable)
        {
            return dataTable.Rows.OfType<DataRow>().ToSerializationDictionary();
        }

        public static List<Dictionary<string, object>> ToSerializationDictionary(this IEnumerable<DataRow> rows)
        {
            var rowsAsDictionaries = new List<Dictionary<string, object>>();
            foreach (DataRow row in rows)
            {
                Dictionary<string, object> rowDict = new Dictionary<string, object>();
                foreach (DataColumn col in row.Table.Columns)
                {
                    rowDict[col.ColumnName] = row[col];
                }
                rowsAsDictionaries.Add(rowDict);
            }

            return rowsAsDictionaries;
        }

        public static void ConvertColumnNamesToLowerCase(this DataTable dataTable)
        {
            foreach (var column in dataTable.Columns.OfType<DataColumn>())
                column.ColumnName = column.ColumnName.ToLowerInvariant();
        }
    }
}
