/* ------------------------------------------- Arrays ------------------------------------------- */

/* Write a method that counts how many even numbers exist in a given integer array. */

Console.WriteLine("how many number do you want to insert?");
int size = int.Parse(Console.ReadLine());
int[] numbers = new int[size];
int evenCount = 0;

for (int i = 0; i < size; i++)
{
    Console.WriteLine($"insert number #{i + 1}");
    numbers[i] = int.Parse(Console.ReadLine());
    if (numbers[i] % 2 == 0)
        evenCount++;
}
Console.WriteLine($"You inserted {evenCount} even numbers");

/* Write a program that asks the user to enter an array of integers, then finds and prints all duplicate values in the array. */

Console.Write("Enter array size: ");
int numsSize = int.Parse(Console.ReadLine());
int[] nums = new int[numsSize];

for (int i = 0; i < numsSize; i++)
{
    Console.Write($"Enter element {i + 1}: ");
    nums[i] = int.Parse(Console.ReadLine());
}

string dupList = "";
for (int i = 0; i < nums.Length; i++)
{
    bool isDuplicate = false;
    // Check if the number appeared before
    for (int k = 0; k < i; k++)
    {
        if (nums[i] == nums[k])
        {
            isDuplicate = true;
            break;
        }
    }

    if (isDuplicate) // Skip, because it is already included in dupList 
        continue;

    // Check for duplicates ahead
    for (int j = i + 1; j < nums.Length; j++)
    {
        if (nums[i] == nums[j])
        {
            dupList += nums[i] + " ";
            break;
        }
    }
}

Console.WriteLine($"The duplicated items: {dupList}");
