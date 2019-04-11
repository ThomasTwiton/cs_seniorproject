using Moq;
using Xunit;
using System.Linq;
using System.Text;
using server.Models;
using server.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;


namespace PluggedIn_Tests
{
    public class LoginActionTests
    {

        private readonly PluggedContext LoadedContext;

        [Fact]
        public void Login_ProfileAssociatedWithUser_ReturnsRedirectToProfile()
        {
            /* Arrange */

            // The following convention will be used for users and passwords:
            //                  email : FirstnameLastname@unit.test
            //      unhashed password : FirstnameLastname

            var userEmails = new List<string>() {
                "ElijasReshmi@unit.test",
                "EugeniaCornelius@unit.test",
                "JaysonFerruccio@unit.test",
                "HoratioRajendra@unit.test",
                "SaraRachyl@unit.test"
            };

            // Create the user DB with appropriate salts
            var rawUData = new List<User>();
            var uid = 1;
            foreach (var email in userEmails)
            {
                // Create new user
                User user = new User();
                user.UserId = uid;
                user.Email = email;

                var pwd = email.Split("@")[0];

                //Generate a cryptographic random number.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] buff = new byte[32];
                rng.GetBytes(buff);

                string salt = System.Convert.ToBase64String(buff);

                // Set the user's salt
                user.Salt = salt;

                string saltAndPwd = pwd + salt;
                byte[] bytes = Encoding.ASCII.GetBytes(saltAndPwd);
                SHA512 shaM = new SHA512Managed();
                byte[] hashedPwdbytes = shaM.ComputeHash(bytes);
                string hashedPwd = System.Convert.ToBase64String(hashedPwdbytes);
                user.Password = hashedPwd;

                uid++;

                rawUData.Add(user);

            }

            var uData = rawUData.AsQueryable();

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

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            var controller = new HomeController(mockDB.Object, mockHostEnv.Object);


            /* Act */
            var result = controller.Login("ElijasReshmi@unit.test", "reshel01");

            /* Assert */
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Profile", redirectResult.ActionName);
            Assert.Equal(11, redirectResult.RouteValues["id"]);

        }

        [Fact]
        public void Login_EnsembleAssociatedWithUser_ReturnsRedirectToEnsemble() { }

        [Fact]
        public void Login_VenueAssociatedWithUser_ReturnsRedirectToVenue() { }

        [Fact]
        public void Login_Always_HandlesUnknownEmailandPassword() { }

        [Fact]
        public void Login_WhenPrevActionIsSet_ReturnsRedirectToThePrevAction() { }

        [Fact]
        public void Login_NoProfileEnsembleVenueAssociatedWithUser_ReturnsCreateProfileView() { }

        [Fact]
        public void Login_WhenErrorExistsInLoginData_ReturnsRedirectToLoginGetAction() { }
    }
}
