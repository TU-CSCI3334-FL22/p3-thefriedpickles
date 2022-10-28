namespace project3
{
    public class main
    {
        static void Main(string[] args)
        {
            // Display the number of command line arguments.
            string filename = "Parens";
            if(args.Length > 0)
            {
                Console.WriteLine("Arguments Passed by the Programmer:");  
                List<string> arguments = args.ToList();

                if(arguments.Contains("-file")){
                    int index = arguments.IndexOf("-file");
                    filename = arguments[index+1];
                }
                // To print the command line 
                // arguments using foreach loop
                foreach(Object obj in args)  
                {  
                    Console.WriteLine(obj);       
                }  
            } 

            //Console.WriteLine("Hello world");
            Scanner scan = new Scanner();
            scan.ScannerDriver(Directory.GetCurrentDirectory() + "\\" + filename);
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
            tableGenerator.computeFollowSet();
            tableGenerator.computeNextSet();

            Utils.PrintSet(tableGenerator._firstSet);
            Console.WriteLine("===============");
            Utils.PrintSet(tableGenerator._followSet);
            Console.WriteLine("===============");
            Utils.PrintSet(tableGenerator._nextSet);
            Console.WriteLine("===============");
            Yaml.PrintYaml(parse.formedTable);

            Console.ReadLine();
        }
    }
}
