namespace project3 {
    public class TableGenerator {
        public Dictionary<string, HashSet<string>> _firstSet = new Dictionary<string, HashSet<string>>();

        public Dictionary<string, HashSet<string>> _firstSetWork = new Dictionary<string, HashSet<string>>();

        public Dictionary<string, HashSet<string>> _followSet = new Dictionary<string, HashSet<string>>();

        public Dictionary<string, HashSet<string>> _followSetWork = new Dictionary<string, HashSet<string>>();

        public Dictionary<string, HashSet<string>> _nextSet = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, HashSet<string>> _nextSetWork = new Dictionary<string, HashSet<string>>();

        public ListWithDuplicates _yamlNext = new ListWithDuplicates();

        Dictionary<string, List<List<string>>> _internalFormedTable = new Dictionary<string, List<List<string>>>();

        public HashSet<string> terminals = new HashSet<string>();
        public HashSet<string> nonterminals = new HashSet<string>();
        public HashSet<string> allsymbols = new HashSet<string>();

        public HashSet<List<string>> worklist = new HashSet<List<string>>();

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
            allsymbols.Add("eof");


            foreach(string symbol in allsymbols){
                _firstSet.Add(symbol, new HashSet<string>());
                _firstSetWork.Add(symbol, new HashSet<string>());
                _followSet.Add(symbol, new HashSet<string>());
                _followSetWork.Add(symbol, new HashSet<string>());
                _nextSet.Add(symbol, new HashSet<string>());
                _nextSetWork.Add(symbol, new HashSet<string>());
            }
        }

