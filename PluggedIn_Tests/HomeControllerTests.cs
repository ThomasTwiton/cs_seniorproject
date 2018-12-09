using Xunit;
using server.Models;
using server.Controllers;

namespace PluggedIn_Tests
{
    public class HomeControllerTests
    {
        private readonly PluggedContext LoadedContext;

        [Fact]
        public void IndexReturnValidView()
        {
            // Arrange
            var controller = new HomeController(LoadedContext);

            // Act
            var result = controller.Index();
        }
    }
}
