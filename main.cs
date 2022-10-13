using System.Text;
namespace project3;

public class main {
    static void Main()
    {
        // Display the number of command line arguments.
        //Console.WriteLine("Hello world");
        Scanner scan = new Scanner();
        scan.scannerDriver("D:\\School\\Fall 2022\\Functional Languages\\p3-thefriedpickles\\grammars\\Parens");

        Utils util = new Utils();
        //util.PrintHashSet(scan.symbolTable);

        util.PrintToken(scan.tokens[3]);
    }
}
