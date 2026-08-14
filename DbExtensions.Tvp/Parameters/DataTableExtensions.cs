using DbExtensions.Tvp.Metadata.Contracts;

using System.Data;

namespace DbExtensions.Tvp.Parameters
{
    internal static class DataTableExtensions
    {
        /// <remarks>
        /// Keep the table columns initialization logic here; it is used by
        /// different parameter type implementations that should not depend
        /// on each other.
        /// </remarks>
        public static DataTable InitColumns<TRow>(this DataTable table) where TRow : ITableValued
        {
            table.BeginInit();

            foreach (IColumnInternalMetadata internalMetadata in TRow.Metadata.Columns)
            {
                table.Columns.Add(internalMetadata is IColumnExternalMetadata externalMetadata ?
                    new DataTableParameterColumn(externalMetadata.AllowDBNull, externalMetadata.Name, externalMetadata.Type, externalMetadata.MaxLength, externalMetadata.Unique) :
                    new DataTableParameterColumn(internalMetadata.AllowDBNull, internalMetadata.Name, internalMetadata.Type));
            }

            table.EndInit();

            return table;
        }
    }
}