using Moq;
using Xunit;
using System.Linq;
using server.Models;
using server.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PluggedIn_Tests
{
    public class CreateProfileActionTests
    {
        [Fact]
        public void CreateProfile_WhenGivenValidData_CreatesNewProfile() { }

        [Fact]
        public void CreateProfile_WhenGivenValidData_ReturnsValidView() { }

        [Fact]
        public void CreateProfile_Always_HandlesDuplicateProfiles() { }

        [Fact]
        public void CreateProfile_Always_HandlesMissingProfileData() { }
    }
}
