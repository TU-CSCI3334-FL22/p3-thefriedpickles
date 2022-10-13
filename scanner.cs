using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace project3;

public class Scanner {

    public HashSet<string> symbolTable = new HashSet<string>();

    public List<TOKEN> tokens = new List<TOKEN>();

    public Scanner() {}

    public int scannerDriver(string filepath) {

        String allText = File.ReadAllText(filepath);

        char [] whitespace = {' ', '\n', '\t'};
        String [] strlist = allText.Split(whitespace, StringSplitOptions.RemoveEmptyEntries);


        foreach(String word in strlist) {
            switch(word.ToLower()){
                case ";":
                    tokens.Add(new TOKEN(TOKENTYPES.SEMICOLON, word));
                    break;
                case ":":
                    tokens.Add(new TOKEN(TOKENTYPES.DERIVES, word));
                    break;
                case "|":
                    tokens.Add(new TOKEN(TOKENTYPES.ALSODERIVES, word));
                    break;
                case "epsilon":
                    tokens.Add(new TOKEN(TOKENTYPES.EPSILON, word));
                    break;
                case " ":
                    //Console.WriteLine("Help me");
                    break;
                case "\t":
                    //Console.WriteLine("liberals");
                    break;
                case "\n":
                    //Console.WriteLine("checkmate");
                    break;
                case "":
                    //Console.WriteLine("freaky deeky");
                    break;
                default:
                    //Console.WriteLine(word);
                    tokens.Add(new TOKEN(TOKENTYPES.SYMBOL, word));
                    symbolTable.Add(word);
                    break;
            }
        }

        return 1;
    }
}
