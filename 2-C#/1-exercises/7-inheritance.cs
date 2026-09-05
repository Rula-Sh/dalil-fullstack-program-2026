/* ---------------------------------------- Inheritance ---------------------------------------- */

/* Single Inheritance: 
   Create a base class Product with properties Name and Price, and a method DisplayInfo(). 
   Derive a class Electronics that adds a WarrantyYears property and an ElectronicsInfo() method. 
   Create an object and call both methods. */
/* Hierarchical Inheritance: 
   Using the same Product base class, also create a Clothing class that inherits from Product and adds a Size property. 
   Demonstrate that both Electronics and Clothing can access DisplayInfo() without redefining it. */

class Product
{
    public string Name { get; set; }
    public int Price { get; set; }

    public Product(string name, int price)
    {
        Name = name;
        Price = price;
    }

    public void DisplayInfo()
    {
        Console.Write($"The {Name} is ${Price}");
    }
}

class Electronics : Product
{
    public int WarrantyYears { get; set; }

    public Electronics(string name, int price, int warranty) : base(name, price)
    {
        WarrantyYears = warranty;
    }
    public void ElectronicsInfo()
    {
        this.DisplayInfo();

        Console.WriteLine($" and has {WarrantyYears} years of warranty.");
    }
}

class Clothing : Product
{
    public string Size { get; set; }

    public Clothing(string name, int price, string size) : base(name, price)
    {
        Size = size;
    }

    public void ClothingInfo()
    {
        this.DisplayInfo();

        Console.WriteLine($" and its size is {Size}.");
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Display chair info:");
        Product chair = new Product("Chair", 15);
        chair.DisplayInfo();
        Console.WriteLine("\n");

        Console.WriteLine("Display mouse electronic info:");
        Electronics mouse = new Electronics("Mouse", 5, 2);
        mouse.ElectronicsInfo();
        Console.WriteLine("Display mouse product info:");
        mouse.DisplayInfo();
        Console.WriteLine("\n");

        Console.WriteLine("Display jacket clothing info:");
        Clothing jacket = new Clothing("Jacket", 20, "L");
        jacket.ClothingInfo();
        Console.WriteLine("Display jacket product info:");
        jacket.DisplayInfo();
        Console.WriteLine("\n");

    }
}


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


