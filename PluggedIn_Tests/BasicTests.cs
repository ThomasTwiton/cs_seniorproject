using Xunit;

namespace PluggedIn_Tests
{
    public class CSharpTests
    {
        // Facts are tests should be used when you expect 
        //  the same result from the test no matter the input.
        [Fact]
        public void PassingTest()
        {
            // The common practice for unit tests is to divide 
            //  the test into 3 parts: Arrange, Act, and Assert.

            //  1.) Arrange all necessary preconditions and inputs.
            //  2.) Act on the object or method under test.
            //  3.) Assert that the expected results have occurred.

            // Arrange
            int num = 2;
            int sum;

            // Act
            sum = num + num;

            // Assert
            Assert.Equal(4, sum);
        }


        // Theories want to test the same method with many 
        //  different inputs to confirm that it holds up.

        // Data is passed into the exam below using the 
        //  [InlineData] attribute.
        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        public void MyFirstTheory(int value)
        {
            // Arrange
            bool isOdd;

            // Act
            isOdd = value % 2 == 1;

            // Assert
            Assert.True(isOdd);
        }

    }
}
