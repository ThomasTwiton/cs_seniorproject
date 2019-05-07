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
    public class EnsembleActionTests
    {
        [Fact]
        public void Ensemble_Always_ReturnsAppropriateView()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = true;

            // The ensemble that is expected to be passed to the view model.
            var expectedEnsemble = new Ensemble { EnsembleId = 21, Ensemble_Name = "Small Garage Band", UserId = 1 };

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            var eData = new List<Ensemble>
            {
                expectedEnsemble,
                new Ensemble { EnsembleId = 22, Ensemble_Name = "Smooth Jazz Combo", UserId = 2 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 23}
            }.AsQueryable();

            var aData = new List<Audition>
            {
                new Audition { AuditionId = 1, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 21, Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice" },
                new Audition { AuditionId = 2, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 22,
                                Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice"},
            }.AsQueryable();

            var poData = new List<Post> { }.AsQueryable();

            var iData = new List<Instrument>
            {
                new Instrument() { InstrumentId = 1, Instrument_Name = "Piano" },
                new Instrument() { InstrumentId = 2, Instrument_Name = "Voice" },
                new Instrument() { InstrumentId = 3, Instrument_Name = "Violin" },
                new Instrument() { InstrumentId = 4, Instrument_Name = "Viola" },
                new Instrument() { InstrumentId = 5, Instrument_Name = "Cello" }
            }.AsQueryable();

            // Create Mocked DB sets
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

            var mockProfileEnsembles = new Mock<DbSet<ProfileEnsemble>>();
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Provider).Returns(peData.Provider);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Expression).Returns(peData.Expression);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.ElementType).Returns(peData.ElementType);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.GetEnumerator()).Returns(peData.GetEnumerator());

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(aData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(aData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(aData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(aData.GetEnumerator());

            var mockPosts = new Mock<DbSet<Post>>();
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Expression).Returns(poData.Expression);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.ElementType).Returns(poData.ElementType);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.GetEnumerator()).Returns(poData.GetEnumerator());

            var mockInstrument = new Mock<DbSet<Instrument>>();
            mockInstrument.As<IQueryable<Instrument>>().Setup(u => u.Provider).Returns(iData.Provider);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.Expression).Returns(iData.Expression);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.ElementType).Returns(iData.ElementType);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.GetEnumerator()).Returns(iData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.ProfileEnsembles)
                .Returns(mockProfileEnsembles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Posts)
                .Returns(mockPosts.Object);

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
            var result = controller.Ensemble(expectedEnsemble.EnsembleId);

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            EnsembleModel viewModel = (EnsembleModel)viewResult.Model;

            Assert.Equal("Ensemble", viewResult.ViewName);
            Assert.Equal(expectedEnsemble, viewModel.Ensemble);
        }

        [Fact]
        public void Ensemble_Always_HandlesInvalidEnsembleId()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = true;

            // The ensemble that is expected to be passed to the view model.
            var expectedEnsemble = new Ensemble { EnsembleId = 21, Ensemble_Name = "Small Garage Band", UserId = 1 };

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            var eData = new List<Ensemble>
            {
                expectedEnsemble,
                new Ensemble { EnsembleId = 22, Ensemble_Name = "Smooth Jazz Combo", UserId = 2 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 23}
            }.AsQueryable();

            var aData = new List<Audition>
            {
                new Audition { AuditionId = 1, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 21, Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice" },
                new Audition { AuditionId = 2, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 22,
                                Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice"},
            }.AsQueryable();

            var poData = new List<Post> { }.AsQueryable();

            var iData = new List<Instrument>
            {
                new Instrument() { InstrumentId = 1, Instrument_Name = "Piano" },
                new Instrument() { InstrumentId = 2, Instrument_Name = "Voice" },
                new Instrument() { InstrumentId = 3, Instrument_Name = "Violin" },
                new Instrument() { InstrumentId = 4, Instrument_Name = "Viola" },
                new Instrument() { InstrumentId = 5, Instrument_Name = "Cello" }
            }.AsQueryable();

            // Create Mocked DB sets
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

            var mockProfileEnsembles = new Mock<DbSet<ProfileEnsemble>>();
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Provider).Returns(peData.Provider);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Expression).Returns(peData.Expression);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.ElementType).Returns(peData.ElementType);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.GetEnumerator()).Returns(peData.GetEnumerator());

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(aData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(aData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(aData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(aData.GetEnumerator());

            var mockPosts = new Mock<DbSet<Post>>();
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Expression).Returns(poData.Expression);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.ElementType).Returns(poData.ElementType);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.GetEnumerator()).Returns(poData.GetEnumerator());

            var mockInstrument = new Mock<DbSet<Instrument>>();
            mockInstrument.As<IQueryable<Instrument>>().Setup(u => u.Provider).Returns(iData.Provider);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.Expression).Returns(iData.Expression);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.ElementType).Returns(iData.ElementType);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.GetEnumerator()).Returns(iData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.ProfileEnsembles)
                .Returns(mockProfileEnsembles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Posts)
                .Returns(mockPosts.Object);

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
            var result = controller.Ensemble(22222);

            /* Assert */
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public void Ensemble_WhenEnsembleHasMembers_DisplaysMembers()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = true;

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            // The ensemble that is expected to be passed to the view model.
            var expectedEnsemble = new Ensemble { EnsembleId = 21, Ensemble_Name = "Small Garage Band", UserId = 1 };

            var eData = new List<Ensemble>
            {
                expectedEnsemble,
                new Ensemble { EnsembleId = 22, Ensemble_Name = "Smooth Jazz Combo", UserId = 2 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 23}
            }.AsQueryable();

            var aData = new List<Audition>
            {
                new Audition { AuditionId = 1, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 21, Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice" },
                new Audition { AuditionId = 2, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 22,
                                Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice"},
            }.AsQueryable();

            var poData = new List<Post> { }.AsQueryable();

            var iData = new List<Instrument>
            {
                new Instrument() { InstrumentId = 1, Instrument_Name = "Piano" },
                new Instrument() { InstrumentId = 2, Instrument_Name = "Voice" },
                new Instrument() { InstrumentId = 3, Instrument_Name = "Violin" },
                new Instrument() { InstrumentId = 4, Instrument_Name = "Viola" },
                new Instrument() { InstrumentId = 5, Instrument_Name = "Cello" }
            }.AsQueryable();

            // Create Mocked DB sets
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

            var mockProfileEnsembles = new Mock<DbSet<ProfileEnsemble>>();
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Provider).Returns(peData.Provider);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Expression).Returns(peData.Expression);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.ElementType).Returns(peData.ElementType);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.GetEnumerator()).Returns(peData.GetEnumerator());

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(aData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(aData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(aData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(aData.GetEnumerator());

            var mockPosts = new Mock<DbSet<Post>>();
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Expression).Returns(poData.Expression);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.ElementType).Returns(poData.ElementType);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.GetEnumerator()).Returns(poData.GetEnumerator());

            var mockInstrument = new Mock<DbSet<Instrument>>();
            mockInstrument.As<IQueryable<Instrument>>().Setup(u => u.Provider).Returns(iData.Provider);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.Expression).Returns(iData.Expression);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.ElementType).Returns(iData.ElementType);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.GetEnumerator()).Returns(iData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.ProfileEnsembles)
                .Returns(mockProfileEnsembles.Object);

            mockDB.Setup(x => x.Posts)
                .Returns(mockPosts.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

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
            var result = controller.Ensemble(21);

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            EnsembleModel viewModel = (EnsembleModel)viewResult.Model;

            Assert.Equal(2, viewModel.Profiles.Count);
        }

        [Fact]
        public void Ensemble_WhenUserOwnsPage_SetsIsOwnerToTrue()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = true;

            // The ensemble that is expected to be passed to the view model.
            var expectedEnsemble = new Ensemble { EnsembleId = 21, Ensemble_Name = "Small Garage Band", UserId = 1 };

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            var eData = new List<Ensemble>
            {
                expectedEnsemble,
                new Ensemble { EnsembleId = 22, Ensemble_Name = "Smooth Jazz Combo", UserId = 2 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 23}
            }.AsQueryable();

            var aData = new List<Audition>
            {
                new Audition { AuditionId = 1, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 21, Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice" },
                new Audition { AuditionId = 2, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 22,
                                Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice"},
            }.AsQueryable();

            var poData = new List<Post> { }.AsQueryable();

            var iData = new List<Instrument>
            {
                new Instrument() { InstrumentId = 1, Instrument_Name = "Piano" },
                new Instrument() { InstrumentId = 2, Instrument_Name = "Voice" },
                new Instrument() { InstrumentId = 3, Instrument_Name = "Violin" },
                new Instrument() { InstrumentId = 4, Instrument_Name = "Viola" },
                new Instrument() { InstrumentId = 5, Instrument_Name = "Cello" }
            }.AsQueryable();

            // Create Mocked DB sets
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

            var mockProfileEnsembles = new Mock<DbSet<ProfileEnsemble>>();
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Provider).Returns(peData.Provider);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Expression).Returns(peData.Expression);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.ElementType).Returns(peData.ElementType);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.GetEnumerator()).Returns(peData.GetEnumerator());

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(aData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(aData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(aData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(aData.GetEnumerator());

            var mockPosts = new Mock<DbSet<Post>>();
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Expression).Returns(poData.Expression);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.ElementType).Returns(poData.ElementType);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.GetEnumerator()).Returns(poData.GetEnumerator());

            var mockInstrument = new Mock<DbSet<Instrument>>();
            mockInstrument.As<IQueryable<Instrument>>().Setup(u => u.Provider).Returns(iData.Provider);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.Expression).Returns(iData.Expression);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.ElementType).Returns(iData.ElementType);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.GetEnumerator()).Returns(iData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.ProfileEnsembles)
                .Returns(mockProfileEnsembles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Posts)
                .Returns(mockPosts.Object);

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
            var result = controller.Ensemble(expectedEnsemble.EnsembleId);

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            EnsembleModel viewModel = (EnsembleModel)viewResult.Model;

            Assert.Equal("Ensemble", viewResult.ViewName);
            Assert.True(viewModel.isOwner);
        }

        [Fact]
        public void Ensemble_WhenPostsExist_DisplaysPosts()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = true;

            // The ensemble that is expected to be passed to the view model.
            var expectedEnsemble = new Ensemble { EnsembleId = 21, Ensemble_Name = "Small Garage Band", UserId = 1 };

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            var eData = new List<Ensemble>
            {
                expectedEnsemble,
                new Ensemble { EnsembleId = 22, Ensemble_Name = "Smooth Jazz Combo", UserId = 2 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 23}
            }.AsQueryable();

            var aData = new List<Audition>
            {
                new Audition { AuditionId = 1, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 21, Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice" },
                new Audition { AuditionId = 2, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 22,
                                Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice"},
            }.AsQueryable();

            var poData = new List<Post>
            {
                new Post { PostId = 44, PosterType = "ensemble", Text = "Test Text", PosterIndex = expectedEnsemble.EnsembleId},
                new Post { PostId = 45, PosterType = "ensemble", Text = "Test Text", PosterIndex = expectedEnsemble.EnsembleId},
                new Post { PostId = 46, PosterType = "venue", Text = "Test Text", PosterIndex = expectedEnsemble.EnsembleId},
                new Post { PostId = 47, PosterType = "ensemble", Text = "Test Text", PosterIndex = 12}

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

            var mockProfileEnsembles = new Mock<DbSet<ProfileEnsemble>>();
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Provider).Returns(peData.Provider);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Expression).Returns(peData.Expression);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.ElementType).Returns(peData.ElementType);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.GetEnumerator()).Returns(peData.GetEnumerator());

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(aData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(aData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(aData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(aData.GetEnumerator());

            var mockPosts = new Mock<DbSet<Post>>();
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Expression).Returns(poData.Expression);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.ElementType).Returns(poData.ElementType);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.GetEnumerator()).Returns(poData.GetEnumerator());

            var mockInstrument = new Mock<DbSet<Instrument>>();
            mockInstrument.As<IQueryable<Instrument>>().Setup(u => u.Provider).Returns(iData.Provider);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.Expression).Returns(iData.Expression);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.ElementType).Returns(iData.ElementType);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.GetEnumerator()).Returns(iData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.ProfileEnsembles)
                .Returns(mockProfileEnsembles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Posts)
                .Returns(mockPosts.Object);

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
            var result = controller.Ensemble(expectedEnsemble.EnsembleId);

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            EnsembleModel viewModel = (EnsembleModel)viewResult.Model;

            Assert.Equal("Ensemble", viewResult.ViewName);
            Assert.Equal(2, viewModel.Posts.Count);
        }

        [Fact]
        public void Ensemble_WhenHasAuditions_DisplaysAuditions()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = true;

            // The ensemble that is expected to be passed to the view model.
            var expectedEnsemble = new Ensemble { EnsembleId = 21, Ensemble_Name = "Small Garage Band", UserId = 1 };

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            var eData = new List<Ensemble>
            {
                expectedEnsemble,
                new Ensemble { EnsembleId = 22, Ensemble_Name = "Smooth Jazz Combo", UserId = 2 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 23}
            }.AsQueryable();

            var aData = new List<Audition>
            {
                new Audition { AuditionId = 1, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = expectedEnsemble.EnsembleId, Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice" },
                new Audition { AuditionId = 2, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = expectedEnsemble.EnsembleId, Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice"},
            }.AsQueryable();

            var poData = new List<Post> { }.AsQueryable();

            var iData = new List<Instrument>
            {
                new Instrument() { InstrumentId = 1, Instrument_Name = "Piano" },
                new Instrument() { InstrumentId = 2, Instrument_Name = "Voice" },
                new Instrument() { InstrumentId = 3, Instrument_Name = "Violin" },
                new Instrument() { InstrumentId = 4, Instrument_Name = "Viola" },
                new Instrument() { InstrumentId = 5, Instrument_Name = "Cello" }
            }.AsQueryable();

            // Create Mocked DB sets
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

            var mockProfileEnsembles = new Mock<DbSet<ProfileEnsemble>>();
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Provider).Returns(peData.Provider);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Expression).Returns(peData.Expression);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.ElementType).Returns(peData.ElementType);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.GetEnumerator()).Returns(peData.GetEnumerator());

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(aData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(aData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(aData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(aData.GetEnumerator());

            var mockPosts = new Mock<DbSet<Post>>();
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Expression).Returns(poData.Expression);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.ElementType).Returns(poData.ElementType);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.GetEnumerator()).Returns(poData.GetEnumerator());

            var mockInstrument = new Mock<DbSet<Instrument>>();
            mockInstrument.As<IQueryable<Instrument>>().Setup(u => u.Provider).Returns(iData.Provider);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.Expression).Returns(iData.Expression);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.ElementType).Returns(iData.ElementType);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.GetEnumerator()).Returns(iData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.ProfileEnsembles)
                .Returns(mockProfileEnsembles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Posts)
                .Returns(mockPosts.Object);

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
            var result = controller.Ensemble(21);

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            EnsembleModel viewModel = (EnsembleModel)viewResult.Model;

            Assert.Equal("Ensemble", viewResult.ViewName);
            Assert.Equal(2, viewModel.Ensemble.Audition.Count);
        }
    }
}
