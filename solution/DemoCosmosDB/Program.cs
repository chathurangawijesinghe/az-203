using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoCosmosDB
{
    class Program
    {
        static string DatabaseName = "maindb";
        static string CollectionName = "employee";
        static DocumentClient dc;

        static string endpoint = "https://azurecosmoscrw.documents.azure.com:443/";
        static string key = "1AnanOdj1f24LQvkABkxc5J7mmi307RSX477rJlhdATctzHXA1UpQSRJ2pt04ueFrCb7TcRw2RK7H72FIaH75g==";

        static void Main(string[] args)
        {
            dc = new DocumentClient(new Uri(endpoint), key);

            Insert(new Employee() { Firstname = "Chathuranga", Lastname = "Wijesinghe" });
            Insert(new Employee() { Firstname = "Tinu", Lastname = "Wijesinghe" });

            Query();

            Console.WriteLine("Hello World!");
        }

        static void Insert(Employee employee)
        {
            var result = dc.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName),
                employee
                ).GetAwaiter().GetResult();
        }

        static void Query()
        {
            FeedOptions queryOptions = new FeedOptions() { MaxItemCount = -1, EnableCrossPartitionQuery = true };

            IQueryable<Employee> query = dc.CreateDocumentQuery<Employee>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), queryOptions
                ).Where(w => w.Lastname == "Wijesinghe");

            var employees = query.ToList();

            foreach (var employee in employees)
            {
                Console.WriteLine(employee.Firstname);
            }

        }
    }

    class Employee
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
