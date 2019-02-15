using Xunit;

namespace PluggedIn_Tests
{
    public class CSharpTests
    {

        [Fact]
        public void PassingTest()
        {

            // Arrange
            int num = 2;
            int sum;

            // Act
            sum = num + num;

            // Assert
            Assert.Equal(4, sum);
        }


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