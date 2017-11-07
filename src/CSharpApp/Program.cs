using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;

namespace CSharpApp
{
    static class Blobs
    {
        static CloudStorageAccount account = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
        static CloudBlobClient blobClient = account.CreateCloudBlobClient();

        static public void ExploreStorageAccount()
        {
            var containers = blobClient.ListContainers();
            foreach (var container in containers)
            {
                Console.WriteLine("Container {0}:", container.Name);

                foreach (var blob in container.ListBlobs(useFlatBlobListing:true))
                    Console.WriteLine("\t\tBlob {0}", blob.Uri.ToString());
            }
        }

        static public void ExploreFile()
        {
            var file = blobClient.GetContainerReference("filse").GetBlockBlobReference("pp-2016.csv");

            var contents =
                file.DownloadText()
                    .Split('\n')
                    .Take(10)
                    .ToArray();

            foreach (var line in contents)
                Console.WriteLine(line);
        }
    }

    class Transaction : TableEntity
    {
        public int Price { get; set; }
        public DateTime SaleDate { get; set; }
        public string PostCode { get; set; } 
        public string PropertyType { get; set; }
        public string Locality { get; set; }
        public string Town { get; set; }
        public string District { get; set; }
        public string County { get; set; }
    }

    static class Tables
    {
        static CloudTable table = CloudStorageAccount.Parse("UseDevelopmentStorage=true")
                                                     .CreateCloudTableClient()
                                                     .GetTableReference("transactions");

        private static void PrintTransaction(Transaction prop)
        {
            Console.WriteLine("{0}: {1}, {2} - {3}",
                    prop.SaleDate.Date,
                    prop.District,
                    prop.County,
                    prop.Price);
        }

        public static void BasicQuery()
        {
            var theQuery = new TableQuery<Transaction>().Take(10);
            var properties = table.ExecuteQuery(theQuery).ToArray();
            foreach (var prop in properties)
                PrintTransaction(prop);
        }


        public static void BadQuery()
        {
            var badQuery = table.CreateQuery<Transaction>().Take(10);
            var properties = table.ExecuteQuery(badQuery).ToArray();
            foreach (var prop in properties)
                PrintTransaction(prop);
        }

        public static void IQueryableQuery()
        {
            var naughtyQuery =
                from txn in table.CreateQuery<Transaction>()
                where txn.PostCode.StartsWith("WD6")
                select txn;

            foreach (var prop in naughtyQuery.Take(10))
                PrintTransaction(prop);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Blobs.ExploreStorageAccount();
            //Blobs.ExploreFile();

            //Tables.BasicQuery();
            //Tables.BadQuery();
            Tables.IQueryableQuery();
        }
    }
}
