# PluggedIn Unit Tests
PluggedIn uses xUnit on Travis-CI for its unit testing solution.

## Writing Tests

### Naming Conventions

Each file should be labeled in such a way as to indicate what 
component is being tested within and should end with `Tests.cs`. 
Similarly, each class within a file should be labeled to indicate 
which sub-component is being tested and each function should be 
labeled to indicate what unit is being tested. Additionally, each 
function should be named as `[Subject]_[Scenario]_[Result]` 
where:

- _Subject_ is the name of the method being tested.
- _Scenario_ is the circumstances that the test covers.
- _Result_ is the expected outcome of the invoking the method under
the test.


For example, a test that is designed to test the functionality 
of a controller named `HomeController` would be called
`HomeController_Tests.cs`. Within that test class, there could
be a function called: `Profile_WhenGivenValidId_ReturnsValidProfileView()` 
that checks to make sure the profile page returns the appropriate 
view with the appropriate and complete model.


### xUnit Syntax

xUnit uses `[Attributes]` to indicate what functions are Facts 
and Theories. The `[Fact]` attribute defines tests that are to 
be used when you expect the same result from the test no matter 
the input. The `[Theory]` attribute is used to define a test a 
method with different inputs to confirm that it holds up in 
various cases.

For Theories, data is passed into the method using the 
`[InlineData()]` attribute and passing the appropriate value 
into attribute (as if you are passing it as a function parameter).

### Best Practices

A common practice for unit testing is to divide the test into 
3 parts: Arrange, Act and Assert.

First, **Arrange** all necessary preconditions and inputs.
Second, **Act** on the object or method under test.
Third, **Assert** that the expected results have occurred.

For an example of how this might look, see `CSharpTests.cs`.


## Running Tests

### Visual Studio

To run tests within Visual Studio, simply go to Test > Run > 
All Tests. Alternatively, you can open the Test Explorer and 
click "Run All".

### Command Line

To run tests via the command line, navigate to the 
`PluggedIn_Tests\` directory and execute the following command:

`
$ dotnet test
`


