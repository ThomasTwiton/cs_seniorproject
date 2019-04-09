using Moq;
using Xunit;
using System.Linq;
using server.Models;
using server.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace PluggedIn_Tests
{

    public class IndexActionTests
    {

        private readonly PluggedContext LoadedContext;
        private const string CookieUserId = "_UserID";
        private const string CookiePrevAct = "_PrevAction";

        [Fact]
        public void Index_WhenNotLoggedIn_ReturnsLandingPage()
        {
            /* Arrange */

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Create a Mocked IRequestCookieCollection with an empty cookie value
            var cookieCollection = new Mock<IRequestCookieCollection>();
            cookieCollection.Setup(c => c[CookieUserId]).Returns("");

            var controller = new HomeController(LoadedContext, mockHostEnv.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Cookies = cookieCollection.Object;

            /* Act */
            var result = controller.Index();

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", viewResult.ViewName);
        }

        [Fact]
        public void Index_WhenLoggedInAsProfile_ReturnsProfilePage()
        {
            /* Arrange */


            // Create a Mocked Profile table
            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            // Create a Mocked DB set
            var mockProfiles = new Mock<DbSet<Profile>>();
            mockProfiles.As<IQueryable<Profile>>().Setup(u => u.Provider).Returns(pData.Provider);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(pData.Expression);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(pData.ElementType);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(pData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Create a Mocked IRequestCookieCollection with an empty cookie value
            var cookieCollection = new Mock<IRequestCookieCollection>();
            cookieCollection.SetupGet(c => c[It.IsAny<string>()]).Returns("11");

            var controller = new HomeController(LoadedContext, mockHostEnv.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Cookies = cookieCollection.Object;

            

            /* Act */
            var result = controller.Index();

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Profile",viewResult.ViewName);

        }

        [Fact]
        public void Index_WhenLoggedInAsEnsemble_ReturnsEnsemblePage()
        {
            

        }

        [Fact]
        public void Index_WhenLoggedInAsVenue_ReturnsVenuePage()
        {

        }

    }
}
