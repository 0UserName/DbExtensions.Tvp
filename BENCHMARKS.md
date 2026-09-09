# Environment

```
BenchmarkDotNet v0.15.8, Windows 11 (10.0.22631.3007/23H2/2023Update/SunValley3)
AMD Ryzen 7 3700X 3.60GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.400
  [Host]     : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3
  Job-KTQWFO : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3
```



Arguments:



```
IterationCount=20  LaunchCount=5  WarmupCount=10
```



# Description

<div align="justify">

At the moment, the measurements have only been performed for the process of creating and populating `DataTableParameter<TRow>` compared to `DataTable`.

</div>



# 0.1.3



```csharp
// Binder lambda: Action<DataTable, TRow, object[]>

buffer[0] = userObj.GetValue(0);
buffer[1] = userObj.GetValue(1);
buffer[2] = userObj.GetValue(2);
```



replaced to



```csharp
// Binder lambda: Action<DataTable, TRow, object[]>

buffer[0] = userObj.Property0;
buffer[1] = userObj.Property1;
buffer[2] = userObj.Property2;
```



<div align="justify">

Accessing properties through `GetValue` (a compiled lambda) is slower than accessing them directly.

</div>



| Method     | RowCount | Iterations | Mean       | Error     | StdDev    | Median     | Ratio | RatioSD | Gen0       | Gen1       | Allocated | Alloc Ratio |
|----------- |--------- |----------- |-----------:|----------:|----------:|-----------:|------:|--------:|-----------:|-----------:|----------:|------------:|
| NaiveNew   | 100      | 100        |   3.048 ms | 0.0176 ms | 0.0501 ms |   3.050 ms |  1.00 |    0.02 |   433.5938 |    35.1563 |   3.47 MB |        1.00 |
| NaiveReuse | 100      | 100        |   3.353 ms | 0.0149 ms | 0.0421 ms |   3.345 ms |  1.10 |    0.02 |   328.1250 |    15.6250 |   2.71 MB |        0.78 |
| TvpPooled  | 100      | 100        |   3.469 ms | 0.0223 ms | 0.0628 ms |   3.457 ms |  1.14 |    0.03 |   296.8750 |    15.6250 |   2.48 MB |        0.71 |
|            |          |            |            |           |           |            |       |         |            |            |           |             |
| NaiveNew   | 500      | 100        |  15.970 ms | 0.0811 ms | 0.2352 ms |  15.952 ms |  1.00 |    0.02 |  2078.1250 |   531.2500 |  16.69 MB |        1.00 |
| NaiveReuse | 500      | 100        |  17.800 ms | 0.1330 ms | 0.3923 ms |  17.698 ms |  1.11 |    0.03 |  1656.2500 |   343.7500 |  13.23 MB |        0.79 |
| TvpPooled  | 500      | 100        |  18.277 ms | 0.0710 ms | 0.2060 ms |  18.307 ms |  1.14 |    0.02 |  1500.0000 |   375.0000 |  12.09 MB |        0.72 |
|            |          |            |            |           |           |            |       |         |            |            |           |             |
| NaiveNew   | 1000     | 100        |  32.768 ms | 0.1932 ms | 0.5573 ms |  32.782 ms |  1.00 |    0.02 |  4125.0000 |  1812.5000 |  32.92 MB |        1.00 |
| NaiveReuse | 1000     | 100        |  35.938 ms | 0.1882 ms | 0.5518 ms |  35.854 ms |  1.10 |    0.02 |  3214.2857 |  1142.8571 |  25.89 MB |        0.79 |
| TvpPooled  | 1000     | 100        |  37.615 ms | 0.1472 ms | 0.4316 ms |  37.543 ms |  1.15 |    0.02 |  2928.5714 |    71.4286 |   23.6 MB |        0.72 |
|            |          |            |            |           |           |            |       |         |            |            |           |             |
| NaiveNew   | 1500     | 100        |  52.431 ms | 0.7384 ms | 2.1422 ms |  51.793 ms |  1.00 |    0.06 |  6600.0000 |  3700.0000 |  52.71 MB |        1.00 |
| NaiveReuse | 1500     | 100        |  56.017 ms | 0.4476 ms | 1.3058 ms |  55.343 ms |  1.07 |    0.05 |  4800.0000 |  1500.0000 |  38.55 MB |        0.73 |
| TvpPooled  | 1500     | 100        |  58.013 ms | 0.1809 ms | 0.5160 ms |  57.963 ms |  1.11 |    0.04 |  4333.3333 |   111.1111 |  35.12 MB |        0.67 |
|            |          |            |            |           |           |            |       |         |            |            |           |             |
| NaiveNew   | 5000     | 100        | 185.706 ms | 0.7366 ms | 2.1487 ms | 185.447 ms |  1.00 |    0.02 | 23000.0000 | 15000.0000 | 183.98 MB |        1.00 |
| NaiveReuse | 5000     | 100        | 205.439 ms | 2.1154 ms | 5.9667 ms | 204.907 ms |  1.11 |    0.03 | 15666.6667 |  7000.0000 | 127.16 MB |        0.69 |
| TvpPooled  | 5000     | 100        | 218.508 ms | 2.0289 ms | 5.9183 ms | 218.209 ms |  1.18 |    0.03 | 14333.3333 |  6666.6667 | 115.72 MB |        0.63 |



