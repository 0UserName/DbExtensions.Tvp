using System;

namespace DbExtensions.Tvp.Metadata
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class TableMetadataAttribute(string name) : Attribute
    {
        /// <summary>
        /// The parameter type name.
        /// </summary>
        /// 
        /// <remarks>
        /// The type name must match the name of 
        /// a compatible type previously created 
        /// on the server.
        /// </remarks>
        public string Name
        {
            get => name;
        }
    }
}