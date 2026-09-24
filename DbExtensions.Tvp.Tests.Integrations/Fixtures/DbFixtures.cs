using DbExtensions.Tvp.Tests.Rows;

using System;
using System.Collections;

using System.Data;

using System.IO;

using Testcontainers.Ado.SqlServer;

namespace DbExtensions.Tvp.Tests.Integrations.Fixtures
{
    internal static class DbFixtures
    {
        public static IEnumerable Runners
        {
            get
            {
                yield return new TestFixtureData(new SqlServerRunner("mcr.microsoft.com/mssql/server:2022-CU27-ubuntu-20.04", Path.Combine("Init", "SqlServer"), default), (int)SqlDbType.Structured) { TypeArgs = new Type[] { typeof(ExternalMetadataTableValued) } };
            }
        }
    }
}