# 0.1.2



```csharp
// Binder lambda: Func<TRow, DataRow, DataRow>

if(!userObj.IsDBNull(0)) row.SetField(0, userObj.GetValue(0);
if(!userObj.IsDBNull(1)) row.SetField(1, userObj.GetValue(1);
if(!userObj.IsDBNull(2)) row.SetField(2, userObj.GetValue(2);

// DataRowExtensions.SetField internally does this internally: row[column] = (object?)value ?? DBNull.Value;

return row;
```



replaced to



```csharp
// Binder lambda: Action<DataTable, TRow, object[]>

buffer[0] = userObj.GetValue(0);
buffer[1] = userObj.GetValue(1);
buffer[2] = userObj.GetValue(2);

Rows.Add(buffer); // Add(Object[])	
```



<div align="justify">

Creating a `DataRow` and then populating its fields one by one with values obtained from `GetValue` (a compiled lambda), after checking for null using `IsDBNull` (also via a compiled lambda), consumes more memory and is slower than simply passing an array of values without checking for null and implicitly creating the `DataRow`.

</div>



| Method     | RowCount | Iterations | Mean       | Error     | StdDev    | Median     | Ratio | RatioSD | Gen0       | Gen1       | Allocated | Alloc Ratio |
|----------- |--------- |----------- |-----------:|----------:|----------:|-----------:|------:|--------:|-----------:|-----------:|----------:|------------:|
| NaiveNew   | 100      | 100        |   3.182 ms | 0.0252 ms | 0.0731 ms |   3.192 ms |  1.00 |    0.03 |   433.5938 |    35.1563 |   3.47 MB |        1.00 |
| NaiveReuse | 100      | 100        |   3.426 ms | 0.0210 ms | 0.0611 ms |   3.439 ms |  1.08 |    0.03 |   335.9375 |    15.6250 |   2.71 MB |        0.78 |
| TvpPooled  | 100      | 100        |   3.886 ms | 0.0223 ms | 0.0655 ms |   3.877 ms |  1.22 |    0.03 |   308.5938 |    15.6250 |   2.48 MB |        0.71 |
|            |          |            |            |           |           |            |       |         |            |            |           |             |
| NaiveNew   | 500      | 100        |  16.845 ms | 0.0695 ms | 0.2005 ms |  16.858 ms |  1.00 |    0.02 |  2062.5000 |   500.0000 |  16.69 MB |        1.00 |
| NaiveReuse | 500      | 100        |  18.056 ms | 0.0884 ms | 0.2566 ms |  17.996 ms |  1.07 |    0.02 |  1656.2500 |   343.7500 |  13.23 MB |        0.79 |
| TvpPooled  | 500      | 100        |  20.469 ms | 0.1437 ms | 0.4170 ms |  20.418 ms |  1.22 |    0.03 |  1500.0000 |   375.0000 |  12.09 MB |        0.72 |
|            |          |            |            |           |           |            |       |         |            |            |           |             |
| NaiveNew   | 1000     | 100        |  33.991 ms | 0.1933 ms | 0.5547 ms |  33.959 ms |  1.00 |    0.02 |  4066.6667 |  1733.3333 |  32.92 MB |        1.00 |
| NaiveReuse | 1000     | 100        |  37.346 ms | 0.2099 ms | 0.6056 ms |  37.239 ms |  1.10 |    0.03 |  3214.2857 |  1142.8571 |  25.89 MB |        0.79 |
| TvpPooled  | 1000     | 100        |  41.582 ms | 0.2020 ms | 0.5862 ms |  41.554 ms |  1.22 |    0.03 |  2916.6667 |    83.3333 |   23.6 MB |        0.72 |
|            |          |            |            |           |           |            |       |         |            |            |           |             |
| NaiveNew   | 1500     | 100        |  52.213 ms | 0.3035 ms | 0.8900 ms |  52.192 ms |  1.00 |    0.02 |  6600.0000 |  3700.0000 |  52.71 MB |        1.00 |
| NaiveReuse | 1500     | 100        |  56.440 ms | 0.2329 ms | 0.6684 ms |  56.501 ms |  1.08 |    0.02 |  4666.6667 |  1333.3333 |  38.55 MB |        0.73 |
| TvpPooled  | 1500     | 100        |  65.217 ms | 0.2946 ms | 0.8594 ms |  65.246 ms |  1.25 |    0.03 |  4375.0000 |   125.0000 |  35.12 MB |        0.67 |
|            |          |            |            |           |           |            |       |         |            |            |           |             |
| NaiveNew   | 5000     | 100        | 187.036 ms | 0.9591 ms | 2.7827 ms | 186.388 ms |  1.00 |    0.02 | 23000.0000 | 15000.0000 | 183.98 MB |        1.00 |
| NaiveReuse | 5000     | 100        | 208.628 ms | 1.1509 ms | 3.2460 ms | 208.530 ms |  1.12 |    0.02 | 15666.6667 |  7000.0000 | 127.16 MB |        0.69 |
| TvpPooled  | 5000     | 100        | 240.509 ms | 1.6084 ms | 4.5890 ms | 239.133 ms |  1.29 |    0.03 | 14333.3333 |  6666.6667 | 115.72 MB |        0.63 |



