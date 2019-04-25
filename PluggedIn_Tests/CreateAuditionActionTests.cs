using Moq;
using Xunit;
using System.Linq;
using server.Models;
using server.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace PluggedIn_Tests
{
    public class CreateAuditionActionTests
    {
        [Fact]
        public void CreateAudition_WhenGivenValidData_CreatesNewAudition() {
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

            var audData = new List<Audition>
            {
                new Audition
               {
                   AuditionId = 1,
                   Open_Date = new System.DateTime(2018, 12, 6),
                   Audition_Location = "A Venue",
                   Audition_Description = "An Audition",
                   Instrument_Name = "Voice",
                   InstrumentId = 2,
                   EnsembleId = 21

               }
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

            var mockAudition = new Mock<DbSet<Audition>>();
            mockAudition.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(audData.Provider);
            mockAudition.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(audData.Expression);
            mockAudition.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(audData.ElementType);
            mockAudition.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(audData.GetEnumerator());

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

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsemble.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAudition.Object);

            mockDB.Setup(x => x.Instruments)
                .Returns(mockInstrument.Object);
            mockDB.Setup(x => x.Instruments.Find(It.IsAny<int>()))
                .Returns(mockInstrument.Object.FirstOrDefault(p => p.InstrumentId == 2));

            List<Audition> addedAuditions = new List<Audition>();

            mockDB.Setup(x => x.Add(It.IsAny<Audition>()))
                .Callback<Audition>(addedAuditions.Add);

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

            var result = controller.CreateAudition(new System.DateTime(2019, 4, 24), new System.DateTime(2019, 6, 24), "Another Venue", "More musicians", "Decorah", 1, 21, 2);

            /* Assert */
            Audition addedAuditon = addedAuditions.Last();
            Assert.Equal(new System.DateTime(2019, 4, 24), addedAuditon.Open_Date);
            Assert.Equal(new System.DateTime(2019, 6, 24), addedAuditon.Closed_Date);
            Assert.Equal("Another Venue", addedAuditon.Audition_Location);
            Assert.Equal("More musicians", addedAuditon.Audition_Description);
            Assert.Equal("Voice", addedAuditon.Instrument_Name);
        }

        [Fact]
        public void CreateAudition_WhenGivenIncompleteData_CreatesNewAudition() { }

        [Fact]
        public void CreateAudition_Always_ReturnsValidView() {
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

            var audData = new List<Audition>
            {
                new Audition
               {
                   AuditionId = 1,
                   Open_Date = new System.DateTime(2018, 12, 6),
                   Audition_Location = "A Venue",
                   Audition_Description = "An Audition",
                   Instrument_Name = "Voice",
                   InstrumentId = 2,
                   EnsembleId = 21

               }
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

            var mockAudition = new Mock<DbSet<Audition>>();
            mockAudition.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(audData.Provider);
            mockAudition.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(audData.Expression);
            mockAudition.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(audData.ElementType);
            mockAudition.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(audData.GetEnumerator());

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

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsemble.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAudition.Object);

            mockDB.Setup(x => x.Instruments)
                .Returns(mockInstrument.Object);
            mockDB.Setup(x => x.Instruments.Find(It.IsAny<int>()))
                .Returns(mockInstrument.Object.FirstOrDefault(p => p.InstrumentId == 2));

            List<Audition> addedAuditions = new List<Audition>();

            mockDB.Setup(x => x.Add(It.IsAny<Audition>()))
                .Callback<Audition>(addedAuditions.Add);

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

            var result = controller.CreateAudition(new System.DateTime(2019,4,24), new System.DateTime(2019,6,24), "Another Venue", "More musicians", "Decorah", 1, 21, 2);

            /* Assert */
            Assert.IsType<RedirectToActionResult>(result.Result);
            RedirectToActionResult redirect = (RedirectToActionResult)result.Result;
            Assert.Equal("Ensemble", redirect.ActionName);
        }

        [Fact]
        public void CreateAudition_Always_HandlesDuplicateAuditionData() { }

    }
}
