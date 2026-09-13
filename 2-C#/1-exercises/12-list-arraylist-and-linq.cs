/* -------------------------------------------- List -------------------------------------------- */

/* Create a Customer class with the following properties:
   • Name — string   • Id — int   • PhoneNumber — string
   Then, define a List<Customer> and populate it with at least two customer objects. 
   Apply Add() and Insert() to modify the list. */

class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }

}
class Program
{
    static void Main(string[] args)
    {
        List<Customer> customers = new List<Customer>() { new Customer() { Id = 1, Name = "Ahmad", PhoneNumber = "0799999999" } };
        customers.Add(new Customer() { Id = 2, Name = "Basel", PhoneNumber = "0799999999" });
        customers.Insert(0, new Customer() { Id = 3, Name = "Noor", PhoneNumber = "0799999999" });

        foreach (var customer in customers)
        {
            Console.WriteLine($"ID: {customer.Id}, Name: {customer.Name}, Phone Number: {customer.PhoneNumber}");
        }

/* ----------------------------------------- Array List ----------------------------------------- */

/* Using the List<Customer> created in the previous exercise, write a LINQ query to:
    1. Display all customer names that contain the letter 'a'.
    2. Order and display all customers sorted by Id in descending order.
    3. Create an Order class and group orders by the fulfilling store.
    4. Join Customer and Order lists on orderId. */


        var namesWithA = from customer in customers where customer.Name.Contains('a') select customer.Name;
        Console.Write("\nNames that contains the letter a: ");
        foreach (var name in namesWithA)
        {
            Console.Write($"{name}  ");
        }

        var namesOrdered = from customer in customers orderby customer.Id ascending select customer.Name;
        Console.Write("\nNames Ordered by Id: ");
        foreach (var name in namesOrdered)
        {
            Console.Write($"{name}  ");
        }

        List<Order> orders = new List<Order>() { new Order() { Id = 1, Product = "Laptop", Price = 250, Store  = "EMarket" },
                                                    new Order() { Id = 2, Product = "PC", Price = 760, Store  = "EMarket" },
                                                    new Order() { Id = 3, Product = "Keyboard", Price = 12, Store  = "PCMarket" }};

        var ordersGroupedByStore = from order in orders group order by order.Store;
        Console.Write("\nNames Ordered by Id: ");
        foreach (var store in ordersGroupedByStore)
        {
            Console.Write($"\n    {store.Key}:  ");
            foreach (var order in store)
            {
                Console.Write($"{order.Product}  ");
            }

        }

        var customersJoinedOrders = from order in orders
                                    join customer in customers on order.Id equals customer.Id
                                    select new
                                    {
                                        customerId = customer.Id,
                                        productName = order.Product,
                                        store = order.Store
                                    };
        Console.Write("\nCustomers and their Product: ");
        foreach (var customer in customersJoinedOrders)
        {
            Console.Write($"{customer}  ");
        }
        Console.WriteLine();
    }
}
class Order
{
    public int Id { get; set; }
    public string Product { get; set; }
    public int Price { get; set; }
    public string Store { get; set; }
}


/* -------------------------------------------- LINQ -------------------------------------------- */
/* ----------------------------------------- In Lecture ----------------------------------------- */

/* Develop a C# Online Shopping System that manages different types of products and calculates their final prices. Requirements:
    1- Create an enum named ProductType: Electronic, Clothing, Food
    2- Create an abstract class Product containing: o Name o Price o ProductType o A nullable double? Discount o An abstract method CalculateFinalPrice().
    3- Create an interface IDiscountable containing: double GetDiscount();
    4- Create three classes: o Electronic o Clothing o Food Each class must inherit from Product and implement IDiscountable. Each type must calculate its final price differently using polymorphism.
    5- Create a static int ProductCount to count the number of created products.
    6- Create a generic class: Cart<T> that can store products of type T.
    7- In Main(): o Create at least 5 products. o Store them in a List<Product>.
    8- Use Exception Handling to handle: o Negative prices. o Invalid discounts. o Attempting to calculate a price when the discount is null.
    9- Use LINQ to: o Find products with a final price greater than 50. o Find the most expensive product. o Calculate the average final price. o Sort products by final price in descending order. o Count products by ProductType.*/

// 1- Create an enum named ProductType: Electronic, Clothing, Food
public enum ProductType { Electronic, Clothing, Food }

// 2- Create an abstract class Product containing: o Name o Price o ProductType o A nullable double? Discount o An abstract method CalculateFinalPrice().
abstract class Product
{
    public string Name { get; set; }
    public double Price { get; set; }
    public ProductType ProductType { get; set; }
    public double? Discount { get; set; }
    // 5- Create a static int ProductCount to count the number of created products.
    public static int ProductCount { get; set; }

