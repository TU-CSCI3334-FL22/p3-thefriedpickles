using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace project3 {

    public class Scanner {

        public HashSet<string> symbolTable = new HashSet<string>();

        public List<TOKEN> tokens = new List<TOKEN>();

        public Scanner() { }

        public int ScannerDriver(string filepath) {

            String allText = File.ReadAllText(filepath);
            allText = allText.Replace("\n", "");

            String[] strlist = allText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (String word in strlist) {

                Console.WriteLine($"{word} has length {word.Length}");
                if (word == ":") {
                    tokens.Add(new TOKEN(TOKENTYPES.DERIVES, ":"));
                } else if (word == ";") {
                    tokens.Add(new TOKEN(TOKENTYPES.SEMICOLON, ";"));
                } else if (word == "epsilon" || word == "EPSILON" || word == "Epsilon") {
                    tokens.Add(new TOKEN(TOKENTYPES.EPSILON, " epsilon "));
                } else if (word == "|") {
                    tokens.Add(new TOKEN(TOKENTYPES.ALSODERIVES, "|"));
                }
            }

            return 1;
        }
    }
}
