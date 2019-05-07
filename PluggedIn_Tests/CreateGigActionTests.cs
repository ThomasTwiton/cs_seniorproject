using Moq;
using Xunit;
using System.Linq;
using server.Models;
using server.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace PluggedIn_Tests
{
    public class CreateGigActionTests
    {
        [Fact]
        public void CreateGig_WhenGivenValidData_CreatesNewGig() {
            /* Arrange */

            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Create DB data
            var uData = new List<User>
            {
                new User { UserId = 1, Password = "reshel01", Email = "ElijasReshmi@unit.test" },
                new User { UserId = 2, Password = "corneu01", Email = "EugeniaCornelius@unit.test" },
                new User { UserId = 3, Password = "ferrja01", Email = "JaysonFerruccio@unit.test" },
                new User { UserId = 4, Password = "rajeho01", Email = "HoratioRajendra@unit.test" },
                new User { UserId = 5, Password = "rachsa01", Email = "SaraRachyl@unit.test" }
            }.AsQueryable();

            var vData = new List<Venue>
            {
                new Venue { VenueId = 31, Venue_Name = "Green Bar", UserId = 1 },
                new Venue { VenueId = 32, Venue_Name = "Yellow Brewery", UserId = 2 }
            }.AsQueryable();


            // Create Mocked DB Sets
            var mockUsers = new Mock<DbSet<User>>();
            mockUsers.As<IQueryable<User>>().Setup(u => u.Provider).Returns(uData.Provider);
            mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(uData.Expression);
            mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(uData.ElementType);
            mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(uData.GetEnumerator());

            var mockVenue = new Mock<DbSet<Venue>>();
            mockVenue.As<IQueryable<Venue>>().Setup(u => u.Provider).Returns(vData.Provider);
            mockVenue.As<IQueryable<Venue>>().Setup(m => m.Expression).Returns(vData.Expression);
            mockVenue.As<IQueryable<Venue>>().Setup(m => m.ElementType).Returns(vData.ElementType);
            mockVenue.As<IQueryable<Venue>>().Setup(m => m.GetEnumerator()).Returns(vData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Venues)
                .Returns(mockVenue.Object);

            List<Gig> addedGigs = new List<Gig>();

            mockDB.Setup(x => x.Add(It.IsAny<Gig>()))
                .Callback<Gig>(addedGigs.Add);

            var controllerMock = new Mock<HomeController>(mockDB.Object, mockHostEnv.Object);

            // Mock the request object and the resulting login information
            SessionModel fakeSM = new SessionModel();
            fakeSM.IsLoggedIn = true;
            fakeSM.UserID = 1;

            var controller = controllerMock.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var mockReq = controller.ControllerContext.HttpContext.Request;

            controllerMock.Setup(x => x.GetSessionInfo(mockReq)).Returns(fakeSM);
            controllerMock.CallBase = true;


            /* Act */

            var result = controller.CreateGig(new System.DateTime(2019, 4, 24), new System.DateTime(2019, 6, 24), new System.TimeSpan(2, 14, 18), "Yes", "Jazz", "Looking for a band to play", 1, 31);

            /* Assert */
            Gig addedGig = addedGigs.Last();
            Assert.Equal("Jazz", addedGig.Genre);
            Assert.Equal("Looking for a band to play\n This is a repeating gig", addedGig.Description);
            Assert.Equal(new System.DateTime(2019, 4, 24, 2, 14, 18), addedGig.Gig_Date);
            Assert.Equal(new System.DateTime(2019, 6, 24), addedGig.Closed_Date);
        }

        [Fact]
        public void CreateGig_WhenGivenValidData_ReturnsValidView() {
            /* Arrange */

            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Create DB data
            var uData = new List<User>
            {
                new User { UserId = 1, Password = "reshel01", Email = "ElijasReshmi@unit.test" },
                new User { UserId = 2, Password = "corneu01", Email = "EugeniaCornelius@unit.test" },
                new User { UserId = 3, Password = "ferrja01", Email = "JaysonFerruccio@unit.test" },
                new User { UserId = 4, Password = "rajeho01", Email = "HoratioRajendra@unit.test" },
                new User { UserId = 5, Password = "rachsa01", Email = "SaraRachyl@unit.test" }
            }.AsQueryable();

            var vData = new List<Venue>
            {
                new Venue { VenueId = 31, Venue_Name = "Green Bar", UserId = 1 },
                new Venue { VenueId = 32, Venue_Name = "Yellow Brewery", UserId = 2 }
            }.AsQueryable();


            // Create Mocked DB Sets
            var mockUsers = new Mock<DbSet<User>>();
            mockUsers.As<IQueryable<User>>().Setup(u => u.Provider).Returns(uData.Provider);
            mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(uData.Expression);
            mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(uData.ElementType);
            mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(uData.GetEnumerator());

            var mockVenue = new Mock<DbSet<Venue>>();
            mockVenue.As<IQueryable<Venue>>().Setup(u => u.Provider).Returns(vData.Provider);
            mockVenue.As<IQueryable<Venue>>().Setup(m => m.Expression).Returns(vData.Expression);
            mockVenue.As<IQueryable<Venue>>().Setup(m => m.ElementType).Returns(vData.ElementType);
            mockVenue.As<IQueryable<Venue>>().Setup(m => m.GetEnumerator()).Returns(vData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Venues)
                .Returns(mockVenue.Object);

            List<Gig> addedGigs = new List<Gig>();

            mockDB.Setup(x => x.Add(It.IsAny<Gig>()))
                .Callback<Gig>(addedGigs.Add);

            var controllerMock = new Mock<HomeController>(mockDB.Object, mockHostEnv.Object);

            // Mock the request object and the resulting login information
            SessionModel fakeSM = new SessionModel();
            fakeSM.IsLoggedIn = true;
            fakeSM.UserID = 1;

            var controller = controllerMock.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var mockReq = controller.ControllerContext.HttpContext.Request;

            controllerMock.Setup(x => x.GetSessionInfo(mockReq)).Returns(fakeSM);
            controllerMock.CallBase = true;


            /* Act */

            var result = controller.CreateGig(new System.DateTime(2019, 4, 24), new System.DateTime(2019, 6, 24), new System.TimeSpan(2,14,18), "no", "Jazz", "Looking for a band to play", 1, 31);

            /* Assert */
            Assert.IsType<RedirectToActionResult>(result.Result);
            RedirectToActionResult redirect = (RedirectToActionResult)result.Result;
            Assert.Equal("Venue", redirect.ActionName);
        }

        [Fact]
        public void CreateGig_Always_HandlesDuplicateGigs() { }

        [Fact]
        public void CreateGig_Always_HandlesMissingGigData() { }
    }
}
