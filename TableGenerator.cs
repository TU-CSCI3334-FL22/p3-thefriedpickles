namespace project3 {
    public class TableGenerator {
        public Dictionary<string, HashSet<string>> _firstSet = new Dictionary<string, HashSet<string>>();

        public Dictionary<string, HashSet<string>> _followSet = new Dictionary<string, HashSet<string>>();

        public Dictionary<string, HashSet<string>> _nextSet = new Dictionary<string, HashSet<string>>();

        Dictionary<string, List<List<string>>> _internalFormedTable = new Dictionary<string, List<List<string>>>();

        public HashSet<string> terminals = new HashSet<string>();
        public HashSet<string> nonterminals = new HashSet<string>();
        public HashSet<string> allsymbols = new HashSet<string>();

        public TableGenerator(Dictionary<string, List<List<string>>> internalFormedTable){
            _internalFormedTable = internalFormedTable;

            foreach(string key in _internalFormedTable.Keys){
                nonterminals.Add(key);
                allsymbols.Add(key);

                foreach(List<string> production in _internalFormedTable[key]){
                    foreach(string elem in production){
                        allsymbols.Add(elem);
                    }
                }
            }

            foreach(string symbol in allsymbols){
                if(!nonterminals.Contains(symbol)){
                    terminals.Add(symbol);
                }
            }

            terminals.Add("eof");
            terminals.Add("ε");
            allsymbols.Add("eof");
            allsymbols.Add("ε");


            foreach(string symbol in allsymbols){
                _firstSet.Add(symbol, new HashSet<string>());
                _followSet.Add(symbol, new HashSet<string>());
                _nextSet.Add(symbol, new HashSet<string>());
            }
        }

        static string epi = "ε";
        HashSet<string> episet = new HashSet<string>(){epi};

        public void computeFirstSet() {
            foreach(string t in terminals){
                _firstSet[t].Add(t);
            }

            bool hasChanged = true;
            while(hasChanged) {
                hasChanged = false;

                foreach(string key in _internalFormedTable.Keys){
                    HashSet<string> prevSet = _firstSet[key].ToHashSet<string>();

                    foreach(List<string> production in _internalFormedTable[key]){
                        int i = 0;
                        //Utils.PrintHashSet(_firstSet[production[0]]);
                        HashSet<string> firstBi = _firstSet[production[0]];
                        //Utils.PrintHashSet(firstBi, "firstBi:");
                        HashSet<string> rhs = firstBi.Except(episet).ToHashSet<string>();
                        //Utils.PrintHashSet(rhs, "rhs:");
                        
                        int k = production.Count-1;

                        while(_firstSet[production[i]].Contains(epi) && i < k){
                            i += 1;
                            rhs = rhs.Union(_firstSet[production[i]].Except(episet).ToHashSet<string>()).ToHashSet<string>();
                        }

                        if( i == k && _firstSet[production[k]].Contains(epi)){
                            rhs = rhs.Union(episet).ToHashSet();
                        }

                        _firstSet[key] = _firstSet[key].Union(rhs).ToHashSet<string>();
                    }
                    
                    if(!hashSetsEqual(prevSet, _firstSet[key])){
                        hasChanged = true;
                    }
                }
            }
        }

        public void computeFollowSet(){

        }

        public void computeNextSet(){

        }

        bool hashSetsEqual(HashSet<string> a, HashSet<string> b){
            if(a.Count != b.Count){
                return false;
            }

            foreach(string elem in a){
                if(!b.Contains(elem)){
                    return false;
                }
            }

            foreach(string elem in b){
                if(!a.Contains(elem)){
                    return false;
                }
            }

            return true;
        }
    }
}