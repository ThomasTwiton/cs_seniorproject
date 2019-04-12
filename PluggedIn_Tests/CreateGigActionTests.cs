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
    public class CreateGigActionTests
    {
        [Fact]
        public void CreateGig_WhenGivenValidData_CreatesNewGig() { }

        [Fact]
        public void CreateGig_WhenGivenValidData_ReturnsValidViewt() { }

        [Fact]
        public void CreateGig_Always_HandlesDuplicateGigs() { }

        [Fact]
        public void CreateGig_Always_HandlesMissingGigData() { }
    }
}
