/* ------------------------------------- Exception Handling ------------------------------------- */

/* Research Challenge:
   Investigate whether C# allows you to throw a custom exception of your own design.
   For example: can you throw a custom OutOfStockException when a customer orders more items than are available in stock? */

public class OutOfStockException() : Exception($"This product is out of stock") { }

class Program
{
    try
    {
        bool isProductOutOfStock = true;
        if (isProductOutOfStock)
            throw new OutOfStockException();
    }
    catch (OutOfStockException e)
    {
        Console.WriteLine(e.Message);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}