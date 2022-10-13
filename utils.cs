namespace project3;

public class Utils {

    public void PrintHashSet(HashSet<string> inc){
        foreach(string thing in inc){
            Console.Write(thing + ", ");
        }

        Console.WriteLine();
    }

    public void PrintList(List<TOKEN> inc){
        foreach(TOKEN thing in inc){
            PrintToken(thing);
        }

        Console.WriteLine();
    }

    public void PrintToken(TOKEN thing){
        Console.WriteLine("<" + thing.token + ", " + thing.val + ">");
    }

}