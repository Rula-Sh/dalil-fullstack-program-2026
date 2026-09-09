/* ------------------------------------------ Methods ------------------------------------------ */

/*Create a method that takes an array of integers and splits it into two arrays:
 * • One containing even numbers   • One containing odd numbers. */

class Program
{
    static void Main(string[] args)
    {
        int[] numbers = { 10, 7, 15, 20, 8, 3, 12 };
        (int[] evenArr, int[] oddArr) = splitEvenOdd(numbers);
        Console.Write("Even Numbers:");
        foreach (int even in evenArr)
        {
            Console.Write(even + " ");
        }
        Console.WriteLine();
        Console.Write("Odd Numbers: ");
        foreach (int odd in oddArr)
        {
            Console.Write(odd + " ");
        }
    }
    public static (int[], int[]) splitEvenOdd(int[] numbers)
    {
        int countEven = 0, countOdd = 0;
        for (int i = 0; i < numbers.Length; i++)
        {
            if (numbers[i] % 2 == 0)
                countEven++;
            else
                countOdd++;
        }

        int[] evens = new int[countEven];
        int[] odds = new int[countOdd];
        int evenIndex = 0;
        int oddIndex = 0;
        foreach (int num in numbers)
        {
            if (num % 2 == 0)
            {
                evens[evenIndex] = num;
                evenIndex++;
            }
            else
            {
                odds[oddIndex] = num;
                oddIndex++;
            }
        }
        return (evens, odds);
    }

}


/* ----------------------------------------- In Lecture ----------------------------------------- */

/* Write a C# Console Application to manage the inventory of a small store. Requirements:
    1- Ask the user to enter the number of products.
    2- Store the prices and quantities of the products using arrays.
    3- Use a 2D array to store product information: o Column 1 → Price o Column 2 → Quantity
    4- Create a method CalculateTotal() that receives the price and quantity and returns the total value of the product.
    5- Use if-else to classify each product: o Total< 100 → Low Value o 100–500 → Medium Value o 500 → High Value
    6- Use switch to display a message based on the product category.
    7- find: o The most expensive product. o The total number of items in stock.
    8- Display a report similar to: Product 1 Price: 50 Quantity: 4 Total Value: 200 Category: Medium Value Message: Normal Product */

class Program
{
    // 4- Create a method CalculateTotal() that receives the price and quantity and returns the total value of the product.
    public static double CalculateTotal(double price, double quantity)
    {
        return price * quantity;
    }

    // 5- Use if-else to classify each product: o Total< 100 → Low Value o 100–500 → Medium Value o 500 → High Value
    public static string CategorizeProduct(double total)
    {
        if (total < 100)
        {
            return "Low Value";
        }
        else if (total > 100 && total < 500)
        {
            return "Medium Value";
        }
        else
        { //if (total > 500)
            return "High Value";
        }
    }

    // 6- Use switch to display a message based on the product category.
    public static string ProductMessage(double total)
    {
        string category = CategorizeProduct(total);
        switch (category)
        {
            case "Low Value":
                return "Cheap Product";
            case "Medium Value":
                return "Normal Product";
            case "High Value":
                return "Expensive Product";
            default: return "Uncategorized Product";
        }
    }

    static void Main()
    {
        // 1- Ask the user to enter the number of products.
        Console.Write("Enter the number of products:");
        int prodNum = int.Parse(Console.ReadLine());

        // 2- Store the prices and quantities of the products using arrays.
        // 3- Use a 2D array to store product information: o Column 1 → Price o Column 2 → Quantity
        double[,] prodArr = new double[prodNum, 2];
        for (int i = 0; i < prodNum; i++)
        {
            Console.Write($"Enter product #{i + 1} price: ");
            prodArr[i, 0] = double.Parse(Console.ReadLine());
            Console.Write($"Enter product #{i + 1} quantity: ");
            prodArr[i, 1] = int.Parse(Console.ReadLine());

            double total = CalculateTotal(prodArr[i, 0], prodArr[i, 1]);
        }

        // 7- find: o The most expensive product. o The total number of items in stock.
        double maxVal = 0;
        double totalQuantity = 0;
        for (int i = 0; i < prodNum; i++)
        {
            totalQuantity += prodArr[i, 1];

            if (prodArr[i, 0] > maxVal)
                maxVal = prodArr[i, 0];
        }
        Console.WriteLine($"\nThe most expensive product costs {maxVal}.");
        Console.WriteLine($"The total number of items in stock is {totalQuantity}.");

        // 8- Display a report similar to: Product 1 Price: 50 Quantity: 4 Total Value: 200 Category: Medium Value Message: Normal Product 
        Console.WriteLine("\nMarket Stock:");
        for (int i = 0; i < prodNum; i++)
        {
            double productTotal = CalculateTotal(prodArr[i, 0], prodArr[i, 1]);
            Console.WriteLine($"Product {i + 1} Price: {prodArr[i, 0]} Quantity: {prodArr[i, 1]} Total Value: {productTotal} Category: {CategorizeProduct(productTotal)} Message: {ProductMessage(productTotal)}");
        }
    }
}