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
    static void Main(string[] args)
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


/* ------------------------------- In Lecture ------------------------------- */

/*Create a C# Console Application to manage employees in a company. Requirements:
    1- Create a class called Employee with the following private fields: o name o id o salary
    2- Create appropriate public methods to: o Set employee information. o Display employee information.
    3- Create a method called CalculateBonus() that returns the employee's bonus: o Normal bonus = 10% of salary.
    4- Create another version of CalculateBonus() : o This method calculates the bonus based on the percentage provided.
    5- Create a class called Manager that inherits from Employee.
    6- Add a private field to Manager: o teamSize
    7- In Manager, override or create a method to display the manager's information, including: o Name o ID o Salary o Team size
    8- Create objects from both classes: o One Employee object. o One Manager object.
    9- Use the objects to: o Display their information. o Calculate the normal bonus. o Calculate a bonus using a custom percentage. */

class Employee
{
    // 1- Create a class called Employee with the following private fields: o name o id o salary
    private int id;
    private string name;
    private double salary;

    public int Id
    {
        get => id;
        set => id = value;
    }
    public string Name
    {
        get => name;
        set => name = value;
    }
    public double Salary
    {
        get => salary;
        set => salary = value;
    }

    // 2- Create appropriate public methods to: o Set employee information.o Display employee information.
    public Employee(int id, string name, double salary)
    {
        this.id = id;
        this.name = name;
        this.salary = salary;
    }        
    public string DisplayEmployeeInformation()
    {
        return $"Employee {name} with ID {id} has the salary of {salary}";
    }
        
    // 3- Create a method called CalculateBonus() that returns the employee's bonus: o Normal bonus = 10% of salary.
    public double CalculateBonus()
    {
        return salary * 0.1;
    }
        
    // 4- Create another version of CalculateBonus() : o This method calculates the bonus based on the percentage provided.
    public double CalculateBonus(double bonus)
    {
        return salary * bonus;
    }

}

// 5- Create a class called Manager that inherits from Employee.
class Manager : Employee
{
    // 6- Add a private field to Manager: o teamSize
    private int teamSize;

    public int TeamSize
    {
        get => teamSize;
        set => teamSize = value;
    }
        
    public Manager(int id, string name, double salary, int teamSize) : base(id, name, salary)
    {
        this.teamSize = teamSize;
    }

    // 7- In Manager, override or create a method to display the manager's information, including: o Name o ID o Salary o Team size
    public string DisplayManagerInformation()
    {
        return DisplayEmployeeInformation() + $" and a team size of: {teamSize}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        // 8- Create objects from both classes: o One Employee object. o One Manager object.
        Manager manager = new Manager(2, "Qasem", 500, 4);
        Employee employee = new Employee(2, "Ahmad", 500);

        // 9- Use the objects to: o Display their information. o Calculate the normal bonus. o Calculate a bonus using a custom percentage. */
        Console.WriteLine(manager.DisplayManagerInformation());
        Console.WriteLine(employee.DisplayEmployeeInformation());

        Console.WriteLine($"{employee.Name} employee received {employee.CalculateBonus(0.2)} bonus");
    }
}