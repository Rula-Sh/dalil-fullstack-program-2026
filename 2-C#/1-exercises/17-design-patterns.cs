/* -------------------------------------- Design Patterns -------------------------------------- */

/* Implement the Strategy pattern for order discounts:
   • Define an IDiscountStrategy interface with one method: CalculateDiscount(double total) 
   • Create two implementations: PercentageDiscount and FlatDiscount, each computing the discount differently 
   • Inject the chosen strategy into an OrderProcessor and swap it at runtime. 
   Hint: define the interface first, then wire the strategy into OrderProcessor via its constructor. */

interface IDiscountStrategy
{
    double CalculateDiscount(double total);
}

class PercentageDiscount : IDiscountStrategy
{
    private double _percentage;

    public PercentageDiscount(double percentage)
    {
        _percentage = percentage;
    }

    public double CalculateDiscount(double total) => total * _percentage;
}

class FlatDiscount : IDiscountStrategy
{
    private double _deduction;

    public FlatDiscount(double deduction)
    {
        _deduction = deduction;
    }

    public double CalculateDiscount(double total) => _deduction;
}

class OrderProcessor
{
    private IDiscountStrategy _discountStrategy;

    public OrderProcessor(IDiscountStrategy discountStrategy)
    {
        _discountStrategy = discountStrategy;
    }

    public double ApplyDiscount(double total) => total - _discountStrategy.CalculateDiscount(total);
}

class Program
{
    static void Main(string[] args)
    {
        bool inputIsValid = false;
        bool isPercentageDiscount;

        Console.WriteLine("Do you want a percentage 0.15 discount \"Enter 1\" or a flat $20 discount \"Enter 2\"?");
        while (!inputIsValid)
        {
            try
            {
                int input = int.Parse(Console.ReadLine());
                if (input == 1)
                    isPercentageDiscount = true;
                else if (input == 2)
                    isPercentageDiscount = false;
                else throw new Exception("Please enter either 1 or 2. Please try again, percentage discount \"Enter 1\" or flat discount \"Enter 2\"");
                inputIsValid = true;

                IDiscountStrategy discountStrategy = isPercentageDiscount ? new PercentageDiscount(0.15) : new FlatDiscount(20);

                OrderProcessor orderProcessor = new OrderProcessor(discountStrategy);

                Console.WriteLine($"$122 Final price after discount: ${orderProcessor.ApplyDiscount(122)}");

            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please try again, percentage discount \"Enter 1\" or flat discount \"Enter 2\"");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}