using Moq;
using Xunit;
using System.Linq;
using server.Models;
using server.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace PluggedIn_Tests
{
    public class APIControllerTests
    {

        /*questions for tyler: do I need to test anything with get/post?
         * Or are we just testing from the perspective of the controller in C#?
         * testing routing?
         */
        private readonly PluggedContext LoadedContext;

        [Fact]
        public void GetPosts_WhenCalled_ReturnsPostIEnumerableEmptyOrNot()
        {
            //Arrange
            var controller = new PluggedAPIController(LoadedContext);

            //Act
            var result = controller.GetPosts();

            //Assert
            var GetPostsResult = Assert.IsType<Task<ActionResult<IEnumerable<Post>>>>(result);


        }


        public class ChangeAudition
        {
            public string auditionId { get; set; }
            public string audition_Description { get; set; }
            public string audition_Location { get; set; }
            public string closed_Date { get; set; }
            public string instrument_Name { get; set; }
            public string open_Date { get; set; }
        }

        [Fact]
        public void GetApplicants_WhenCalled_ReturnsValidNumberofApplicants()
        {
            //get people who have applied and not those who haven't 

            //ensemble, list of audition, auditionprofile

            //Arrange
            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 1, Ensemble_Name = "Luther Dan", UserId = 5}

            }.AsQueryable();

            var audData = new List<Audition>
            {
                new Audition {AuditionId = 2, EnsembleId = 1, InstrumentId = 3, Audition_Description = "Goodbye"}
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 },
                new Profile { ProfileId = 13, First_Name = "Elwood", Last_Name = "Birch", UserId = 3 }
            }.AsQueryable();

            var apData = new List<AuditionProfile>
            {
                new AuditionProfile{AuditionId = 2, ProfileId = 11},
                new AuditionProfile{AuditionId = 2, ProfileId = 13}
            }.AsQueryable();

            // Create a Mocked DB set
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

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(audData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(audData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(audData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(audData.GetEnumerator());

            var mockAuditionProfiles = new Mock<DbSet<AuditionProfile>>();
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(u => u.Provider).Returns(apData.Provider);
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(m => m.Expression).Returns(apData.Expression);
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(m => m.ElementType).Returns(apData.ElementType);
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(m => m.GetEnumerator()).Returns(apData.GetEnumerator());
            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.AuditionProfiles)
                .Returns(mockAuditionProfiles.Object);

            var controller = new PluggedAPIController(mockDB.Object);


            //Act

            var result = controller.GetApplicants(2);

            //Assert



            Assert.IsType<Task<ActionResult<IEnumerable<Profile>>>>(result);

            Task<ActionResult<IEnumerable<Profile>>> resultdata = result;

            var profiles = result.Result.Value.ToList();

            Assert.NotNull(profiles);
            Assert.IsType<List<Profile>>(profiles);
            Assert.Equal(2, profiles.Count());

        }


        [Fact]
        public void GetMembers_WhenCalled_ReturnsValidNumberofApplicants()
        {

            //Arrange
            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 1, Ensemble_Name = "Luther Dan", UserId = 5}

            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 },
                new Profile { ProfileId = 13, First_Name = "Elwood", Last_Name = "Birch", UserId = 3 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble{EnsembleId = 1, ProfileId = 11},
                new ProfileEnsemble{EnsembleId = 1, ProfileId = 13}
            }.AsQueryable();

            // Create a Mocked DB set
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
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.Expression).Returns(peData.Expression);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.ElementType).Returns(peData.ElementType);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.GetEnumerator()).Returns(peData.GetEnumerator());
            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.ProfileEnsembles)
                .Returns(mockProfileEnsembles.Object);

            var controller = new PluggedAPIController(mockDB.Object);


            //Act

            var result = controller.GetMembers(1);

            //Assert



            //Assert.IsType<Task<ActionResult<IEnumerable<Profile>>>>(result);

            //Task<ActionResult<IEnumerable<Profile>>> resultdata = result;

            //var profiles = result.Result.Value.ToList();

            //Assert.NotNull(profiles);
            //Assert.IsType<List<Profile>>(profiles);
            //Assert.Equal(2, profiles.Count());

        }

        [Fact]
        public void GetAudition_WhenCalled_IsNotNull()
        {
            //get people who have applied and not those who haven't 

            //ensemble, list of audition, auditionprofile

            //Arrange
            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 1, Ensemble_Name = "Luther Dan", UserId = 5}

            }.AsQueryable();

            var audData = new List<Audition>
            {
                new Audition {AuditionId = 2, EnsembleId = 1, InstrumentId = 3}
            }.AsQueryable();

            // Create a Mocked DB set

            var mockEnsembles = new Mock<DbSet<Ensemble>>();
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(u => u.Provider).Returns(eData.Provider);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.Expression).Returns(eData.Expression);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.ElementType).Returns(eData.ElementType);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.GetEnumerator()).Returns(eData.GetEnumerator());

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(audData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(audData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(audData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(audData.GetEnumerator());


            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            var controller = new PluggedAPIController(mockDB.Object);


            //Act

            var result = controller.GetAudition(2);

            //Assert



            Assert.IsType<Task<ActionResult<Audition>>>(result);

            Task<ActionResult<Audition>> resultdata = result;

            var audition = resultdata.Result;

            Assert.NotNull(audition);

        }

        [Fact]
        public void PostAudition_WhenCalled_ChangesAudition()
        {
            //get people who have applied and not those who haven't 

            //ensemble, list of audition, auditionprofile

            //Arrange
            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 1, Ensemble_Name = "Luther Dan", UserId = 5}

            }.AsQueryable();

            var audData = new List<Audition>
            {
                new Audition {AuditionId = 2, EnsembleId = 1, InstrumentId = 3, Audition_Description = "Goodbye"},
                new Audition {AuditionId = 3}
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 },
                new Profile { ProfileId = 13, First_Name = "Elwood", Last_Name = "Birch", UserId = 3 }
            }.AsQueryable();

            var apData = new List<AuditionProfile>
            {
                new AuditionProfile{AuditionId = 2, ProfileId = 11},
                new AuditionProfile{AuditionId = 2, ProfileId = 13}
            }.AsQueryable();

            // Create a Mocked DB set
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

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(audData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(audData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(audData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(audData.GetEnumerator());

            var mockAuditionProfiles = new Mock<DbSet<AuditionProfile>>();
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(u => u.Provider).Returns(apData.Provider);
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(m => m.Expression).Returns(apData.Expression);
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(m => m.ElementType).Returns(apData.ElementType);
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(m => m.GetEnumerator()).Returns(apData.GetEnumerator());
            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.AuditionProfiles)
                .Returns(mockAuditionProfiles.Object);

            mockDB.Setup(x => x.Auditions.Find(It.IsAny<int>())).Returns(mockAuditions.Object.FirstOrDefault(a => a.AuditionId == 2));

            var controller = new PluggedAPIController(mockDB.Object);


            var changeaud = new server.Controllers.PluggedAPIController.ChangeAudition();
            changeaud.auditionId = "2";
            changeaud.audition_Description = "Hello";
            changeaud.audition_Location = "Mars";
            changeaud.open_Date = "2014-04-01";
            changeaud.closed_Date = "2015-08-27";


            //Act

            controller.PostAudition(2, changeaud);

            //Assert


            var audition = mockDB.Object.Auditions.Find();
            foreach (Audition aud in audData)
            {
                if (aud.AuditionId == 2)
                {
                    Assert.Equal("Hello", aud.Audition_Description);
                }
            }
            //var audition = audData.Where()
            //Assert.NotNull(audition);
            //Assert.Equal("Hello", audition.Audition_Description);

        }
        [Fact]
        public void CloseAudition_WhenCalled_ChangesAuditionClosedDate()
        {
            //get people who have applied and not those who haven't 

            //ensemble, list of audition, auditionprofile

            //Arrange
            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 1, Ensemble_Name = "Luther Dan", UserId = 5}

            }.AsQueryable();

            var audData = new List<Audition>
            {
                new Audition {AuditionId = 2, EnsembleId = 1, InstrumentId = 3, Closed_Date = DateTime.ParseExact("2020-01-20", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) },
                new Audition {AuditionId = 3}
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 },
                new Profile { ProfileId = 13, First_Name = "Elwood", Last_Name = "Birch", UserId = 3 }
            }.AsQueryable();

            var apData = new List<AuditionProfile>
            {
                new AuditionProfile{AuditionId = 2, ProfileId = 11},
                new AuditionProfile{AuditionId = 2, ProfileId = 13}
            }.AsQueryable();

            // Create a Mocked DB set
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

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(audData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(audData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(audData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(audData.GetEnumerator());

            var mockAuditionProfiles = new Mock<DbSet<AuditionProfile>>();
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(u => u.Provider).Returns(apData.Provider);
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(m => m.Expression).Returns(apData.Expression);
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(m => m.ElementType).Returns(apData.ElementType);
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(m => m.GetEnumerator()).Returns(apData.GetEnumerator());
            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.AuditionProfiles)
                .Returns(mockAuditionProfiles.Object);

            mockDB.Setup(x => x.Auditions.Find(It.IsAny<int>())).Returns(mockAuditions.Object.FirstOrDefault(a => a.AuditionId == 2));

            var controller = new PluggedAPIController(mockDB.Object);


            //Act

            controller.CloseAudition(2);

            //Assert


            var audition = mockDB.Object.Auditions.Find(2);
            Assert.IsType<Audition>(audition);
            Assert.IsType<DateTime>(audition.Closed_Date);
            //Assert.True(audition.Closed_Date < System.DateTime.Now);

        }

        [Fact]
        public void OpenAudition_WhenCalled_ChangesAuditionClosedDate()
        {
            //get people who have applied and not those who haven't 

            //ensemble, list of audition, auditionprofile

            //Arrange
            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 1, Ensemble_Name = "Luther Dan", UserId = 5}

            }.AsQueryable();

            var audData = new List<Audition>
            {
                new Audition {AuditionId = 2, EnsembleId = 1, InstrumentId = 3, Closed_Date = DateTime.ParseExact("2018-01-20", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) },
                new Audition {AuditionId = 3}
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1 },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 },
                new Profile { ProfileId = 13, First_Name = "Elwood", Last_Name = "Birch", UserId = 3 }
            }.AsQueryable();

            var apData = new List<AuditionProfile>
            {
                new AuditionProfile{AuditionId = 2, ProfileId = 11},
                new AuditionProfile{AuditionId = 2, ProfileId = 13}
            }.AsQueryable();

            // Create a Mocked DB set
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

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(audData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(audData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(audData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(audData.GetEnumerator());

            var mockAuditionProfiles = new Mock<DbSet<AuditionProfile>>();
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(u => u.Provider).Returns(apData.Provider);
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(m => m.Expression).Returns(apData.Expression);
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(m => m.ElementType).Returns(apData.ElementType);
            mockAuditionProfiles.As<IQueryable<AuditionProfile>>().Setup(m => m.GetEnumerator()).Returns(apData.GetEnumerator());
            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.AuditionProfiles)
                .Returns(mockAuditionProfiles.Object);

            mockDB.Setup(x => x.Auditions.Find(It.IsAny<int>())).Returns(mockAuditions.Object.FirstOrDefault(a => a.AuditionId == 2));

            var controller = new PluggedAPIController(mockDB.Object);


            //Act

            controller.OpenAudition(2);

            //Assert


            var audition = mockDB.Object.Auditions.Find(2);
            Assert.IsType<Audition>(audition);
            Assert.IsType<DateTime>(audition.Closed_Date);
            //Assert.True(System.DateTime.Now < audition.Closed_Date);

        }
        
        [Fact]
        public void addProfile_WhenCalled_AddsProfile()
        {
            //get people who have applied and not those who haven't 

            //ensemble, list of audition, auditionprofile

            //Arrange
            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 1, Ensemble_Name = "Luther Dan", UserId = 5}

            }.AsQueryable();

            var audData = new List<Audition>
            {
                new Audition {AuditionId = 2, EnsembleId = 1, InstrumentId = 3, Audition_Description = "Goodbye"},
                new Audition {AuditionId = 3}
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1},
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2},
                new Profile { ProfileId = 13, First_Name = "Elwood", Last_Name = "Birch", UserId = 3 }
            }.AsQueryable();

            List<ProfileEnsemble> addedProfileEnsembles = new List<ProfileEnsemble>();

            // Create a Mocked DB set
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

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(audData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(audData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(audData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(audData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            mockDB.Setup(x => x.Add(It.IsAny<ProfileEnsemble>())).Callback<ProfileEnsemble>(addedProfileEnsembles.Add);

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            //mockDB.Setup(x => x.ProfileEnsembles.Find(It.IsAny<int>())).Returns(mockProfileEnsembles.Object.FirstOrDefault(a => a.ProfileId == 13));

            var controller = new PluggedAPIController(mockDB.Object);


            var addprof = new server.Controllers.PluggedAPIController.AddProfiletoEnsemble();
            addprof.EnsembleId = "1";
            addprof.name = "Elwood Birch";


            //Act

            controller.addProfile(addprof);

            //Assert

            ProfileEnsemble addedProfile = addedProfileEnsembles.Last();
            Assert.Equal(1, addedProfile.EnsembleId);
            Assert.Equal(13, addedProfile.ProfileId);
            //var audition = audData.Where()
            //Assert.NotNull(audition);
            //Assert.Equal("Hello", audition.Audition_Description);

        }

        [Fact]
        public void addApplicant_WhenCalled_AddsApplicant()
        {
            //get people who have applied and not those who haven't 

            //ensemble, list of audition, auditionprofile

            //Arrange
            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 1, Ensemble_Name = "Luther Dan", UserId = 5}

            }.AsQueryable();

            var audData = new List<Audition>
            {
                new Audition {AuditionId = 2, EnsembleId = 1, InstrumentId = 3, Audition_Description = "Goodbye"},
                new Audition {AuditionId = 3}
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1},
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2},
                new Profile { ProfileId = 13, First_Name = "Elwood", Last_Name = "Birch", UserId = 3 }
            }.AsQueryable();

            List<AuditionProfile> addedAuditionProfiles = new List<AuditionProfile>();

            // Create a Mocked DB set
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

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(audData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(audData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(audData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(audData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            mockDB.Setup(x => x.Add(It.IsAny<AuditionProfile>())).Callback<AuditionProfile>(addedAuditionProfiles.Add);

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            //mockDB.Setup(x => x.ProfileEnsembles.Find(It.IsAny<int>())).Returns(mockProfileEnsembles.Object.FirstOrDefault(a => a.ProfileId == 13));

            var controller = new PluggedAPIController(mockDB.Object);


            var addprof = new server.Controllers.PluggedAPIController.AddProfiletoAudition();
            addprof.AuditionId = "2";
            addprof.ProfileId = "13";


            //Act

            controller.addApplicant(addprof);

            //Assert

            AuditionProfile addedProfile = addedAuditionProfiles.Last();
            Assert.Equal(2, addedProfile.AuditionId);
            Assert.Equal(13, addedProfile.ProfileId);
            //var audition = audData.Where()
            //Assert.NotNull(audition);
            //Assert.Equal("Hello", audition.Audition_Description);

        }

        [Fact]
        public void acceptNewApplicant_WhenCalledOnNewProfile_AddsApplicanttoEnsemble()
        {
            //get people who have applied and not those who haven't 

            //ensemble, list of audition, auditionprofile

            //Arrange
            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 1, Ensemble_Name = "Luther Dan", UserId = 5}

            }.AsQueryable();

            var audData = new List<Audition>
            {
                new Audition {AuditionId = 2, EnsembleId = 1, InstrumentId = 3, Audition_Description = "Goodbye"},
                new Audition {AuditionId = 3}
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1},
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2},
                new Profile { ProfileId = 13, First_Name = "Elwood", Last_Name = "Birch", UserId = 3 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble{EnsembleId = 1, ProfileId = 11},
                new ProfileEnsemble{EnsembleId = 1, ProfileId = 12}
            }.AsQueryable();

            List<ProfileEnsemble> addedProfileEnsembles = new List<ProfileEnsemble>();

            // Create a Mocked DB set
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

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(audData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(audData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(audData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(audData.GetEnumerator());

            var mockProfileEnsembles = new Mock<DbSet<ProfileEnsemble>>();
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Provider).Returns(peData.Provider);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.Expression).Returns(peData.Expression);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.ElementType).Returns(peData.ElementType);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.GetEnumerator()).Returns(peData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            mockDB.Setup(x => x.Add(It.IsAny<ProfileEnsemble>())).Callback<ProfileEnsemble>(addedProfileEnsembles.Add);

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.ProfileEnsembles)
                .Returns(mockProfileEnsembles.Object);

            //mockDB.Setup(x => x.ProfileEnsembles.Find(It.IsAny<int>())).Returns(mockProfileEnsembles.Object.FirstOrDefault(a => a.ProfileId == 13));

            var controller = new PluggedAPIController(mockDB.Object);


            var addprof = new server.Controllers.PluggedAPIController.AcceptApplicant();
            addprof.AuditionId = "2";
            addprof.ProfileId = "13";
            addprof.EnsembleId = "1";


            //Act

            controller.acceptApplicant(addprof);

            //Assert

            ProfileEnsemble addedProfile = addedProfileEnsembles.Last();
            Assert.Equal(1, addedProfile.EnsembleId);
            Assert.Equal(13, addedProfile.ProfileId);
            //var audition = audData.Where()
            //Assert.NotNull(audition);
            //Assert.Equal("Hello", audition.Audition_Description);

        }

        [Fact]
        public void acceptNewApplicant_WhenCalledOnOldProfile_ReturnsNoContent()
        {
            //get people who have applied and not those who haven't 

            //ensemble, list of audition, auditionprofile

            //Arrange
            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 1, Ensemble_Name = "Luther Dan", UserId = 5}

            }.AsQueryable();

            var audData = new List<Audition>
            {
                new Audition {AuditionId = 2, EnsembleId = 1, InstrumentId = 3, Audition_Description = "Goodbye"},
                new Audition {AuditionId = 3}
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1},
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2},
                new Profile { ProfileId = 13, First_Name = "Elwood", Last_Name = "Birch", UserId = 3 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble{EnsembleId = 1, ProfileId = 11},
                new ProfileEnsemble{EnsembleId = 1, ProfileId = 12}
            }.AsQueryable();

            List<ProfileEnsemble> addedProfileEnsembles = new List<ProfileEnsemble>();

            // Create a Mocked DB set
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

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(audData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(audData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(audData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(audData.GetEnumerator());

            var mockProfileEnsembles = new Mock<DbSet<ProfileEnsemble>>();
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Provider).Returns(peData.Provider);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.Expression).Returns(peData.Expression);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.ElementType).Returns(peData.ElementType);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.GetEnumerator()).Returns(peData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            mockDB.Setup(x => x.Add(It.IsAny<ProfileEnsemble>())).Callback<ProfileEnsemble>(addedProfileEnsembles.Add);

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.ProfileEnsembles)
                .Returns(mockProfileEnsembles.Object);

            //mockDB.Setup(x => x.ProfileEnsembles.Find(It.IsAny<int>())).Returns(mockProfileEnsembles.Object.FirstOrDefault(a => a.ProfileId == 13));

            var controller = new PluggedAPIController(mockDB.Object);


            var addprof = new server.Controllers.PluggedAPIController.AcceptApplicant();
            addprof.AuditionId = "2";
            addprof.ProfileId = "11";
            addprof.EnsembleId = "1";


            //Act

            Task<IActionResult> result = controller.acceptApplicant(addprof);

            //Assert

            //ProfileEnsemble addedProfile = addedProfileEnsembles.Last();
            Assert.IsType<Microsoft.AspNetCore.Mvc.NoContentResult>(result.Result);
            //var audition = audData.Where()
            //Assert.NotNull(audition);
            //Assert.Equal("Hello", audition.Audition_Description);

        }

        /*
        [Fact]
        public void getAllAuditions_WhenCalled_ReturnsAllAuditions()
        {
            //get people who have applied and not those who haven't 

            //ensemble, list of audition, auditionprofile
            //Arrange
            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 1, Ensemble_Name = "Luther Dan", UserId = 5, Audition=new List<Audition>{new Audition {AuditionId = 2, EnsembleId = 1, InstrumentId = 3, Audition_Description = "Goodbye", Open_Date = DateTime.ParseExact("2019-01-20", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Closed_Date = DateTime.ParseExact("2019-01-21", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)}, } },
                new Ensemble { EnsembleId = 2, Ensemble_Name = "Derp Group", UserId = 6, Audition=new List<Audition>{new Audition {AuditionId = 3, EnsembleId = 2, InstrumentId = 2, Audition_Description = "loop", Open_Date = DateTime.ParseExact("2019-01-20", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Closed_Date = DateTime.ParseExact("2020-01-20", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)}, new Audition {AuditionId = 4, EnsembleId = 2, InstrumentId = 5, Audition_Description = "loop2", Open_Date = DateTime.ParseExact("2018-02-12", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), Closed_Date = DateTime.ParseExact("2020-01-20", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)} } }
                //new Ensemble { EnsembleId = 1, Ensemble_Name = "Luther Dan", UserId = 5, Audition=new List<Audition>{new Audition()} },
                //new Ensemble { EnsembleId = 2, Ensemble_Name = "Derp Group", UserId = 6, Audition=new List<Audition>{new Audition(), new Audition()} }

            }.AsQueryable();

            // Create a Mocked DB set
        
            var mockEnsembles = new Mock<DbSet<Ensemble>>();
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(u => u.Provider).Returns(eData.Provider);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.Expression).Returns(eData.Expression);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.ElementType).Returns(eData.ElementType);
            mockEnsembles.As<IQueryable<Ensemble>>().Setup(m => m.GetEnumerator()).Returns(eData.GetEnumerator());


            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);


            //mockDB.Setup(x => x.ProfileEnsembles.Find(It.IsAny<int>())).Returns(mockProfileEnsembles.Object.FirstOrDefault(a => a.ProfileId == 13));

            var controller = new PluggedAPIController(mockDB.Object);



            //Act

            Task<ActionResult<IEnumerable<Audition>>> result = controller.getAllAuditions(2);

            //Assert

            var answer = result;


        }
        */
        /*
        public void removeNoReturn(Object O)
        {

        }
        
        [Fact]
        public void removeProfile_WhenCalled_RemovesProfile()
        {
            //get people who have applied and not those who haven't 

            //ensemble, list of audition, auditionprofile

            //Arrange
            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 1, Ensemble_Name = "Luther Dan", UserId = 5}

            }.AsQueryable();

            var audData = new List<Audition>
            {
                new Audition {AuditionId = 2, EnsembleId = 1, InstrumentId = 3, Audition_Description = "Goodbye"},
                new Audition {AuditionId = 3}
            }.AsQueryable();

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = 1},
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2},
                new Profile { ProfileId = 13, First_Name = "Elwood", Last_Name = "Birch", UserId = 3 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble{ProfileId = 11, EnsembleId = 1},
                new ProfileEnsemble{ProfileId = 12, EnsembleId = 1}
            }.AsQueryable();

            List<ProfileEnsemble> addedProfileEnsembles = new List<ProfileEnsemble>();
            List<ProfileEnsemble> removedProfileEnsembles = new List<ProfileEnsemble>() { new ProfileEnsemble { ProfileId = 12, EnsembleId = 1 } };

            // Create a Mocked DB set
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

            var mockAuditions = new Mock<DbSet<Audition>>();
            mockAuditions.As<IQueryable<Audition>>().Setup(u => u.Provider).Returns(audData.Provider);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.Expression).Returns(audData.Expression);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.ElementType).Returns(audData.ElementType);
            mockAuditions.As<IQueryable<Audition>>().Setup(m => m.GetEnumerator()).Returns(audData.GetEnumerator());

            var mockProfileEnsembles = new Mock<DbSet<ProfileEnsemble>>();
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Provider).Returns(peData.Provider);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.Expression).Returns(peData.Expression);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.ElementType).Returns(peData.ElementType);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(m => m.GetEnumerator()).Returns(peData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            mockDB.Setup(x => x.Add(It.IsAny<ProfileEnsemble>())).Callback<ProfileEnsemble>(addedProfileEnsembles.Add);

            mockDB.Setup(x => x.Remove(It.IsAny<ProfileEnsemble>())).Callback<ProfileEnsemble>(removedProfileEnsembles.RemoveAt(0));
            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Auditions)
                .Returns(mockAuditions.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.ProfileEnsembles)
                .Returns(mockProfileEnsembles.Object);

            //mockDB.Setup(x => x.ProfileEnsembles.Find(It.IsAny<int>())).Returns(mockProfileEnsembles.Object.FirstOrDefault(a => a.ProfileId == 13));

            var controller = new PluggedAPIController(mockDB.Object);


            var remprof = new server.Controllers.PluggedAPIController.RemoveProfileEnsemble();
            remprof.EnsembleId = "1";
            remprof.ProfileId = "12";


            //Act

            controller.removeProfile(remprof);

            //Assert

            //var audition = audData.Where()
            //Assert.NotNull(audition);
            //Assert.Equal("Hello", audition.Audition_Description);

        }
        */
    


    }
}