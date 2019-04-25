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
        public void CreateEnsemble_WhenGivenValidData_CreatesNewEnsemble() { }

        [Fact]
        public void CreateEnsemble_WhenGivenValidData_ReturnsValidRedirect() { }

        [Fact]
        public void CreateEnsemble_Always_HandlesDuplicateEnsembles() { }

        [Fact]
        public void CreateEnsemble_Always_HandlesMissingEnsembleData() { }
    }
}
