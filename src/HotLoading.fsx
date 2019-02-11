#load @"..\packages\FSharp.Azure.StorageTypeProvider\StorageTypeProvider.fsx"

open FSharp.Azure.StorageTypeProvider
open FSharp.Azure.StorageTypeProvider.Blob
open FSharp.Azure.StorageTypeProvider.Table
open FSharp.Azure.StorageTypeProvider.Queue

type StorageAccount = AzureTypeProvider<"UseDevelopmentStorage=true", autoRefresh = 5>



// Blobs
let f = StorageAccount.Containers.files.``pp-2016.csv``

// Tables
let txns = StorageAccount.Tables.transactions
let p = txns.GetPartition "ADUR"

let isByTheSea = p.[0].Town.EndsWith "SEA"




// Queues
let theQueue = StorageAccount.Queues.``sample-queue``

// Enqueue a message
theQueue.Enqueue("HELLO From Osnbrück") |> Async.Start

// Peek at the queue!
//let msg = theQueue.Peek


// Dequeue a message
let dequeued = theQueue.Dequeue() |> Async.RunSynchronously

// Inspect the contents
dequeued.Value.Contents.Value

// Delete it from the queue
theQueue.DeleteMessage(dequeued.Value.Id) |> Async.Start