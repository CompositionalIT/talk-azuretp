#load @"..\packages\FSharp.Azure.StorageTypeProvider\StorageTypeProvider.fsx"

open FSharp.Azure.StorageTypeProvider
open FSharp.Azure.StorageTypeProvider.Blob
type StorageAccount = AzureTypeProvider<"UseDevelopmentStorage=true">

let file = StorageAccount.Containers.files.``pp-2016.csv``

file.ReadLines() |> Seq.take 5 |> Seq.toArray