#load @"..\packages\FSharp.Azure.StorageTypeProvider\StorageTypeProvider.fsx"
open FSharp.Azure.StorageTypeProvider
open FSharp.Azure.StorageTypeProvider.Table

type StorageAccount = AzureTypeProvider<"UseDevelopmentStorage=true">

// Get table
let table = StorageAccount.Tables.transactions

// Get partition
let p = table.GetPartition "ADUR"

// Get results
let txn = table.Get(Row "2ac10e50-3e2a-1af6-e050-a8c063052ba1", Partition "ADUR")

// Optional inference

// Queries

// Static schema




















// Analysis with charts
#r @"..\packages\FSharp.Plotly\lib\net45\FSharp.Plotly.dll"

open FSharp.Plotly

let transactions =
   StorageAccount.Tables.transactions
                        .Query()
                        .``Where Price Is``.``Less Than``(500000)
                        .Execute(5000)

let labels, values =
   transactions
   |> Array.groupBy(fun r -> r.County)
   |> Array.map(fun (county, values) -> county, values |> Array.sumBy(fun v -> float v.Price))
   |> Array.sortByDescending snd
   |> Array.take 10
   |> Array.unzip

Chart.Pie(values, labels)
|> Chart.Show