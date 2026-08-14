using DbExtensions.Tvp.Metadata.Contracts;

using System;
using System.Diagnostics;

namespace DbExtensions.Tvp.Metadata
{
    [DebuggerDisplay("AllowDBNull = { AllowDBNull }, Name = { Name }, Type = { Type }, Ordinal = { Ordinal }")]
    public record class ColumnInternalMetadata(bool AllowDBNull, string Name, Type Type, int Ordinal) : IColumnInternalMetadata
    { }
}