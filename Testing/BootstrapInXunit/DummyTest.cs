using System;
using Microsoft.Extensions.Options;
using Xunit;

namespace BootstrapInXunit
{
    [Collection("Bootstrap")]
    public class Dummy
    {
        private IServiceProvider _provider;

        public Dummy(BootstrapFixture fixture)
        {
            _provider = fixture.Provider;
        }

        [Fact]
        public void Test()
        {
            var op = (IOptions<CommonOptions>)_provider.GetService(typeof(IOptions<CommonOptions>));

            Assert.Equal("DummyConnection", op.Value.ConnectionString);
        }
    }
}