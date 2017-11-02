#load @"..\packages\FSharp.Azure.StorageTypeProvider\StorageTypeProvider.fsx"

open FSharp.Azure.StorageTypeProvider
open FSharp.Azure.StorageTypeProvider.Blob
open FSharp.Azure.StorageTypeProvider.Table
open FSharp.Azure.StorageTypeProvider.Queue

type StorageAccount = AzureTypeProvider<"UseDevelopmentStorage=true", autoRefresh = 5>

// Blobs

// Tables

// Queues
