using DbExtensions.Tvp.Metadata;
using DbExtensions.Tvp.Metadata.Contracts;

using DbExtensions.Tvp.Parameters;

using DbExtensions.Tvp.Tests.Rows;

using NUnit.Framework.Constraints;

using System;
using System.Collections.Generic;

using System.Data.Common;

namespace DbExtensions.Tvp.Tests.Abstracts
{
    public abstract class AbstractTests
    {
        /// <summary>
        /// Apply a constraint
        /// to an actual value.
        /// </summary>
        protected static void That<TConstraintType, TRow, TParameter>(Func<IEnumerable<TRow>, TParameter, TConstraintType> actualFactory, IResolveConstraint expression) where TRow : ITableValued
        {
            IEnumerable<TRow> rows = RowsFactory.Create<TRow>();

            using (IDisposable parameter = rows.Build(typeof(TParameter).IsAssignableTo(typeof(DbDataReader))))
            {
                Assert.That(actualFactory(rows, (TParameter)parameter), expression);
            }
        }

        [OneTimeSetUp]
        protected void SetUp()
        {
            MetadataStorage.AddColumns(nameof(ExternalMetadataTableValued), new IColumnExternalMetadata[]
            {
                new ColumnExternalMetadata(default, true , nameof(ExternalMetadataTableValued.Property0), typeof(int)   , 5, -1, false),
                new ColumnExternalMetadata(default, true , nameof(ExternalMetadataTableValued.Property1), typeof(int)   , 4, -1, false),
                new ColumnExternalMetadata(default, false, nameof(ExternalMetadataTableValued.Property2), typeof(int)   , 3, -1, false),
                new ColumnExternalMetadata(default, false, nameof(ExternalMetadataTableValued.Property3), typeof(int)   , 2, -1, false),
                new ColumnExternalMetadata(default, false, nameof(ExternalMetadataTableValued.Property4), typeof(string), 1, -1, false),
                new ColumnExternalMetadata(default, false, nameof(ExternalMetadataTableValued.Property5), typeof(string), 0, -1, false)
            });
        }
    }
}