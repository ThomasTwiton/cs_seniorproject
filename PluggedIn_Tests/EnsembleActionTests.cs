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
    public class EnsembleActionTests
    {
        [Fact]
        public void Ensemble_Always_ReturnsAppropriateView() { }

        [Fact]
        public void Ensemble_Always_HandlesInvalidEnsembleId() { }

        [Fact]
        public void Ensemble_Alawys_DisplaysMembersForEnsemble() { }

        [Fact]
        public void Ensemble_WhenUserOwnsPage_SetsIsOwnerToTrue() { }

        [Fact]
        public void Profile_Always_DisplaysPosts() { }
    }
}
