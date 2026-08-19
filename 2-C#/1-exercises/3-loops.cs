/* ----------------------------------------- While Loop ----------------------------------------- */

/* Write a program that displays a menu repeatedly until the user chooses to exit. The menu should include the following options:
    * Add two numbers 
    * Subtract two numbers 
    * Multiply two numbers 
    * Exit 
   The program should:
    • Prompt the user to enter their choice 
    • Perform the selected operation using a switch statement
    • Use a while loop to keep displaying the menu until the user selects "Exit" 
    • Display an appropriate message for invalid choices */

int choice = 0;
while (choice != 4)
{
    Console.WriteLine("========== MENU ========== \n " +
        "Select one of the following: \n" +
        "1- Add two numbers \n" +
        "2- Subtract two numbers \n" +
        "3- Multiply two numbers \n" +
        "4- Exit");
    choice = Convert.ToInt32(Console.ReadLine());
    if (choice == 4)
    {
        Console.WriteLine("Exiting program...");
    }
    else if (choice >= 1 && choice <= 3)
    {
        Console.WriteLine("Enter the first number");
        double num1 = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine("Enter the second number");
        double num2 = Convert.ToDouble(Console.ReadLine());
        switch (choice)
        {
            case 1:
                Console.WriteLine($"{num1} + {num2} = {num1 + num2}");
                break;
            case 2:
                Console.WriteLine($"{num1} - {num2} =  {num1 - num2}");
                break;
            case 3:
                Console.WriteLine($"{num1} * {num2} =  {num1 * num2}");
                break;
            default:
                Console.WriteLine("Something went wrong");
                break;
        }
    }
    else
    {
        Console.WriteLine("you have inserted an invalid choice");
    }
}


/* ----------------------------------------- In Lecture ----------------------------------------- */

/* Write a C# Console Application for a smart parking system. The program should ask the user to enter:
    - Driver name
    - Number of parking hours
    - Whether the driver has a parking membership (true or false)
   Requirements: The program must calculate the parking fee according to the following rules:
    - The first 2 hours cost $3 per hour.
    - Every additional hour costs $2.5 per hour.
    - Membership discount Members receive a 20% discount.
    - Non-members receive no discount.
    - Determine whether the driver is eligible for a free parking voucher:
    - The driver must be a member AND The total parking hours must be less than or equal to 2 */


Console.WriteLine("What is your name?");
string name = Console.ReadLine();
Console.WriteLine("What is your number of parking hours?");
int parkHours = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("Do you have a parking membership? yes or no");
bool membership = Console.ReadLine() == "yes" ? true : false;

double fee = 0;

if (parkHours <= 2)
{
    fee = 3 * parkHours;

    if (membership)
        fee = 0;
}
else
{
    for (int i = 3; i <= parkHours; i++)
    {
        fee += 2.5;
    }

    if (membership)
    {
        fee -= fee * 0.20;
    }
}

Console.WriteLine($"Your fee {name} is: {fee}");