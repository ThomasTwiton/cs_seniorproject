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
    public class AuditionActionTests
    {
        [Fact]
        public void Audition_Always_DisplaysAppropriateProfile() { }

        [Fact]
        public void Audition_Always_DisplayAppropriateAudition() { }

        [Fact]
        public void Audition_Always_HandlesInvalidAuditionId() { }
    }
}
