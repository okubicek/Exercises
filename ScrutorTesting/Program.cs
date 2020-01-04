using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace ScrutorTesting
{

    public interface IQuery<TIn, TOut>
    {
        TOut Get(TIn query);
    }

    public interface IGetAIds : IQuery<int, List<int>>
    {}

    public class GetAIds : IGetAIds
    {
        public List<int> Get(int query)
        {
            return new List<int>{ 1, 2 };
        }
    }

    public class GetBIds : IQuery<int, List<int>>
    {
        public List<int> Get(int query)
        {
            return new List<int> { 3 };
        }
    }

    public class BaseClass
    {
        public void PrintToConsole()
        {
            Console.WriteLine(this.ToString());
        }
    }

    public class A : BaseClass
    {
    }

    public class B : BaseClass
    {        
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.Scan(x => x.FromCallingAssembly()
                .AddClasses(c => c.AssignableTo<BaseClass>())
                .AsSelf()
                .WithScopedLifetime()
                
                .AddClasses(c => c.AssignableTo(typeof(IQuery<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                );

            var provider = serviceCollection.BuildServiceProvider();
            var c = provider.GetService<A>();
            c.PrintToConsole();

            TryResolveInterface<IGetAIds>(provider);
            TryResolveInterface<IQuery<int, List<int>>>(provider);
        }

        static void TryResolveInterface<T>(IServiceProvider provider)
        {
            var s = provider.GetService<T>();
            Console.WriteLine(s.GetType().ToString());
        }
    }
}
