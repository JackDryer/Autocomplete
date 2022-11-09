using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocomplete
{
    class Trie
    {
        static double MATCH = 0.999;
        static double NEAR = 0.4;
        static double WRONG = 0.01;
        static bool DEBUG = false;
        public static Trie LoadFromFile()
        {
            Trie outtrie = new Trie();
            using (var sr = new System.IO.StreamReader("processed.txt"))
            {
                string all = sr.ReadToEnd();
                string[] allList = all.Split('\n');
                foreach (string i in allList)
                {
                    string[] split = i.Split(':');
                    outtrie.Add(split[0], int.Parse(split[1]));
                }
                return outtrie;
            }
        }
        public void Add(string word, int frequency)
        {
            if (word.Length > 0)
            {
                if (children.ContainsKey(word[0]))
                {
                    children[word[0]].Add(word.Substring(1), frequency);
                    frequencies[word[0]] += frequency;
                }
                else
                {
                    children[word[0]] = new Trie(word.Substring(1), frequency);
                    frequencies[word[0]] = frequency;
                }


            }
            else
            {
                if (frequencies.ContainsKey('$'))
                    frequencies['$'] += frequency;
                else
                    frequencies['$'] = frequency;
            }
            totalcount += frequency;
        }
        private Dictionary<char, Trie> children;
        private Dictionary<char, int> frequencies;
        private int totalcount =0; // for speed
        public Trie()
        {
            this.children = new Dictionary<char, Trie>();
            frequencies = new Dictionary<char, int>();
        }
        public Trie(string word, int frequency)
        {
            this.frequencies = new Dictionary<char, int>();
            this.children = new Dictionary<char, Trie>();
            if (word.Length > 0)
            {
                this.children[word[0]] = new Trie(word.Substring(1), frequency);
                frequencies[word[0]] = frequency;
            }
            else
            {
                frequencies['$'] = frequency;
            }


            totalcount += frequency;
        }

        public bool Contains(string word)
        {
            if (word.Length == 0)
            {
                return frequencies.ContainsKey('$');
            }
            else if (children.ContainsKey(word[0]))

                return children[word[0]].Contains(word.Substring(1));
            else
                return false;
        }

        public List<string> GetCompletions(string incomplete, int maxreturn = 100, double minimumprobability = 0)
        {
            //var toSearch = new SortedList <double, Tuple<Trie, string>>(new DuplicateKeyComparer<double>());
            var toSearch = new BinaryHeap <Tuple<Trie, string>> (300,null);
            toSearch.Insert(1,new Tuple<Trie, string>(this, ""));
            var output = new List<string>();
            double probability = 1;
            Trie mostProbable;
            double wordProbability;
            string pastWord;
            int index;
            string surround;
            while (toSearch.Count > 0 && output.Count < maxreturn)
            {
                //if (DEBUG)
                //    Console.WriteLine(toSearch.Count);
                probability = toSearch.PeekOfHeap().Key;
                mostProbable = toSearch.PeekOfHeap().Value.Item1;
                pastWord = toSearch.PeekOfHeap().Value.Item2;
                index = pastWord.Length;
                toSearch.extractHeadOfHeap();
                if (mostProbable == null)
                {
                    if (DEBUG)
                        output.Add(pastWord+probability.ToString());
                    else
                        output.Add(pastWord);
                }
                else
                {
                    if (index > 0 && index < incomplete.Length - 1)
                        surround = incomplete.Substring(index - 1, 3);
                    else if (index == 0 && incomplete.Length>1)
                        surround = incomplete.Substring(index, 2);
                    else if (index == incomplete.Length - 1 && incomplete.Length > 1)
                        surround = incomplete.Substring(index - 1, 2);
                    else surround = "";
                    foreach (KeyValuePair<char, double> i in mostProbable.getKeyProbabilities(incomplete.ElementAtOrDefault(index), surround))
                    {
                        wordProbability = i.Value * probability;
                        if (i.Key == '$')
                        {
                            var lengthdiff = incomplete.Length - pastWord.Length;// only is an issue when the word is too short, this is an autocomplete after all
                            if (lengthdiff > 1)
                            {
                                wordProbability = wordProbability * Math.Pow(WRONG, lengthdiff);
                            }
                            if (wordProbability > minimumprobability)
                                toSearch.Insert(wordProbability,new Tuple<Trie, string>(null, pastWord));

                        }
                        else
                            if (wordProbability > minimumprobability)
                                toSearch.Insert(wordProbability,new Tuple<Trie, string>(mostProbable.children[i.Key], pastWord + i.Key));
                    }
                }
            }
            return output;
        }
        double getKeyProbability(char targetkey, char referancekey,string nearkeys = "")
        {
            if (char.ToLower(targetkey) == char.ToLower(referancekey)) return MATCH;
            if (targetkey == '\'') return MATCH; // noone ever types these
            else if (nearkeys.Contains(targetkey)) return NEAR;
            else return WRONG;
        }
        Dictionary<char, double> getKeyProbabilities(char referance, string nearkeys = "")
        {
            var output = new Dictionary<char, double>();
            foreach (KeyValuePair<char, int> i in this.frequencies)
            {
                if (referance == '\0')
                    output[i.Key] = (double)i.Value / (double)this.totalcount;
                else
                    output[i.Key] = ((double)i.Value / (double)this.totalcount) * this.getKeyProbability(i.Key, referance, nearkeys);
            }
            return output;
        }

    }

    //from https://stackoverflow.com/questions/5716423/c-sharp-sortable-collection-which-allows-duplicate-keys
    public class DuplicateKeyComparer<TKey>
                :
             IComparer<TKey> where TKey : IComparable
    {
        #region IComparer<TKey> Members

        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);

            if (result == 0)
                return 1; // Handle equality as being greater. Note: this will break Remove(key) or
            else          // IndexOfKey(key) since the comparer never returns 0 to signal key equality
                return result;
        }

        #endregion
    }

}
