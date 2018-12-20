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
    public class CreateVenueActionTests
    {
        [Fact]
        public void CreateVenue_WhenGivenValidData_CreatesNewVenue() { }

        [Fact]
        public void CreateVenue_WhenGivenValidData_ReturnsValidView() { }

        [Fact]
        public void CreateVenue_Always_HandlesDuplicateVenues() { }

        [Fact]
        public void CreateVnue_Always_HandlesMissingVenueData() { }
    }
}
