using DbExtensions.Tvp.Metadata.Contracts;

using System;

namespace DbExtensions.Tvp.Metadata
{
    public sealed record class ColumnExternalMetadata(string Table, bool AllowDBNull, string Name, Type Type, int Ordinal, int MaxLength, bool Unique) : ColumnInternalMetadata(AllowDBNull, Name, Type, Ordinal), IColumnExternalMetadata
    { }
}