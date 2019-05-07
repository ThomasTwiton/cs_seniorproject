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
using System;

namespace PluggedIn_Tests
{

    public class CreatePostActionTests
    {
        //public Post returningAdd(List<Post> addedposts, Post post_toAdd)
        //{
        //    addedposts.Add(post_toAdd);
        //    return post_toAdd;
        //}

        [Fact]
        public void CreatePost_WhenGivenValidData_CreatesNewPost() {

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

            var poData = new List<Post>
            {
                new Post { PostId = 1, PosterType = "profile", PosterIndex = 11, Text = "Hi everybody!"},
                new Post{ PostId = 2, PosterType = "ensemble", PosterIndex = 21, Type = "aud", Ref_Id = 1, Text = "We're looking for a new member!" }
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

            var mockPost = new Mock<DbSet<Post>>();
            mockPost.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPost.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(poData.Expression);
            mockPost.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(poData.ElementType);
            mockPost.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(poData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfile.Object);

            Post post = new Post();
            post.PostId = 3;
            post.Text = "Hello world";
            post.PosterType = "profile";
            post.PosterIndex = 11;

            List<Post> addedPosts = new List<Post>();
            
            mockDB.Setup(x => x.Posts)
                .Returns(mockPost.Object);

            mockDB.Setup(x => x.Add(It.IsAny<Post>()))
                .Callback<Post>(addedPosts.Add)
                .Returns(mockDB.Object.Add(post));
            mockDB.Setup(x => x.Posts.Add(It.IsAny<Post>()))
                .Callback<Post>(addedPosts.Add)
                .Returns(mockDB.Object.Add(post));

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

            ProfileModel model = new ProfileModel();
            model.Profile = pData.First();

            /* Act */
            var result = controller.createPost(post, model, post.PosterType, post.PosterIndex);

            /* Assert */
            Post newPost = addedPosts.Last();
            Assert.Equal(3, newPost.PostId);
            Assert.Equal("Hello world", newPost.Text);
            Assert.Equal("profile", newPost.PosterType);
            Assert.Equal(11, newPost.PosterIndex);
        }

        [Fact]
        public void CreatePost_WhenProfileGivesValidData_ReturnsValidView() {
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

            var poData = new List<Post>
            {
                new Post { PostId = 1, PosterType = "profile", PosterIndex = 11, Text = "Hi everybody!"},
                new Post{ PostId = 2, PosterType = "ensemble", PosterIndex = 21, Type = "aud", Ref_Id = 1, Text = "We're looking for a new member!" }
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

            var mockPost = new Mock<DbSet<Post>>();
            mockPost.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPost.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(poData.Expression);
            mockPost.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(poData.ElementType);
            mockPost.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(poData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfile.Object);

            mockDB.Setup(x => x.Posts)
                .Returns(mockPost.Object);
            mockDB.Setup(x => x.Posts.Find((It.IsAny<int>())))
                .Returns(mockPost.Object.FirstOrDefault(p => p.PostId == 3));


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

            ProfileModel model = new ProfileModel();
            model.Profile = pData.First();

            Post post = new Post();
            post.PostId = 3;
            post.Text = "Hello world";
            post.PosterType = "profile";
            post.PosterIndex = 11;

            /* Act */
            var result = controller.createPost(post, model, post.PosterType, post.PosterIndex);

            /* Assert */
            Assert.IsType<RedirectToActionResult>(result.Result);
            RedirectToActionResult redirect = (RedirectToActionResult)result.Result;
            Assert.Equal("Profile", redirect.ActionName);
        }
        [Fact]
        public void CreatePost_WhenEnsembleGivesValidData_ReturnsValidView()
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

            var poData = new List<Post>
            {
                new Post { PostId = 1, PosterType = "profile", PosterIndex = 11, Text = "Hi everybody!"},
                new Post{ PostId = 2, PosterType = "ensemble", PosterIndex = 21, Type = "aud", Ref_Id = 1, Text = "We're looking for a new member!" }
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

            var mockPost = new Mock<DbSet<Post>>();
            mockPost.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPost.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(poData.Expression);
            mockPost.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(poData.ElementType);
            mockPost.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(poData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfile.Object);

            mockDB.Setup(x => x.Posts)
                .Returns(mockPost.Object);
            mockDB.Setup(x => x.Posts.Find((It.IsAny<int>())))
                .Returns(mockPost.Object.FirstOrDefault(p => p.PostId == 3));


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

            ProfileModel model = new ProfileModel();
            model.Profile = pData.First();

            Post post = new Post();
            post.PostId = 3;
            post.Text = "Hello world";
            post.PosterType = "ensemble";
            post.PosterIndex = 21;

            /* Act */
            var result = controller.createPost(post, model, post.PosterType, post.PosterIndex);

            /* Assert */
            Assert.IsType<RedirectToActionResult>(result.Result);
            RedirectToActionResult redirect = (RedirectToActionResult)result.Result;
            Assert.Equal("Ensemble", redirect.ActionName);
        }

        [Fact]
        public void CreatePost_WhenVenueGivesValidData_ReturnsValidView()
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

            var poData = new List<Post>
            {
                new Post { PostId = 1, PosterType = "profile", PosterIndex = 11, Text = "Hi everybody!"},
                new Post{ PostId = 2, PosterType = "ensemble", PosterIndex = 21, Type = "aud", Ref_Id = 1, Text = "We're looking for a new member!" }
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

            var mockPost = new Mock<DbSet<Post>>();
            mockPost.As<IQueryable<Post>>().Setup(u => u.Provider).Returns(poData.Provider);
            mockPost.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(poData.Expression);
            mockPost.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(poData.ElementType);
            mockPost.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(poData.GetEnumerator());

            // Create a Mocked DB
            var mockDB = new Mock<PluggedContext>();

            // Set up necessary Mocked DB methods
            mockDB.Setup(x => x.Users)
                .Returns(mockUsers.Object);

            mockDB.Setup(x => x.Profiles)
                .Returns(mockProfile.Object);

            mockDB.Setup(x => x.Posts)
                .Returns(mockPost.Object);
            mockDB.Setup(x => x.Posts.Find((It.IsAny<int>())))
                .Returns(mockPost.Object.FirstOrDefault(p => p.PostId == 3));


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

            ProfileModel model = new ProfileModel();
            model.Profile = pData.First();

            Post post = new Post();
            post.PostId = 3;
            post.Text = "Hello world";
            post.PosterType = "venue";
            post.PosterIndex = 31;

            /* Act */
            var result = controller.createPost(post, model, post.PosterType, post.PosterIndex);

            /* Assert */
            Assert.IsType<RedirectToActionResult>(result.Result);
            RedirectToActionResult redirect = (RedirectToActionResult)result.Result;
            Assert.Equal("Venue", redirect.ActionName);
        }
    }
}
