public static class MathOperation
{
  public  static void Init(double num1, double num2)
  {
    Console.WriteLine("Result of addition is {0}", Add(num1, num2));
    Console.WriteLine("Result of substraction is {0}", Subtract(num1, num2));
    Console.WriteLine("Result of multiply is {0}", Multiply(num1, num2));
    Console.WriteLine("Result of divide is {0}", Divide(num1, num2));

  }
  public static double Add(double number1, double number2)
  {
    return (number1 + number2);
  }

  public static double Subtract(double number1, double number2)
  {
    return (number1 - number2);
  }

  public static double Multiply(double number1, double number2)
  {
    return (number1 * number2);
  }

  public static double Divide(double number1, double number2)
  {
    return (number1 / number2);
  }
}