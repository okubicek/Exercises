using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HttpClientRegistration
{
    class Program
    {
        static async Task Main(string[] args)
        {            
            var services = new ServiceCollection();
            services.Register<FirstClient, IFirstClient>("First");
            services.Register<SecondClient, ISecondClient>("Second");

            var provider = services.BuildServiceProvider();
            var f = provider.GetService<IFirstClient>();
            var s = provider.GetService<ISecondClient>();

            await f.Call();
            await s.Call();

            var fs = provider.GetService<IFirstClient>();
            await fs.Call();

            Console.Read();
        }
    }

    public static class ServiceCollectionExtension
    {
        public static void Register<T, IT>(this IServiceCollection services, string message) where T : class, IT
            where IT : class
        {
            services.AddHttpClient<IT, T>()                
                .AddHttpMessageHandler(() => {
                    return new MessageHandler(message);
                });
        }
    }

    public class MessageHandler : DelegatingHandler
    {
        private string _message;
        public MessageHandler(string message)
        {
            _message = message;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine(_message);
            return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage());
        }
    }

    public class FirstClient : IFirstClient
    {        
        private HttpClient _client;
        public FirstClient(HttpClient client)
        {
            _client = client;
        }

        public Task Call()
        {
            _client.BaseAddress = new Uri("http://dummy.com");
            _client.SendAsync(new HttpRequestMessage());

            return Task.CompletedTask;
        }
    }

    public interface IFirstClient
    {
        Task Call();
    }

    public class SecondClient : ISecondClient
    {
        private HttpClient _client;
        public SecondClient(HttpClient client)
        {
            _client = client;
        }

        public Task Call()
        {
            _client.BaseAddress = new Uri("http://dummy.com");
            _client.SendAsync(new HttpRequestMessage());

            return Task.CompletedTask;
        }
    }

    public interface ISecondClient
    {
        Task Call();
    }
}
