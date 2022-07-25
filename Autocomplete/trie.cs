using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocomplete
{
    class Trie
    {
        private Dictionary<char, Trie> children;
        private int? rank;
        public void Add(char value)
        {
            if (!children.ContainsKey(value))
                this.children[value] = new Trie();
        }
        public void Add(string word, int? rank)
        {
            if (word.Length > 0)
            {
                if (children.ContainsKey(word[0]))
                {
                    children[word[0]].Add(word.Substring(1), rank);
                }
                else
                    children[word[0]] = new Trie(word.Substring(1), rank);

            }
            else
            {
                this.rank = rank;
            }
        }
        public Trie()
        {
            this.children = new Dictionary<char, Trie>();
            rank = null;
        }
        public Trie(string word, int? rank)
        {
            this.children = new Dictionary<char, Trie>();
            if (word.Length > 0)
            {
                this.rank = null;
                this.children[word[0]] = new Trie(word.Substring(1), rank);
            }
            else this.rank = rank;

        }

        public bool Contains(string word)
        {
            if (word.Length == 0)
            {
                return (this.rank != null);
            }
            else if (children.ContainsKey(word[0]))

                return children[word[0]].Contains(word.Substring(1));
            else
                return false;
        }

        public List<string> Traverse()
        {
            List<string> output = new List<string>();
            if (this.rank != null)
                output.Add("");
            foreach (var item in children)
            {
                foreach (string wordPart in item.Value.Traverse())
                    output.Add(item.Key + wordPart);
            }
            return output;
        }
        public Dictionary<int, string> OrderedTraverse()
        {
            Dictionary<int, string> output = new Dictionary<int, string>();
            if (this.rank != null)
                output[(int)this.rank] = ("");
            foreach (var item in children)
            {
                foreach (var wordkeyvaluepair in item.Value.OrderedTraverse())
                    output[wordkeyvaluepair.Key] = (item.Key + wordkeyvaluepair.Value);
            }
            return output;
        }
        public Dictionary<int, string> GetCompletions(string incomplete)
        {
            if (incomplete.Length == 0)
            {
                return this.OrderedTraverse();
            }
            if (children.ContainsKey(incomplete[0]))
            {
                Dictionary<int, string> output;
                output = children[incomplete[0]].GetCompletions(incomplete.Substring(1));
                foreach (var keyvalue in output.ToArray())
                    output[keyvalue.Key] = incomplete[0] + keyvalue.Value;
                return output;
            }
            else
                return new Dictionary<int, string>();
        }

    }
}
