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
    public class CreateAuditionActionTests
    {
        [Fact]
        public void CreateAudition_WhenGivenValidData_CreatesNewAudition() { }

        [Fact]
        public void CreateAudition_WhenGivenIncompleteData_CreatesNewAudition() { }

        [Fact]
        public void CreateAudition_Always_ReturnsValidView() { }

        [Fact]
        public void CreateAudition_Always_HandlesDuplicateAuditionData() { }

    }
}
