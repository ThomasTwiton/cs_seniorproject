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
using System.Collections;

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

            // Set desired UserId
            var activeUserId = 1;

            // Create a Mocked Profile table
            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = activeUserId },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = activeUserId + 1 }
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

            // Create a Mocked HomeController
            /*  Q: Why are we mocking the thing we are testing?
             *  A: We need to mock the Home Controller because of our login system
             *      cannot realistically be tested using unit tests. Instead, it
             *      would require integration tests. As a result, we mock our 
             *      HomeController so that we can mock the GetSessionInfo() method. 
             *      In doing this, we are able to specify whether a user is logged in
             *      and who they are. One very important thing to include in any test
             *      that has log in behavior, is the following line:
             *      
             *      controllerMock.CallBase = true;
             *      
             *      This line tells Moq that if a mocked method/action has not been
             *      specified, then it should call the method on an actual instance
             *      of HomeController. In doing so, we can test the appropriate 
             *      methods/actions while mocking the login system.
             */
            var controllerMock = new Mock<HomeController>(mockDB.Object, mockHostEnv.Object);

            // Mock the request object and the resulting login information
            SessionModel fakeSM = new SessionModel();
            fakeSM.IsLoggedIn = true;
            fakeSM.UserID = activeUserId;

            var controller = controllerMock.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var mockReq = controller.ControllerContext.HttpContext.Request;

            controllerMock.Setup(x => x.GetSessionInfo(mockReq)).Returns(fakeSM);
            controllerMock.CallBase = true;


            /* Act */
            var result = controller.Index();

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Profile", viewResult.ViewName);

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
