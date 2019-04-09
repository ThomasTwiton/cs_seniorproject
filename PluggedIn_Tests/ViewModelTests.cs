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
  /* Since the model objects in ViewObjects.cs only
   * encapsulate the data being passed to the view, 
   * the following tests only ensure that they are
   * not changed unexpectedly.
   */
  
  public class ViewModelTests
  {
  
      public void ProfileModel_Always_HoldsExpectedItems()
      {
          /* Arrange */
          
          /* Act */
          
          /* Assert */
          
      }
  }
}
