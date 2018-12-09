using Xunit;
using server.Models;
using server.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace PluggedIn_Tests
{
    public class HomeControllerTests
    {
        private readonly PluggedContext LoadedContext;

        [Fact]
        public void Index_ReturnValidView()
        {
            // Arrange
            var controller = new HomeController(LoadedContext);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
