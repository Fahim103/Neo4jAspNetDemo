using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jAspNetDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hello = new HelloWorldExample("bolt://localhost:7687", "demo", "demo");
            await hello.PrintGreeting("Hello Neo4j");
            Console.ReadLine();
        }
    }
}
