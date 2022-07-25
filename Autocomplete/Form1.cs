using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autocomplete
{
    public partial class Form1 : Form
    {
        ComboBox combo;
        Trie wordPredictor;
        public Form1()
        {
            InitializeComponent();
            combo = new ComboBox();
            wordPredictor = new Trie();
            using (var sr = new StreamReader("U:\\Source\\Autocomplete\\google-10000-english-no-swears.txt"))
            {
                string all = sr.ReadToEnd();
                string[] allList = all.Split('\n');
                int rank = 1;
                foreach (string i in allList)
                {
                    wordPredictor.Add(i,rank);
                    rank++;
                }
                /*Dictionary<int, string> ordered = wordPredictor.GetCompletions("bec");
                List<int> keys = ordered.Keys.ToList();
                keys.Sort();
                List<string> values = new List<string>();
                foreach (int i in keys)
                {
                    values.Add(ordered[i]);
                }
                foreach (string i in values)
                    MessageBox.Show(i);*/
                
            }
            

        }
        private void InputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            string word = GetCurrentWord();
            if (word.Length == 0)
            {
                combo.Hide();
                return;
            }
            Dictionary<int, string> ordered = wordPredictor.GetCompletions(word);
            List<int> keys = ordered.Keys.ToList();
            keys.Sort();
            List<string> values = new List<string>();
            foreach (int i in keys)
            {
                values.Add(ordered[i]);
            }
            CreateDropDown(InputBox.GetPositionFromCharIndex(InputBox.SelectionStart), values) ;
        }
        private string GetCurrentWord()
        {
            int wordEndPosition = InputBox.SelectionStart;
            int currentPosition = wordEndPosition;
            while (currentPosition != 0 && InputBox.Text.Substring(currentPosition - 1, 1) != " ")
                currentPosition--;
            return InputBox.Text.Substring(currentPosition, wordEndPosition - currentPosition);
        }
        private void CreateDropDown(Point pos, List<string> list)
        { 
            combo.DataSource =list ;
            pos.Offset(0, 15);
            combo.Location = pos;
            this.Controls.Add(combo);
            combo.BringToFront();
            combo.Show();
        }
    }

}





