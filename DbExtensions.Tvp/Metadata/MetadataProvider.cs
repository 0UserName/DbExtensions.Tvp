using DbExtensions.Tvp.Metadata.Contracts;

using System;
using System.Reflection;

namespace DbExtensions.Tvp.Metadata
{
    internal static class MetadataProvider<TRow> where TRow : ITableValued
    {
        /// <summary>
        /// Returns the 
        /// underlying type argument of 
        /// the specified nullable type.
        /// </summary>
        /// 
        /// <remarks>
        /// Can be used with enum types.
        /// </remarks>
        private static Type GetType(Type type)
        {
            return (type = Nullable.GetUnderlyingType(type) ?? type).IsEnum ? Enum.GetUnderlyingType(type) : type;
        }

        /// <summary>
        /// Returns 
        /// a custom attribute 
        /// from a type member.
        /// </summary>
        public static TMetadata Get<TMetadata>() where TMetadata : Attribute
        {
            return TRow.Type.GetCustomAttribute<TMetadata>();
        }

        /// <summary>
        /// Creates metadata for 
        /// columns based on the 
        /// class properties.
        /// </summary>
        public static IColumnInternalMetadata[] ExtractColumns()
        {
            PropertyInfo[] properties = TRow.Type.GetProperties();

            ColumnInternalMetadata[] metadata = new
            ColumnInternalMetadata
            [properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                metadata[i] = new ColumnInternalMetadata(properties[i].Name, GetType(properties[i].PropertyType), i);
            }

            return metadata;
        }
    }
}