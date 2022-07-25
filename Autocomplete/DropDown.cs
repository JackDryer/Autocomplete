using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autocomplete
{
    public partial class DropDown : Form
    {
        public EventHandler<string> OnComplete;
        private List<string> _suggestions;
        private int selectedIndex;
        private LowLevelKeyBoardListener listener;
        public List<string> suggestions {
            get { return _suggestions; }
            set
            {
                _suggestions = value;
                UpdateContents();
            }
        }
        public void UpdateContents()
        {
            string outtext = "";
            foreach (string word in suggestions)
            {
                outtext += word;
                outtext += "\n";
            }
            this.wordsBox.Text = outtext;
        }
        private void Complete(object sender, EventArgs e)
        {
            OnComplete?.Invoke(this, suggestions[selectedIndex]);
        }
        void OnKeyPressed(object sender, KeyPressedArgs e)
        {
            suggestions.Add(e.KeyPressed.ToString());
            UpdateContents();
        }
        public DropDown()
        {
            InitializeComponent();
            _suggestions = new List<string>();
            selectedIndex = 0;
            listener = new LowLevelKeyBoardListener();
            listener.blockKeys.Add("Tab");
            listener.AddEvent("Tab", Complete);
            listener.HookKeyboard();
            //listener.OnKeyPressed += OnKeyPressed;
            suggestions.Add("Ran");
            UpdateContents();
        }
    }
}
