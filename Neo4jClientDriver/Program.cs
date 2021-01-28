using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jClientDriver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new GraphClient(new Uri("http://localhost:7474/"), username: "demo", password: "demo");
            await client.ConnectAsync();

            // Create book, person and relationship between them

            var createQuery = await client.Cypher.Create("(b:Book {Title: 'Test', PageCount: 200}) <-[rel:HAS_BOOK]-(p:Person {Name: 'Fahim'})")
                .Return((b, p) => new 
                { 
                    Book = b.As<Book>(),
                    Person = p.As<Person>()
                }).ResultsAsync;

            foreach (var item in createQuery)
            {
                Console.WriteLine($"{item.Person.Name} has book with title {item.Book.Title}");
            }


            // Filter items with where clause

            var query = await client.Cypher
                .Match(matchText: "(c)-[HAS_BOOK]->(b)")
                .Where((Book b) => b.PageCount > 190)
                .Return<Book>(identity: "b").ResultsAsync;

            var firstBook = query.FirstOrDefault();
            if (firstBook != null)
            {
                Console.WriteLine($"{firstBook.Title} has {firstBook.PageCount} pages");
            }
            else
            {
                Console.WriteLine("No Book found with given conditions");
            }

            Console.ReadLine();
        }
    }
}
