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
    public class ProfileActionTests
    {
        [Fact]
        public void Profile_WhenUserOwnsPage_SetsIsOwnerToTrue()
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
                new User { UserId = 1, Email = "ElijasReshmi@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },
                new User { UserId = 2, Email = "EugeniaCornelius@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" }
            }.AsQueryable();

            // The profile that is expected to be passed to the view model.
            var expectedProfile = new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = aUserId };

            var pData = new List<Profile>
            {
                expectedProfile,
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 21, Ensemble_Name = "Small Garage Band", UserId = 1 },
                new Ensemble { EnsembleId = 22, Ensemble_Name = "Smooth Jazz Combo", UserId = 2 },
                new Ensemble { EnsembleId = 23, Ensemble_Name = "Chamber Group", UserId = 2 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 23}
            }.AsQueryable();

            var poData = new List<Post> { }.AsQueryable();

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

            var mockProfileEnsembles = new Mock<DbSet<ProfileEnsemble>>();
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Provider).Returns(peData.Provider);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Expression).Returns(peData.Expression);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.ElementType).Returns(peData.ElementType);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.GetEnumerator()).Returns(peData.GetEnumerator());

            var mockPosts = new Mock<DbSet<Post>>();
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Expression).Returns(poData.Expression);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.ElementType).Returns(poData.ElementType);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.GetEnumerator()).Returns(poData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.ProfileEnsembles)
                .Returns(mockProfileEnsembles.Object);

            mockDB.Setup(x => x.Posts)
                .Returns(mockPosts.Object);

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
            var result = controller.Profile(11);

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            ProfileModel viewModel = (ProfileModel)viewResult.Model;

            Assert.Equal("Profile", viewResult.ViewName);
            Assert.True(viewModel.isOwner);
        }

        [Fact]
        public void Profile_Always_ReturnsAppropriateView()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 2;
            var aLoggedIn = false;

            // The following convention will be used for users and passwords:
            //                  email : FirstnameLastname@unit.test
            //      unhashed password : bestRA
            //   Targeted User's Salt : unsafe3
            //       Misc Users' Salt : wrong

            var uData = new List<User>() {
                new User { UserId = 1, Email = "ElijasReshmi@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },
                new User { UserId = 2, Email = "EugeniaCornelius@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" }
            }.AsQueryable();

            // The profile that is expected to be passed to the view model.
            var expectedProfile = new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = aUserId };

            var pData = new List<Profile>
            {
                expectedProfile,
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 21, Ensemble_Name = "Small Garage Band", UserId = 1 },
                new Ensemble { EnsembleId = 22, Ensemble_Name = "Smooth Jazz Combo", UserId = 2 },
                new Ensemble { EnsembleId = 23, Ensemble_Name = "Chamber Group", UserId = 2 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 23}
            }.AsQueryable();

            var poData = new List<Post> { }.AsQueryable();

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

            var mockProfileEnsembles = new Mock<DbSet<ProfileEnsemble>>();
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Provider).Returns(peData.Provider);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Expression).Returns(peData.Expression);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.ElementType).Returns(peData.ElementType);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.GetEnumerator()).Returns(peData.GetEnumerator());

            var mockPosts = new Mock<DbSet<Post>>();
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Expression).Returns(poData.Expression);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.ElementType).Returns(poData.ElementType);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.GetEnumerator()).Returns(poData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.ProfileEnsembles)
                .Returns(mockProfileEnsembles.Object);

            mockDB.Setup(x => x.Posts)
                .Returns(mockPosts.Object);

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
            var result = controller.Profile(11);

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            ProfileModel viewModel = (ProfileModel)viewResult.Model;

            Assert.Equal("Profile", viewResult.ViewName);
            Assert.Equal(expectedProfile, viewModel.Profile);
            Assert.False(viewModel.isOwner);
            Assert.False(viewModel.isLoggedIn);
            Assert.Equal("profile", viewModel.ViewType);
        }

        [Fact]
        public void Profile_Always_HandlesInvalidProfileId()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 2;
            var aLoggedIn = false;

            var pData = new List<Profile>
            {
                new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = aUserId },
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            // Create Mocked DB sets
            var mockProfiles = new Mock<DbSet<Profile>>();
            mockProfiles.As<IQueryable<Profile>>().Setup(u => u.Provider).Returns(pData.Provider);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.Expression).Returns(pData.Expression);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.ElementType).Returns(pData.ElementType);
            mockProfiles.As<IQueryable<Profile>>().Setup(m => m.GetEnumerator()).Returns(pData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

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
            var result = controller.Profile(1111111);

            /* Assert */
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public void Profile_WhenProfileIsEnembleMember_DisplaysEnsemblesForProfile()
        {
            /* Arrange */

            // Set active user parameters (For GetSessionInfo)
            var aUserId = 1;
            var aLoggedIn = false;

            // The profile that is expected to be passed to the view model.
            var expectedProfile = new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = aUserId };

            var pData = new List<Profile>
            {
                expectedProfile,
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();


            // Expected Ensembles to show up in the profile view
            var e1 = new Ensemble { EnsembleId = 21, Ensemble_Name = "Small Garage Band", UserId = 1 };
            var e2 = new Ensemble { EnsembleId = 22, Ensemble_Name = "Smooth Jazz Combo", UserId = 2 };

            var eData = new List<Ensemble>
            {
                e1,
                e2,
                new Ensemble { EnsembleId = 23, Ensemble_Name = "Chamber Group", UserId = 2 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble { ProfileId = expectedProfile.ProfileId, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = expectedProfile.ProfileId, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 23}
            }.AsQueryable();

            var poData = new List<Post> { }.AsQueryable();

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

            var mockPosts = new Mock<DbSet<Post>>();
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Expression).Returns(poData.Expression);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.ElementType).Returns(poData.ElementType);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.GetEnumerator()).Returns(poData.GetEnumerator());

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
            var result = controller.Profile(11);

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            ProfileModel viewModel = (ProfileModel)viewResult.Model;

            Assert.Equal(2, viewModel.Ensembles.Count);

        }

        [Fact]
        public void Profile_WhenPostsExist_DisplaysPosts()
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
                new User { UserId = 1, Email = "ElijasReshmi@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" },
                new User { UserId = 2, Email = "EugeniaCornelius@unit.test", Password = "JGrs/FmgBDsAFA2td+MusWZffFZ4zh80l20UWPyekwrKpxTfa0wrZ7xNDL5QyNbv8beD93EaGN+4179NoBsAIg==", Salt = "wrong" }
            }.AsQueryable();

            // The profile that is expected to be passed to the view model.
            var expectedProfile = new Profile { ProfileId = 11, First_Name = "Elijas", Last_Name = "Reshmi", UserId = aUserId };

            var pData = new List<Profile>
            {
                expectedProfile,
                new Profile { ProfileId = 12, First_Name = "Eugenia", Last_Name = "Cornelius", UserId = 2 }
            }.AsQueryable();

            var eData = new List<Ensemble>
            {
                new Ensemble { EnsembleId = 21, Ensemble_Name = "Small Garage Band", UserId = 1 },
                new Ensemble { EnsembleId = 22, Ensemble_Name = "Smooth Jazz Combo", UserId = 2 },
                new Ensemble { EnsembleId = 23, Ensemble_Name = "Chamber Group", UserId = 2 }
            }.AsQueryable();

            var peData = new List<ProfileEnsemble>
            {
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 11, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 21},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 22},
                new ProfileEnsemble { ProfileId = 12, EnsembleId = 23}
            }.AsQueryable();

            var poData = new List<Post>
            {
                new Post { PostId = 44, PosterType = "profile", Text = "Test Text", PosterIndex = expectedProfile.ProfileId},
                new Post { PostId = 45, PosterType = "profile", Text = "Test Text", PosterIndex = expectedProfile.ProfileId},
                new Post { PostId = 46, PosterType = "ensemble", Text = "Test Text", PosterIndex = expectedProfile.ProfileId},
                new Post { PostId = 47, PosterType = "profile", Text = "Test Text", PosterIndex = 12}

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

            var mockProfileEnsembles = new Mock<DbSet<ProfileEnsemble>>();
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Provider).Returns(peData.Provider);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.Expression).Returns(peData.Expression);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.ElementType).Returns(peData.ElementType);
            mockProfileEnsembles.As<IQueryable<ProfileEnsemble>>().Setup(u => u.GetEnumerator()).Returns(peData.GetEnumerator());

            var mockPosts = new Mock<DbSet<Post>>();
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.Expression).Returns(poData.Expression);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.ElementType).Returns(poData.ElementType);
            mockPosts.As<IQueryable<Post>>().Setup(u => u.GetEnumerator()).Returns(poData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfiles.Object);

            mockDB.Setup(x => x.Ensembles)
                .Returns(mockEnsembles.Object);

            mockDB.Setup(x => x.ProfileEnsembles)
                .Returns(mockProfileEnsembles.Object);

            mockDB.Setup(x => x.Posts)
                .Returns(mockPosts.Object);

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
            var result = controller.Profile(11);

            /* Assert */
            var viewResult = Assert.IsType<ViewResult>(result);
            ProfileModel viewModel = (ProfileModel)viewResult.Model;

            Assert.Equal("Profile", viewResult.ViewName);
            Assert.Equal(2, viewModel.Posts.Count);
        }


    }
}
