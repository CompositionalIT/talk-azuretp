#load @"..\packages\FSharp.Azure.StorageTypeProvider\StorageTypeProvider.fsx"
#r "System.Text.Encoding"

open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Blob
let storageAccount = CloudStorageAccount.Parse "UseDevelopmentStorage=true"

/// Navigating through storage accounts
let client = storageAccount.CreateCloudBlobClient()

// Get all containers
let containers = client.ListContainersSegmentedAsync(null).Result.Results |> Seq.toArray

// print the names out
containers |> Array.iter(fun c -> printfn "%s" c.Name)

let file = client.GetContainerReference("files").GetBlockBlobReference("pp-2016.csv")
let text = file.DownloadTextAsync().Result

















// Example of type-safe version of above
let getBlobSafe container blob = async {
    let! containers = client.ListContainersSegmentedAsync(null) |> Async.AwaitTask

    match containers.Results |> Seq.tryFind(fun c -> c.Name = container) with
    | None -> return None
    | Some container ->
        let! blobs =
            container.ListBlobsSegmentedAsync(null, true, BlobListingDetails.All, System.Nullable(), null, null, null, System.Threading.CancellationToken.None)
            |> Async.AwaitTask

        return
            blobs.Results
            |> Seq.tryPick(function
            | :? CloudBlockBlob as b when b.Name = blob -> Some b
            | _ -> None) }

let container = "files"
let blob = "pp-2016.csv"



// Safe usage
let maybeFile = getBlobSafe "files" "pp-2016.csv" |> Async.RunSynchronously

match maybeFile with
| Some blob -> printfn "Downloaded %s" blob.Name
| None -> printfn "Got nothing!"