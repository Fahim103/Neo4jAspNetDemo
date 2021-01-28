using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jAspNetDemo
{
    public class HelloWorldExample : IDisposable
    {
        private readonly IDriver _driver;

        public HelloWorldExample(string url, string userName, string password)
        {
            _driver = GraphDatabase.Driver(url, AuthTokens.Basic(userName, password));
        }

        public async Task PrintGreeting(string message)
        {
            var session = _driver.AsyncSession();

            var greeting = await session.WriteTransactionAsync(async tx =>
            {
                var result = await tx.RunAsync(
                    "CREATE (a:Greeting) SET a.message = $message RETURN a.message + ', from node ' + id(a)",
                    new { message }
                );

                var data = await result.SingleAsync();

                return data[0].As<string>();
            });

            Console.WriteLine(greeting);

            await session.CloseAsync();
            
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