        static string epi = "epsilon";
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
        public void computeFirstSetWorklist() {
            List<Tuple<string, List<string>>> worklist = new List<Tuple<string, List<string>>>();

            foreach(string key in _internalFormedTable.Keys){
                    foreach(List<string> production in _internalFormedTable[key]){
                        worklist.Add(Tuple.Create(key, production));
                    }
            }
            foreach(string t in terminals){
                _firstSetWork[t].Add(t);
            }

            //bool hasChanged = true;
            while(worklist.Count != 0) {
                //hasChanged = false;

                    Tuple<string, List<string>> item = worklist[0];
                    HashSet<string> prevSet = _firstSetWork[item.Item1].ToHashSet<string>();

                        int i = 0;
                        //Utils.PrintHashSet(_firstSet[production[0]]);
                        HashSet<string> firstBi = _firstSetWork[item.Item2[0]];
                        //Utils.PrintHashSet(firstBi, "firstBi:");
                        HashSet<string> rhs = firstBi.Except(episet).ToHashSet<string>();
                        //Utils.PrintHashSet(rhs, "rhs:");
                        
                        int k = item.Item2.Count-1;

                        while(_firstSetWork[item.Item2[i]].Contains(epi) && i < k){
                            i += 1;
                            rhs = rhs.Union(_firstSetWork[item.Item2[i]].Except(episet).ToHashSet<string>()).ToHashSet<string>();
                        }

                        if( i == k && _firstSetWork[item.Item2[k]].Contains(epi)){
                            rhs = rhs.Union(episet).ToHashSet();
                        }

                        _firstSetWork[item.Item1] = _firstSetWork[item.Item1].Union(rhs).ToHashSet<string>();
                    
                    worklist.Remove(item);
                    //index++;
                    if(!hashSetsEqual(prevSet, _firstSetWork[item.Item1])){
                        //hasChanged = true;
                         foreach(string key in _internalFormedTable.Keys){
                        foreach(List<string> production in _internalFormedTable[key]){
                            if(production.Contains(item.Item1) && !worklist.Contains(Tuple.Create(key, production))){
                                worklist.Add(Tuple.Create(key, production));
                            }
                        }
                    }
                    }
                }
            
        }
        public void computeFollowSet(){

            _followSet[nonterminals.ElementAt(0)].Add("eof");

            bool hasChanged = true;
            while(hasChanged) {
                hasChanged = false;

                foreach(string key in _internalFormedTable.Keys){

                    //Console.WriteLine("Key: " + key);
                    HashSet<string> prevSet = _followSet[key].ToHashSet<string>();
                    //Utils.PrintHashSet(prevSet, "Previous Set");

                    foreach(List<string> production in _internalFormedTable[key]){
                        HashSet<string> trailer = _followSet[key];
                        //Utils.PrintHashSet(trailer, "Trailer:");

                        for(int i = production.Count() - 1; i >= 0; i--) {
                            if(nonterminals.Contains(production[i])) {

                                //Console.WriteLine("NonTerminals");

                                _followSet[production[i]] = trailer.Union(_followSet[production[i]].ToHashSet<string>()).ToHashSet<string>();

                                if(_firstSet[production[i]].Contains(epi)) {
                                    trailer = trailer.Union(_firstSet[production[i]].Except(episet).ToHashSet<string>()).ToHashSet<string>();
                                } else {
                                    trailer = _firstSet[production[i]].ToHashSet<string>();
                                }
                                //Utils.PrintHashSet(trailer, "Trailer:");

                            } else {
                                ///Console.WriteLine("Terminals");
                                trailer = _firstSet[production[i]].ToHashSet<string>();
                            }
                        }
                    }
                    if(!hashSetsEqual(prevSet, _followSet[key])){
                        hasChanged = true;
                    }
                }
            }

        }
    public void computeFollowSetWorklist(){
        List<Tuple<string, List<string>>> worklist = new List<Tuple<string, List<string>>>();
        foreach(string key in _internalFormedTable.Keys){
                    foreach(List<string> production in _internalFormedTable[key]){
                        worklist.Add(Tuple.Create(key, production));
                    }
            }
            _followSetWork[nonterminals.ElementAt(0)].Add("eof");

            //bool hasChanged = true;
            while(worklist.Count != 0) {
               //hasChanged = false;
                Tuple<string, List<string>> item = worklist[0];
                    //Console.WriteLine("Key: " + key);
                    HashSet<string> prevSet = _followSetWork[item.Item1].ToHashSet<string>();
                    //Utils.PrintHashSet(prevSet, "Previous Set");

                        HashSet<string> trailer = _followSetWork[item.Item1];
                        //Utils.PrintHashSet(trailer, "Trailer:");

                        for(int i = item.Item2.Count() - 1; i >= 0; i--) {
                            if(nonterminals.Contains(item.Item2[i])) {

                                //Console.WriteLine("NonTerminals");

                                _followSetWork[item.Item2[i]] = trailer.Union(_followSetWork[item.Item2[i]].ToHashSet<string>()).ToHashSet<string>();

                                if(_firstSetWork[item.Item2[i]].Contains(epi)) {
                                    trailer = trailer.Union(_firstSetWork[item.Item2[i]].Except(episet).ToHashSet<string>()).ToHashSet<string>();
                                } else {
                                    trailer = _firstSetWork[item.Item2[i]].ToHashSet<string>();
                                }
                                //Utils.PrintHashSet(trailer, "Trailer:");

                            } else {
                                ///Console.WriteLine("Terminals");
                                trailer = _firstSetWork[item.Item2[i]].ToHashSet<string>();
                            }
                        }
                        worklist.Remove(item);
                        if(!hashSetsEqual(prevSet, _followSetWork[item.Item1])){
                            foreach (string b in item.Item2){
                                if(nonterminals.Contains(b)){
                                    foreach(List<string> prod in  _internalFormedTable[b]){
                                        if(!worklist.Contains(Tuple.Create(b,prod))){
                                            worklist.Add(Tuple.Create(b,prod));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                
            

        }

        public void computeNextSetWorklist(){
            bool hasChanged = true;
            int indx = 0;

            foreach(string t in terminals){
                _nextSetWork[t].Add(t);
            }

            while(hasChanged) {
                hasChanged = false;

                foreach(string key in _internalFormedTable.Keys){
                    HashSet<string> prevSet = _nextSetWork[key].ToHashSet<string>();

                    foreach(List<string> production in _internalFormedTable[key]){
                        HashSet<string> trailer = _nextSetWork[key];

                        int n = 0;
                        bool allHaveEpi = true;
                        foreach(string elem in production) {
                            if(!_firstSetWork[elem].Contains(epi)) {
                                allHaveEpi = false;
                                n = production.IndexOf(elem);
                                break;
                            }
                        }

                        //Console.WriteLine("Key: " + key);
                        //Console.WriteLine("N: " + n);

                        if(allHaveEpi){
                            //Console.WriteLine("All productions have epsilon in the firsts");
                            HashSet<string> allFirsts = new HashSet<string>();
                            foreach(string elem in production){
                                foreach(string first in _firstSetWork[elem]){
                                    allFirsts.Add(first);
                                }
                            }
                            _nextSetWork[key] = _nextSetWork[key].Union(allFirsts.Union(_followSetWork[key]).ToHashSet<string>()).ToHashSet<string>();
                            _yamlNext.Add(indx, key, allFirsts.Union(_followSetWork[key]).ToHashSet<string>());
                        } else {
                            //Console.WriteLine("Not all elemens have epsilon in the firsts");
                            HashSet<string> allFirsts = new HashSet<string>();
                            for(int i = 0; i <= n; i++){
                                foreach(string first in _firstSetWork[production[i]]){
                                    allFirsts.Add(first);
                                }
                            }
                            _nextSetWork[key] = _nextSetWork[key].Union(allFirsts.Except(episet).ToHashSet<string>()).ToHashSet<string>();
                            _yamlNext.Add(indx, key, allFirsts.Except(episet).ToHashSet<string>());
                        }
                        indx++;
                    }
                    if(!hashSetsEqual(prevSet, _nextSet[key])){
                        hasChanged = true;
                    }
                }
            }
        }

        public void computeNextSet(){
            bool hasChanged = true;
            int indx = 0;

            foreach(string t in terminals){
                _nextSet[t].Add(t);
            }

            while(hasChanged) {
                hasChanged = false;

                foreach(string key in _internalFormedTable.Keys){
                    HashSet<string> prevSet = _nextSet[key].ToHashSet<string>();

                    foreach(List<string> production in _internalFormedTable[key]){
                        HashSet<string> trailer = _nextSet[key];

                        int n = 0;
                        bool allHaveEpi = true;
                        foreach(string elem in production) {
                            if(!_firstSet[elem].Contains(epi)) {
                                allHaveEpi = false;
                                n = production.IndexOf(elem);
                                break;
                            }
                        }

                        //Console.WriteLine("Key: " + key);
                        //Console.WriteLine("N: " + n);

                        if(allHaveEpi){
                            //Console.WriteLine("All productions have epsilon in the firsts");
                            HashSet<string> allFirsts = new HashSet<string>();
                            foreach(string elem in production){
                                foreach(string first in _firstSet[elem]){
                                    allFirsts.Add(first);
                                }
                            }
                            _nextSet[key] = _nextSet[key].Union(allFirsts.Union(_followSet[key]).ToHashSet<string>()).ToHashSet<string>();
                            _yamlNext.Add(indx, key, allFirsts.Union(_followSet[key]).ToHashSet<string>());
                        } else {
                            //Console.WriteLine("Not all elemens have epsilon in the firsts");
                            HashSet<string> allFirsts = new HashSet<string>();
                            for(int i = 0; i <= n; i++){
                                foreach(string first in _firstSet[production[i]]){
                                    allFirsts.Add(first);
                                }
                            }
                            _nextSet[key] = _nextSet[key].Union(allFirsts.Except(episet).ToHashSet<string>()).ToHashSet<string>();
                            _yamlNext.Add(indx, key, allFirsts.Except(episet).ToHashSet<string>());
                        }
                        indx++;
                    }
                    if(!hashSetsEqual(prevSet, _nextSet[key])){
                        hasChanged = true;
                    }
                }
            }
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

    public class ListWithDuplicates : List<(int, KeyValuePair<string, HashSet<string>>)> {
        public void Add(int num, string key, HashSet<string> value) {
            var element = new KeyValuePair<string, HashSet<string>>(key, value);
            this.Add((num, element));
        }
        public void Remove(int num, string key, HashSet<string> value) {
            var element = new KeyValuePair<string, HashSet<string>>(key, value);
            this.Remove((num, element));
        }
    }
    
}