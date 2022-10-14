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

    }
}