using Moq;
using Xunit;
using System.Linq;
using server.Models;
using server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;

namespace PluggedIn_Tests
{
    public class GetSessionInfoTests
    {

        private readonly PluggedContext LoadedContext;
        private const string CookieUserId = "_UserID";
        private const string CookiePrevAct = "_PrevAction";

        [Fact]
        public void GetSessionInfo_WhenNoCookies_SetsLoggedInFalse()
        {
            /* Arrange */

            var userIdString = "";

            // Create a Mocked HttpRequest
            var mockedRequest = new Mock<HttpRequest>();

            mockedRequest.Setup(x => x.Cookies[CookieUserId])
                .Returns(userIdString);

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Create a HomeController
            var controller = new HomeController(LoadedContext, mockHostEnv.Object);

            /* Act */

            var result = controller.GetSessionInfo(mockedRequest.Object);

            /* Assert */
            var sessResult = Assert.IsType<SessionModel>(result);

            Assert.False(sessResult.IsLoggedIn);
        }

        [Fact]
        public void GetSessionInfo_WhenUserIdSet_SetsUserIdAndLoggedIn()
        {
            /* Arrange */

            var userIdString = "1";
            var prevActString = "";


            // Create a Mocked HttpRequest
            var mockedRequest = new Mock<HttpRequest>();

            mockedRequest.Setup(x => x.Cookies[CookieUserId])
                .Returns(userIdString);

            mockedRequest.Setup(x => x.Cookies[prevActString])
                .Returns(prevActString);

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Create a HomeController
            var controller = new HomeController(LoadedContext, mockHostEnv.Object);

            /* Act */

            var result = controller.GetSessionInfo(mockedRequest.Object);

            /* Assert */
            var sessResult = Assert.IsType<SessionModel>(result);

            Assert.Equal(Int32.Parse(userIdString), sessResult.UserID);
            Assert.Null(sessResult.PrevAction);
            Assert.True(sessResult.IsLoggedIn);
        }

        [Fact]
        public void  GetSessionInfo_WhenPrevActionSet_SetsPrevAction()
        {
            /* Arrange */

            var userIdString = "114243";
            var prevActString = "/Home/Audition/1";


            // Create a Mocked HttpRequest
            var mockedRequest = new Mock<HttpRequest>();

            mockedRequest.Setup(x => x.Cookies[CookieUserId])
                .Returns(userIdString);

            mockedRequest.Setup(x => x.Cookies[CookiePrevAct])
                .Returns(prevActString);

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Create a HomeController
            var controller = new HomeController(LoadedContext, mockHostEnv.Object);

            /* Act */

            var result = controller.GetSessionInfo(mockedRequest.Object);

            /* Assert */
            var sessResult = Assert.IsType<SessionModel>(result);

            Assert.Equal(Int32.Parse(userIdString), sessResult.UserID);
            Assert.Equal(prevActString, sessResult.PrevAction);
            Assert.True(sessResult.IsLoggedIn);
        }
    }
}
