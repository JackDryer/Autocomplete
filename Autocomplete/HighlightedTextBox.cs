using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Autocomplete
{
    public partial class HighlightedTextBox : UserControl
    {
        private decimal textSize;
        private Color textColour;
        private Color backgroundColour;
        private Color highlightColour;
        private Color highlightBackgroundColour;
        private Color mouseHighlightColour;//= Color.IndianRed;
        private int selectedIndex;
        int[] lastRecoredMouseSelection = { 0, 0 };

        public event MouseEventHandler WhenClicked;

        private List<string> _suggestions;
        public List<string> Suggestions
        {
            get { return _suggestions; }
            set
            {
                _suggestions = value;
                UpdateContents();
            }
        }
        public HighlightedTextBox()
        {
            InitializeComponent();
            _suggestions = new List<string>();
            selectedIndex = 0;
        }
        public string MouseSelection
        {
            get => richTextBox.Text.Substring(lastRecoredMouseSelection[0], lastRecoredMouseSelection[1]);
        }
        public string Selection
        {
            get 
            {
                if (Suggestions.Count > 0)
                    return Suggestions[selectedIndex];
                else 
                    return "";
            }
        }
        public void ResetSelection()
        {
            selectedIndex = 0;
        }
        delegate void UpdateCallBack(); // for threading
        public void UpdateContents()
        {
            if (this.richTextBox.InvokeRequired)
            {
                if (!this.richTextBox.IsDisposed)
                {
                    UpdateCallBack d = new UpdateCallBack(UpdateContents);
                    this.Invoke(d, new object[] { });
                }
            }
            else
            {
                if (selectedIndex >= Suggestions.Count)
                    selectedIndex = 0;
                string outtext = "";
                int start = 0, range = 0;
                for (int i = 0; i < Suggestions.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        start = outtext.Length;
                        range = Suggestions[i].Length;
                    }
                    outtext += Suggestions[i];
                    outtext += "\n";
                }
                outtext = outtext.TrimEnd('\n');
                SetText(outtext);
                richTextBox.SelectionStart = start;
                richTextBox.SelectionLength = range;
                richTextBox.SelectionColor = highlightColour;
                richTextBox.SelectionBackColor = highlightBackgroundColour;
                if (!richTextBox.ClientRectangle.Contains(richTextBox.GetPositionFromCharIndex(richTextBox.SelectionStart)))
                    {
                    richTextBox.ScrollToCaret();
                    }
                //richTextBox.DeselectAll();
                lastRecoredMouseSelection[0] = 0;//how does this help?
                lastRecoredMouseSelection[1] = 0;
            }
        }
        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.richTextBox.InvokeRequired)
            {
                if (!this.richTextBox.IsDisposed)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    this.Invoke(d, new object[] { text });
                }
            }
            else
            {
                this.richTextBox.Text = text;
            }
        }
        public void moveSelecionUp(object sender, EventArgs e)
        {
            if (selectedIndex > 0)
                selectedIndex--;
            else if (Suggestions.Count > 0)
                selectedIndex = Suggestions.Count - 1;
            UpdateContents();
        }
        public void moveSelecionDown(object sender, EventArgs e)
        {
            if (selectedIndex < Suggestions.Count - 1)
                selectedIndex++;
            else
                selectedIndex = 0;
            UpdateContents();
        }
        public void LoadSettings()
        {
            string fileName = "settings.json";
            string jsonString = File.ReadAllText(fileName);
            Console.WriteLine(jsonString);
            Settings settings = JsonSerializer.Deserialize<Settings>(jsonString);
            ApplySettings(settings);
        }
        public void ApplySettings(Settings settings)
        {
            this.textSize = settings.textSize;
            this.textColour = Color.FromArgb(settings.textColour);
            this.highlightColour = Color.FromArgb(settings.highlightColour);
            this.backgroundColour = Color.FromArgb(settings.backgroundColour);
            this.highlightBackgroundColour = Color.FromArgb(settings.highlightBackgroundColour);
            this.richTextBox.ForeColor = this.textColour;
            this.richTextBox.BackColor = this.backgroundColour;
            this.mouseHighlightColour = this.highlightColour;
            this.richTextBox.Font = new Font("Microsoft Sans Serif", (float)this.textSize);

            UpdateContents();
        }
        public int GetHandle()// for ignoring clicks to itself
        {
            return richTextBox.Handle.ToInt32();
        }

        private void richTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            //deselect the last mouse move
            richTextBox.SelectionStart = lastRecoredMouseSelection[0];
            richTextBox.SelectionLength = lastRecoredMouseSelection[1];
            richTextBox.SelectionColor = textColour;
            richTextBox.SelectionBackColor = backgroundColour;

            //locate the word under the mouse
            Point p = e.Location;
            int selected = richTextBox.GetCharIndexFromPosition(p);
            int end = richTextBox.Text.IndexOf('\n', selected);
            int start = richTextBox.Text.Substring(0, selected).LastIndexOf('\n');
            start++;
            if (start < end)
            {
                richTextBox.SelectionStart = start;
                richTextBox.SelectionLength = end - start;
                richTextBox.SelectionColor = mouseHighlightColour;
                richTextBox.SelectionBackColor = highlightBackgroundColour;
                //richTextBox.DeselectAll();
                lastRecoredMouseSelection[0] = start;
                lastRecoredMouseSelection[1] = end - start;
            }
        }
        private void richTextBox_MouseLeave(object sender, EventArgs e)
        {
            UpdateContents();
        }
        private void richTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            WhenClicked?.Invoke(this, e);
        }

    }
}

