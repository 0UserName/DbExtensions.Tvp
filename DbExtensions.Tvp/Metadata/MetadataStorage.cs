using DbExtensions.Tvp.Metadata.Contracts;

using System;
using System.Collections.Concurrent;

namespace DbExtensions.Tvp.Metadata
{
    public static class MetadataStorage
    {
        private static readonly
            ConcurrentDictionary<string, IColumnInternalMetadata[]> _storage = new
            ConcurrentDictionary<string, IColumnInternalMetadata[]>
            ();

        private static IColumnInternalMetadata[] Sort(IColumnInternalMetadata[] metadata)
        {
            Array.Sort(metadata, (x, y) => x.Ordinal.CompareTo(y.Ordinal));

            return metadata;
        }

        internal static IColumnInternalMetadata[] GetColumns<TRow>() where TRow : ITableValued
        {
            return Sort(_storage.TryRemove(TRow.Metadata.Name, out IColumnInternalMetadata[] metadata) ? metadata : MetadataProvider<TRow>.ExtractColumns());
        }

        public static void AddColumns(string name, IColumnInternalMetadata[] metadata)
        {
            _storage[name] = metadata;
        }
    }
}