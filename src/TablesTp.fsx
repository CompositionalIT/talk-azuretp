#load @"..\packages\FSharp.Azure.StorageTypeProvider\StorageTypeProvider.fsx"
open FSharp.Azure.StorageTypeProvider
open FSharp.Azure.StorageTypeProvider.Table

type StorageAccount = AzureTypeProvider<"UseDevelopmentStorage=true">

// Get table
let table = StorageAccount.Tables.transactions


// Get partition
let rows = table.GetPartition "ADUR"
rows.[0]


// Optional inference - with Locality
rows.[0].District.EndsWith "Sea"




// Get results
let txn = table.Get(Row "2ac10e50-3e2a-1af6-e050-a8c063052ba1", Partition "ADUR")





// Queries
let results =
    table.Query()
         .``Where Locality Is``.``Equal To``("SOUTHWICK")
         .``Where Price Is``.``Greater Than``(500000)
         .Execute()

results.Length

// Static schema
[<Literal>]
let Schema = __SOURCE_DIRECTORY__ + @"\TableSchema.json"
type StaticAccount = AzureTypeProvider<"UseDevelopmentStorage=true", tableSchema = Schema>






// Analysis with charts
#r @"..\packages\FSharp.Plotly\lib\net45\FSharp.Plotly.dll"

open FSharp.Plotly

let transactions =
   StorageAccount.Tables.transactions
                        .Query()
                        .``Where Price Is``.``Less Than``(500000)
                        .Execute(5000)




transactions
|> Array.countBy(fun r -> r.County)
|> Array.sortByDescending snd
|> Array.take 10
|> Chart.Column
|> Chart.Show



