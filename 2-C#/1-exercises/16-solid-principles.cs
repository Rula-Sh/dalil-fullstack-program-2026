/* -------------------------------------- SOLID Principles -------------------------------------- */

/* Refactor a class called ReportGenerator that currently does three jobs in one place:
   • Fetches data, formats it, and writes it to disk — all in one class (violates SRP) 
   • Instantiates a concrete FileWriter directly inside its constructor (violates DIP) 
   • Split it so each class has one job only, and inject the writer instead. 
   Hint: introduce an IReportWriter interface and inject it via the constructor. */

interface IReportWriter
{
    void Write(string content);
}

class FileReportWriter : IReportWriter
{
    private string _filePath;
    private FileInfo _file;

    public FileReportWriter(string filePath)
    {
        _filePath = filePath;
        _file = new FileInfo(filePath);
        if (!_file.Exists)
            _file.Create();
    }

    public void Write(string content)
    {
        try
        {
            using (StreamWriter writer = File.AppendText(_filePath))
            {
                writer.WriteLine(content);
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("You don't have permission to this file.");
        }
        catch (IOException)
        {
            Console.WriteLine("the file is currently being used.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}

class ReportGenerator
{
    private IReportWriter _reportWriter;

    public ReportGenerator(IReportWriter reportWriter)
    {
        _reportWriter = reportWriter;
    }

    public void Generate(string content) => _reportWriter.Write(content);
}

class Program
{
    static void Main(string[] args)
    {
        IReportWriter fileReportWriter = new FileReportWriter(@"C:\Users\User\Desktop\report.txt");

        ReportGenerator reportGenerator = new ReportGenerator(fileReportWriter);

        reportGenerator.Generate("First Report Generated.");
    }
}