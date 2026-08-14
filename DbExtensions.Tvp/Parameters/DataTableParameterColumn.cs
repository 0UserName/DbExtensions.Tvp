using System;
using System.Data;

namespace DbExtensions.Tvp.Parameters
{
    internal sealed class DataTableParameterColumn : DataColumn
    {
        /// <summary>
        /// Sets the maximum
        /// length of a text
        /// column.
        /// </summary>
        private DataTableParameterColumn SetMaxLength(int maxLength)
        {
            MaxLength = DataType == typeof(string) ? maxLength : MaxLength;

            return this;
        }

        /// <summary>
        /// Sets a value that indicates whether the values in each row of the column must be unique.
        /// </summary>
        private DataTableParameterColumn SetUnique(bool unique)
        {
            Unique = unique;

            return this;
        }

        public DataTableParameterColumn(bool allowDBNull, string name, Type type) : base(name, type)
        {
            AllowDBNull = allowDBNull;
        }

        public DataTableParameterColumn(bool allowDBNull, string name, Type type, int maxLength, bool unique) : this(allowDBNull, name, type)
        {
            SetMaxLength(maxLength).SetUnique(unique);
        }
    }
}