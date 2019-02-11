#load @"..\packages\FSharp.Azure.StorageTypeProvider\StorageTypeProvider.fsx"
#r @"..\packages\FSharp.Data\lib\net45\FSharp.Data.dll"

open FSharp.Azure.StorageTypeProvider
open FSharp.Azure.StorageTypeProvider.Table
open FSharp.Data
open System

type Storage = AzureTypeProvider<"UseDevelopmentStorage=true", autoRefresh=10>

type PricePaid = CsvProvider< @"C:\Users\Isaac\Downloads\Datasets\pp-2016.csv">

type Row =
    { TransactionId : Guid
      Price : int
      Date : DateTime
      PostCode : string
      PropertyType : string
      Town : string
      Locality : string
      District : string
      County : string }

let clean field = if String.IsNullOrWhiteSpace field then null else field

let data =
    (PricePaid.GetSample()).Rows
    |> Seq.map(fun row ->
        let record =
            { TransactionId = row.TransactionId
              Price = row.Price
              Date = (DateTime.Parse row.Date).ToUniversalTime().Date
              PostCode = clean row.Postcode
              PropertyType = clean row.PropertyType
              Locality = clean row.Locality
              Town = clean row.``Town/City``
              District = clean row.District
              County = clean row.County }
        let partition = Partition (clean row.District)
        let row = Row (string row.TransactionId)
        partition, row, record)
    |> Seq.take 5000
    |> Seq.toArray

let resp = Storage.Tables.transactions.Insert data

// Error handling
let response = Storage.Tables.transactions.Insert (data |> Seq.take 10)


// // 2MB
// // On the same partition

// let rec retryRequests getRow responses =   
//     let failedRequests =
//         responses
//         |> Array.Parallel.collect(fun (_, results) ->
//             results
//             |> Array.choose(function
//                 | BatchError(key, 429, _) -> Some key
//                 | _ -> None))
//         |> Array.map(fun (p,k) -> p, k, getRow(p,k))

//     printfn "Retrying %d failed requests" failedRequests.Length
//     match failedRequests with
//     | [||] -> ()
//     | _ ->
//         printfn "Trying to insert %d rows" failedRequests.Length
//         failedRequests
//         |> Storage.Tables.transactions.Insert
//         |> retryRequests getRow

// let getRow = data |> Array.map(fun (a,b,c) -> (a,b), c) |> Map.ofArray |> fun m -> m.TryFind >> Option.get

// let results =
//     data
//     |> Storage.Tables.transactions.Insert
// //    |> retryRequests getRow

// results
// |> Array.countBy(
//     snd >>
//     Array.pick(function | SuccessfulResponse (_,code) | BatchError (_, code, _) | EntityError(_, code, _) -> Some code | _ -> None))

// results |> Array.distinctBy fst |> Array.length

// results
// |> Array.countBy (snd >> Array.length)