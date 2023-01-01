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

namespace Autocomplete
{

    public partial class DropDown : Form
    {
        public List<string> Suggestions
        {
            get => selectionBox.Suggestions;
            set => selectionBox.Suggestions = value;
        }
        
        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }
        public EventHandler<string> OnComplete;

        private LowLevelKeyBoardListener listener;
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
            listener.AddEventOnce("Up", selectionBox.moveSelecionUp);
            listener.blockKeys.Add("Down");
            listener.AddEventOnce("Down", selectionBox.moveSelecionDown);
            listener.blockKeys.Add("Escape");
            listener.AddEventOnce("Escape", Stop);
            //listener.blockKeys.Add("D");
            if (!listener.Hooked) // 
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
            if (listener.Hooked)
                listener.UnHookKeyboard();
        }
        void Stop (object sender, EventArgs e)
        {
            Stop();
        }

        public int GetHandle()
        {
            return selectionBox.GetHandle();
        }
        private void Complete(object sender, EventArgs e)
        {
            if (selectionBox.Selection != "")
            {
                OnComplete?.Invoke(this, selectionBox.Selection);
            }
            selectionBox.ResetSelection();
        }
        void OnKeyPressed(object sender, KeyPressedArgs e)
        {
            if (listener.blockKeys.Contains(e.KeyPressed.ToString()))
                MessageBox.Show("blocked");
            Console.WriteLine(e.KeyPressed.ToString());
            //UpdateContents();
            //MessageBox.Show(e.KeyPressed.ToString());
        }
        public DropDown()
        {
            InitializeComponent();
            LoadSettings();
            listener = new LowLevelKeyBoardListener();
            SetStyle(ControlStyles.Selectable, false);
            //listener.OnKeyPressed += OnKeyPressed;
            selectionBox.WhenClicked += selectionBox_MouseClick;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams param = base.CreateParams;
                param.ExStyle |= 0x08000000;// this might make the window unable to take focus
                return param;
            }
        }
        private void loadunload()
        {
            ShowWindow(this.Handle, SW_SHOWNA);
            Hide();
        }

        public void SetTop()
        {
            TopLevel = true;
            TopMost = true;
        }
        private void selectionBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (selectionBox.MouseSelection != "")
            {
                OnComplete?.Invoke(this, selectionBox.MouseSelection);
            }
            selectionBox.ResetSelection();
        }
        public void LoadSettings()
        {
            selectionBox.LoadSettings();
        }
    }
}
