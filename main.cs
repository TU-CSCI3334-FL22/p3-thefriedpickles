namespace project3
{
    public class main
    {
        static void Main(string[] args)
        {
            // Display the number of command line arguments.
            if(args.Length > 0)
            {
                Console.WriteLine("Arguments Passed by the Programmer:");  
              
                // To print the command line 
                // arguments using foreach loop
                foreach(Object obj in args)  
                {  
                    Console.WriteLine(obj);       
                }  
            } 

            //Console.WriteLine("Hello world");
            Scanner scan = new Scanner();
            scan.ScannerDriver("D:\\School\\Fall 2022\\Functional Languages\\p3-thefriedpickles\\grammars\\Parens");
            //util.PrintHashSet(scan.symbolTable);

            //util.PrintList(scan.tokens);

            Parser parse = new Parser();
            int ret = parse.driver(scan.tokens);

            if(ret == -1){
                Console.WriteLine("Failed parsing");
            } else {
                //util.PrintProductions(parse.productions);
                //util.PrintFormedTable(parse.formedTable);
                Console.WriteLine("Successfully Parsed");
            }

            TableGenerator tableGenerator = new TableGenerator(parse.formedTable);
            //Utils.PrintHashSet(tableGenerator.allsymbols);
            //Utils.PrintHashSet(tableGenerator.terminals);
            //Utils.PrintHashSet(tableGenerator.nonterminals);

            tableGenerator.computeFirstSet();

            Utils.PrintSet(tableGenerator._firstSet);

            Console.ReadLine();
        }
    }
}
