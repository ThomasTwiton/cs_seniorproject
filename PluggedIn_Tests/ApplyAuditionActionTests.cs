using Moq;
using Xunit;
using System.Linq;
using server.Models;
using server.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace PluggedIn_Tests
{
    public class ApplyAuditionActionTests
    {

        private readonly PluggedContext LoadedContext;

        [Fact]
        public async Task ApplyAudition_WhenGivenValidData_CreatesNewProfileAuditionAndDisplaysAuditionView()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aProfId = 11;
            var aLoggedIn = true;

            // Profile that is applying:
            

            var pData = new List<Profile>
            {
                new Profile { ProfileId = aProfId, First_Name = "Elijas", Last_Name = "Reshmi", UserId = aUserId },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();


            // Audition to be applied for:
            var appAud = new Audition { AuditionId = 1, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 21, Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice" };

            var aData = new List<Audition>
            {
                appAud,
                new Audition { AuditionId = 2, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 22,
                                Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice"},
            }.AsQueryable();

            // Create Mocked DB Sets
            var mockProfiles = new Mock<DbSet<Profile>>();
            mockProfiles.As<IQueryable<Profile>>().Setup(u => u.Provider).Returns(pData.Provider);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(pData.Expression);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(pData.ElementType);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(pData.GetEnumerator());

            var mockAudition = new Mock<DbSet<Audition>>();
            mockAudition.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(aData.Provider);
            mockAudition.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(aData.Expression);
            mockAudition.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(aData.ElementType);
            mockAudition.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(aData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAudition.Object);


            // Create the "Table" that will save the new AuditionProfile
            List<AuditionProfile> addedAuditionProfiles = new List<AuditionProfile>();

            // Mock the behavior of adding the new AuditionProfile
            mockDB.Setup(x => x.Add(It.IsAny<AuditionProfile>()))
                .Callback<AuditionProfile>(addedAuditionProfiles.Add);

            // Create a Mocked hosting environment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            var controllerMock = new Mock<HomeController>(mockDB.Object, mockHostEnv.Object);

            // Mock the request object and the resulting login information
            SessionModel fakeSM = new SessionModel();
            fakeSM.IsLoggedIn = aLoggedIn;
            fakeSM.UserID = aUserId;

            var controller = controllerMock.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var mockReq = controller.ControllerContext.HttpContext.Request;

            controllerMock.Setup(x => x.GetSessionInfo(mockReq)).Returns(fakeSM);
            controllerMock.CallBase = true;

            // Create the AuditionModel to be passed
            AuditionModel mo = new AuditionModel() { Audition = appAud };

            /* Act */

            var result = controller.ApplyAudition(mo);

            /* Assert */

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Audition", redirectToActionResult.ActionName);

            AuditionProfile addedAuditon = addedAuditionProfiles.Last();
            Assert.Equal(aProfId, addedAuditon.ProfileId);
            Assert.Equal(addedAuditon.AuditionId, appAud.AuditionId);

        }

        [Fact]
        public void ApplyAudition_WhenGivenAuditionIdIsNotInDB_RedirectsToIndex()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aProfId = 11;
            var aLoggedIn = true;

            // Profile that is applying:


            var pData = new List<Profile>
            {
                new Profile { ProfileId = aProfId, First_Name = "Elijas", Last_Name = "Reshmi", UserId = aUserId },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();


            // Audition to be applied for:
            var appAud = new Audition { AuditionId = 1, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 21, Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice" };

            var aData = new List<Audition>
            {
                appAud,
                new Audition { AuditionId = 2, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 22,
                                Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice"},
            }.AsQueryable();

            // Create Mocked DB Sets
            var mockProfiles = new Mock<DbSet<Profile>>();
            mockProfiles.As<IQueryable<Profile>>().Setup(u => u.Provider).Returns(pData.Provider);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(pData.Expression);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(pData.ElementType);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(pData.GetEnumerator());

            var mockAudition = new Mock<DbSet<Audition>>();
            mockAudition.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(aData.Provider);
            mockAudition.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(aData.Expression);
            mockAudition.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(aData.ElementType);
            mockAudition.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(aData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAudition.Object);


            // Create the "Table" that will save the new AuditionProfile
            List<AuditionProfile> addedAuditionProfiles = new List<AuditionProfile>();

            // Mock the behavior of adding the new AuditionProfile
            mockDB.Setup(x => x.Add(It.IsAny<AuditionProfile>()))
                .Callback<AuditionProfile>(addedAuditionProfiles.Add);

            // Create a Mocked hosting environment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            var controllerMock = new Mock<HomeController>(mockDB.Object, mockHostEnv.Object);

            // Mock the request object and the resulting login information
            SessionModel fakeSM = new SessionModel();
            fakeSM.IsLoggedIn = aLoggedIn;
            fakeSM.UserID = aUserId;

            var controller = controllerMock.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var mockReq = controller.ControllerContext.HttpContext.Request;

            controllerMock.Setup(x => x.GetSessionInfo(mockReq)).Returns(fakeSM);
            controllerMock.CallBase = true;

            // Create the AuditionModel to be passed
            AuditionModel mo = new AuditionModel() { Audition = appAud };

            /* Act */

            var result = controller.ApplyAudition(mo);

            /* Assert */

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Audition", redirectToActionResult.ActionName);

            AuditionProfile addedAuditon = addedAuditionProfiles.Last();
            Assert.Equal(aProfId, addedAuditon.ProfileId);
            Assert.Equal(addedAuditon.AuditionId, appAud.AuditionId);
        }

        [Fact]
        public void ApplyAudition_WhenGivenProfileIdIsNotInDB_RedirectsToIndex()
        {

        }

        [Fact]
        public void ApplyAudition_WhenNotLoggedIn_RedirectsToLogin()
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

            // Create the AuditionModel to be passed
            var a = new Audition { AuditionId = 1, Open_Date = System.DateTime.Now, Closed_Date = System.DateTime.Now, EnsembleId = 21, Audition_Location = "Galena, IL", Audition_Description = "Come audition with us", Instrument_Name = "Voice" };

            AuditionModel mo = new AuditionModel() { Audition = a };

            /* Act */
            var result = controller.ApplyAudition(mo);

            /* Assert */

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Login", redirectToActionResult.ActionName);
        }
    }
}
