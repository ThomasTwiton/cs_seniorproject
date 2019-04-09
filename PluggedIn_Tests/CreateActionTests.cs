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
    public class CreateActionTests
    {
        [Fact]
        public void Create_WhenGivenValidData_CreatesNewUser() { }

        [Fact]
        public void Create_WhenGivenValidData_ReturnsValidView() { }

        [Fact]
        public void Create_Always_HandlesDuplicateUsers() { }

        [Fact]
        public void Create_Always_HandlesMissingUserData() { }

    }
}
