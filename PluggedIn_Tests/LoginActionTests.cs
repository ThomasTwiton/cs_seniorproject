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
    public class LoginActionTests
    {

        private readonly PluggedContext LoadedContext;


        /* Login Testing */

        [Fact]
        public void Login_WhenProfileAssociatedWithUser_ReturnsRedirectToProfile()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = true;

            // The following convention will be used for users and passwords:
            //                  email : FirstnameLastname@unit.test
            //      unhashed password : bestRA
            //   Targeted User's Salt : unsafe3
            //       Misc Users' Salt : wrong

            var uData = new List<User>() {
                new User { UserId = 1, Email = "ElijasReshmi@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "unsafe3" }, // Has Profile
                new User { UserId = 2, Email = "EugeniaCornelius@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Profile
                new User { UserId = 3, Email = "JaysonFerruccio@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Ensemble
                new User { UserId = 4, Email = "HoratioRajendra@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Ensemble
                new User { UserId = 5, Email = "SaraRachyl@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Venue
                new User { UserId = 6, Email = "ChikeluChanda@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" }    // Has Venue
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 11, Ensemble_Name = "Small Garage Band", UserId = 3 },
                new Ensemble { EnsembleId = 12, Ensemble_Name = "Smooth Jazz Combo", UserId = 4 }
            }.AsQueryable();

            var vData = new List<Venue>
            {
                new Venue { VenueId = 11, Venue_Name = "Big Stadium", UserId = 5 },
                new Venue { VenueId = 12, Venue_Name = "Small Cafe", UserId = 6 }
            }.AsQueryable();

            // Create Mocked DB sets
            var mockUsers = new Mock<DbSet<User>>();
            mockUsers.As<IQueryable<User>>().Setup(u => u.Provider).Returns(uData.Provider);
            mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(uData.Expression);
            mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(uData.ElementType);
            mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(uData.GetEnumerator());

            var mockProfiles = new Mock<DbSet<Profile>>();
            mockProfiles.As<IQueryable<Profile>>().Setup(u => u.Provider).Returns(pData.Provider);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(pData.Expression);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(pData.ElementType);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(pData.GetEnumerator());

            var mockEnsembles = new Mock<DbSet<Ensemble>>();
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(u => u.Provider).Returns(eData.Provider);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.Expression).Returns(eData.Expression);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.ElementType).Returns(eData.ElementType);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.GetEnumerator()).Returns(eData.GetEnumerator());

            var mockVenues = new Mock<DbSet<Venue>>();
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.Provider).Returns(vData.Provider);
            mockVenues.As<IQueryable<Venue>>().Setup(m => m.Expression).Returns(vData.Expression);
            mockVenues.As<IQueryable<Venue>>().Setup(m => m.ElementType).Returns(vData.ElementType);
            mockVenues.As<IQueryable<Venue>>().Setup(m => m.GetEnumerator()).Returns(vData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Venues)
                .Returns(mockVenues.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

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
            var result = controller.Login("ElijasReshmi@unit.test", "bestRA");

            /* Assert */
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Profile", redirectResult.ActionName);
            Assert.Equal(11, redirectResult.RouteValues["id"]);

        }

        [Fact]
        public void Login_WhenEnsembleAssociatedWithUser_ReturnsRedirectToEnsemble()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 3;
            var aLoggedIn = true;

            // The following convention will be used for users and passwords:
            //                  email : FirstnameLastname@unit.test
            //      unhashed password : bestRA
            //   Targeted User's Salt : unsafe3
            //       Misc Users' Salt : wrong

            var uData = new List<User>() {
                new User { UserId = 1, Email = "ElijasReshmi@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" }, // Has Profile
                new User { UserId = 2, Email = "EugeniaCornelius@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Profile
                new User { UserId = 3, Email = "JaysonFerruccio@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "unsafe3" },   // Has Ensemble
                new User { UserId = 4, Email = "HoratioRajendra@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Ensemble
                new User { UserId = 5, Email = "SaraRachyl@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Venue
                new User { UserId = 6, Email = "ChikeluChanda@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" }    // Has Venue
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 21, Ensemble_Name = "Small Garage Band", UserId = 3 },
                new Ensemble { EnsembleId = 22, Ensemble_Name = "Smooth Jazz Combo", UserId = 4 }
            }.AsQueryable();

            var vData = new List<Venue>
            {
                new Venue { VenueId = 31, Venue_Name = "Big Stadium", UserId = 5 },
                new Venue { VenueId = 32, Venue_Name = "Small Cafe", UserId = 6 }
            }.AsQueryable();

            // Create Mocked DB sets
            var mockUsers = new Mock<DbSet<User>>();
            mockUsers.As<IQueryable<User>>().Setup(u => u.Provider).Returns(uData.Provider);
            mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(uData.Expression);
            mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(uData.ElementType);
            mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(uData.GetEnumerator());

            var mockProfiles = new Mock<DbSet<Profile>>();
            mockProfiles.As<IQueryable<Profile>>().Setup(u => u.Provider).Returns(pData.Provider);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(pData.Expression);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(pData.ElementType);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(pData.GetEnumerator());

            var mockEnsembles = new Mock<DbSet<Ensemble>>();
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(u => u.Provider).Returns(eData.Provider);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.Expression).Returns(eData.Expression);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.ElementType).Returns(eData.ElementType);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.GetEnumerator()).Returns(eData.GetEnumerator());

            var mockVenues = new Mock<DbSet<Venue>>();
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.Provider).Returns(vData.Provider);
            mockVenues.As<IQueryable<Venue>>().Setup(m => m.Expression).Returns(vData.Expression);
            mockVenues.As<IQueryable<Venue>>().Setup(m => m.ElementType).Returns(vData.ElementType);
            mockVenues.As<IQueryable<Venue>>().Setup(m => m.GetEnumerator()).Returns(vData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Venues)
                .Returns(mockVenues.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

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
            var result = controller.Login("JaysonFerruccio@unit.test", "bestRA");

            /* Assert */
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Ensemble", redirectResult.ActionName);
            Assert.Equal(21, redirectResult.RouteValues["id"]);
        }

        [Fact]
        public void Login_WhenVenueAssociatedWithUser_ReturnsRedirectToVenue()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 3;
            var aLoggedIn = true;

            // The following convention will be used for users and passwords:
            //                  email : FirstnameLastname@unit.test
            //      unhashed password : bestRA
            //   Targeted User's Salt : unsafe3
            //       Misc Users' Salt : wrong

            var uData = new List<User>() {
                new User { UserId = 1, Email = "ElijasReshmi@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" }, // Has Profile
                new User { UserId = 2, Email = "EugeniaCornelius@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Profile
                new User { UserId = 3, Email = "JaysonFerruccio@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Ensemble
                new User { UserId = 4, Email = "HoratioRajendra@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Ensemble
                new User { UserId = 5, Email = "SaraRachyl@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "unsafe3" },   // Has Venue
                new User { UserId = 6, Email = "ChikeluChanda@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" }    // Has Venue
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 21, Ensemble_Name = "Small Garage Band", UserId = 3 },
                new Ensemble { EnsembleId = 22, Ensemble_Name = "Smooth Jazz Combo", UserId = 4 }
            }.AsQueryable();

            var vData = new List<Venue>
            {
                new Venue { VenueId = 31, Venue_Name = "Big Stadium", UserId = 5 },
                new Venue { VenueId = 32, Venue_Name = "Small Cafe", UserId = 6 }
            }.AsQueryable();

            // Create Mocked DB sets
            var mockUsers = new Mock<DbSet<User>>();
            mockUsers.As<IQueryable<User>>().Setup(u => u.Provider).Returns(uData.Provider);
            mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(uData.Expression);
            mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(uData.ElementType);
            mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(uData.GetEnumerator());

            var mockProfiles = new Mock<DbSet<Profile>>();
            mockProfiles.As<IQueryable<Profile>>().Setup(u => u.Provider).Returns(pData.Provider);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(pData.Expression);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(pData.ElementType);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(pData.GetEnumerator());

            var mockEnsembles = new Mock<DbSet<Ensemble>>();
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(u => u.Provider).Returns(eData.Provider);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.Expression).Returns(eData.Expression);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.ElementType).Returns(eData.ElementType);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.GetEnumerator()).Returns(eData.GetEnumerator());

            var mockVenues = new Mock<DbSet<Venue>>();
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.Provider).Returns(vData.Provider);
            mockVenues.As<IQueryable<Venue>>().Setup(m => m.Expression).Returns(vData.Expression);
            mockVenues.As<IQueryable<Venue>>().Setup(m => m.ElementType).Returns(vData.ElementType);
            mockVenues.As<IQueryable<Venue>>().Setup(m => m.GetEnumerator()).Returns(vData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Venues)
                .Returns(mockVenues.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

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
            var result = controller.Login("SaraRachyl@unit.test", "bestRA");

            /* Assert */
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Venue", redirectResult.ActionName);
            Assert.Equal(31, redirectResult.RouteValues["id"]);
        }

        [Fact]
        public void Login_WhenInvalidEmail_ReturnsIndexViewWithViewData()
        {
            /* Arrange */

            // The following convention will be used for users and passwords:
            //                  email : FirstnameLastname@unit.test
            //      unhashed password : bestRA
            //   Targeted User's Salt : unsafe3
            //       Misc Users' Salt : wrong

            var uData = new List<User>() {
                new User { UserId = 1, Email = "ElijasReshmi@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },
                new User { UserId = 2, Email = "EugeniaCornelius@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" }
            }.AsQueryable();

            // Create Mocked DB sets
            var mockUsers = new Mock<DbSet<User>>();
            mockUsers.As<IQueryable<User>>().Setup(u => u.Provider).Returns(uData.Provider);
            mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(uData.Expression);
            mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(uData.ElementType);
            mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(uData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Create a controller
            var controller = new HomeController(mockDB.Object, mockHostEnv.Object);


            /* Act */
            var result = controller.Login("JaysonFerruccio@unit.test", "bestRA");

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("Index", viewResult.ViewName);
            Assert.Equal("Email not registered", viewResult.ViewData["Error"]);
        }

        [Fact]
        public void Login_WhenInvalidPassword_ReturnsIndexViewWithViewData()
        {
            /* Arrange */

            // The following convention will be used for users and passwords:
            //                  email : FirstnameLastname@unit.test
            //      unhashed password : bestRA
            //   Targeted User's Salt : unsafe3
            //       Misc Users' Salt : wrong

            var uData = new List<User>() {
                new User { UserId = 1, Email = "ElijasReshmi@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },
                new User { UserId = 2, Email = "EugeniaCornelius@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" }
            }.AsQueryable();

            // Create Mocked DB sets
            var mockUsers = new Mock<DbSet<User>>();
            mockUsers.As<IQueryable<User>>().Setup(u => u.Provider).Returns(uData.Provider);
            mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(uData.Expression);
            mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(uData.ElementType);
            mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(uData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Create a controller
            var controller = new HomeController(mockDB.Object, mockHostEnv.Object);


            /* Act */
            var result = controller.Login("ElijasReshmi@unit.test", "worstRA");

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("Index", viewResult.ViewName);
            Assert.Equal("Username and password do not match", viewResult.ViewData["Error"]);
        }

        [Fact]
        public void Login_NoProfileEnsembleVenueAssociatedWithUser_ReturnsCreateProfileView()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 7;
            var aLoggedIn = true;

            // The following convention will be used for users and passwords:
            //                  email : FirstnameLastname@unit.test
            //      unhashed password : bestRA
            //   Targeted User's Salt : unsafe3
            //       Misc Users' Salt : wrong

            var uData = new List<User>() {
                new User { UserId = 1, Email = "ElijasReshmi@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" }, // Has Profile
                new User { UserId = 2, Email = "EugeniaCornelius@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Profile
                new User { UserId = 3, Email = "JaysonFerruccio@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Ensemble
                new User { UserId = 4, Email = "HoratioRajendra@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Ensemble
                new User { UserId = 5, Email = "SaraRachyl@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Venue
                new User { UserId = 6, Email = "ChikeluChanda@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },    // Has Venue
                new User { UserId = aUserId, Email = "YatingNeo@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "unsafe3" } // No association
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 11, Ensemble_Name = "Small Garage Band", UserId = 3 },
                new Ensemble { EnsembleId = 12, Ensemble_Name = "Smooth Jazz Combo", UserId = 4 }
            }.AsQueryable();

            var vData = new List<Venue>
            {
                new Venue { VenueId = 11, Venue_Name = "Big Stadium", UserId = 5 },
                new Venue { VenueId = 12, Venue_Name = "Small Cafe", UserId = 6 }
            }.AsQueryable();

            var iData = new List<Instrument>
            {
                new Instrument() { InstrumentId = 1, Instrument_Name = "Piano" },
                    new Instrument() { InstrumentId = 2, Instrument_Name = "Voice" },
                    new Instrument() { InstrumentId = 3, Instrument_Name = "Violin" },
                    new Instrument() { InstrumentId = 4, Instrument_Name = "Viola" },
                    new Instrument() { InstrumentId = 5, Instrument_Name = "Cello" }
            }.AsQueryable();

            // Create Mocked DB sets
            var mockUsers = new Mock<DbSet<User>>();
            mockUsers.As<IQueryable<User>>().Setup(u => u.Provider).Returns(uData.Provider);
            mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(uData.Expression);
            mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(uData.ElementType);
            mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(uData.GetEnumerator());

            var mockProfiles = new Mock<DbSet<Profile>>();
            mockProfiles.As<IQueryable<Profile>>().Setup(u => u.Provider).Returns(pData.Provider);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(pData.Expression);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(pData.ElementType);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(pData.GetEnumerator());

            var mockEnsembles = new Mock<DbSet<Ensemble>>();
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(u => u.Provider).Returns(eData.Provider);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.Expression).Returns(eData.Expression);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.ElementType).Returns(eData.ElementType);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.GetEnumerator()).Returns(eData.GetEnumerator());

            var mockVenues = new Mock<DbSet<Venue>>();
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.Provider).Returns(vData.Provider);
            mockVenues.As<IQueryable<Venue>>().Setup(m => m.Expression).Returns(vData.Expression);
            mockVenues.As<IQueryable<Venue>>().Setup(m => m.ElementType).Returns(vData.ElementType);
            mockVenues.As<IQueryable<Venue>>().Setup(m => m.GetEnumerator()).Returns(vData.GetEnumerator());

            var mockInstrument = new Mock<DbSet<Instrument>>();
            mockInstrument.As<IQueryable<Instrument>>().Setup(u => u.Provider).Returns(iData.Provider);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.Expression).Returns(iData.Expression);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.ElementType).Returns(iData.ElementType);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.GetEnumerator()).Returns(iData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Venues)
                .Returns(mockVenues.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.Instruments)
                .Returns(mockInstrument.Object);

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
            var result = controller.Login("YatingNeo@unit.test", "bestRA");

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("CreateProfile", viewResult.ViewName);
            Assert.Equal(aUserId, viewResult.ViewData["id"]);
        }

        [Fact]
        public void Login_WhenPrevActionIsSet_ReturnsRedirectToThePrevAction()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = true;

            // The following convention will be used for users and passwords:
            //                  email : FirstnameLastname@unit.test
            //      unhashed password : bestRA
            //   Targeted User's Salt : unsafe3
            //       Misc Users' Salt : wrong

            var uData = new List<User>() {
                new User { UserId = 1, Email = "ElijasReshmi@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "unsafe3" }, // Has Profile
                new User { UserId = 2, Email = "EugeniaCornelius@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Profile
                new User { UserId = 3, Email = "JaysonFerruccio@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Ensemble
                new User { UserId = 4, Email = "HoratioRajendra@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Ensemble
                new User { UserId = 5, Email = "SaraRachyl@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },   // Has Venue
                new User { UserId = 6, Email = "ChikeluChanda@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" }    // Has Venue
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 11, Ensemble_Name = "Small Garage Band", UserId = 3 },
                new Ensemble { EnsembleId = 12, Ensemble_Name = "Smooth Jazz Combo", UserId = 4 }
            }.AsQueryable();

            var vData = new List<Venue>
            {
                new Venue { VenueId = 11, Venue_Name = "Big Stadium", UserId = 5 },
                new Venue { VenueId = 12, Venue_Name = "Small Cafe", UserId = 6 }
            }.AsQueryable();

            // Create Mocked DB sets
            var mockUsers = new Mock<DbSet<User>>();
            mockUsers.As<IQueryable<User>>().Setup(u => u.Provider).Returns(uData.Provider);
            mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(uData.Expression);
            mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(uData.ElementType);
            mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(uData.GetEnumerator());

            var mockProfiles = new Mock<DbSet<Profile>>();
            mockProfiles.As<IQueryable<Profile>>().Setup(u => u.Provider).Returns(pData.Provider);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(pData.Expression);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(pData.ElementType);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(pData.GetEnumerator());

            var mockEnsembles = new Mock<DbSet<Ensemble>>();
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(u => u.Provider).Returns(eData.Provider);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.Expression).Returns(eData.Expression);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.ElementType).Returns(eData.ElementType);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.GetEnumerator()).Returns(eData.GetEnumerator());

            var mockVenues = new Mock<DbSet<Venue>>();
            mockVenues.As<IQueryable<Venue>>().Setup(u => u.Provider).Returns(vData.Provider);
            mockVenues.As<IQueryable<Venue>>().Setup(m => m.Expression).Returns(vData.Expression);
            mockVenues.As<IQueryable<Venue>>().Setup(m => m.ElementType).Returns(vData.ElementType);
            mockVenues.As<IQueryable<Venue>>().Setup(m => m.GetEnumerator()).Returns(vData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Venues)
                .Returns(mockVenues.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

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
            var result = controller.Login("ElijasReshmi@unit.test", "bestRA");

            /* Assert */
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Profile", redirectResult.ActionName);
            Assert.Equal(11, redirectResult.RouteValues["id"]);
        }

        [Fact]
        public void Login_WhenPassedNoParameters_ReturnsLoginView()
        {
            /* Arrange */

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Create a controller
            var controller = new HomeController(LoadedContext, mockHostEnv.Object);
   
            /* Act */
            var result = controller.Login();

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("Login", viewResult.ViewName);

        }


        /* Logout Testing */

        [Fact]
        public void Logout_Always_ExpiresCookieAndRedirectsToIndex()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = true;

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Create a Mocked HomeController
            var controllerMock = new Mock<HomeController>(LoadedContext, mockHostEnv.Object);

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
            var result = controller.Logout();
            /* Assert */

            var retCookieData = controller.Response.Headers["Set-Cookie"].ToString();

            var startLoc = retCookieData.IndexOf("=",10);
            var endLoc = retCookieData.IndexOf(";", 12) - 1;

            var dateInfo = retCookieData.Substring(startLoc + 1, endLoc - startLoc);
        
            Assert.True(new System.TimeSpan(10,0,0,0) <= System.DateTime.Now - System.DateTime.Parse(dateInfo));

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }
}
