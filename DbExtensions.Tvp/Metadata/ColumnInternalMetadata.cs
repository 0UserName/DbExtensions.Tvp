using DbExtensions.Tvp.Metadata.Contracts;

using System;

namespace DbExtensions.Tvp.Metadata
{
    public record class ColumnInternalMetadata(bool AllowDBNull, string Name, Type Type, int Ordinal) : IColumnInternalMetadata
    { }
}