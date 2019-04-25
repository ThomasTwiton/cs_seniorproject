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
    public class CreateVenueActionTests
    {
        [Fact]
        public void CreateVenue_WhenGivenValidData_CreatesNewVenue() {
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

            List<Venue> addedVenues = new List<Venue>();

            mockDB.Setup(x => x.Add(It.IsAny<Venue>()))
                .Callback<Venue>(addedVenues.Add);


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

            ProfileModel model = new ProfileModel();
 
            /* Act */
            var result = controller.CreateVenue("Pink Concert Hall", "Main Street", "Apartment 2", "Chicago", "IL", "(123) 456-7890", "www.google.com", "Come for the concert", 6, model);

            /* Assert */
            Venue addedVenue = addedVenues.Last();
            Assert.Equal("Pink Concert Hall", addedVenue.Venue_Name);
            Assert.Equal("Main Street", addedVenue.Address1);
            Assert.Equal("Apartment 2", addedVenue.Address2);
            Assert.Equal("Chicago", addedVenue.City);
            Assert.Equal("IL", addedVenue.State);
            Assert.Equal("(123) 456-7890", addedVenue.Phone);
            Assert.Equal("www.google.com", addedVenue.Website);
        }

        [Fact]
        public void CreateVenue_WhenGivenValidData_ReturnsValidView() {
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

            List<Venue> addedVenues = new List<Venue>();

            mockDB.Setup(x => x.Add(It.IsAny<Venue>()))
                .Callback<Venue>(addedVenues.Add);


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

            ProfileModel model = new ProfileModel();

            /* Act */
            var result = controller.CreateVenue("Pink Concert Hall", "Main Street", "Apartment 2", "Chicago", "IL", "(123) 456-7890", "www.google.com", "Come for the concert", 6, model);

            /* Assert */
            Assert.IsType<RedirectToActionResult>(result.Result);
            RedirectToActionResult redirect = (RedirectToActionResult)result.Result;
            Assert.Equal("Venue", redirect.ActionName);

        }

        [Fact]
        public void CreateVenue_Always_HandlesDuplicateVenues() { }

        [Fact]
        public void CreateVnue_Always_HandlesMissingVenueData() { }
    }
}
