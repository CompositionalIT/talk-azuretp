#load @"..\packages\FSharp.Azure.StorageTypeProvider\StorageTypeProvider.fsx"

open FSharp.Azure.StorageTypeProvider
open FSharp.Azure.StorageTypeProvider.Blob
type StorageAccount = AzureTypeProvider<"UseDevelopmentStorage=true">
