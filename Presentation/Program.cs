using Data.IO.Implementation;

namespace Presentation
{
    class Program
    {
        static void Main(string[] args)
        {
            var console = new PersonConsoleUtils(new PseudoJsonDataProcessor(), new FileDataWriter(), new FileDataReader());
            console.WaitForInput();
        }
    }
}