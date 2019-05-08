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

namespace PluggedIn_Tests
{
    public class VenueActionTests
    {
        [Fact]
        public void Venue_WhenUserOwnsPage_SetsIsOwnerToTrue()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = true;

            var vData = new List<Venue>
            {
                new Venue { VenueId = 31, Venue_Name = "Big Stadium", UserId = 1 },
                new Venue { VenueId = 32, Venue_Name = "Small Cafe", UserId = 2 }
            }.AsQueryable();

            var poData = new List<Post> { }.AsQueryable();

            // Create Mocked DB sets
            var mockVenues = new Mock<DbSet<Venue>>();
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.Provider).Returns(vData.Provider);
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.Expression).Returns(vData.Expression);
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.ElementType).Returns(vData.ElementType);
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.GetEnumerator()).Returns(vData.GetEnumerator());

            var mockPosts = new Mock<DbSet<Post>>();
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Expression).Returns(poData.Expression);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.ElementType).Returns(poData.ElementType);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.GetEnumerator()).Returns(poData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Venues)
                .Returns(mockVenues.Object);

            mockDB.Setup(x => x.Posts)
                .Returns(mockPosts.Object);

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Create a Mocked HomeController
            var controllerMock = new Mock<HomeController>(mockDB.Object, mockHostEnv.Object);

            // Create a ControllerContext and set the HttpContext to be the default
            //  This is done so that we can setup the behavior for GetSessionInfo()
            var controller = controllerMock.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var specifiedReq = controller.ControllerContext.HttpContext.Request;

            // Create the appropriate SessionModel to be returned by GetSessionInfo()
            SessionModel fakeSM = new SessionModel();
            fakeSM.IsLoggedIn = aLoggedIn;
            fakeSM.UserID = aUserId;

            // Set up GetSessionInfo method
            controllerMock.Setup(x => x.GetSessionInfo(specifiedReq)).Returns(fakeSM);
            controllerMock.CallBase = true;


            /* Act */
            var result = controller.Venue(31);

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            VenueModel viewModel = (VenueModel)viewResult.Model;

            Assert.Equal("Venue", viewResult.ViewName);
            Assert.True(viewModel.isOwner);
        }

        [Fact]
        public void Venue_Always_ReturnsAppropriateView()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 2;
            var aLoggedIn = false;

            var expectedVenue = new Venue { VenueId = 31, Venue_Name = "Big Stadium", UserId = 1 };

            var vData = new List<Venue>
            {
                expectedVenue,
                new Venue { VenueId = 32, Venue_Name = "Small Cafe", UserId = 2 }
            }.AsQueryable();

            var poData = new List<Post> { }.AsQueryable();

            // Create Mocked DB sets
            var mockVenues = new Mock<DbSet<Venue>>();
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.Provider).Returns(vData.Provider);
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.Expression).Returns(vData.Expression);
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.ElementType).Returns(vData.ElementType);
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.GetEnumerator()).Returns(vData.GetEnumerator());

            var mockPosts = new Mock<DbSet<Post>>();
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Expression).Returns(poData.Expression);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.ElementType).Returns(poData.ElementType);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.GetEnumerator()).Returns(poData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Venues)
                .Returns(mockVenues.Object);

            mockDB.Setup(x => x.Posts)
                .Returns(mockPosts.Object);

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Create a Mocked HomeController
            var controllerMock = new Mock<HomeController>(mockDB.Object, mockHostEnv.Object);

            // Create a ControllerContext and set the HttpContext to be the default
            //  This is done so that we can setup the behavior for GetSessionInfo()
            var controller = controllerMock.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var specifiedReq = controller.ControllerContext.HttpContext.Request;

            // Create the appropriate SessionModel to be returned by GetSessionInfo()
            SessionModel fakeSM = new SessionModel();
            fakeSM.IsLoggedIn = aLoggedIn;
            fakeSM.UserID = aUserId;

            // Set up GetSessionInfo method
            controllerMock.Setup(x => x.GetSessionInfo(specifiedReq)).Returns(fakeSM);
            controllerMock.CallBase = true;

            /* Act */
            var result = controller.Venue(31);

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            VenueModel viewModel = (VenueModel)viewResult.Model;

            Assert.Equal("Venue", viewResult.ViewName);
            Assert.Equal(expectedVenue, viewModel.Venue);
            Assert.False(viewModel.isOwner);
            Assert.False(viewModel.isLoggedIn);
            Assert.Equal("venue", viewModel.ViewType);
        }

        [Fact]
        public void Venue_Always_HandlesInvalidVenueId()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 2;
            var aLoggedIn = false;

            var vData = new List<Venue>
            {
                new Venue { VenueId = 31, Venue_Name = "Big Stadium", UserId = 1 },
                new Venue { VenueId = 32, Venue_Name = "Small Cafe", UserId = 2 }
            }.AsQueryable();

            // Create Mocked DB sets
            var mockVenues = new Mock<DbSet<Venue>>();
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.Provider).Returns(vData.Provider);
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.Expression).Returns(vData.Expression);
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.ElementType).Returns(vData.ElementType);
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.GetEnumerator()).Returns(vData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Venues)
                .Returns(mockVenues.Object);

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Create a Mocked HomeController
            var controllerMock = new Mock<HomeController>(mockDB.Object, mockHostEnv.Object);

            // Create a ControllerContext and set the HttpContext to be the default
            //  This is done so that we can setup the behavior for GetSessionInfo()
            var controller = controllerMock.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var specifiedReq = controller.ControllerContext.HttpContext.Request;

            // Create the appropriate SessionModel to be returned by GetSessionInfo()
            SessionModel fakeSM = new SessionModel();
            fakeSM.IsLoggedIn = aLoggedIn;
            fakeSM.UserID = aUserId;

            // Set up GetSessionInfo method
            controllerMock.Setup(x => x.GetSessionInfo(specifiedReq)).Returns(fakeSM);
            controllerMock.CallBase = true;

            /* Act */
            var result = controller.Venue(3333333);

            /* Assert */
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public void Venue_WhenPostsExist_DisplaysPosts()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = true;

            // The venue that is expected to be passed to the view model.
            var expectedVenue = new Venue { VenueId = 31, Venue_Name = "Big Stadium", UserId = 1 };

            var vData = new List<Venue>
            {
                expectedVenue,
                new Venue { VenueId = 32, Venue_Name = "Small Cafe", UserId = 2 }
            }.AsQueryable();

            var poData = new List<Post>
            {
                new Post { PostId = 44, PosterType = "venue", Text = "Test Text", PosterIndex = expectedVenue.VenueId},
                new Post { PostId = 45, PosterType = "venue", Text = "Test Text", PosterIndex = expectedVenue.VenueId},
                new Post { PostId = 46, PosterType = "ensemble", Text = "Test Text", PosterIndex = expectedVenue.VenueId},
                new Post { PostId = 47, PosterType = "venue", Text = "Test Text", PosterIndex = 12}

            }.AsQueryable();

            // Create Mocked DB sets
            var mockVenues = new Mock<DbSet<Venue>>();
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.Provider).Returns(vData.Provider);
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.Expression).Returns(vData.Expression);
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.ElementType).Returns(vData.ElementType);
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.GetEnumerator()).Returns(vData.GetEnumerator());

            var mockPosts = new Mock<DbSet<Post>>();
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Expression).Returns(poData.Expression);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.ElementType).Returns(poData.ElementType);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.GetEnumerator()).Returns(poData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Venues)
                .Returns(mockVenues.Object);

            mockDB.Setup(x => x.Posts)
                .Returns(mockPosts.Object);

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Create a Mocked HomeController
            var controllerMock = new Mock<HomeController>(mockDB.Object, mockHostEnv.Object);

            // Create a ControllerContext and set the HttpContext to be the default
            //  This is done so that we can setup the behavior for GetSessionInfo()
            var controller = controllerMock.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var specifiedReq = controller.ControllerContext.HttpContext.Request;

            // Create the appropriate SessionModel to be returned by GetSessionInfo()
            SessionModel fakeSM = new SessionModel();
            fakeSM.IsLoggedIn = aLoggedIn;
            fakeSM.UserID = aUserId;

            // Set up GetSessionInfo method
            controllerMock.Setup(x => x.GetSessionInfo(specifiedReq)).Returns(fakeSM);
            controllerMock.CallBase = true;


            /* Act */
            var result = controller.Venue(expectedVenue.VenueId);

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            VenueModel viewModel = (VenueModel)viewResult.Model;

            Assert.Equal("Venue", viewResult.ViewName);
            Assert.Equal(2, viewModel.Posts.Count);
        }
    }
}
