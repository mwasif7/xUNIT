using Xunit;

public class MathOperationTest{

[Fact]
  public void Add_Two_Number()
  {
    //arrange
    var num1 = 15;
    var num2 = 10;
    var expectedValue = 25;

    //act
    var sum =  MathOperation.Add(num1, num2);

    //assert
    Assert.Equal(expectedValue, sum, 1); 

  }

[Fact]
  public void Sub_Two_Number()
  {
    //arrange
    var num1 = 15;
    var num2 = 10;
    var expectedValue = 5;

    //act
    var sum = MathOperation.Subtract(num1, num2);

    //assert
    Assert.Equal(expectedValue, sum, 1);

  }

  [Theory]
  [InlineData(5,8,13)]
  [InlineData(0, 5, 5)]
  [InlineData(-6, 8, 2)]
  public void Test_Add(int num1, int num2, int expectedValue)
  {

    var sum = MathOperation.Add(num1, num2);

    //assert
    Assert.Equal(expectedValue, sum, 1);

  }

}