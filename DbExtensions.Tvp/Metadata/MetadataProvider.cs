using DbExtensions.Tvp.Metadata.Contracts;

using System;
using System.Reflection;

namespace DbExtensions.Tvp.Metadata
{
    internal static class MetadataProvider<TRow> where TRow : ITableValued
    {
        /// <summary>
        /// Returns a custom 
        /// attribute from a
        /// type member.
        /// </summary>
        public static TMetadata Get<TMetadata>() where TMetadata : Attribute
        {
            return TRow.Type.GetCustomAttribute<TMetadata>();
        }

        /// <summary>
        /// Returns the underlying type of the specified property.
        /// </summary>
        /// 
        /// <returns>
        /// True if the specified type is a nullable type.
        /// </returns>
        public static bool GetUnderlyingType(PropertyInfo property, out Type type)
        {
            Type nullableType = Nullable.GetUnderlyingType(property.PropertyType);

            type = nullableType ?? property.PropertyType;

            if (type.IsEnum)
            {
                type = Enum.GetUnderlyingType(type);
            }

            return nullableType != default || !type.IsValueType;
        }

        /// <summary>
        /// Creates column metadata by inspecting
        /// the properties of the specified class.
        /// </summary>
        public static IColumnInternalMetadata[] ExtractColumns()
        {
            PropertyInfo[] properties = TRow.Type.GetProperties();

            ColumnInternalMetadata[] metadata = new
            ColumnInternalMetadata
            [properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];

                metadata[i] = new ColumnInternalMetadata(GetUnderlyingType(property, out Type type), property.Name, type, i);
            }

            return metadata;
        }
    }
}