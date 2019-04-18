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
    public class ProfileActionTests
    {
        [Fact]
        public void Profile_Always_ReturnsAppropriateView() { }

        [Fact]
        public void Profile_Always_HandlesInvalidProfileId() { }

        [Fact]
        public void Profile_Alawys_DisplaysEnsemblesForProfile() { }

        [Fact]
        public void Profile_WhenUserOwnsPage_SetsIsOwnerToTrue() { }

        [Fact]
        public void Profile_Always_DisplaysPosts() { }


    }
}
