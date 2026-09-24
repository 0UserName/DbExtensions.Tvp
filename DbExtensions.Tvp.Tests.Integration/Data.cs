namespace DbExtensions.Tvp.Tests.Integration
{
    internal static class Data
    {
        public static class Procedures
        {
            /// <remarks>
            /// <code>
            /// dbo.table_valued_insert @i_p dbo.ExternalMetadataTableValued READONLY, @o_p BIGINT OUTPUT
            /// </code>
            /// </remarks>
            public const string INSERT = "dbo.table_valued_insert";
        }
    }
}