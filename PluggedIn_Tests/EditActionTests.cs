using Moq;
using Xunit;
using System.Linq;
using server.Models;
using server.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PluggedIn_Tests
{
    public class EditActionTests
    {

        [Fact]
        public void EditProfile_WhenCalledWithGetNotLoggedin_ReturnsValidView() {

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

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 },
                new Profile { ProfileId = 13, First_Name = "Jayson", Last_Name = "Ferruccio", UserId = 3 },
                new Profile { ProfileId = 14, First_Name = "Horatio", Last_Name = "Rajendra", UserId = 4 },
                new Profile { ProfileId = 15, First_Name = "Sara", Last_Name = "Rachyl", UserId = 5 }
            }.AsQueryable();

            // Create Mocked DB Sets
            var mockUsers = new Mock<DbSet<User>>();
            mockUsers.As<IQueryable<User>>().Setup(u => u.Provider).Returns(uData.Provider);
            mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(uData.Expression);
            mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(uData.ElementType);
            mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(uData.GetEnumerator());

            var mockProfile = new Mock<DbSet<Profile>>();
            mockProfile.As<IQueryable<Profile>>().Setup(u => u.Provider).Returns(pData.Provider);
            mockProfile.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(pData.Expression);
            mockProfile.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(pData.ElementType);
            mockProfile.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(pData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfile.Object);


            var controllerMock = new Mock<HomeController>(mockDB.Object, mockHostEnv.Object);

            // Mock the request object and the resulting login information
            SessionModel fakeSM = new SessionModel();
            fakeSM.IsLoggedIn = false;
            fakeSM.UserID = 2;

            var controller = controllerMock.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var mockReq = controller.ControllerContext.HttpContext.Request;

            controllerMock.Setup(x => x.GetSessionInfo(mockReq)).Returns(fakeSM);
            controllerMock.CallBase = true;

            /* Act */
            var result = controller.EditProfile(12);

            /* Assert */
            Assert.IsType<RedirectToActionResult>(result.Result);
            RedirectToActionResult redirect = (RedirectToActionResult)result.Result;
            Assert.Equal("Login", redirect.ActionName);
        }

        [Fact]
        public void EditProfile_WhenCalledWithGetLoggedin_ReturnsValidView()
        {

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

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 },
                new Profile { ProfileId = 13, First_Name = "Jayson", Last_Name = "Ferruccio", UserId = 3 },
                new Profile { ProfileId = 14, First_Name = "Horatio", Last_Name = "Rajendra", UserId = 4 },
                new Profile { ProfileId = 15, First_Name = "Sara", Last_Name = "Rachyl", UserId = 5 }
            }.AsQueryable();

            var iData = new List<Instrument>
            {
                new Instrument() { InstrumentId = 1, Instrument_Name = "Piano" },
                    new Instrument() { InstrumentId = 2, Instrument_Name = "Voice" },
                    new Instrument() { InstrumentId = 3, Instrument_Name = "Violin" },
                    new Instrument() { InstrumentId = 4, Instrument_Name = "Viola" },
                    new Instrument() { InstrumentId = 5, Instrument_Name = "Cello" },
                    new Instrument() { InstrumentId = 6, Instrument_Name = "Bass" },
                    new Instrument() { InstrumentId = 7, Instrument_Name = "Guitar" },
                    new Instrument() { InstrumentId = 8, Instrument_Name = "Drums" },
                    new Instrument() { InstrumentId = 9, Instrument_Name = "Trumpet" },
                    new Instrument() { InstrumentId = 10, Instrument_Name = "Trombone" },
                    new Instrument() { InstrumentId = 22, Instrument_Name = "Bass Trombone" },
                    new Instrument() { InstrumentId = 11, Instrument_Name = "Tuba" },
                    new Instrument() { InstrumentId = 12, Instrument_Name = "Baritone" },
                    new Instrument() { InstrumentId = 13, Instrument_Name = "French Horn" },
                    new Instrument() { InstrumentId = 14, Instrument_Name = "Flute" },
                    new Instrument() { InstrumentId = 15, Instrument_Name = "Clarinet" },
                    new Instrument() { InstrumentId = 16, Instrument_Name = "Basoon" },
                    new Instrument() { InstrumentId = 17, Instrument_Name = "Saxophone" },
                    new Instrument() { InstrumentId = 18, Instrument_Name = "Bagpipes" },
                    new Instrument() { InstrumentId = 19, Instrument_Name = "Xylophone" },
                    new Instrument() { InstrumentId = 20, Instrument_Name = "Accordion" },
                    new Instrument() { InstrumentId = 21, Instrument_Name = "Harmonica" }
            }.AsQueryable();

            var piData = new List<Plays_Instrument>
            {
                new Plays_Instrument() { Id = 1, ProfileId = 11, InstrumentId = 1 },
                new Plays_Instrument() { Id = 2, ProfileId = 11, InstrumentId = 10 },
                new Plays_Instrument() { Id = 3, ProfileId = 12, InstrumentId = 2 }
            }.AsQueryable();

            // Create Mocked DB Sets
            var mockUsers = new Mock<DbSet<User>>();
            mockUsers.As<IQueryable<User>>().Setup(u => u.Provider).Returns(uData.Provider);
            mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(uData.Expression);
            mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(uData.ElementType);
            mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(uData.GetEnumerator());

            var mockProfile = new Mock<DbSet<Profile>>();
            mockProfile.As<IQueryable<Profile>>().Setup(u => u.Provider).Returns(pData.Provider);
            mockProfile.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(pData.Expression);
            mockProfile.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(pData.ElementType);
            mockProfile.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(pData.GetEnumerator());

            var mockInstrument = new Mock<DbSet<Instrument>>();
            mockInstrument.As<IQueryable<Instrument>>().Setup(u => u.Provider).Returns(iData.Provider);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.Expression).Returns(iData.Expression);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.ElementType).Returns(iData.ElementType);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.GetEnumerator()).Returns(iData.GetEnumerator());

            var mockPlays_Instrument = new Mock<DbSet<Plays_Instrument>>();
            mockPlays_Instrument.As<IQueryable<Plays_Instrument>>().Setup(u => u.Provider).Returns(piData.Provider);
            mockPlays_Instrument.As<IQueryable<Plays_Instrument>>().Setup(m => m.Expression).Returns(piData.Expression);
            mockPlays_Instrument.As<IQueryable<Plays_Instrument>>().Setup(m => m.ElementType).Returns(piData.ElementType);
            mockPlays_Instrument.As<IQueryable<Plays_Instrument>>().Setup(m => m.GetEnumerator()).Returns(piData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfile.Object);
            mockDB.Setup(x => x.Profiles.Find(It.IsAny<int>()))
                .Returns(mockProfile.Object.FirstOrDefault(p => p.ProfileId == 12));
            mockDB.Setup(x => x.Instruments)
                .Returns(mockInstrument.Object);
            mockDB.Setup(x => x.Plays_Instruments)
                .Returns(mockPlays_Instrument.Object);

            var controllerMock = new Mock<HomeController>(mockDB.Object, mockHostEnv.Object);

            // Mock the request object and the resulting login information
            SessionModel fakeSM = new SessionModel();
            fakeSM.IsLoggedIn = true;
            fakeSM.UserID = 2;

            var controller = controllerMock.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var mockReq = controller.ControllerContext.HttpContext.Request;

            controllerMock.Setup(x => x.GetSessionInfo(mockReq)).Returns(fakeSM);
            controllerMock.CallBase = true;

            /* Act */
            var result = controller.EditProfile(12);

            /* Assert */
            Assert.IsType<ViewResult>(result.Result);
            ViewResult view = (ViewResult)result.Result;
            ProfileModel model = (ProfileModel) view.Model;
            Assert.Equal(12, model.Profile.ProfileId);
        }

        [Fact]
        public void EditProfile_WhenCalledWithPost_UpdatesDatabase()
        {
            /* Arrange */
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

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 },
                new Profile { ProfileId = 13, First_Name = "Jayson", Last_Name = "Ferruccio", UserId = 3 },
                new Profile { ProfileId = 14, First_Name = "Horatio", Last_Name = "Rajendra", UserId = 4 },
                new Profile { ProfileId = 15, First_Name = "Sara", Last_Name = "Rachyl", UserId = 5 }
            }.AsQueryable();

            var iData = new List<Instrument>
            {
                new Instrument() { InstrumentId = 1, Instrument_Name = "Piano" },
                    new Instrument() { InstrumentId = 2, Instrument_Name = "Voice" },
                    new Instrument() { InstrumentId = 3, Instrument_Name = "Violin" },
                    new Instrument() { InstrumentId = 4, Instrument_Name = "Viola" },
                    new Instrument() { InstrumentId = 5, Instrument_Name = "Cello" },
                    new Instrument() { InstrumentId = 6, Instrument_Name = "Bass" },
                    new Instrument() { InstrumentId = 7, Instrument_Name = "Guitar" },
                    new Instrument() { InstrumentId = 8, Instrument_Name = "Drums" },
                    new Instrument() { InstrumentId = 9, Instrument_Name = "Trumpet" },
                    new Instrument() { InstrumentId = 10, Instrument_Name = "Trombone" },
                    new Instrument() { InstrumentId = 22, Instrument_Name = "Bass Trombone" },
                    new Instrument() { InstrumentId = 11, Instrument_Name = "Tuba" },
                    new Instrument() { InstrumentId = 12, Instrument_Name = "Baritone" },
                    new Instrument() { InstrumentId = 13, Instrument_Name = "French Horn" },
                    new Instrument() { InstrumentId = 14, Instrument_Name = "Flute" },
                    new Instrument() { InstrumentId = 15, Instrument_Name = "Clarinet" },
                    new Instrument() { InstrumentId = 16, Instrument_Name = "Basoon" },
                    new Instrument() { InstrumentId = 17, Instrument_Name = "Saxophone" },
                    new Instrument() { InstrumentId = 18, Instrument_Name = "Bagpipes" },
                    new Instrument() { InstrumentId = 19, Instrument_Name = "Xylophone" },
                    new Instrument() { InstrumentId = 20, Instrument_Name = "Accordion" },
                    new Instrument() { InstrumentId = 21, Instrument_Name = "Harmonica" }
            }.AsQueryable();

            var piData = new List<Plays_Instrument>
            {
                new Plays_Instrument() { Id = 1, ProfileId = 11, InstrumentId = 1 },
                new Plays_Instrument() { Id = 2, ProfileId = 11, InstrumentId = 10 },
                new Plays_Instrument() { Id = 3, ProfileId = 12, InstrumentId = 2 }
            }.AsQueryable();

            // Create Mocked DB Sets
            var mockUsers = new Mock<DbSet<User>>();
            mockUsers.As<IQueryable<User>>().Setup(u => u.Provider).Returns(uData.Provider);
            mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(uData.Expression);
            mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(uData.ElementType);
            mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(uData.GetEnumerator());

            var mockProfile = new Mock<DbSet<Profile>>();
            mockProfile.As<IQueryable<Profile>>().Setup(u => u.Provider).Returns(pData.Provider);
            mockProfile.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(pData.Expression);
            mockProfile.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(pData.ElementType);
            mockProfile.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(pData.GetEnumerator());

            var mockInstrument = new Mock<DbSet<Instrument>>();
            mockInstrument.As<IQueryable<Instrument>>().Setup(u => u.Provider).Returns(iData.Provider);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.Expression).Returns(iData.Expression);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.ElementType).Returns(iData.ElementType);
            mockInstrument.As<IQueryable<Instrument>>().Setup(m => m.GetEnumerator()).Returns(iData.GetEnumerator());

            var mockPlays_Instrument = new Mock<DbSet<Plays_Instrument>>();
            mockPlays_Instrument.As<IQueryable<Plays_Instrument>>().Setup(u => u.Provider).Returns(piData.Provider);
            mockPlays_Instrument.As<IQueryable<Plays_Instrument>>().Setup(m => m.Expression).Returns(piData.Expression);
            mockPlays_Instrument.As<IQueryable<Plays_Instrument>>().Setup(m => m.ElementType).Returns(piData.ElementType);
            mockPlays_Instrument.As<IQueryable<Plays_Instrument>>().Setup(m => m.GetEnumerator()).Returns(piData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfile.Object);
            mockDB.Setup(x => x.Profiles.Find(It.IsAny<int>()))
                .Returns(mockProfile.Object.FirstOrDefault(p => p.ProfileId == 12));
            mockDB.Setup(x => x.Instruments)
                .Returns(mockInstrument.Object);
            mockDB.Setup(x => x.Plays_Instruments)
                .Returns(mockPlays_Instrument.Object);

            var controllerMock = new Mock<HomeController>(mockDB.Object, mockHostEnv.Object);

            // Mock the request object and the resulting login information
            SessionModel fakeSM = new SessionModel();
            fakeSM.IsLoggedIn = true;
            fakeSM.UserID = 2;

            var controller = controllerMock.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var mockReq = controller.ControllerContext.HttpContext.Request;

            controllerMock.Setup(x => x.GetSessionInfo(mockReq)).Returns(fakeSM);
            controllerMock.CallBase = true;

            //make the model we are posting
            ProfileModel model = new ProfileModel();

            var profile = new Profile();
            profile.First_Name = "Laurraine";
            profile.Last_Name = "Wilson";
            profile.Bio = "My bio was changed";
            profile.City = "Houston";
            profile.State = "TX";
            var instruments = new List<SelectListItem>();
            var selected = new List<string>{ "1", "10", "2"};
            
            model.Profile = profile;
            model.Instruments = instruments;
            model.SelectedInsIds = selected;

            /* Act */
            var result= controller.EditProfile(12, model);

            /* Assert */
            
            Assert.IsType<RedirectToActionResult>(result.Result);
            RedirectToActionResult redirect = (RedirectToActionResult) result.Result;
            Assert.Equal("Profile", redirect.ActionName);
            var editedprofile = mockDB.Object.Profiles.Find(12);
            Assert.Equal(3, editedprofile.Plays_Instrument.ToList().Count());
            Assert.Equal("Laurraine", editedprofile.First_Name);
            Assert.Equal("Wilson", editedprofile.Last_Name);
            Assert.Equal("My bio was changed", editedprofile.Bio);
            Assert.Equal("Houston", editedprofile.City);
            Assert.Equal("TX", editedprofile.State);
        }

        [Fact]
        public void EditEnsemble_WhenCalledWithGet_ReturnsValidView() {

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

            var eData = new List<Ensemble>
            {
                new Ensemble{ EnsembleId = 21, Ensemble_Name = "Blue Group", UserId = 1 },
                new Ensemble{ EnsembleId = 22, Ensemble_Name = "Red Group", UserId = 2 }
            }.AsQueryable();


            // Create Mocked DB Sets
            var mockUsers = new Mock<DbSet<User>>();
            mockUsers.As<IQueryable<User>>().Setup(u => u.Provider).Returns(uData.Provider);
            mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(uData.Expression);
            mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(uData.ElementType);
            mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(uData.GetEnumerator());

            var mockEnsemble = new Mock<DbSet<Ensemble>>();
            mockEnsemble.As<IQueryable<Ensemble>>().Setup(u => u.Provider).Returns(eData.Provider);
            mockEnsemble.As<IQueryable<Ensemble>>().Setup(m => m.Expression).Returns(eData.Expression);
            mockEnsemble.As<IQueryable<Ensemble>>().Setup(m => m.ElementType).Returns(eData.ElementType);
            mockEnsemble.As<IQueryable<Ensemble>>().Setup(m => m.GetEnumerator()).Returns(eData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsemble.Object);
            mockDB.Setup(x => x.Ensembles.Find(It.IsAny<int>()))
                .Returns(mockEnsemble.Object.FirstOrDefault(p => p.EnsembleId == 21));

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
            var result = controller.EditEnsemble(21);

            /* Assert */
            Assert.IsType<ViewResult>(result.Result);
            ViewResult view = (ViewResult)result.Result;
            EnsembleModel model = (EnsembleModel)view.Model;
            Assert.Equal(21, model.Ensemble.EnsembleId);
        }

        [Fact]
        public void EditEnsemble_WhenCalledWithPost_UpdatesDatabase() {
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

            var eData = new List<Ensemble>
            {
                new Ensemble{ EnsembleId = 21, Ensemble_Name = "Blue Group", UserId = 1 },
                new Ensemble{ EnsembleId = 22, Ensemble_Name = "Red Group", UserId = 2 }
            }.AsQueryable();


            // Create Mocked DB Sets
            var mockUsers = new Mock<DbSet<User>>();
            mockUsers.As<IQueryable<User>>().Setup(u => u.Provider).Returns(uData.Provider);
            mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(uData.Expression);
            mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(uData.ElementType);
            mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(uData.GetEnumerator());

            var mockEnsemble = new Mock<DbSet<Ensemble>>();
            mockEnsemble.As<IQueryable<Ensemble>>().Setup(u => u.Provider).Returns(eData.Provider);
            mockEnsemble.As<IQueryable<Ensemble>>().Setup(m => m.Expression).Returns(eData.Expression);
            mockEnsemble.As<IQueryable<Ensemble>>().Setup(m => m.ElementType).Returns(eData.ElementType);
            mockEnsemble.As<IQueryable<Ensemble>>().Setup(m => m.GetEnumerator()).Returns(eData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsemble.Object);
            mockDB.Setup(x => x.Ensembles.Find(It.IsAny<int>()))
                .Returns(mockEnsemble.Object.FirstOrDefault(p => p.EnsembleId == 21));

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

            EnsembleModel model = new EnsembleModel();
            var ensemble = new Ensemble();
            model.Ensemble = ensemble;
            model.Ensemble.Ensemble_Name = "Purple Group";
            model.Ensemble.Formed_Date = new System.DateTime(2015, 4, 13);
            model.Ensemble.Disbanded_Date = new System.DateTime(2020, 4, 13);
            model.Ensemble.City = "Prospit";
            model.Ensemble.State = "HS";
            model.Ensemble.Type = "Brass Band";
            model.Ensemble.Genre = "Jazz";
            model.Ensemble.Bio = "Here is an about us";

            /* Act */
            var result = controller.EditEnsemble(21, model);

            /* Assert */

            Assert.IsType<RedirectToActionResult>(result.Result);
            RedirectToActionResult redirect = (RedirectToActionResult)result.Result;
            Assert.Equal("Ensemble", redirect.ActionName);
            var editedensemble = mockDB.Object.Ensembles.Find(21);
            Assert.Equal("Purple Group", editedensemble.Ensemble_Name);
            Assert.Equal(new System.DateTime(2015, 4, 13), editedensemble.Formed_Date);
            Assert.Equal(new System.DateTime(2020, 4, 13), editedensemble.Disbanded_Date);
            Assert.Equal("Brass Band", editedensemble.Type);
            Assert.Equal("Jazz", editedensemble.Genre);
            Assert.Equal("Here is an about us", editedensemble.Bio);
            Assert.Equal("Prospit", editedensemble.City);
            Assert.Equal("HS", editedensemble.State);
        }

        [Fact]
        public void EditVenue_WhenCalledWithGet_ReturnsValidView() {
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
            mockDB.Setup(x => x.Venues.Find(It.IsAny<int>()))
                .Returns(mockVenue.Object.FirstOrDefault(p => p.VenueId == 31));

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
            var result = controller.EditVenue(31);

            /* Assert */
            Assert.IsType<ViewResult>(result.Result);
            ViewResult view = (ViewResult)result.Result;
            VenueModel model = (VenueModel)view.Model;
            Assert.Equal(31, model.Venue.VenueId);
        }
    

        [Fact]
        public void EditVenue_WhenCalledWithPost_UpdatesDatabase() {
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
            mockDB.Setup(x => x.Venues.Find(It.IsAny<int>()))
                .Returns(mockVenue.Object.FirstOrDefault(p => p.VenueId == 31));

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

            VenueModel model = new VenueModel();
            var venue = new Venue();
            model.Venue = venue;
            model.Venue.Venue_Name = "Pink Concert Hall";
            model.Venue.Address1 = "Main Street";
            model.Venue.Address2 = "Apartment 2";
            model.Venue.City = "Chicago";
            model.Venue.State = "IL";
            model.Venue.Phone = "(123) 456-7890";
            model.Venue.Website = "www.google.com";
            model.Venue.Bio = "Come for the concert";


            /* Act */
            var result = controller.EditVenue(31, model);

            /* Assert */

            Assert.IsType<RedirectToActionResult>(result.Result);
            RedirectToActionResult redirect = (RedirectToActionResult)result.Result;
            Assert.Equal("Venue", redirect.ActionName);
            var editedVenue = mockDB.Object.Venues.Find(31);
            Assert.Equal("Pink Concert Hall", editedVenue.Venue_Name);
            Assert.Equal("Come for the concert", editedVenue.Bio);
            Assert.Equal("Main Street", editedVenue.Address1);
            Assert.Equal("Apartment 2", editedVenue.Address2);
            Assert.Equal("Chicago", editedVenue.City);
            Assert.Equal("IL", editedVenue.State);
            Assert.Equal("(123) 456-7890", editedVenue.Phone);
            Assert.Equal("www.google.com", editedVenue.Website);

        }



    }
}
