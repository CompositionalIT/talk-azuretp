#load @"..\packages\FSharp.Azure.StorageTypeProvider\StorageTypeProvider.fsx"
#r @"..\packages\FSharp.Data\lib\net45\FSharp.Data.dll"

open FSharp.Azure.StorageTypeProvider
open FSharp.Azure.StorageTypeProvider.Table
open FSharp.Data
open System

type Storage = AzureTypeProvider<"UseDevelopmentStorage=true">
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

let data =
    (PricePaid.GetSample()).Rows
    |> Seq.map(fun row ->
        let record =
            { TransactionId = row.TransactionId
              Price = row.Price
              Date = DateTime.Parse row.Date
              PostCode = row.Postcode
              PropertyType = row.PropertyType
              Locality = row.Locality
              Town = row.``Town/City``
              District = row.District
              County = row.County }
        let partition = Partition row.District
        let row = Row (string row.TransactionId)
        partition, row, record)
    |> Seq.take 5000
    |> Seq.toArray

let resp = Storage.Tables.transactions.Insert data

resp |> Array.filter(fst >> (=) "EAST LINDSEY")