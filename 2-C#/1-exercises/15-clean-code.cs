/* ----------------------------------------- Clean Code ----------------------------------------- */

/* Refactor a method called ProcessOrder that mixes three jobs in one cramped block:
    • Cryptic names like p, q, x instead of price, quantity, taxRate 
    • Magic number 0.08 for tax, mixed with validation and console output 
    • Extract each concern into its own method and replace the magic number with a named const. 
    Hint: rename variables first, then pull the tax calculation into its own method. */

public class OrderProcessor
{
    private const double TaxRate = 0.08;
    public double ProcessOrder(double price, int quantity)
    {
        double subtotal = price * quantity;
        return subtotal + CalculateTax(subtotal);
    }
    private double CalculateTax(double subtotal) => subtotal * TaxRate;
}