namespace project3 {
    public class Parser{
        public List<Tuple<string, List<string>>> productions = new List<Tuple<string, List<string>>>();

        public Dictionary<string, List<List<string>>> formedTable = new Dictionary<string, List<List<string>>>();
        List<TOKEN> destructibleTokens = new List<TOKEN>();

        public int driver(List<TOKEN> tokens){
            destructibleTokens = tokens;

            int ret = 0;
            while(ret != -1 && destructibleTokens.Count > 0){
                ret = Grammer();
            }

            if(ret == 1){
                constructFullyFormedTable();
                return 1;
            } else {
                foreach(TOKEN tok in destructibleTokens){
                    Console.Write(tok + " ");
                }
                return -1;
            }
        }

        public void constructFullyFormedTable(){
            foreach(Tuple<string, List<string>> elem in productions){
                Console.WriteLine("Current elem is: " + elem.Item1);
                if(!formedTable.ContainsKey(elem.Item1)){
                    formedTable.Add(elem.Item1, new List<List<string>>());
                }
                
                List<List<string>> tmp = formedTable[elem.Item1];
                    tmp.Add(elem.Item2);
                    formedTable[elem.Item1] = tmp;
            }
        }

        public int Grammer() {
            return ProductionList();
        }
        public int ProductionList(){
            int ret = ProductionSet();
            if(ret != 1){
                return -1;
            }

            TOKEN shouldBeSemiColon = destructibleTokens[0];
            destructibleTokens.RemoveAt(0);
            if(shouldBeSemiColon.token != TOKENTYPES.SEMICOLON){
                return -1;
            }

            ret = ProductionListPrime();
            if(ret != 1){
                return -1;
            }

            return 1;
        }
        public int ProductionListPrime(){
            

            if(destructibleTokens.Count == 0){ //Rule 4 -> Rule 1
                return 1;
            } else {
                TOKEN curr = destructibleTokens[0];
                destructibleTokens.RemoveAt(0);

                if(curr.token == TOKENTYPES.SYMBOL){
                    destructibleTokens.Insert(0, curr);
                    int ret = ProductionSet();
                    if(ret != 1){
                        return -1;
                    }

                    TOKEN shouldBeSemiColon = destructibleTokens[0];
                    destructibleTokens.RemoveAt(0);
                    if(shouldBeSemiColon.token != TOKENTYPES.SEMICOLON){
                        return -1;
                    }

                    ret = ProductionListPrime();
                    if(ret != 1){
                        return -1;
                    }

                    return 1;

                }

                return -1;
            }
        }
        public int ProductionSet(){
            
            TOKEN curr = destructibleTokens[0];
            destructibleTokens.RemoveAt(0);
            
            //Add new production to the production set with an empty list
            productions.Add(new Tuple<string, List<string>>(curr.val, new List<string>()));

            if(curr.token != TOKENTYPES.SYMBOL){
                return -1;
            }

            curr = destructibleTokens[0];
            destructibleTokens.RemoveAt(0);
            if(curr.token != TOKENTYPES.DERIVES){
                return -1;
            }

            int ret = RightHandSide();
            if(ret != 1){
                return -1;
            }

            ret = ProductionSetPrime();
            if(ret != 1){
                return -1;
            }

            return 1;
        }
        public int ProductionSetPrime(){
            
            TOKEN curr = destructibleTokens[0];
            destructibleTokens.RemoveAt(0);

            if(curr.token == TOKENTYPES.SEMICOLON){ //Rule 7 -> Rule 2 & 3
                destructibleTokens.Insert(0, curr);
                return 1;
            } else {
                if(curr.token == TOKENTYPES.ALSODERIVES){ //Rule 6
                    int ret = RightHandSide();
                    if(ret != 1){
                        return -1;
                    }

                    ret = ProductionSetPrime();
                    if(ret != 1){
                        return -1;
                    }
                    return 1;

                }
                return -1;
            }
        }
        public int RightHandSide(){
            
            TOKEN curr = destructibleTokens[0];
            destructibleTokens.RemoveAt(0);

            if(curr.token == TOKENTYPES.EPSILON) { //Rule 9

                Tuple<string, List<string>> last = productions.Last();
                last.Item2.Add(curr.val);
                productions[productions.Count-1] = last;

                return 1;
            }
            
            if(curr.token == TOKENTYPES.SYMBOL) { //First of SymbolList (Rule 8)
                destructibleTokens.Insert(0, curr);
                int ret = SymbolList();
                if(ret != 1){
                    return -1;
                }
                return 1;
            }

            return -1;

        }
        public int SymbolList(){
            TOKEN curr = destructibleTokens[0];
            destructibleTokens.RemoveAt(0);

            if(curr.token == TOKENTYPES.SYMBOL){ //Rule 10

                //Add the current symbol to the last element
                Tuple<string, List<string>> last = productions.Last();
                last.Item2.Add(curr.val);
                productions[productions.Count-1] = last;


                int ret = SymbolListPrime();
                if(ret != 1){
                    return -1;
                } else {return 1;}
            }
            return -1;
        }
        public int SymbolListPrime(){
            TOKEN curr = destructibleTokens[0];
            destructibleTokens.RemoveAt(0);
            
            if(curr.token == TOKENTYPES.SEMICOLON){ //Rule 12
                destructibleTokens.Insert(0, curr);
                return 1;
            } else {

                if(curr.token == TOKENTYPES.ALSODERIVES){ //Rule 12 -> Rule 8 -> Rule 6 & 5
                    destructibleTokens.Insert(0, curr);
                    Tuple<string, List<string>> last = productions.Last();
                    productions.Add(new Tuple<string, List<string>>(last.Item1, new List<string>()));
                    return 1;
                }

                if(curr.token == TOKENTYPES.SYMBOL) { //Rule 11

                    //Add the current symbol to the last element
                    Tuple<string, List<string>> last = productions.Last();
                    last.Item2.Add(curr.val);
                    productions[productions.Count-1] = last;

                    int ret = SymbolListPrime();
                    if(ret != 1){
                        return -1;
                    } else {return 1;}
                }
            }

            return -1;
        }


