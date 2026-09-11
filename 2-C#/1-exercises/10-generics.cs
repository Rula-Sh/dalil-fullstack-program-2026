/* ------------------------------------------ Generics ------------------------------------------ */

/* Create a generic class called Matcher that:
   • Accepts two generic type parameters (T1 and T2)
   • Contains one method called Compare that receives one input of each type
   • Prints whether the two inputs are equal to each other or not */

class Matcher<T1, T2>
{
    public void Compare(T1 t1, T2 t2)
    {
        Console.WriteLine($"{t1} and {t2} are {(t1.ToString() == t2.ToString() ? "equal" : "not equal")}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Matcher<double, string> matcher1 = new Matcher<double, string>();
        matcher1.Compare(3.5, "g");
        Matcher<int, int> matcher2 = new Matcher<int, int>();
        matcher2.Compare(1, 1);
    }
}