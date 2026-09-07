using System;

namespace DbExtensions.Tvp.Metadata
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class TableMetadataAttribute(string name) : Attribute
    {
        /// <summary>
        /// Gets the parameter type name.
        /// </summary>
        public string Name
        {
            get => name;
        }
    }
}