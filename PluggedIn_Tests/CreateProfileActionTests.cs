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
    public class CreateProfileActionTests
    {
        [Fact]
        public void CreateProfile_WhenGivenValidData_CreatesNewProfile() {
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
            mockDB.Setup(x => x.Instruments)
                .Returns(mockInstrument.Object);
            mockDB.Setup(x => x.Plays_Instruments)
                .Returns(mockPlays_Instrument.Object);

            List<Profile> addedProfiles = new List<Profile>();

            mockDB.Setup(x => x.Add(It.IsAny<Profile>()))
                .Callback<Profile>(addedProfiles.Add);

            var controllerMock = new Mock<HomeController>(mockDB.Object, mockHostEnv.Object);

            // Mock the request object and the resulting login information
            SessionModel fakeSM = new SessionModel();
            fakeSM.IsLoggedIn = true;
            fakeSM.UserID = 6;

            var controller = controllerMock.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var mockReq = controller.ControllerContext.HttpContext.Request;

            controllerMock.Setup(x => x.GetSessionInfo(mockReq)).Returns(fakeSM);
            controllerMock.CallBase = true;

            ProfileModel model = new ProfileModel();

            /* Act */
            var result = controller.CreateProfile("Allison", "Weathers", "Chicago", "IL", "I am a musician", 6, model);

            /* Assert */
            Profile addedProfile = addedProfiles.Last();
            Assert.Equal("Allison", addedProfile.First_Name);
            Assert.Equal("Weathers", addedProfile.Last_Name);
            Assert.Equal("Chicago", addedProfile.City);
            Assert.Equal("IL", addedProfile.State);
            Assert.Equal("I am a musician", addedProfile.Bio);
        }

        [Fact]
        public void CreateProfile_WhenGivenValidData_ReturnsValidView() {
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
            mockDB.Setup(x => x.Instruments)
                .Returns(mockInstrument.Object);
            mockDB.Setup(x => x.Plays_Instruments)
                .Returns(mockPlays_Instrument.Object);

            List<Profile> addedProfiles = new List<Profile>();

            mockDB.Setup(x => x.Add(It.IsAny<Profile>()))
                .Callback<Profile>(addedProfiles.Add);

            var controllerMock = new Mock<HomeController>(mockDB.Object, mockHostEnv.Object);

            // Mock the request object and the resulting login information
            SessionModel fakeSM = new SessionModel();
            fakeSM.IsLoggedIn = true;
            fakeSM.UserID = 6;

            var controller = controllerMock.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var mockReq = controller.ControllerContext.HttpContext.Request;

            controllerMock.Setup(x => x.GetSessionInfo(mockReq)).Returns(fakeSM);
            controllerMock.CallBase = true;

            ProfileModel model = new ProfileModel();
            model.SelectedInsIds = new List<string>();
            model.SelectedInsIds.Add("1");
            model.SelectedInsIds.Add("2");

            /* Act */
            var result = controller.CreateProfile("Allison", "Weathers", "Chicago", "IL", "I am a musician", 6, model);

            /* Assert */
            Assert.IsType<RedirectToActionResult>(result.Result);
            RedirectToActionResult redirect = (RedirectToActionResult)result.Result;
            Assert.Equal("Profile", redirect.ActionName);
        }
        
    }
}
