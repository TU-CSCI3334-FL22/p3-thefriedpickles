namespace project3
{
    public static class Yaml {
        public static List<string> nonterms = new List<string>();
        public static List<string> terms = new List<string>();
        public static List<string> allSymbols = new List<string>();

        public static void fancyPrintList(string wing, List<string> lst){
            Console.Write(wing +": [");
            for(int i = 0; i < lst.Count; i++){
            if(i == 0 && lst.Count > 1){
                Console.Write(lst[i]+",");
            }
            else if(i == 0){
                Console.Write( lst[i]);
            }
            else if(i == lst.Count - 1){
                Console.Write(" "+ lst[i]);
            }
            else{
                Console.Write(" "+ lst[i]+",");
            }   
        }
        Console.Write("]");
        }
        public static void PrintYaml(Dictionary<string, List<List<string>>> table){
            foreach(string key in table.Keys){
                nonterms.Add(key);
                allSymbols.Add(key);
                foreach(List<string> prod in table[key]){
                    foreach(string elem in prod){
                        allSymbols.Add(elem);
                    }
                }

            }
            foreach(string symbol in allSymbols){
                    if(!nonterms.Contains(symbol)){
                        terms.Add(symbol);
                    }
                }
            fancyPrintList("terminals", terms);
            Console.WriteLine();
            fancyPrintList("non-terminals", nonterms);
            Console.WriteLine();
            Console.WriteLine("eof-marker: eof");
            Console.WriteLine("error-marker: --");
            Console.WriteLine("start-symbol: "+nonterms[0]);
            int count = 0;
            Console.WriteLine("productions: ");
            foreach(string key in table.Keys){
                foreach (List<string> prod in table[key]){
                    Console.Write("\t" + count + ": {");
                    fancyPrintList(key, prod);
                    count++;
                    Console.Write("}");
                    Console.WriteLine();
                }
            }
        
        }
    }
}