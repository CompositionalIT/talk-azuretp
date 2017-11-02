#load @"..\packages\FSharp.Azure.StorageTypeProvider\StorageTypeProvider.fsx"

open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Blob

let storageAccount = CloudStorageAccount.Parse "UseDevelopmentStorage=true"

/// Navigating through storage accounts
let client = storageAccount.CreateCloudBlobClient()

// Get all containers
let containers = client.ListContainers() |> Seq.toArray

// print the names out
containers |> Array.iter(fun c -> printfn "%s" c.Name)

let file = client.GetContainerReference("files").GetBlockBlobReference("pp-2016.csv")
file.DownloadText()


















// Example of type-safe version of above
let getBlobSafe container blob =
  client.ListContainers()
  |> Seq.tryFind(fun c -> c.Name = container)
  |> Option.bind(fun container ->
    container.ListBlobs(useFlatBlobListing = true)
    |> Seq.tryPick(function
    | :? CloudBlockBlob as b when b.Name = blob -> Some b
    | _ -> None))

// Safe usage
let maybeFile = getBlobSafe "files" "pp-2016.csv"

match maybeFile with
| Some blob -> printfn "Downloaded %s" blob.Name
| None -> printfn "Got nothing!"


