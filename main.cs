namespace project3
{
    public class main
    {
        static void Main(string[] args)
        {
            // Display the number of command line arguments.
            string filename = "Parens";
            bool shouldWorklist = false;
            bool shouldPrintYaml = false;
        
            if(args.Length > 0)
            {  
                List<string> arguments = args.ToList();

                if(arguments.Contains("-file")){
                    int index = arguments.IndexOf("-file");
                    filename = arguments[index+1];
                }

                if(arguments.Contains("-help")) {
                    Console.WriteLine("-help: Print a list of valid command-line arugments and descriptions to `stdout` and quit.");
                    Console.WriteLine("-t: Print the LL(1) table in YAML format, as specified below. Produces no output to `stdout` other than the LL(1) table. If there is an error message, prints that messagwe to `stderr`");
                    Console.WriteLine("-recursion: Unsupported");
                    Console.WriteLine("-w: Uses a worklist optimization of the fixpoint algorithms.");
                    return;
                }

                if(arguments.Contains("-w")){
                    shouldWorklist = true;
                }

                if(arguments.Contains("-t")){
                    shouldPrintYaml = true;
                }

                if(arguments.Contains("-recursion")){
                    Console.WriteLine("Unsupported D:");
                    return;
                }

                // To print the command line 
                // arguments using foreach loop
                // foreach(Object obj in args)  
                // {  
                //     Console.WriteLine(obj);       
                // }  
            } 

            //Console.WriteLine("Hello world");
            Scanner scan = new Scanner();
            scan.ScannerDriver(Directory.GetCurrentDirectory() + "\\" + filename);
            //util.PrintHashSet(scan.symbolTable);

            //util.PrintList(scan.tokens);

            Parser parse = new Parser();
            int ret = parse.driver(scan.tokens);
            bool canParse = true;

            if(ret == -1){
                Console.WriteLine("Failed parsing");
                canParse = false;
            } else {
                //util.PrintProductions(parse.productions);
                //util.PrintFormedTable(parse.formedTable);
                Console.WriteLine("Successfully Parsed");
            }

            if(canParse){
                TableGenerator tableGenerator = new TableGenerator(parse.formedTable);
                //Utils.PrintHashSet(tableGenerator.allsymbols);
                //Utils.PrintHashSet(tableGenerator.terminals);
                //Utils.PrintHashSet(tableGenerator.nonterminals);

                if(shouldWorklist){
                    tableGenerator.computeFirstSetWorklist();
                    tableGenerator.computeFollowSetWorklist();
                    tableGenerator.computeNextSetWorklist();
                } else {
                    tableGenerator.computeFirstSet();
                    tableGenerator.computeFollowSet();
                    tableGenerator.computeNextSet();
                }
                

                //Utils.PrintSet(tableGenerator._firstSet);
                //Console.WriteLine("===============");

                if(shouldPrintYaml){
                    Console.WriteLine("========= Yaml ==========");
                    Yaml.PrintYaml(parse.formedTable, tableGenerator._yamlNext);
                }
                else if(shouldWorklist){
                    Console.WriteLine("======== First Set =======");
                    Utils.PrintSet(tableGenerator._firstSetWork);
                    Console.WriteLine("======== Follow Set =======");
                    Utils.PrintSet(tableGenerator._followSetWork);
                    Console.WriteLine("======== Next Set =======");
                    Utils.PrintSet(tableGenerator._nextSetWork);
                    Console.WriteLine("===============");
                } else {
                    Console.WriteLine("======== First Set =======");
                    Utils.PrintSet(tableGenerator._firstSet);
                    Console.WriteLine("======== Follow Set =======");
                    Utils.PrintSet(tableGenerator._followSet);
                    Console.WriteLine("======== Next Set ========");
                    Utils.PrintSet(tableGenerator._nextSet);
                }
            }
        }
    }
}
