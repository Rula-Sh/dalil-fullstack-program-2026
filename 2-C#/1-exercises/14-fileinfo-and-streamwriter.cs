/* --------------------------------- FileInfo and StreamWriter --------------------------------- */

/* Write a C# program that:
   1. Prompts the user to enter lines of customer feedback
   2. Writes each non-empty line to a feedback file
   3. Stops when the user submits an empty line */

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string filePath = @"C:\Users\User\Desktop\customer-feedback.txt";
            FileInfo file = new FileInfo(filePath);

            if (!file.Exists)
                file.Create();

            using (StreamWriter writer = File.AppendText(filePath))
            {
                string feedback = Console.ReadLine();
                while (feedback != "")
                {
                    writer.WriteLine(feedback);
                    feedback = Console.ReadLine();
                }
            }
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine("You don't have permission to this file.");
        }
        catch (IOException e)
        {
            Console.WriteLine("the file is currently being used.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine("Feedback saved successfully!");
    }
}