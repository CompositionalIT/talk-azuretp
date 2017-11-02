#load @"..\packages\FSharp.Azure.StorageTypeProvider\StorageTypeProvider.fsx"

open FSharp.Azure.StorageTypeProvider
open FSharp.Azure.StorageTypeProvider.Table
open System
open System.IO

type Storage = AzureTypeProvider<"UseDevelopmentStorage=true">

let table = Storage.Tables.transactions

type Row =
    { Price : int
      Date : DateTime
      PostCode : string
      PropertyType : string
      Town : string
      Region : string
      County : string }

let clear (s:string) =
    let s = s.Replace("\"", "")
    if (String.IsNullOrWhiteSpace s) then null else s

let data =
    File.ReadLines @"C:\Users\Isaac\Downloads\Datasets\pp-2016.csv"
    |> Seq.map(fun row ->
        let fields = row.Split ','
        let postCode = fields.[3] |> clear
        let county = fields.[13] |> clear
        let partition = Partition county
        let row = Row (sprintf "%s - %s" postCode (clear fields.[2]))
        let record =
            { Price = fields.[1] |> clear |> int
              Date = fields.[2] |> clear |> DateTime.Parse
              PostCode = postCode
              PropertyType = fields.[4] |> clear
              Town = fields.[11] |> clear
              Region = fields.[12] |> clear
              County = county }
        partition, row, record)
    |> Seq.take 5000
    |> Seq.toArray

table.Insert data