# 0.1.1

<div align="justify">

Removed `BeginLoadData` and `EndLoadData` calls from `DataTableParameter<TRow>.Load` due to their negative impact on overall performance.

</div>



| Method     | RowCount | Iterations | Mean       | Error     | StdDev    | Median     | Ratio | RatioSD | Gen0       | Gen1       | Allocated | Alloc Ratio |
|----------- |--------- |----------- |-----------:|----------:|----------:|-----------:|------:|--------:|-----------:|-----------:|----------:|------------:|
| NaiveNew   | 100      | 100        |   3.138 ms | 0.0163 ms | 0.0469 ms |   3.150 ms |  1.00 |    0.02 |   433.5938 |    35.1563 |   3.47 MB |        1.00 |
| NaiveReuse | 100      | 100        |   3.368 ms | 0.0180 ms | 0.0522 ms |   3.368 ms |  1.07 |    0.02 |   335.9375 |    15.6250 |   2.71 MB |        0.78 |
| TvpPooled  | 100      | 100        |   5.359 ms | 0.0737 ms | 0.2149 ms |   5.338 ms |  1.71 |    0.07 |   500.0000 |    23.4375 |   4.01 MB |        1.15 |
|            |          |            |            |           |           |            |       |         |            |            |           |             |
| NaiveNew   | 500      | 100        |  17.109 ms | 0.1626 ms | 0.4794 ms |  17.085 ms |  1.00 |    0.04 |  2062.5000 |   500.0000 |  16.69 MB |        1.00 |
| NaiveReuse | 500      | 100        |  18.298 ms | 0.1763 ms | 0.5170 ms |  18.270 ms |  1.07 |    0.04 |  1656.2500 |   343.7500 |  13.23 MB |        0.79 |
| TvpPooled  | 500      | 100        |  27.190 ms | 0.1570 ms | 0.4604 ms |  27.224 ms |  1.59 |    0.05 |  2468.7500 |   593.7500 |  19.72 MB |        1.18 |
|            |          |            |            |           |           |            |       |         |            |            |           |             |
| NaiveNew   | 1000     | 100        |  34.285 ms | 0.2029 ms | 0.5918 ms |  34.214 ms |  1.00 |    0.02 |  4066.6667 |  1733.3333 |  32.92 MB |        1.00 |
| NaiveReuse | 1000     | 100        |  36.998 ms | 0.2248 ms | 0.6558 ms |  36.959 ms |  1.08 |    0.03 |  3214.2857 |  1142.8571 |  25.89 MB |        0.79 |
| TvpPooled  | 1000     | 100        |  55.291 ms | 0.2585 ms | 0.7623 ms |  55.220 ms |  1.61 |    0.04 |  4800.0000 |   100.0000 |  38.86 MB |        1.18 |
|            |          |            |            |           |           |            |       |         |            |            |           |             |
| NaiveNew   | 1500     | 100        |  52.502 ms | 0.2273 ms | 0.6702 ms |  52.425 ms |  1.00 |    0.02 |  6600.0000 |  3700.0000 |  52.71 MB |        1.00 |
| NaiveReuse | 1500     | 100        |  57.292 ms | 0.2205 ms | 0.6433 ms |  57.246 ms |  1.09 |    0.02 |  4777.7778 |  1444.4444 |  38.55 MB |        0.73 |
| TvpPooled  | 1500     | 100        |  85.425 ms | 0.5227 ms | 1.5413 ms |  85.319 ms |  1.63 |    0.04 |  7166.6667 |   166.6667 |     58 MB |        1.10 |
|            |          |            |            |           |           |            |       |         |            |            |           |             |
| NaiveNew   | 5000     | 100        | 190.925 ms | 0.8823 ms | 2.5738 ms | 190.760 ms |  1.00 |    0.02 | 23000.0000 | 15000.0000 | 183.98 MB |        1.00 |
| NaiveReuse | 5000     | 100        | 212.231 ms | 3.1572 ms | 9.1093 ms | 207.833 ms |  1.11 |    0.05 | 15666.6667 |  7000.0000 | 127.16 MB |        0.69 |
| TvpPooled  | 5000     | 100        | 312.740 ms | 1.4254 ms | 4.1805 ms | 312.551 ms |  1.64 |    0.03 | 24000.0000 |  4000.0000 | 192.01 MB |        1.04 |



