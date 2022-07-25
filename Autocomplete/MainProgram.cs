using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autocomplete
{
    public class MainProgram : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private AppReadWriter appHandler;
        private LowLevelKeyBoardListener listener;
        private DropDown sugestionBox = null;
        private Trie trie;
        public MainProgram()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = new Icon("C:\\Program Files\\Notepad++\\updater\\updater.ico"),
                ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Exit", Exit)
            }),
                Visible = true
            };
            trayIcon.Click += trayIcon_Click;
            // start listening for typing in other apps.
            appHandler = new AppReadWriter();
            appHandler.OnTextChange += AppHandler_OnTextChange;
            //listener = new LowLevelKeyBoardListener();
            sugestionBox = new DropDown();
            sugestionBox.Hide();
            trie = Trie.LoadFromFile();
            sugestionBox.OnComplete += complete;
        }
        void complete(object sender, string word)
        {
            appHandler.writeWord(word);
        }
        private const int SW_SHOWNA = 4;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private void AppHandler_OnTextChange(object sender, string e)
        {
            ShowWindow(sugestionBox.Handle, SW_SHOWNA);
            sugestionBox.BringToFront();
            string word = GetLastWord(e);
            if (word.Length > 0)
            {
                Dictionary<int, string> ordered = trie.GetCompletions(word);
                List<int> keys = ordered.Keys.ToList();
                keys.Sort();
                List<string> values = new List<string>();
                foreach (int i in keys)
                {
                    values.Add(ordered[i]);
                }
                sugestionBox.suggestions = values; 
            }
            //sugestionBox.UpdateContents();
        }
        
        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

            Application.Exit();
        }
        private void trayIcon_Click(object sender, EventArgs e)
        {
            var eventArgs = e as MouseEventArgs;
            switch (eventArgs.Button)
            {
                // Left click to reactivate
                case MouseButtons.Left:
                    var form1 = new Form1();
                    form1.Show();
                    break;
            }
        }
        public static string GetLastWord(string phrase, int start = -1)
        {
            int wordEndPosition;
            if (phrase == null)
                return "";
            if (start<0)
                wordEndPosition = phrase.Length;
            else
                wordEndPosition = start;
            int currentPosition = wordEndPosition;
            while (currentPosition != 0 && (phrase.Substring(currentPosition - 1, 1) != " "|| phrase.Substring(currentPosition - 1, 1) != "\n"))
                currentPosition--;
            return phrase.Substring(currentPosition, wordEndPosition - currentPosition);
        }
    }
}
