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
        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }
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
        private const int SW_SHOWNA = 4;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public void Start()
        {
            ShowWindow(this.Handle, SW_SHOWNA);
            this.BringToFront();
            listener.blockKeys.Add("Tab");
            listener.AddEventOnce("Tab", Complete);
            listener.blockKeys.Add("Up");
            listener.AddEventOnce("Up", aboveOption);
            listener.blockKeys.Add("Down");
            listener.AddEventOnce("Down", bellowOption);
            listener.blockKeys.Add("Escape");
            listener.AddEventOnce("Escape", Stop);
            listener.HookKeyboard();
        }
        public void Stop()
        {
            this.Hide();
            listener.blockKeys.Remove("Tab");
            listener.RemoveEvent("Tab");
            listener.blockKeys.Remove("Up");
            listener.RemoveEvent("Up");
            listener.blockKeys.Remove("Down");
            listener.RemoveEvent("Down");
            listener.blockKeys.Remove("Escape");
            listener.RemoveEvent("Escape");
            listener.UnHookKeyboard();
        }
        void Stop (object sender, EventArgs e)
        {
            Stop();
        }
        delegate void UpdateCallBack();
        public void UpdateContents()
        {
            if (this.wordsBox.InvokeRequired)
            {
                if (!this.wordsBox.IsDisposed)
                {
                    UpdateCallBack d = new UpdateCallBack(UpdateContents);
                    this.Invoke(d, new object[] { });
                }
            }
            else
            {
                if (selectedIndex >= suggestions.Count)
                    selectedIndex = 0;
                string outtext = "";
                int start = 0, range = 0;
                for (int i = 0; i < suggestions.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        start = outtext.Length;
                        range = suggestions[i].Length;
                    }
                    outtext += suggestions[i];
                    outtext += "\n";
                }
                SetText(outtext);
                wordsBox.SelectionStart = start;
                wordsBox.SelectionLength = range;
                wordsBox.SelectionColor = Color.Red;
                oldSelection[0] = 0;
                oldSelection[1] = 0;
            }

        }
        private void Complete(object sender, EventArgs e)
        {
            if (suggestions.Count > 0)
                OnComplete?.Invoke(this, suggestions[selectedIndex]);
            selectedIndex = 0;
        }
        void OnKeyPressed(object sender, KeyPressedArgs e)
        {
            //suggestions.Add(e.KeyPressed.ToString());
            //UpdateContents();
            //MessageBox.Show(e.KeyPressed.ToString());
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
        delegate void Callback();
        public DropDown()
        {
            InitializeComponent();
            _suggestions = new List<string>();
            selectedIndex = 0;
            listener = new LowLevelKeyBoardListener();
            //listener.OnKeyPressed += OnKeyPressed;
        }
        private void loadunload()
        {
            ShowWindow(this.Handle, SW_SHOWNA);
            Hide();
        }
        public void aboveOption(object sender, EventArgs e)
        {
            if (selectedIndex > 0)
                selectedIndex--;
            else if (suggestions.Count > 0)
                selectedIndex = suggestions.Count - 1;
            UpdateContents();
        }
        public void bellowOption(object sender, EventArgs e)
        {
            if (selectedIndex < suggestions.Count - 1)
                selectedIndex++;
            else
                selectedIndex = 0;
            UpdateContents();
        }

        int[] oldSelection = {0,0};

        private void wordsBox_MouseMove(object sender, MouseEventArgs e)
        {
            wordsBox.SelectionStart = oldSelection[0];
            wordsBox.SelectionLength = oldSelection[1];
            wordsBox.SelectionColor = Color.Black;
            var p = new Point(MousePosition.X - Left, MousePosition.Y - Top);
            int selected = wordsBox.GetCharIndexFromPosition(p);
            int end = wordsBox.Text.IndexOf('\n', selected);
            int start = wordsBox.Text.Substring(0, selected).LastIndexOf('\n');
            start++;
            if (start < end)
            {
                wordsBox.SelectionStart = start;
                wordsBox.SelectionLength = end - start;
                wordsBox.SelectionColor = Color.IndianRed;
                oldSelection[0] = start;
                oldSelection[1] = end - start;
            }


        }

        private void wordsBox_MouseLeave(object sender, EventArgs e)
        {
            UpdateContents();
        }

        private void wordsBox_Click(object sender, EventArgs e)
        {
            OnComplete?.Invoke(this, wordsBox.Text.Substring(oldSelection[0], oldSelection[1]));
        }
    }
}
