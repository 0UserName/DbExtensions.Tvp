![Release workflow](https://github.com/0UserName/dbextensions.Tvp/actions/workflows/release.yml/badge.svg)



# Motivation

<div align="justify">

The library simplifies creating table-valued parameters that can be consumed by `SQL Server` and `PostgreSQL` (via the [Npgsql.Tvp](https://github.com/0UserName/npgsql.tvp) plugin).

</div>



# Usage

<div align="justify">

Inherit `AbstractTableValued<>` with your class as the type parameter and add `TableMetadata` attribute with specified name of the parameter previously created on the server. After that you can build table-valued parameter from your rows:

</div>



```csharp
[TableMetadata(nameof(UserRow))]
public sealed class UserRow : AbstractTableValued<UserRow>
{
    public int Property0 
    { 
        get; 
        set; 
    }
    
    public int Property1 
    { 
        get; 
        set; 
    }
    
    public int Property2
    { 
        get; 
        set; 
    }
    
    public int Property3 
    { 
        get; 
        set; 
    }
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

In this example, the table-valued parameter structure is based on class metadata, which may differ from the database schema. Mismatches are typically detected only at runtime during remote calls. Structure and data validation can be performed using external metadata, which can be specified explicitly or loaded directly from the database:

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
> The `Table` argument is used only for grouping columns in user code after they are loaded from the database.



> [!WARNING]
>  Metadata is applied once during class initialization.



# Constraints

<div align="justify">

The following constraints are currently supported for user data validation: [AllowDBNull](https://learn.microsoft.com/en-us/dotnet/api/system.data.datacolumn.allowdbnull?view=net-10.0), [MaxLength](https://learn.microsoft.com/en-us/dotnet/api/system.data.datacolumn.maxlength?view=net-10.0) and [Unique](https://learn.microsoft.com/en-us/dotnet/api/system.data.datacolumn.unique?view=net-10.0). If a constraint is violated, a [ConstraintException](https://learn.microsoft.com/en-us/dotnet/api/system.data.constraintexception?view=net-10.0) is thrown.

</div>



> [!NOTE]
> `DbDataReader`-based parameters is forward-only, so it supports only the `AllowDBNull` constraint during enumeration. `DataTable`-based parameters support all constraints, with validation occurring immediately after rows are loaded.



# Ordinals

<div align="justify">

When external metadata is provided, ordinals remain synchronized with the database schema and unused columns are automatically filtered - columns removed from the database are ignored during parameter building. Otherwise, ordinals are determined by class property order.

</div>



# Performance considerations


## Expression trees

<div align="justify">

Row values are accessed through compiled lambda expressions constructed for each class and cached for reuse. These lambdas encapsulate property getters and null-checks, eliminating reflection overhead.

</div>



## Parameter types

<div align="justify">

The `Build` method accepts a Boolean argument that determines the underlying implementation. When `true` is passed, a lightweight `DbDataReader`-based implementation is used, which acts as an enumerator over user objects without storing data, with structure and data validation performed during enumeration. When `false` is passed, a `DataTable`-based implementation is used, which maintains an internal copy of the data, with full structure and constraint validation performed during data copying from user objects to the table.

</div>



## Pooling

<div align="justify">

To minimize allocation overhead, pooling is used: `DbDataReader`-based pooling reuses the entire parameter, while `DataTable`-based pooling reuses only the table structure. On disposal, each parameter is cleared of user data and returned to its pool.

</div>



## Npgsql.Tvp

<div align="justify">

The plugin optimizes internal buffer usage during remote calls by classifying parameters as variable-size or constant-size using column metadata (types, nullability) and row count. For variable-size parameters, column values are copied to the internal buffer; for constant-size parameters, the buffer streams data since each row has a known size.

</div>



> [!WARNING]
> For `DbDataReader`-based parameters, the `RecordsAffected` property specifies the total row count instead of rows affected by DML operations.



# References

- [Table-Valued Parameters - ADO.NET](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql/table-valued-parameters)