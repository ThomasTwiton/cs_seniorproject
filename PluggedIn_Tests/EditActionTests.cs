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
    public class EditActionTests
    {
        [Fact]
        public void Edit_WhenCalledWithGet_ReturnsValidView() { }

        [Fact]
        public void Edit_WhenCalledWithPost_UpdatesDatabase() { }

        [Fact]
        public void Edit_WhenGivenNewInstruments_DeletesOldInstruments() { }

    }
}