    protected Product(string name, double price, double? discount)
    {
        //8 - Use Exception Handling to handle: o Negative prices.o Invalid discounts.o Attempting to calculate a price when the discount is null.
        if (price < 0)
            throw new ArgumentException("Price cannot be negative.");

        if (discount.HasValue && (discount.Value < 0 || discount.Value > 100))
            throw new ArgumentException("Discount must be between 0 and 100.");

        Name = name;
        Price = price;
        Discount = discount;
        ProductCount++;
    }

    public abstract double CalculateFinalPrice();
}

// 3- Create an interface IDiscountable containing: double GetDiscount();
public interface IDiscountable
{
    double GetDiscount();
}

// 4- Create three classes: o Electronic o Clothing o Food Each class must inherit from Product and implement IDiscountable.Each type must calculate its final price differently using polymorphism.
class Electronic : Product, IDiscountable
{
    public Electronic(string name, double price, double? discount) : base(name, price, discount)
    {
        ProductType = ProductType.Electronic;
    }

    public double GetDiscount() => Discount ?? 0.1;

    public override double CalculateFinalPrice()
    {
        double discount = GetDiscount();

        return Price - (Price * discount);
    }
}
class Clothing : Product, IDiscountable
{
    public Clothing(string name, double price, double? discount) : base(name, price, discount)
    {
        ProductType = ProductType.Clothing;
    }

    public double GetDiscount() => Discount ?? 0.2;

    public override double CalculateFinalPrice()
    {
        double discount = GetDiscount();

        return Price - (Price > 100 ? 20 : Price * discount);
    }
}
class Food : Product, IDiscountable
{
    public Food(string name, double price, double? discount) : base(name, price, discount)
    {
        ProductType = ProductType.Food;
    }

    public double GetDiscount() => Discount ?? 0;

    public override double CalculateFinalPrice()
    {
        double discount = GetDiscount();

        return Price - (Price > discount ? discount : Price);
    }
}

// 6- Create a generic class: Cart<T> that can store products of type T.
class Cart<T>
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        items.Add(item);
    }
}

class Program
{
    static void Main(string[] args)
    {
        //7 - In Main(): o Create at least 5 products.o Store them in a List<Product>.
        List<Product> products = new List<Product>(){
            new Electronic("Laptop", 1200, 0.4),
            new Electronic("Headphones", 80, 0.8),
            new Clothing("Jacket", 90, 0.2),
            new Clothing("T-Shirt", 25, 0.5),
            new Food("Coffee", 40, 2),
            new Food("Cereal", 15, null),
            //new Electronic("TV", -50, 10),
            //new Clothing("Jeans", 60, 150),
        };

        Cart<Product> shoppingCart = new Cart<Product>();
        foreach (var p in products)
        {
            shoppingCart.Add(p);
        }

        Console.WriteLine($"Total Products Instantiated: {Product.ProductCount}\n");

        //9 - Use LINQ to:
        //o Find products with a final price greater than 50.
        var productsWithFinalPriceAbove50 = from product in products
                                            where product.CalculateFinalPrice() > 50
                                            select product;
        Console.Write("Products that have its final price above 50: ");
        foreach (var product in productsWithFinalPriceAbove50)
            Console.Write(product.Name + " ");

        //o Find the most expensive product.
        var mostExpensiveProduct = (from product in products
                                    select product).FirstOrDefault();
        Console.WriteLine($"\nThe most expensive product is: {mostExpensiveProduct.Name}");

        //o Calculate the average final price.
        var averageFinalProduct = (from product in products
                                    select product.CalculateFinalPrice()).Average();
        Console.WriteLine($"The average of the products final price is: {averageFinalProduct:F2}");

        //o Sort products by final price in descending order.
        var finalPriceinDescending = from product in products
                                        orderby product.CalculateFinalPrice() descending
                                        select product;
        Console.Write("Products in descending based on the final price: ");
        foreach (var product in finalPriceinDescending)
            Console.Write(product.Name + " ");

        //o Count products by ProductType.*/
        var countProductsByProductType = from product in products
                                            group product by product.ProductType into typeGroup
                                            select new
                                            {
                                                ProductType = typeGroup.Key,
                                                Count = typeGroup.Count()
                                            };
        Console.Write("\nThe number of products per product type: ");
        foreach (var product in countProductsByProductType)
        {
            Console.Write($"{product.ProductType}({product.Count}) ");
        }
        Console.WriteLine();
    }
}