using DbExtensions.Tvp.Metadata.Contracts;
using DbExtensions.Tvp.Parameters.Contracts;

using Microsoft.Extensions.ObjectPool;

using System;
using System.Collections;
using System.Collections.Generic;

using System.Data;
using System.Data.Common;

using System.Runtime.InteropServices;

namespace DbExtensions.Tvp.Parameters
{
    internal sealed class ParameterDR<TRow>(ObjectPool<ParameterDR<TRow>> pool) : DbDataReader, IParameter<TRow> where TRow : ITableValued
    {
        /// <summary>
        /// The DataTable that describes the column metadata.
        /// </summary>
        /// 
        /// <remarks>
        /// Not used directly.
        /// </remarks>
        private readonly static DataTable _schema = new DataTable(TRow.Metadata.Name).GetSchemaTable<TRow>();

        private IEnumerator<TRow> _enumerator;

        /// <inheritdoc/>
        public override object this[int ordinal]
        {
            get => _enumerator.Current.GetValue<object>(ordinal);
        }

        /// <inheritdoc/>
        public override object this[string name]
        {
            get => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override int Depth
        {
            get => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override int FieldCount
        {
            get => TRow.Metadata.Columns.Length;
        }

        /// <inheritdoc/>
        public override bool HasRows
        {
            get => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool IsClosed
        {
            get => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override int RecordsAffected
        {
            get => throw new NotImplementedException();
        }

        private static ReadOnlySpan<T> Cast<T>(object value) where T : struct
        {
            if (value is T[] array)
            {
                return array;
            }

            // Microsoft.Data.SqlClient treats non-Unicode strings as char[].
            if (value is string val)
            {
                return MemoryMarshal.Cast<char, T>(val.AsSpan());
            }

            throw new InvalidOperationException($"{ value.GetType().FullName } is not supported");
        }

        private static long CopyToBuffer<T>(ReadOnlySpan<T> value, long dataOffset, Span<T> buffer, int bufferOffset, int length)
        {
            long copy = 0;

            if (value.Length > dataOffset)
            {
                copy = Math.Min(length, value.Length - dataOffset);

                value.Slice((int)dataOffset, (int)copy).CopyTo(buffer.Slice(bufferOffset, (int)copy));
            }

            return copy;
        }

        /// <inheritdoc/>
        public override bool GetBoolean(int ordinal)
        {
            return _enumerator.Current.GetValue<bool>(ordinal);
        }

        /// <inheritdoc/>
        public override byte GetByte(int ordinal)
        {
            return _enumerator.Current.GetValue<byte>(ordinal);
        }

        /// <inheritdoc/>
        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return CopyToBuffer(Cast<byte>(this[ordinal]), dataOffset, buffer, bufferOffset, length);
        }

        /// <inheritdoc/>
        public override char GetChar(int ordinal)
        {
            return _enumerator.Current.GetValue<char>(ordinal);
        }

        /// <inheritdoc/>
        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return CopyToBuffer(Cast<char>(this[ordinal]), dataOffset, buffer, bufferOffset, length);
        }

        /// <inheritdoc/>
        public override string GetDataTypeName(int ordinal)
        {
            return TRow.Metadata.Columns[ordinal].Type.Name;
        }

        /// <inheritdoc/>
        public override DateTime GetDateTime(int ordinal)
        {
            return _enumerator.Current.GetValue<DateTime>(ordinal);
        }

        /// <inheritdoc/>
        public override decimal GetDecimal(int ordinal)
        {
            return _enumerator.Current.GetValue<decimal>(ordinal);
        }

        /// <inheritdoc/>
        public override double GetDouble(int ordinal)
        {
            return _enumerator.Current.GetValue<double>(ordinal);
        }

        /// <inheritdoc/>
        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Type GetFieldType(int ordinal)
        {
            return TRow.Metadata.Columns[ordinal].Type;
        }

        /// <inheritdoc/>
        public override float GetFloat(int ordinal)
        {
            return _enumerator.Current.GetValue<float>(ordinal);
        }

        /// <inheritdoc/>
        public override Guid GetGuid(int ordinal)
        {
            return _enumerator.Current.GetValue<Guid>(ordinal);
        }

        /// <inheritdoc/>
        public override short GetInt16(int ordinal)
        {
            return _enumerator.Current.GetValue<short>(ordinal);
        }

        /// <inheritdoc/>
        public override int GetInt32(int ordinal)
        {
            return _enumerator.Current.GetValue<int>(ordinal);
        }

        /// <inheritdoc/>
        public override long GetInt64(int ordinal)
        {
            return _enumerator.Current.GetValue<long>(ordinal);
        }

        /// <inheritdoc/>
        public override string GetName(int ordinal)
        {
            return TRow.Metadata.Columns[ordinal].Name;
        }

        /// <inheritdoc/>
        public override int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override DataTable GetSchemaTable()
        {
            return _schema;
        }

        /// <inheritdoc/>
        public override string GetString(int ordinal)
        {
            return _enumerator.Current.GetValue<string>(ordinal);
        }

        /// <inheritdoc/>
        public override object GetValue(int ordinal)
        {
            return this[ordinal];
        }

        /// <inheritdoc/>
        public override T GetFieldValue<T>(int ordinal)
        {
            return _enumerator.Current.GetValue<T>(ordinal);
        }

        /// <inheritdoc/>
        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool IsDBNull(int ordinal)
        {
            return _enumerator.Current.GetValue<object>(ordinal) == default;
        }

        /// <inheritdoc/>
        public override bool NextResult()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool Read()
        {
            return _enumerator.MoveNext();
        }

        /// <inheritdoc/>
        public void Load(IEnumerable<TRow> rows)
        {
            _enumerator = rows.GetEnumerator();
        }

        /// <inheritdoc/>
        public bool TryReset()
        {
            _enumerator = default;

            return true;
        }

        /// <inheritdoc/>
        public new void Dispose()
        {
            pool.Return(this);
        }
    }
}