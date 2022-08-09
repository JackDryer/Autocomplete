using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocomplete
{
    class Trie
    {
        public static Trie LoadFromFile()
        {
            Trie outtrie = new Trie();
            using (var sr = new System.IO.StreamReader("google-10000-english-no-swears.txt"))
            {
                string all = sr.ReadToEnd();
                string[] allList = all.Split('\n');
                int rank = 1;
                foreach (string i in allList)
                {
                    outtrie.Add(i, rank);
                    rank++;
                }
                return outtrie;
            }
        }
        public void Add(string word, double probability)
        {
            if (word.Length > 0)
            {
                if (children.ContainsKey(word[0]))
                {
                    children[word[0]].Add(word.Substring(1), probability);
                    probabilities[word[0]] += probability;
                }
                else
                {
                    children[word[0]] = new Trie(word.Substring(1), probability);
                    probabilities[word[0]] = probability;
                }


            }
            else
            {
                if (probabilities.ContainsKey('\0'))
                    probabilities['\0'] += probability;
                else
                    probabilities['\0'] = probability;
            }
        }
        private Dictionary<char, Trie> children;
        private Dictionary<char, double> probabilities;
        public Trie()
        {
            this.children = new Dictionary<char, Trie>();
            probabilities = new Dictionary<char, double>();
        }
        public Trie(string word, double probability)
        {
            this.children = new Dictionary<char, Trie>();
            if (word.Length > 0)
            {
                this.children[word[0]] = new Trie(word.Substring(1), probability);
                probabilities[word[0]] = probability;
            }
            else probabilities['\0'] = probability;

        }

        public bool Contains(string word)
        {
            if (word.Length == 0)
            {
                return probabilities.ContainsKey('\0');
            }
            else if (children.ContainsKey(word[0]))

                return children[word[0]].Contains(word.Substring(1));
            else
                return false;
        }


        public List<string> OrderedTraverse(int maxreturn = 100, double minimumprobability = 0)
            {
            var toSearch = new List<Tuple<double, Trie,string>>();
            toSearch.Add(new Tuple<double, Trie, string>(1, this,""));
            var output = new List<string>();
            double probabilty;
            Trie mostProbable;
            string pastWord;
            while (toSearch.Count >0 && output.Count < maxreturn)
            {
                toSearch.Sort((a, b) => b.Item1.CompareTo(a.Item1));
                probabilty = toSearch[0].Item1;
                mostProbable = toSearch[0].Item2;
                pastWord = toSearch[0].Item3;
                toSearch.RemoveAt(0);
                if (mostProbable == null)
                    output.Add(pastWord);
                else {
                    foreach (KeyValuePair<char, double> i in mostProbable.probabilities)
                    {
                        if (i.Key == '\0')
                            toSearch.Add(new Tuple<double, Trie, string>(i.Value * probabilty, null, pastWord));
                        else
                            toSearch.Add(new Tuple<double, Trie, string>(i.Value * probabilty, mostProbable, pastWord + i.Key));
                    }
                }
            }
            return output;
            }
        public List<string> GetCompletions(string incomplete, int maxreturn = 100, double minimumprobability = 0)
        {
            var toSearch = new List<Tuple<double, Trie, string>>();
            toSearch.Add(new Tuple<double, Trie, string>(1, this, ""));
            var output = new List<string>();
            double probabilty;
            Trie mostProbable;
            string pastWord;
            int index;
            double keyprobablity;
            while (toSearch.Count > 0 && output.Count < maxreturn)
            {
                toSearch.Sort((a, b) => b.Item1.CompareTo(a.Item1));
                probabilty = toSearch[0].Item1;
                mostProbable = toSearch[0].Item2;
                pastWord = toSearch[0].Item3;
                index = pastWord.Length;
                toSearch.RemoveAt(0);
                if (mostProbable == null)
                    output.Add(pastWord);
                else
                {
                    foreach (KeyValuePair<char, double> i in mostProbable.probabilities)
                    {
                        if (index < incomplete.Length)
                            keyprobablity = i.Value * caclucateKeyProbability(i.Key, incomplete[index]);
                        else
                            keyprobablity = i.Value;
                        if (i.Key == '\0')

                            toSearch.Add(new Tuple<double, Trie, string>(keyprobablity * probabilty, null, pastWord));
                        else
                            toSearch.Add(new Tuple<double, Trie, string>(keyprobablity * probabilty, mostProbable, pastWord + i.Key));
                    }
                }
            }
            return output;
        }
        double caclucateKeyProbability(char targetkey, char referancekey)
        {
            if (targetkey == referancekey) return 0.99;
            else return 0.01;
        }

    }
}
