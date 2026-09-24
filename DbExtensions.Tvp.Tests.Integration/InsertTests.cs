using DbExtensions.Tvp.Metadata.Contracts;

using DbExtensions.Tvp.Tests.Contracts.Abstracts;

using DbExtensions.Tvp.Tests.Rows;

using System.Data;
using System.Data.Common;

using System.Threading.Tasks;

using Testcontainers.Ado.Contracts;

using Testcontainers.Ado.SqlServer;

namespace DbExtensions.Tvp.Tests.Integration
{
    [TestFixture(typeof(SqlServerTestRunner), typeof(ExternalMetadataTableValued), "mcr.microsoft.com/mssql/server:2022-CU27-ubuntu-20.04", "Init\\SqlServer", Data.Procedures.INSERT)]
    public sealed class InsertTests<TDbTestRunner, TRow>(string image, string schemaDir, string procedure) : AbstractTests where TDbTestRunner : IDbTestRunner, new() where TRow : ITableValued
    {
        private readonly TDbTestRunner _runner = new TDbTestRunner();

        [OneTimeSetUp]
        public async Task StartAsync()
        {
            await _runner.StartAsync(image, schemaDir);
        }

        [Test]
        public async Task TestDataReaderAdoAsync()
        {
            await ThatAsync<bool, TRow, DbDataReader>(async (_, parameter) => await _runner.InsertValueAdoAsync(procedure, parameter) == parameter.RecordsAffected, Is.True);
        }

        [Test]
        public async Task TestDataTableAdoAsync()
        {
            await ThatAsync<bool, TRow, DataTable>(async (_, parameter) => await _runner.InsertValueAdoAsync(procedure, parameter) == parameter.Rows.Count, Is.True);
        }

        [OneTimeTearDown]
        public async Task StopAsync()
        {
            await _runner.DisposeAsync();
        }
    }
}