using DbExtensions.Tvp.Metadata.Contracts;

using System;
using System.Diagnostics;

namespace DbExtensions.Tvp.Metadata
{
    [DebuggerDisplay("AllowDBNull = { AllowDBNull }, Name = { Name }, Type = { Type }, Ordinal = { Ordinal }, MaxLength = { MaxLength }, Unique = { Unique }")]
    public sealed record class ColumnExternalMetadata(string Table, bool AllowDBNull, string Name, Type Type, int Ordinal, int MaxLength, bool Unique) : ColumnInternalMetadata(AllowDBNull, Name, Type, Ordinal), IColumnExternalMetadata
    { }
}