        // public int NewProduction(){

        //     TOKEN curr = destructibleTokens[0];
        //     destructibleTokens.RemoveAt(0);

        //     //Add a new production to the list
        //     productions.Add(curr.val, new List<List<string>>());
        //     currentProduction = curr.val;
        //     whereToAppend = 0;

        //     if(curr.token != TOKENTYPES.SYMBOL){
        //         return -1;
        //     }

        //     return Derives();
        // }

        // public int Derives() {
        //     TOKEN curr = destructibleTokens[0];
        //     destructibleTokens.RemoveAt(0);

        //     if(curr.token != TOKENTYPES.DERIVES){
        //         return -1;
        //     }

        //     //Add a new production to the list of productions of the current production
        //     productions[currentProduction].Add(new List<string>());

        //     return AnyNumberOfSymbols();
        // }

        // public int AlsoDerives() {
        //     TOKEN curr = destructibleTokens[0];
        //     destructibleTokens.RemoveAt(0);
            
        //     if(curr.token != TOKENTYPES.SYMBOL || curr.token != TOKENTYPES.EPSILON){
        //         return -1;
        //     }

        //     whereToAppend++;
        //     destructibleTokens.Insert(0, curr);
        //     return AnyNumberOfSymbols();
        // }

        // public int AnyNumberOfSymbols() {
        //     TOKEN curr = destructibleTokens[0];
        //     destructibleTokens.RemoveAt(0);

        //     if(curr.token == TOKENTYPES.EPSILON){
        //         curr = destructibleTokens[0];
        //         destructibleTokens.RemoveAt(0);
        //         if(curr.token == TOKENTYPES.SEMICOLON){
        //             productions[currentProduction][whereToAppend].Add(curr.val);
        //             return 1;
        //         } else if(curr.token == TOKENTYPES.ALSODERIVES) {
        //             productions[currentProduction][whereToAppend].Add(curr.val);
        //             return AlsoDerives();
        //         } else {
        //             return -1;
        //         }
        //     }

        //     while(curr.token == TOKENTYPES.SYMBOL){
        //         curr = destructibleTokens[0];
        //         destructibleTokens.RemoveAt(0);

        //         productions[currentProduction][whereToAppend].Add(curr.val);
        //     }

        //     if(curr.token == TOKENTYPES.ALSODERIVES){
        //         return AlsoDerives();
        //     }

        //     if(curr.token == TOKENTYPES.SEMICOLON){
        //         return 1;
        //     }

        //     return -1;
        // }
    }
}