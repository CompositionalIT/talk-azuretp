#I @"..\packages\"
#load @"FSharp.Azure.StorageTypeProvider\StorageTypeProvider.fsx"
#r @"FSharp.Plotly\lib\net45\FSharp.Plotly.dll"

open FSharp.Azure.StorageTypeProvider
open FSharp.Azure.StorageTypeProvider.Table
type StorageAccount = AzureTypeProvider<"UseDevelopmentStorage=true">

// Get table

// Get results

// Get partition

// Optional inference

// Queries

// Static schema




















// Analysis with charts

//open FSharp.Plotly
//
//let transactions =
//    StorageAccount.Tables.transactions
//                         .Query()
//                         .``Where Price Is``.``Less Than``(500000)
//                         .Execute(5000)
//
//let labels, values =
//    transactions
//    |> Array.groupBy(fun r -> r.County)
//    |> Array.map(fun (county, values) -> county, values |> Array.sumBy(fun v -> v.Price))
//    |> Array.sortByDescending snd
//    |> Array.take 10
//    |> Array.unzip
//
//Chart.Pie(values, labels)
//|> Chart.Show