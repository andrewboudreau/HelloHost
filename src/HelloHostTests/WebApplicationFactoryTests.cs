using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;

namespace HelloHostTests
{
    [TestClass]
    public class WebApplicationFactoryTests
    {
        private readonly WebApplicationFactory<HelloHost.Startup> _factory;
        private HttpClient _client;

        public WebApplicationFactoryTests()
        {
            _factory = new WebApplicationFactory<HelloHost.Startup>();
            _client = _factory.CreateClient();
        }

        [TestMethod]
        public async Task GetHomePage()
        {
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(responseString.Contains("Welcome"));
        }
    }
}
