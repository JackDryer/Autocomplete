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
            
            SetText(outtext);
        }
        private void Complete(object sender, EventArgs e)
        {
            OnComplete?.Invoke(this, suggestions[selectedIndex]);
            //SetText(suggestions[selectedIndex]);
        }
        void OnKeyPressed(object sender, KeyPressedArgs e)
        {
            suggestions.Add(e.KeyPressed.ToString());
            UpdateContents();
        }
        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.wordsBox.InvokeRequired)
            {
                if (!this.wordsBox.IsDisposed)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    this.Invoke(d, new object[] { text });
                }
            }
            else
            {
                this.wordsBox.Text = text;
            }
        }
        public DropDown()
        {
            InitializeComponent();
            _suggestions = new List<string>();
            selectedIndex = 0;
            listener = new LowLevelKeyBoardListener();
            listener.blockKeys.Add("Tab");
            listener.AddEvent("Tab", Complete);
            listener.blockKeys.Add("Up");
            listener.AddEvent("Up",aboveOption);
            listener.blockKeys.Add("Down");
            listener.AddEvent("Down", bellowOption);
            listener.HookKeyboard();
            //listener.OnKeyPressed += OnKeyPressed;
            suggestions.Add("Ran");
            UpdateContents();
        }
        public void aboveOption(object sender, EventArgs e)
        {
            if (selectedIndex >0)
                selectedIndex--;
        }
        public void bellowOption(object sender, EventArgs e)
        {
            if (selectedIndex < suggestions.Count - 1) 
                selectedIndex++;
        }
    }
}
