/* ------------------------------------------ Nullable ------------------------------------------ */

/* Create a class that contains a method to calculate the sum of two numbers.
   The method should return null if either number is undefined (null) or missing. 
   Use nullable value types for the input fields. */

class Calculator { 
    public int? Add(int? x, int? y)
    {
        if (x == null || y == null)
            return null;

        return x + y;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Calculator calculator = new Calculator();

        int? sumResult = calculator.Add(2, 3);
        if (!sumResult.HasValue)
        {
            sumResult = sumResult.GetValueOrDefault();
            Console.WriteLine($"invalid calculation, returning {sumResult}");
        }                
        else
            Console.WriteLine($"The summation result is {sumResult}");
    }
}
