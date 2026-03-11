using Blackfish.Materials;
using Blackfish.Subfiles;

namespace ConsoleStudioCloud;

internal class Program
{
    static void Main(string[] args)
    {
        var m = new Material();
        m.Name = "Test";
        m.Subfiles.Add(new Subfile()
        {
            Name = "Subfile1"
        });
        Console.WriteLine("Hello, World! (StudioCloud)");
        Console.ReadLine();
    }
}