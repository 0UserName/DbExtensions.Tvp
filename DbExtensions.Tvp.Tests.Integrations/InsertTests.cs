using DbExtensions.Tvp.Metadata.Contracts;

using DbExtensions.Tvp.Tests.Integrations.Fixtures;

using System;
using System.Data;
using System.Data.Common;

using System.Linq;

using System.Threading.Tasks;

using Testcontainers.Ado.Contracts;

using Testcontainers.Ado.NUnit;
using Testcontainers.Ado.NUnit.Contracts.Abstracts;

using Testcontainers.Ado.NUnit.Extensions;

namespace DbExtensions.Tvp.Tests.Integrations
{
    /// <param name="dbType">
    /// Specifies the database-specific data type used to
    /// represent the structured data in the table-valued
    /// parameter.
    /// </param>
    [TestFixtureSource(typeof(DbFixtures), nameof(DbFixtures.Runners))]
    public sealed class InsertTests<TRow>(IDbRunner runner, int? dbType) : AbstractDbTests<IDbRunner>(runner) where TRow : ITableValued
    {
        private Task TestAsync<TParameter>(string procedure)
        {
            return AssertionContext.ExecuteAsync<TRow, TParameter>((rows, parameter) => Runner.ThatOutputRowsAsync(procedure, DbParameterNames.O_P, rows.Count(), parameter.AsIn(dbType), default(long).AsOut()));
        }

        [TestCase(Data.Procedures.INSERT)]
        public async Task TestDataReaderAsync(string procedure)
        {
            await TestAsync<DbDataReader>(procedure);
        }

        [TestCase(Data.Procedures.INSERT)]
        public async Task TestDataTableAsync(string procedure)
        {
            await TestAsync<DataTable>(procedure);
        }
    }
}