/*Write a C# Console Application to manage the inventory of a small store.
Requirements
- Ask the user to enter the number of products.
- Store the prices and quantities of the products using arrays.
- Use a 2D array to store product information: o Column 1 → Price o Column 2 → Quantity
- Create a method CalculateTotal() that receives the price and quantity and returns the total value of the product.
- Use if-else to classify each product: o Total< 100 → Low Value o 100–500 → Medium Value o 500 → High Value
- Use switch to display a message based on the product category.
- find: o The most expensive product. o The total number of items in stock.
- Display a report similar to: Product 1 Price: 50 Quantity: 4 Total Value: 200 Category: Medium Value Message: Normal Product */



using System;

class Program
{
    // -Create a method CalculateTotal() that receives the price and quantity and returns the total value of the product.
    public static int CalculateTotal(int price, int quantity)
    {
        return price * quantity;
    }

    public static string CategorizeProduct(int price, int quantity)
    {
        int total=0;
        if (total < 100)
        {
            return"Low Value";
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

    static void Main()
    {

        //-Ask the user to enter the number of products.

        Console.WriteLine("Enter the number of products");
        int prodNum = int.Parse(Console.ReadLine());

        // - Store the prices and quantities of the products using arrays.
        // - Use a 2D array to store product information: o Column 1 → Price o Column 2 → Quantity

        int[,] prodArr = new int[prodNum, 2];
        for (int i = 0; i < prodNum; i++)
        {
            Console.WriteLine($"Enter product #{i} price");
            prodArr[i,0] = int.Parse(Console.ReadLine());
            Console.WriteLine($"Enter product #{i} quantity");
            prodArr[i,1] = int.Parse(Console.ReadLine());
        }


        // - Use if-else to classify each product: o Total< 100 → Low Value o 100–500 → Medium Value o 500 → High Value
        

        // - Use switch to display a message based on the product category.



        // -find: o The most expensive product. o The total number of items in stock.
        int maxVal = 0;
        for (int i = 0; i <= prodNum; i++)
        {
            if (prodArr[i, 0] > maxVal)
            {
                maxVal = prodArr[i, 0];
            }
        }

        int totalQuantity = 0;
        for (int i = 0; i <= prodNum; i++)
        {
            totalQuantity += prodArr[i, 1];
        }

        // - Display a report similar to: Product 1 Price: 50 Quantity: 4 Total Value: 200 Category: Medium Value Message: Normal Product 
    }
}


