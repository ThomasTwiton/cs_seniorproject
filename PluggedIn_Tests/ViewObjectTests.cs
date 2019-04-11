using Moq;
using Xunit;
using System.Linq;
using server.Models;
using server.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PluggedIn_Tests
{
    /* Since the model objects in ViewObjects.cs only
    * encapsulate the data being passed to the view, 
    * the following tests only ensure that they are
    * not changed unexpectedly.
    */

    public class ViewObjectTests
  {
        [Fact]
        public void PageModel_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            //Create a Page Model
            var model = new PageModel();

            //Create valid data
            var file = new Mock<IFormFile>();
            var user = new User();
            var view = "profile";
            var owns = false;
            var logged = true;
            var posts = new HashSet<Post>();
            

            /* Act */
            model.File = file.Object;
            model.User = user;
            model.ViewType = view;
            model.isOwner = owns;
            model.isLoggedIn = logged;
            model.Posts = posts;

            /* Assert */
            Assert.IsAssignableFrom<IFormFile>(model.File);
            Assert.IsType<User>(model.User);
            Assert.IsType<string>(model.ViewType);
            Assert.IsType<bool>(model.isOwner);
            Assert.IsType<bool>(model.isLoggedIn);
            Assert.IsType<HashSet<Post>>(model.Posts);                      
        }
        

        [Fact]
        public void PageModel_WhenGivenInvalidData_Errors() {
            /* Arrange */
            /* Act */    
            /* Assert */
        }

        [Fact]
        public void ProfileModel_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            //Create a Profile Model
            var model = new ProfileModel();

            var profile = new Profile();
            var ensembles = new List<Ensemble>();
            var instruments = new List<SelectListItem>();
            var selected = new List<string>();

            /* Act */
            model.Profile = profile;
            model.Ensembles = ensembles;
            model.Instruments = instruments;
            model.SelectedInsIds = selected;

            /* Assert */
            Assert.IsType<Profile>(model.Profile);
            Assert.IsType<List<Ensemble>>(model.Ensembles);
            Assert.IsType<List<SelectListItem>>(model.Instruments);
            Assert.IsType<List<string>>(model.SelectedInsIds);
        }

        [Fact]
        public void ProfileModel_WhenGivenInvalidData_Errors()
        {
            /* Arrange */
            /* Act */
            /* Assert */
        }

        [Fact]
        public void EnsembleModel_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */
            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            //Create a Ensemble Model
            var model = new EnsembleModel();

            //create valid data
            var ensemble = new Ensemble();
            var profiles = new List<Profile>();
            var instruments = new List<SelectListItem>();
            var selected = new List<string>();
            var auds = new HashSet<Audition>();

            /* Act */
            model.Ensemble = ensemble;
            model.Profiles = profiles;
            model.Instruments = instruments;
            model.SelectedInsId = selected;
            model.Audition = auds;

            /* Assert */
            Assert.IsType<Ensemble>(model.Ensemble);
            Assert.IsType<List<Profile>>(model.Profiles);
            Assert.IsType<List<string>>(model.SelectedInsId);
            Assert.IsAssignableFrom<ICollection<Audition>>(model.Audition);
        }

        [Fact]
        public void EnsembleModel_WhenGivenInvalidData_Errors() {
            /* Arrange */
            /* Act */
            /* Assert */
        }

        [Fact]
        public void VenueModel_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */
            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            //Create a Venue Model
            var model = new VenueModel();

            //create valid data
            var venue = new Venue();

            /* Act */
            model.Venue = venue;

            /* Assert */
            Assert.IsType<Venue>(model.Venue);
        }

        [Fact]
        public void VenueModel_WhenGivenInvalidData_Errors()
        {
            /* Arrange */
            /* Act */
            /* Assert */
        }

        [Fact]
        public void AuditionModel_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */
            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            //Create a Audition Model
            var model = new AuditionModel();

            //create valid data
            var ensemble = new Ensemble();
            var audition = new Audition();
            var view = "profile";
            var profiles = new List<Profile>();


            /* Act */
            model.Ensemble = ensemble;
            model.Audition = audition;
            model.ViewType = view;
            model.Profiles = profiles;

            /* Assert */
            Assert.IsType<Ensemble>(model.Ensemble);
            Assert.IsType<Audition>(model.Audition);
            Assert.IsType<string>(model.ViewType);
            Assert.IsAssignableFrom<ICollection<Profile>>(model.Profiles);
        }

        [Fact]
        public void AuditionModel_WhenGivenInvalidData_Errors()
        {
            /* Arrange */
            /* Act */
            /* Assert */
        }

        [Fact]
        public void GigModel_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */

            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            //Create a Gig Model
            var model = new GigModel();

            //create valid data
            var view = "ensemble";
            var gig = new Gig();
            var venue = new Venue();

            /* Act */
            model.ViewType = view;
            model.Gig = gig;
            model.Venue = venue;

            /* Assert */
            Assert.IsType<string>(model.ViewType);
            Assert.IsType<Gig>(model.Gig);
            Assert.IsType<Venue>(model.Venue);
        }

        [Fact]
        public void GigModel_WhenGivenInvalidData_Errors()
        {
            /* Arrange */
            /* Act */
            /* Assert */
        }

        [Fact]
        public void EnsembleDashModel_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */
            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            //Create a Dash Model
            var model = new EnsembleDashModel();

            //create valid data
            var ensemble = new Ensemble();
            var members = new List<ProfileEnsemble>();
            var auds = new List<Audition>();
            var audnums = new Dictionary<int, int>();

            /* Act */
            model.Ensemble = ensemble;
            model.Members = members;
            model.AuditionList = auds;
            model.AuditionNumbers = audnums;

            /* Assert */
            Assert.IsType<Ensemble>(model.Ensemble);
            Assert.IsType<List<ProfileEnsemble>>(model.Members);
            Assert.IsType<List<Audition>>(model.AuditionList);
            Assert.IsType<Dictionary<int, int>>(model.AuditionNumbers);
        }

        [Fact]
        public void EnsembleDashModel_WhenGivenInvalidData_Errors()
        {
            /* Arrange */
            /* Act */
            /* Assert */
        }

        [Fact]
        public void SessionModel_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */
            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            //Create a Session Model
            var model = new SessionModel();

            //create valid data
            var userid = 1;
            var logged = true;
            var prev = "Index";

            /* Act */
            model.UserID = userid;
            model.IsLoggedIn = logged;
            model.PrevAction = prev;

            /* Assert */
            Assert.IsType<int>(model.UserID);
            Assert.IsType<bool>(model.IsLoggedIn);
            Assert.IsType<string>(model.PrevAction);
        }

        [Fact]
        public void SessionModel_WhenGivenInvalidData_Errors()
        {
            /* Arrange */
            /* Act */
            /* Assert */
        }

        [Fact]
        public void SearchModel_WhenGivenValidData_StoresValidData()
        {
            /* Arrange */
            // Create a Mocked IHostingEnviornment
            var mockHostEnv = new Mock<IHostingEnvironment>();

            //Create a Search Model
            var model = new SearchModel();

            //create valid data
            var auditions = new HashSet<Audition>();
            var audcount = 100;
            var gigs = new HashSet<Gig>();
            var gigcount = 50;
            var profiles = new HashSet<Profile>();
            var profcount = 6;
            var ensembles = new HashSet<Ensemble>();
            var enscount = 32;
            var venues = new HashSet<Venue>();
            var vencount = 1;
            var query = "Jazz";

            /* Act */
            model.Auditions = auditions;
            model.AuditionCount = audcount;
            model.Gigs = gigs;
            model.GigCount = gigcount;
            model.Profiles = profiles;
            model.ProfileCount = profcount;
            model.Ensembles = ensembles;
            model.EnsembleCount = enscount;
            model.Venues = venues;
            model.VenueCount = vencount;
            model.Query = query;

            /* Assert */
            Assert.IsType<HashSet<Audition>>(model.Auditions);
            Assert.IsType<HashSet<Gig>>(model.Gigs);
            Assert.IsType<HashSet<Profile>>(model.Profiles);
            Assert.IsType<HashSet<Ensemble>>(model.Ensembles);
            Assert.IsType<HashSet<Venue>>(model.Venues);
            Assert.IsType<int>(model.AuditionCount);
            Assert.IsType<int>(model.GigCount);
            Assert.IsType<int>(model.ProfileCount);
            Assert.IsType<int>(model.VenueCount);
            Assert.IsType<int>(model.EnsembleCount);
            Assert.IsType<string>(model.Query);
        }

        [Fact]
        public void SearchModel_WhenGivenInvalidData_Errors()
        {
            /* Arrange */
            /* Act */
            /* Assert */
        }
    }
  
}
