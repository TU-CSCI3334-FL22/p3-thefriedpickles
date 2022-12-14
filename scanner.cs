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
            String newText = "";
            foreach(char c in allText){
                if(c >= 65 && c <= 127 || c == 32 || c == 58 || c == 59){
                    newText += c;
                }
                if(c == '\n'){
                    newText += ' ';
                }
            }

            String[] strlist = newText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (String word in strlist) {

                if (word == ":") {
                    tokens.Add(new TOKEN(TOKENTYPES.DERIVES, ":"));
                } else if (word == ";") {
                    tokens.Add(new TOKEN(TOKENTYPES.SEMICOLON, ";"));
                } else if (word == "epsilon" || word == "EPSILON" || word == "Epsilon") {
                    tokens.Add(new TOKEN(TOKENTYPES.EPSILON, "epsilon"));
                } else if (word == "|") {
                    tokens.Add(new TOKEN(TOKENTYPES.ALSODERIVES, "|"));
                } else {
                    symbolTable.Add(word);
                    tokens.Add(new TOKEN(TOKENTYPES.SYMBOL, word));
                }
            }

            return 1;
        }
    }
}
