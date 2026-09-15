/* ------------------------------------- Lambda Expressions ------------------------------------- */

/* Using Lambda Expressions: Create a list of 10 integers. 
   Calculate the square of each number, then find and display only those that are divisible by 3. */

class Program
{
    static void Main(string[] args)
    {
        List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        var squares = numbers.Select(number => number * number);
        Console.Write("The squares of numbers are: ");
        foreach (var item in squares)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();

        Console.Write("Numbers divisible by 3: ");
        numbers.FindAll(x => x % 3 == 0).ForEach(x => { Console.Write(" "); Console.Write(x); });
        Console.WriteLine();

        Console.Write("Squares of the numbers that are divisible by 3: ");
        var squaresDivisible = numbers.Select(number => number * number).Where(number => number % 3 == 0);
        foreach (var square in squaresDivisible)
        {
            Console.Write(square + " ");
        }
        Console.WriteLine();
    }
}
