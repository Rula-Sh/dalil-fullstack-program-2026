/* ----------------------------------- Classes & Constructors ----------------------------------- */

/* 1- Define the Class: Create a class called Cart with two properties : ProductName and itemPrice. 
 * 2-DisplaySummary Method: Create a method called DisplaySummary() that outputs the product name, item price, price with 15% tax, and price after a 10% discount.
 * 3- IsOnSale Method Create a method called IsOnSale() that returns true if the item price is less than 50.0, and false otherwise. */

using System;

namespace MyFirstApp
{
    public class Cart
    {
        // Parameterized constructor
        public Cart(string ProductName, double ItemPrice)
        {
            this.ProductName = ProductName;
            this.ItemPrice = ItemPrice;
        }

        // Auto-implemented properties
        public string ProductName { get; set; }
        public double ItemPrice { get; set; }

        public void DisplaySummary()
        {
            Console.WriteLine($"Product name        : {ProductName} \n " + // The :C format specifier formats numbers as currency using the local culture settings.
                              $"Price               : {ItemPrice:C} \n " +
                              $"Price with tax      : {ItemPrice + (ItemPrice * 0.15):C} \n " + // OR {ItemPrice * 1.15:C} => 1.15  = original price (1.0) + 15% tax (0.15)
                              $"Price after discount: {ItemPrice - (ItemPrice * 0.1):C}"); // OR {ItemPrice * 0.90:C} => 0.90 = original price (1.0) - 10% discount (0.10)
        }

        public bool IsOnSale()
        {
            if (ItemPrice < 50.0)
                return true;
            else
                return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Cart item = new Cart("Wireless Headphones", 39.99);
            item.DisplaySummary();
            if (item.IsOnSale())
                Console.WriteLine("This item is on sale!");
            else
                Console.WriteLine("Regular price item.");
        }
    }
}