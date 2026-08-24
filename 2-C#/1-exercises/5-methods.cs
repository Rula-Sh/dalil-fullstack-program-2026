/* ------------------------------------------ Methods ------------------------------------------ */

/*Create a method that takes an array of integers and splits it into two arrays:
 * • One containing even numbers   • One containing odd numbers. */

using System;

namespace MyFirstApp
{
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
}