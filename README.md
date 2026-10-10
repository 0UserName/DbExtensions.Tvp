![Release workflow](https://github.com/0UserName/dbextensions.tvp/actions/workflows/release.yml/badge.svg)



# Motivation

<div align="justify">

The library simplifies the creation of table-valued parameters for `SQL Server` and `PostgreSQL` (via the [Npgsql.Tvp](https://github.com/0UserName/npgsql.tvp) plugin), keeping configuration to an absolute minimum.

</div>



# Usage

<div align="justify">

Define a class that inherits from `AbstractTableValued<>`, using the class itself as the type parameter, and apply the `TableMetadata` attribute with the name of the parameter previously defined on the server:

</div>



```csharp
[TableMetadata(nameof(UserRow))]
public sealed class UserRow : AbstractTableValued<UserRow>
{
    public int Property0 { get; set; }
    public int Property1 { get; set; }
    public int Property2 { get; set; }
    public int Property3 { get; set; }
}

UserRow[] rows = new
UserRow[]
{
    new UserRow { Property0 = 0, Property1 = 1, Property2 = 2, Property3 = 3 },
    new UserRow { Property0 = 4, Property1 = 5, Property2 = 6, Property3 = 7 }
};

using (IDisposable tvp = rows.Build(true)) // true: DbDataReader-based, false: DataTable-based.
{
    // Execute stored procedure...
}
```



<div align="justify">

In this example, the table-valued parameter structure is derived from class metadata, which may differ from the database schema. Such mismatches are typically detected only at runtime during remote calls. Structure and data validation can also be performed using external metadata, which can be specified explicitly or loaded directly from the database:

</div>



```csharp
MetadataStorage.AddColumns(nameof(UserRow), new IColumnExternalMetadata[]
{
    new ColumnExternalMetadata(Table: default, AllowDBNull: false, Name: nameof(UserRow.Property0), Type: typeof(int), Ordinal: 3, MaxLength: -1, Unique: false),
    new ColumnExternalMetadata(Table: default, AllowDBNull: false, Name: nameof(UserRow.Property1), Type: typeof(int), Ordinal: 2, MaxLength: -1, Unique: false),
    new ColumnExternalMetadata(Table: default, AllowDBNull: false, Name: nameof(UserRow.Property2), Type: typeof(int), Ordinal: 1, MaxLength: -1, Unique: false),
    new ColumnExternalMetadata(Table: default, AllowDBNull: false, Name: nameof(UserRow.Property3), Type: typeof(int), Ordinal: 0, MaxLength: -1, Unique: false)
});
```



> [!NOTE]
> The `Table` argument is used only to group column metadata in user code after it has been loaded from the database.



> [!WARNING]
>  Metadata is applied only once, during class initialization.



# Constraints

<div align="justify">

The following constraints are currently supported for user data validation: [AllowDBNull](https://learn.microsoft.com/en-us/dotnet/api/system.data.datacolumn.allowdbnull?view=net-10.0), [MaxLength](https://learn.microsoft.com/en-us/dotnet/api/system.data.datacolumn.maxlength?view=net-10.0) and [Unique](https://learn.microsoft.com/en-us/dotnet/api/system.data.datacolumn.unique?view=net-10.0). A violation of any supported constraint results in a [ConstraintException](https://learn.microsoft.com/en-us/dotnet/api/system.data.constraintexception?view=net-10.0).

</div>



# Column mapping

<div align="justify">

When external metadata is provided, column ordinals remain synchronized with the database schema and unused columns are automatically filtered out. Columns that have been removed from the database are ignored during parameter construction. Otherwise, ordinals are determined by class property order.

</div>



# Performance considerations


## Expression trees

<div align="justify">

Row values are accessed through compiled lambda expressions that are created for each class and cached for reuse. These lambdas encapsulate property getters and null-checks, eliminating reflection overhead.

</div>



## Parameter types

<div align="justify">

The `Build` method accepts a Boolean argument that determines the underlying implementation. When `true` is passed, a lightweight `DbDataReader`-based implementation is used. It acts as an enumerator without storing a copy of the data, performing validation only during enumeration, typically when the parameter is being written by the database driver. When `false` is passed, a `DataTable`-based implementation is used. It maintains an internal copy of the data, with validation performed as the data is copied from user objects into the table.

</div>



## Pooling

<div align="justify">

To minimize allocation overhead, object pooling is used. `DbDataReader`-based implementation reuses the entire parameter, whereas the `DataTable`-based implementation reuses only the table structure. Upon disposal, each parameter is cleared of user data and returned to its pool.

</div>



## Npgsql.Tvp

<div align="justify">

The plugin optimizes internal buffer usage during remote calls by classifying parameters as variable-size or constant-size based on column metadata (types and nullability) and row count. For variable-size parameters, column values are copied into the internal buffer. For constant-size parameters, data is streamed directly into the internal buffer because the size of each row is known.

</div>



> [!WARNING]
> For `DbDataReader`-based parameters, the `RecordsAffected` property returns the total number of rows rather than the number of rows affected by DML operations.



# References

- [Table-Valued Parameters - ADO.NET](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql/table-valued-parameters)