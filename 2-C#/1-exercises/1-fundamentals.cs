/* -------------------------------------- Type Conversion -------------------------------------- */

/* 1. Write a program that reads two string values from the keyboard, then converts them into integers and prints their sum.
 * 2. Write a second program that calculates the average of the two numbers and displays the result as a double value. */

// Using Convert Class
Console.WriteLine("Enter the first number: ");
int number1 = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("Enter the second number: ");
int number2 = Convert.ToInt32(Console.ReadLine());
int sum = number1 + number2;
Console.Write("the sum of " + number1 + " and " + number2 + " is: " + sum);

double avg = (number1 + number2) / 2.0; // 2.0 was used on purpose so that the compiler calculates the result as double
Console.WriteLine("and the is: " + avg);

// Using Parse Method (designed exclusively to convert from strings)
Console.WriteLine("Enter the first number: ");
int number1 = int.Parse(Console.ReadLine());
Console.WriteLine("Enter the second number: ");
int number2 = int.Parse(Console.ReadLine());
int sum = number1 + number2;
Console.Write("the sum of " + number1 + " and " + number2 + " is: " + sum);

double avg = sum / 2.0;
Console.WriteLine("and the is: " + avg);

/* ------------------------------------- Boxing & Unboxing ------------------------------------- */

/* 1. Write a program that stores the character 'A' in an object variable using boxing, then prints its value and ASCII equivalent. */

char a = 'A';
object o = a;
int ascii = a;
Console.WriteLine(o + "has the value of" + ascii + "in ASCII");

/* ----------------------------------------- In Lecture ----------------------------------------- */

/* Write a C# program that:
    1- Creates a variable to store the student's name.
    2- Creates a variable to store the student's age as an integer.
    3- Creates a constant called InstituteName with the value "Dalil Training Academy".
    4- Converts the student's age to double.
    5- Stores the age inside an object variable using Boxing.
    6- Converts the object back to int using Unboxing.
    7- Prints all the information using Console.WriteLine() */

Console.WriteLine("Please enter your name");
string name = Console.ReadLine();
Console.WriteLine("Please enter your age");
int age = Convert.ToInt32(Console.ReadLine());

const string InstituteName = "Dalil Training Academy";

double dubAge = age;
object obj = dubAge;
age = (int)obj;
Console.WriteLine("Your name " + name + "And age" + age);