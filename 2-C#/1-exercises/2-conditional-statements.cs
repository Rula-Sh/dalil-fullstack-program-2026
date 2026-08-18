/* ---------------------------------------- If Statement ---------------------------------------- */

/* Uppercase or Lowercase?
 * Write a program that reads a single character and checks whether it is an uppercase letter, lowercase letter, or not a letter at all. */

char c = Convert.ToChar(Console.ReadLine());

if (c >= 'A' && c <= 'Z') // c >= 65 && c <= 90
    Console.WriteLine(c + " is uppercase.");
else if (c >= 'a' && c <= 'z') // c >= 97 && c <= 122
    Console.WriteLine(c + " is lowercase.");
else
    Console.WriteLine(c + "is not a letter");


/* -------------------------------------- Switch Statement -------------------------------------- */

/* Vowel Checker
 * Write a program that reads a character and determines whether it is: vowel or a consonant using a switch statement.*/

Console.WriteLine("Please enter a character");
char ch = Convert.ToChar(Console.ReadLine().ToLower());

switch (ch)
{
    case 'a':
    case 'e':
    case 'i':
    case 'o':
    case 'u':
        Console.WriteLine(ch + "is a vowel");
        break;
    default:
        Console.WriteLine(ch + "is a constatnt");
        break;
}

/* Grade Classifier
 * Write a program that accepts a student's mark (0–100) and prints the corresponding letter grade using a switch statement.
 * Use integer division by 10 to group the marks into ranges:A(90–100), B(80–89), C(70–79), D(6069), E(50–59), F(below 50).*/

Console.WriteLine("Please enter your grade");
int grade = Convert.ToInt32(Console.ReadLine());

if (grade >= 0 && grade <= 100)
{
    switch (grade / 10)
    {
        case 10:
        case 9:
            Console.WriteLine("A");
            break;
        case 8:
            Console.WriteLine("B");
            break;
        case 7:
            Console.WriteLine("C");
            break;
        case 6:
            Console.WriteLine("D");
            break;
        case 5:
            Console.WriteLine("E");
            break;
        default:
            Console.WriteLine("F");
            break;
    }
}
else
{
    Console.WriteLine("You did not provide a grade between 0 and 100");
}


/* -------------------------------- Ternary Conditional Operator -------------------------------- */

/*Check Number Sign
 * Write a program that reads an integer from the keyboard, then uses the ternary operator to determine whether the number is
 * positive or negative or zero.Display the result on the console. */

Console.WriteLine("Please enter a number");
int num = Convert.ToInt32(Console.ReadLine());
Console.WriteLine(num == 0 ? "Zero" : (num > 0 ? "Positive" : "Negative"));
