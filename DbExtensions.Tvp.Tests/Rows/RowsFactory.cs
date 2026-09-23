using DbExtensions.Tvp.Metadata.Contracts;

using System.IO;

using System.Text.Json;

namespace DbExtensions.Tvp.Tests.Rows
{
    internal static class RowsFactory
    {
        /// <summary>
        /// Creates TRow instances by reading a file whose name matches the class name.
        /// </summary>
        public static TRow[] Create<TRow>() where TRow : ITableValued
        {
            return JsonSerializer.Deserialize<TRow[]>(File.ReadAllBytes(Path.Combine("Data", TRow.Type.Name + ".json")));
        }
    }
}