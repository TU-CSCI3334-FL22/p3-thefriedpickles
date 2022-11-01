namespace project3
{
    public static class Yaml {
        public static List<string> nonterms = new List<string>();
        public static List<string> terms = new List<string>();
        public static List<string> allSymbols = new List<string>();
        public static HashSet<string> terminals = new HashSet<string>();

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

        public static void printProd(string t, int index) {
            if(t == "eof")
                Console.Write(t + ": " + index);
            else
                Console.Write(t + ": " + index + ", ");
        }

        public static void printError(string t) {
            if(t == "eof")
                Console.Write(t + ": --");
            else
                Console.Write(t + ": --, ");
        }

        public static void PrintYaml(Dictionary<string, List<List<string>>> table, ListWithDuplicates next){
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
            foreach(string t in terms) {
                terminals.Add(t);
            }
            terminals.Add("eof");
            
            fancyPrintList("terminals", terms);
            Console.WriteLine();
            fancyPrintList("non-terminals", nonterms);
            Console.WriteLine();
            Console.WriteLine("eof-marker: eof");
            Console.WriteLine("error-marker: --");
            Console.WriteLine("start-symbol: "+nonterms[0]);
            int count = 0;

            //Display Grammar
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

            //Display Next Table
            int index = 0;
            Console.WriteLine("table: ");
            for(int nt = 0; nt < next.Count/2; nt++) {
                Console.Write("\t" + next[nt].Item2.Key + ": {");
                foreach(string t in terminals) {   
                    if(nt != 0) {
                        /*
                        if(next[nt].Item2.Key == next[nt-1].Item2.Key && next[nt].Item2.Key != next[nt+1].Item2.Key) {
                            next.Remove(next[nt]);
                        }
                        */
                    }
                    if(next[nt].Item2.Key == next[nt+1].Item2.Key && nt != next.Count -1) {
                        if(next[nt].Item2.Value.Contains(t)) {
                            index = next[nt].Item1;
                            printProd(t, index);
                        } else if(next[nt+1].Item2.Value.Contains(t)) {
                            index = next[nt].Item1 + 1;
                            printProd(t, index);
                        } else {
                            index = -1;                  
                            printError(t);
                        }
                    } else {
                        if(next[nt].Item2.Value.Contains(t)) {
                            index = next[nt].Item1;
                            printProd(t, index);
                        }
                        else 
                            printError(t);
                    }
                }
                Console.Write("}");
                Console.WriteLine();
            }
        }
    }
}