# 0.1.0



| Method     | RowCount | Iterations | Mean       | Error     | StdDev    | Median     | Ratio | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|----------- |--------- |----------- |-----------:|----------:|----------:|-----------:|------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| NaiveNew   | 100      | 100        |   3.146 ms | 0.0167 ms | 0.0476 ms |   3.158 ms |  1.00 |    0.02 |   433.5938 |    35.1563 |         - |   3.47 MB |        1.00 |
| NaiveReuse | 100      | 100        |   3.394 ms | 0.0171 ms | 0.0488 ms |   3.376 ms |  1.08 |    0.02 |   335.9375 |    15.6250 |         - |   2.71 MB |        0.78 |
| TvpPooled  | 100      | 100        |  11.437 ms | 0.0351 ms | 0.1008 ms |  11.436 ms |  3.64 |    0.06 |   984.3750 |    93.7500 |         - |   7.97 MB |        2.30 |
|            |          |            |            |           |           |            |       |         |            |            |           |           |             |
| NaiveNew   | 500      | 100        |  16.480 ms | 0.0712 ms | 0.2065 ms |  16.444 ms |  1.00 |    0.02 |  2062.5000 |   500.0000 |         - |  16.69 MB |        1.00 |
| NaiveReuse | 500      | 100        |  17.936 ms | 0.0879 ms | 0.2565 ms |  17.966 ms |  1.09 |    0.02 |  1656.2500 |   343.7500 |         - |  13.23 MB |        0.79 |
| TvpPooled  | 500      | 100        |  67.365 ms | 0.2711 ms | 0.7780 ms |  67.285 ms |  4.09 |    0.07 |  4750.0000 |   125.0000 |         - |  38.51 MB |        2.31 |
|            |          |            |            |           |           |            |       |         |            |            |           |           |             |
| NaiveNew   | 1000     | 100        |  34.002 ms | 0.1638 ms | 0.4779 ms |  34.009 ms |  1.00 |    0.02 |  4066.6667 |  1733.3333 |         - |  32.92 MB |        1.00 |
| NaiveReuse | 1000     | 100        |  37.405 ms | 0.4502 ms | 1.3062 ms |  36.866 ms |  1.10 |    0.04 |  3214.2857 |  1142.8571 |         - |  25.89 MB |        0.79 |
| TvpPooled  | 1000     | 100        | 144.646 ms | 0.9621 ms | 2.7758 ms | 144.079 ms |  4.25 |    0.10 |  9250.0000 |   250.0000 |         - |  75.01 MB |        2.28 |
|            |          |            |            |           |           |            |       |         |            |            |           |           |             |
| NaiveNew   | 1500     | 100        |  52.050 ms | 0.2332 ms | 0.6690 ms |  52.144 ms |  1.00 |    0.02 |  6600.0000 |  3700.0000 |         - |  52.71 MB |        1.00 |
| NaiveReuse | 1500     | 100        |  57.067 ms | 0.2040 ms | 0.5788 ms |  57.014 ms |  1.10 |    0.02 |  4666.6667 |  1333.3333 |         - |  38.55 MB |        0.73 |
| TvpPooled  | 1500     | 100        | 225.782 ms | 0.7223 ms | 2.1183 ms | 225.570 ms |  4.34 |    0.07 | 13666.6667 |   333.3333 |         - |  111.5 MB |        2.12 |
|            |          |            |            |           |           |            |       |         |            |            |           |           |             |
| NaiveNew   | 5000     | 100        | 188.773 ms | 1.1768 ms | 3.3574 ms | 189.335 ms |  1.00 |    0.03 | 23000.0000 | 15000.0000 |         - | 183.98 MB |        1.00 |
| NaiveReuse | 5000     | 100        | 209.876 ms | 1.0174 ms | 2.9515 ms | 209.052 ms |  1.11 |    0.03 | 15666.6667 |  7000.0000 |         - | 127.16 MB |        0.69 |
| TvpPooled  | 5000     | 100        | 878.034 ms | 3.1961 ms | 9.0146 ms | 876.556 ms |  4.65 |    0.10 | 46000.0000 | 29000.0000 | 1000.0000 | 366.95 MB |        1.99 |