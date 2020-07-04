using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DIContainer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddTransient<ISomeInterface, SomeImplementation>();
            services.AddTransient<SomeClient>();
            services.AddTransient<Func<string, Task>>(c => async (string s) => { 
                    Console.WriteLine($"wrap {s} va");
                    await Task.Delay(500);
                });

            services.AddTransient<Func<string, Task>>(c => async (string s) => { 
                    Console.WriteLine($"blii {s}");
                    await Task.Delay(500);
                });

            var provider = services.BuildServiceProvider();

            var client = provider.GetService<SomeClient>();
            await client.RunAsync();

            Console.Read();
        }
    }

    public class SomeClient
    {
        private ISomeInterface _some;

        private IEnumerable<Func<string, Task>> _actionsToPerform;

        public SomeClient(ISomeInterface some)
        {
            _some = some;
        }

        public SomeClient(ISomeInterface some, IEnumerable<Func<string, Task>> actionsToPerform) //: this(some)
        {
            _some = some;
            _actionsToPerform = actionsToPerform;
        }

        public async Task RunAsync()
        {
            _some.PrintSomething("Something");
            if (_actionsToPerform != null)
            {
                foreach(var action in _actionsToPerform)
                {
                    await action("tralala");
                }
            }
        }
    }

    public interface ISomeInterface
    {
        void PrintSomething(string text);
    }

    public class SomeImplementation : ISomeInterface
    {
        public void PrintSomething(string text)
        {
            Console.WriteLine(text);
        }
    }
}