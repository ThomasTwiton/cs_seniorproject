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
    public class AuditionActionTests
    {

        private readonly PluggedContext LoadedContext;

        [Fact]
        public void Audition_WhenNotLoggedIn_RedirectsToLoginAction()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = false;

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
            var result = controller.Audition(1);

            /* Assert */
            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Login", viewResult.ActionName);
        }

        [Fact]
        public void Audition_WhenLoggedIn_DisplaysAppropriateEnsemble()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = true;

            var aData = new List<Audition>
            {
                new Audition { AuditionId = 1, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 21,
                                Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice"},
                new Audition { AuditionId = 2, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 22,
                                Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice"},
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble { EnsembleId = 21, Start_Date = System.DateTime.Now, End_Date = System.DateTime.Now, ProfileId = 11},
                new ProfileEnsemble { EnsembleId = 21, Start_Date = System.DateTime.Now, End_Date = System.DateTime.Now, ProfileId = 12},
                new ProfileEnsemble { EnsembleId = 22, Start_Date = System.DateTime.Now, End_Date = System.DateTime.Now, ProfileId = 11}
            }.AsQueryable();

            // The ensemble that we expect to get as a result for this test.
            var expectedEns = new Ensemble { EnsembleId = 21, Ensemble_Name = "Queen" };

            var eData = new List<Ensemble>
            {
                expectedEns,
                new Ensemble { EnsembleId = 22, Ensemble_Name = "WFLCCB" },
                new Ensemble { EnsembleId = 23, Ensemble_Name = "Zedd" }
            }.AsQueryable();

            var iData = new List<Instrument>
            {
                new Instrument() { InstrumentId = 1, Instrument_Name = "Piano" },
                    new Instrument() { InstrumentId = 2, Instrument_Name = "Voice" },
                    new Instrument() { InstrumentId = 3, Instrument_Name = "Violin" },
                    new Instrument() { InstrumentId = 4, Instrument_Name = "Viola" },
                    new Instrument() { InstrumentId = 5, Instrument_Name = "Cello" }
            }.AsQueryable();

            // Create mocked DB sets
            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(aData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(aData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(aData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(aData.GetEnumerator());

            var mockProfiles = new Mock<DbSet<Profile>>();
            mockProfiles.As<IQueryable<Profile>>().Setup(u => u.Provider).Returns(pData.Provider);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(pData.Expression);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(pData.ElementType);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(pData.GetEnumerator());

            var mockProfileEnsembles = new Mock<DbSet<ProfileEnsemble>>();
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Provider).Returns(peData.Provider);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.Expression).Returns(peData.Expression);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.ElementType).Returns(peData.ElementType);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.GetEnumerator()).Returns(peData.GetEnumerator());

            var mockEnsembles = new Mock<DbSet<Ensemble>>();
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(u => u.Provider).Returns(eData.Provider);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.Expression).Returns(eData.Expression);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.ElementType).Returns(eData.ElementType);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.GetEnumerator()).Returns(eData.GetEnumerator());

            var mockInstrument = new Mock<DbSet<Instrument>>();
            mockInstrument.As<IQueryable<Instrument>>().Setup(u => u.Provider).Returns(iData.Provider);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.Expression).Returns(iData.Expression);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.ElementType).Returns(iData.ElementType);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.GetEnumerator()).Returns(iData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.ProfileEnsembles)
                .Returns(mockProfileEnsembles.Object);

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
            var result = controller.Audition(1);

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            AuditionModel viewModel = (AuditionModel)viewResult.Model;

            Assert.Equal("Audition", viewResult.ViewName);
            Assert.Equal(expectedEns, viewModel.Ensemble);

        }

        [Fact]
        public void Audition_WhenLoggedIn_DisplayAppropriateAudition()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = true;

            // The Audition that we expect to get as a result for this test.
            var expectedAud = new Audition { AuditionId = 1, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 21, Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice" };

            var aData = new List<Audition>
            {
                expectedAud,
                new Audition { AuditionId = 2, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 22,
                                Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice"},
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble { EnsembleId = 21, Start_Date = System.DateTime.Now, End_Date = System.DateTime.Now, ProfileId = 11},
                new ProfileEnsemble { EnsembleId = 21, Start_Date = System.DateTime.Now, End_Date = System.DateTime.Now, ProfileId = 12},
                new ProfileEnsemble { EnsembleId = 22, Start_Date = System.DateTime.Now, End_Date = System.DateTime.Now, ProfileId = 11}
            }.AsQueryable();
            
            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 21, Ensemble_Name = "Queen" },
                new Ensemble { EnsembleId = 22, Ensemble_Name = "WFLCCB" },
                new Ensemble { EnsembleId = 23, Ensemble_Name = "Zedd" }
            }.AsQueryable();

            var iData = new List<Instrument>
            {
                new Instrument() { InstrumentId = 1, Instrument_Name = "Piano" },
                    new Instrument() { InstrumentId = 2, Instrument_Name = "Voice" },
                    new Instrument() { InstrumentId = 3, Instrument_Name = "Violin" },
                    new Instrument() { InstrumentId = 4, Instrument_Name = "Viola" },
                    new Instrument() { InstrumentId = 5, Instrument_Name = "Cello" }
            }.AsQueryable();

            // Create mocked DB sets
            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(aData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(aData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(aData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(aData.GetEnumerator());

            var mockProfiles = new Mock<DbSet<Profile>>();
            mockProfiles.As<IQueryable<Profile>>().Setup(u => u.Provider).Returns(pData.Provider);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(pData.Expression);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(pData.ElementType);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(pData.GetEnumerator());

            var mockProfileEnsembles = new Mock<DbSet<ProfileEnsemble>>();
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Provider).Returns(peData.Provider);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.Expression).Returns(peData.Expression);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.ElementType).Returns(peData.ElementType);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.GetEnumerator()).Returns(peData.GetEnumerator());

            var mockEnsembles = new Mock<DbSet<Ensemble>>();
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(u => u.Provider).Returns(eData.Provider);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.Expression).Returns(eData.Expression);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.ElementType).Returns(eData.ElementType);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.GetEnumerator()).Returns(eData.GetEnumerator());

            var mockInstrument = new Mock<DbSet<Instrument>>();
            mockInstrument.As<IQueryable<Instrument>>().Setup(u => u.Provider).Returns(iData.Provider);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.Expression).Returns(iData.Expression);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.ElementType).Returns(iData.ElementType);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.GetEnumerator()).Returns(iData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.ProfileEnsembles)
                .Returns(mockProfileEnsembles.Object);

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
            var result = controller.Audition(1);

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            AuditionModel viewModel = (AuditionModel)viewResult.Model;

            Assert.Equal("Audition", viewResult.ViewName);
            Assert.Equal(expectedAud, viewModel.Audition);
        }

        [Fact]
        public void Audition_WhenPassedInvalidAuditionId_RedirectsToIndex()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = true;

            var aData = new List<Audition>
            {
                new Audition { AuditionId = 1, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 21,
                                Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice"},
                new Audition { AuditionId = 2, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 22,
                                Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice"},
            }.AsQueryable();

            // Create mocked DB sets
            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(aData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(aData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(aData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(aData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

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
            var result = controller.Audition(111111);

            /* Assert */
            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", viewResult.ActionName);
        }

    }
}