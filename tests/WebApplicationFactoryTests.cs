using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Net.Http;
using System.Threading.Tasks;

namespace HelloHostTests
{
    [TestClass]
    public class HelloHostApplicationTestHarness : WebApplicationFactory<Program>
    {
        public HelloHostApplicationTestHarness()
            : base()
        {
        }

        [TestMethod]
        public async Task GetHomePage()
        {
            using var server = CreateServer(CreateWebHostBuilder());
            using var client = server.CreateClient();

            var response = await client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(responseString.Contains("Hello, World!"));
        }
    }
}
