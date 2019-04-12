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
    public class CreatePostActionTests
    {
        [Fact]
        public void CreatePost_WhenGivenValidData_CreatesNewPost() { }

        [Fact]
        public void CreatePost_WhenGivenValidData_ReturnsValidView() { }

        [Fact]
        public void CreatePost_Always_HandlesMissingEnsembleData() { }
    }
}
