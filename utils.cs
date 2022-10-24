namespace project3
{

    public class Utils
    {

        public void PrintHashSet(HashSet<string> inc)
        {
            foreach (string thing in inc)
            {
                Console.Write(thing + ", ");
            }

            Console.WriteLine();
        }

        public void PrintList(List<TOKEN> inc)
        {
            foreach (TOKEN thing in inc)
            {
                Console.Write(thing.ToString() + " ");
            }

            Console.WriteLine();
        }

        public void PrintProductions(List<Tuple<string, List<string>>> productions){
            Console.WriteLine("| " + PadString(15, "LeftHandSide") + " -> " + PadString(15, "RightHandSide") + " |");
            Console.WriteLine("|--------------------------------------|"); 

            foreach (Tuple<string, List<string>> thing in productions){
                Console.Write("| " + PadString(15, thing.Item1) + " -> ");

                foreach(string elem in thing.Item2){
                    Console.Write(PadString(5, elem));
                }
                Console.WriteLine();
            }
        }

        public void PrintFormedTable(Dictionary<string, List<List<string>>> dict){
            foreach(string key in dict.Keys){
                Console.WriteLine(key);
                foreach(List<string> list in dict[key]){
                    foreach(string elem in list){
                        Console.Write(elem + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("-------------");
            }
        }

        public string PadString(int spacing, string toPrint) {
            string ret = toPrint;

            if(spacing < toPrint.Length){
                return ret += " ";
            }

            for(int i = 0; i <= spacing - toPrint.Length; i++){
                ret += " ";
            }
            return ret;
        }

    }
}