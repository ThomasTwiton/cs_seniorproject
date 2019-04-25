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
    public class CreateEnsembleActionTests
    {
        [Fact]
        public void CreateEnsemble_WhenGivenValidData_CreatesNewEnsemble() {
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

            List<Ensemble> addedEnsembles = new List<Ensemble>();

            mockDB.Setup(x => x.Add(It.IsAny<Ensemble>()))
                .Callback<Ensemble>(addedEnsembles.Add);

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

            /* Act */

            var result = controller.CreateEnsemble("Turqoise Group", new System.DateTime(2019, 4, 24), new System.DateTime(9999, 9, 9), "Decorah", "A group", "IA", "Brass Band", "Jazz", 6, model);

            /* Assert */
            Ensemble addedEnsemble = addedEnsembles.Last();
            Assert.Equal("Turqoise Group", addedEnsemble.Ensemble_Name);
            Assert.Equal(new System.DateTime(2019, 4, 24), addedEnsemble.Formed_Date);
            Assert.Equal(new System.DateTime(9999, 9, 9), addedEnsemble.Disbanded_Date);
            Assert.Equal("Decorah", addedEnsemble.City);
            Assert.Equal("IA", addedEnsemble.State);
            Assert.Equal("A group", addedEnsemble.Bio);
            Assert.Equal("Brass Band", addedEnsemble.Type);
            Assert.Equal("Jazz", addedEnsemble.Genre);

        }

        [Fact]
        public void CreateEnsemble_WhenGivenValidData_ReturnsValidRedirect() {
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

            List<Ensemble> addedEnsembles = new List<Ensemble>();

            mockDB.Setup(x => x.Add(It.IsAny<Ensemble>()))
                .Callback<Ensemble>(addedEnsembles.Add);

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

            /* Act */

            var result = controller.CreateEnsemble("Turqoise Group", new System.DateTime(2019, 4, 24), new System.DateTime(9999, 9, 9), "Decorah", "A group", "IA", "Brass Band", "Jazz", 6, model);

            /* Assert */
            Assert.IsType<RedirectToActionResult>(result.Result);
            RedirectToActionResult redirect = (RedirectToActionResult)result.Result;
            Assert.Equal("Ensemble", redirect.ActionName);
        }

        [Fact]
        public void CreateEnsembleModal_WhenGivenValidData_CreatesNewEnsemble()
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

            List<Ensemble> addedEnsembles = new List<Ensemble>();

            mockDB.Setup(x => x.Add(It.IsAny<Ensemble>()))
                .Callback<Ensemble>(addedEnsembles.Add);

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

            var result = controller.CreateEnsembleModal("Turqoise Group", new System.DateTime(2019, 4, 24), new System.DateTime(9999, 9, 9), "Decorah", "A group", "IA", "Brass Band", "Jazz", 6);

            /* Assert */
            Ensemble addedEnsemble = addedEnsembles.Last();
            Assert.Equal("Turqoise Group", addedEnsemble.Ensemble_Name);
            Assert.Equal(new System.DateTime(2019, 4, 24), addedEnsemble.Formed_Date);
            Assert.Equal(new System.DateTime(9999, 9, 9), addedEnsemble.Disbanded_Date);
            Assert.Equal("Decorah", addedEnsemble.City);
            Assert.Equal("IA", addedEnsemble.State);
            Assert.Equal("A group", addedEnsemble.Bio);
            Assert.Equal("Brass Band", addedEnsemble.Type);
            Assert.Equal("Jazz", addedEnsemble.Genre);
        }

        [Fact]
        public void CreateEnsembleModal_WhenGivenValidData_ReturnsValidRedirect() {
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

            List<Ensemble> addedEnsembles = new List<Ensemble>();

            mockDB.Setup(x => x.Add(It.IsAny<Ensemble>()))
                .Callback<Ensemble>(addedEnsembles.Add);

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

            var result = controller.CreateEnsembleModal("Turqoise Group", new System.DateTime(2019, 4, 24), new System.DateTime(9999, 9, 9), "Decorah", "A group", "IA", "Brass Band", "Jazz", 6);

            /* Assert */
            Assert.IsType<RedirectToActionResult>(result.Result);
            RedirectToActionResult redirect = (RedirectToActionResult)result.Result;
            Assert.Equal("Ensemble", redirect.ActionName);
        }
    }
}
