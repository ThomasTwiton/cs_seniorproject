using Xunit;
using System;
using server.Models;
using Xunit.Abstractions;
using server.Controllers;
using System.Collections;
using Microsoft.AspNetCore.Mvc;

namespace PluggedIn_Tests
{

    public class HomeControllerTests
    {
        /* The following tests assume the database is seeded in 
         * accordance to release v0.6-mvp. 
         */

        private readonly PluggedContext LoadedContext;
        private ITestOutputHelper output;

        public void MyTestClass(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Index_ReturnValidView()
        {
            // Arrange
            var controller = new HomeController(LoadedContext);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Login_ReturnRedirect()
        {
            // Arrange
            var controller = new HomeController(LoadedContext);

            string testEmail = "miley@cyrus.com";
            string testPassword = "bestObothWorlds";
            

            // Act
            var result = controller.Login(testEmail, testPassword);

            // Assert
            var redirectResult = Assert.IsType<RedirectToRouteResult>(result);

            Assert.Equal("Home/Profile", redirectResult.RouteName);
            Assert.Equal("", redirectResult.RouteValues.ToString());
            
        }
        
    }
}
