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
    public class SearchActionTests
    {
        [Fact]
        public void Search_WhenCalled_ReturnsValidView() {
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

            var eData = new List<Ensemble>
            {
                new Ensemble{ EnsembleId = 21, Ensemble_Name = "Blue Group", UserId = 1 },
                new Ensemble{ EnsembleId = 22, Ensemble_Name = "Red Group", UserId = 2, Genre = "Jazz" }
            }.AsQueryable();

            var vData = new List<Venue>
            {
                new Venue { VenueId = 31, Venue_Name = "Green Bar", UserId = 1 },
                new Venue { VenueId = 32, Venue_Name = "Yellow Brewery", UserId = 2 },
                new Venue { VenueId = 33, Venue_Name = "Jazz Piano", UserId = 3 }
            }.AsQueryable();

            var audData = new List<Audition>
            {
                new Audition
               {
                   AuditionId = 1,
                   Open_Date = new System.DateTime(2018, 12, 6),
                   Audition_Location = "A Venue",
                   Audition_Description = "An Audition",
                   Instrument_Name = "Piano",
                   InstrumentId = 1,
                   EnsembleId = 21

               }
            }.AsQueryable();

            var gData = new List<Gig>
            {
                new Gig
                {
                    GigId = 41,
                    Gig_Date = new System.DateTime(2019, 12, 1),
                    Closed_Date = new System.DateTime(2006, 12, 30),
                    Genre = "Jazz",
                    Description = "We're looking for some live entertainment!",

                    VenueId = 31
                }
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

            var mockEnsemble = new Mock<DbSet<Ensemble>>();
            mockEnsemble.As<IQueryable<Ensemble>>().Setup(u => u.Provider).Returns(eData.Provider);
            mockEnsemble.As<IQueryable<Ensemble>>().Setup(m => m.Expression).Returns(eData.Expression);
            mockEnsemble.As<IQueryable<Ensemble>>().Setup(m => m.ElementType).Returns(eData.ElementType);
            mockEnsemble.As<IQueryable<Ensemble>>().Setup(m => m.GetEnumerator()).Returns(eData.GetEnumerator());

            var mockVenue = new Mock<DbSet<Venue>>();
            mockVenue.As<IQueryable<Venue>>().Setup(u => u.Provider).Returns(vData.Provider);
            mockVenue.As<IQueryable<Venue>>().Setup(m => m.Expression).Returns(vData.Expression);
            mockVenue.As<IQueryable<Venue>>().Setup(m => m.ElementType).Returns(vData.ElementType);
            mockVenue.As<IQueryable<Venue>>().Setup(m => m.GetEnumerator()).Returns(vData.GetEnumerator());

            var mockAudition = new Mock<DbSet<Audition>>();
            mockAudition.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(audData.Provider);
            mockAudition.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(audData.Expression);
            mockAudition.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(audData.ElementType);
            mockAudition.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(audData.GetEnumerator());

            var mockGig = new Mock<DbSet<Gig>>();
            mockGig.As<IQueryable<Gig>>().Setup(u => u.Provider).Returns(gData.Provider);
            mockGig.As<IQueryable<Gig>>().Setup(m => m.Expression).Returns(gData.Expression);
            mockGig.As<IQueryable<Gig>>().Setup(m => m.ElementType).Returns(gData.ElementType);
            mockGig.As<IQueryable<Gig>>().Setup(m => m.GetEnumerator()).Returns(gData.GetEnumerator());

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
            mockDB.Setup(x => x.Ensembles)
               .Returns(mockEnsemble.Object);
            mockDB.Setup(x => x.Venues)
                .Returns(mockVenue.Object);
            mockDB.Setup(x => x.Auditions)
                .Returns(mockAudition.Object);
            mockDB.Setup(x => x.Gigs)
                .Returns(mockGig.Object);

            mockDB.Setup(x => x.Profiles.Find(It.IsAny<int>()))
                .Returns(mockProfile.Object.Find(11));
            
           /*
            mockDB.Setup(x => x.Auditions.Where(a => a.Instrument_Name == It.IsAny<string>()))
                .Returns(mockAudition.Object.Where(a => a.Instrument_Name == "Jazz"));
            mockDB.Setup(x => x.Auditions.Where(a => a.Instrument_Name == "Piano").ToList())
                .Returns(mockAudition.Object.Where(a => a.Instrument_Name == "Piano").ToList());

            mockDB.Setup(x => x.Gigs.Where(g => g.Genre == "Jazz" || g.Genre == "Jazz Piano").ToList())
                .Returns(mockGig.Object.Where(g => g.Genre == "Jazz" || g.Genre == "Jazz Piano").ToList());
            mockDB.Setup(x => x.Gigs.Where(g => g.Genre == "Piano" || g.Genre == "Jazz Piano").ToList())
               .Returns(mockGig.Object.Where(g => g.Genre == "Piano" || g.Genre == "Jazz Piano").ToList());

            mockDB.Setup(x => x.Profiles.Where(p => p.First_Name == "Jazz" || p.Last_Name == "Jazz").ToList())
                .Returns(mockProfile.Object.Where(p => p.First_Name == "Jazz" || p.Last_Name == "Jazz").ToList());
            mockDB.Setup(x => x.Profiles.Where(p => p.First_Name == "Piano" || p.Last_Name == "Piano").ToList())
                .Returns(mockProfile.Object.Where(p => p.First_Name == "Piano" || p.Last_Name == "Piano").ToList());

            mockDB.Setup(x => x.Plays_Instruments.Where(pi => pi.Instrument.Instrument_Name == "Jazz").ToList())
                .Returns(mockPlays_Instrument.Object.Where(pi => pi.Instrument.Instrument_Name == "Jazz").ToList());
            mockDB.Setup(x => x.Plays_Instruments.Where(pi => pi.Instrument.Instrument_Name == "Piano").ToList())
                .Returns(mockPlays_Instrument.Object.Where(pi => pi.Instrument.Instrument_Name == "Piano").ToList());
            mockDB.Setup(x => x.Profiles.Find(11))
                .Returns(mockProfile.Object.Where(p => p.ProfileId == 11).ToList()[0]);

            mockDB.Setup(x => x.Ensembles.Where(e => e.Ensemble_Name == "Jazz" || e.Ensemble_Name == "Jazz Piano" || e.Genre == "Jazz" || e.Genre == "Jazz Piano" || e.Type == "Jazz" || e.Type == "Jazz Piano").ToList())
                .Returns(mockEnsemble.Object.Where(e => e.Ensemble_Name == "Jazz" || e.Ensemble_Name == "Jazz Piano" || e.Genre == "Jazz" || e.Genre == "Jazz Piano" || e.Type == "Jazz" || e.Type == "Jazz Piano").ToList());
            mockDB.Setup(x => x.Ensembles.Where(e => e.Ensemble_Name == "Piano" || e.Ensemble_Name == "Jazz Piano" || e.Genre == "Piano" || e.Genre == "Jazz Piano" || e.Type == "Piano" || e.Type == "Jazz Piano").ToList())
                .Returns(mockEnsemble.Object.Where(e => e.Ensemble_Name == "Piano" || e.Ensemble_Name == "Jazz Piano" || e.Genre == "Piano" || e.Genre == "Jazz Piano" || e.Type == "Piano" || e.Type == "Jazz Piano").ToList());

            mockDB.Setup(x => x.Venues.Where(v => v.Venue_Name == "Jazz" || v.Venue_Name == "Jazz Piano").ToList())
                .Returns(mockVenue.Object.Where(v => v.Venue_Name == "Jazz" || v.Venue_Name == "Jazz Piano").ToList());
            mockDB.Setup(x => x.Venues.Where(v => v.Venue_Name == "Piano" || v.Venue_Name == "Jazz Piano").ToList())
                .Returns(mockVenue.Object.Where(v => v.Venue_Name == "Jazz" || v.Venue_Name == "Jazz Piano").ToList());
            */

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
            //var result = controller.Search("profile", "Jazz Piano");

            /* Assert */
            //Assert.IsType<ViewResult>(result);
            //ViewResult view = (ViewResult)result;
            //Assert.Equal("Search", view.ViewName);
        }

        [Fact]
        public void Search_WhenCalled_PopulatesQuerySets()
        {
            /* Arrange */

            /* Act */

            /* Assert */
        }
    }
}
