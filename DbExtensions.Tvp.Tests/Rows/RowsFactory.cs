using DbExtensions.Tvp.Metadata.Contracts;

using System.IO;

using System.Text.Json;

namespace DbExtensions.Tvp.Tests.Rows
{
    internal static class RowsFactory
    {
        /// <summary>
        /// Creates instances of TRow by reading a file
        /// whose name matches the table name specified
        /// in the attribute.
        /// </summary>
        public static TRow[] Create<TRow>() where TRow : ITableValued
        {
            return JsonSerializer.Deserialize<TRow[]>(File.ReadAllBytes(Path.Combine("Data", TRow.Metadata.Name + ".json")));
        }
    }
}