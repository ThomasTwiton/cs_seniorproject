using Moq;
using Xunit;
using System.Linq;
using server.Models;
using server.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace PluggedIn_Tests
{
    public class AuditionActionTests
    {

        protected IHostingEnvironment HostingEnvironment { get; private set; }

        [Fact]
        public void Audition_Always_DisplaysAppropriateEnsemble()
        {
            /* Arrange */

            // Uses Audition, Profile, ProfileEnsemble and Ensemble tables
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

            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 21, Ensemble_Name = "Queen" },
                new Ensemble { EnsembleId = 22, Ensemble_Name = "WFLCCB" },
                new Ensemble { EnsembleId = 23, Ensemble_Name = "Zedd" }
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

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            // Set up necessary Mocked Environment methods
            //  NONE NEEDED FOR THIS TEST

            var controller = new HomeController(mockDB.Object, mockHostEnv.Object);

            /* Act */
            var result = controller.Audition(1);

            /* Assert */
        }

        [Fact]
        public void Audition_Always_DisplayAppropriateAudition() { }

        [Fact]
        public void Audition_Always_HandlesInvalidAuditionId() { }
    }
}