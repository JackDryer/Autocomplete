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
        //private LowLevelKeyBoardListener listener;
        private DropDown sugestionBox = null;
        private Trie trie;
        private System.Windows.Automation.Text.TextPatternRange textrange;
        public MainProgram()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.Autocomplete_Logo,
                Text = "Autocomplete.exe",
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
            appHandler.OnUneditableWindow += AppHandler_OnUneditableWindow;
        }

        private void AppHandler_OnUneditableWindow(object sender, string name)
        {
            sugestionBox.Stop();
        }

        void complete(object sender, string word)
        {
            appHandler.ReplaceWord(word,textrange);
        }

        private void AppHandler_OnTextChange(object sender, string e)
        {
            sugestionBox.SetTop();
            textrange = appHandler.GetActiveWord();
            if (textrange != null && textrange.GetBoundingRectangles().Count()>0 && textrange.GetText(-1).Any(x=>char.IsLetter(x)))
            {
                var loc = textrange.GetBoundingRectangles()[0].BottomLeft;
                string word = textrange.GetText(-1);
                sugestionBox.Left = (int)loc.X;
                sugestionBox.Top = (int)loc.Y;
                List<string> values = trie.GetCompletions(word, 10, 1E-307);// tested to 1E-324, minimum value of a float is ~ 2E-308
                 sugestionBox.suggestions = values;
                //sugestionBox.UpdateContents();
                sugestionBox.Start();
            }
            else
                sugestionBox.Stop();
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
            while (currentPosition != 0 && phrase.Substring(currentPosition - 1, 1) != " " &&phrase.Substring(currentPosition - 1, 1) != "\n")
                currentPosition--;
            return phrase.Substring(currentPosition, wordEndPosition - currentPosition);
        }
    }
}
