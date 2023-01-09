using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;


namespace Autocomplete
{
    public class MainProgram : ApplicationContext
    {
        public const string pathToWordList = "Wordlist.txt";
        private NotifyIcon trayIcon;
        private AppReadWriter appHandler;
        //private LowLevelKeyBoardListener listener;
        private DropDown sugestionBox = null;
        private Trie trie;
        private TextInformation textInfo;
        public MainProgram()
        {
            // start listening for typing in other apps.
            appHandler = new AppReadWriter();
            appHandler.OnTextChange += AppHandler_OnTextChange;
            //listener = new LowLevelKeyBoardListener();
            sugestionBox = new DropDown();
            sugestionBox.Start();
            sugestionBox.Stop();
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
            sugestionBox.OnComplete += complete;
            appHandler.OnAppChange += AppHandler_onAppChange;
            trie = Trie.LoadFromFile();
        }

        private void AppHandler_onAppChange(object sender, EventArgs e)
        {
            if (this.sugestionBox.InvokeRequired)
            {
                if (!this.sugestionBox.IsDisposed)
                {
                    AppHandler_OnTextChangeCallback d = new AppHandler_OnTextChangeCallback(AppHandler_onAppChange);
                    sugestionBox.Invoke(d, new object[] { sender, e });
                }
                return;
            }
            appHandler.ignorehandles.Add(sugestionBox.GetHandle());
            sugestionBox.Stop();
        }

        void complete(object sender, string word)
        {
            appHandler.Insert(word);
        }
        private delegate void AppHandler_OnTextChangeCallback(object sender, EventArgs e);
        private void AppHandler_OnTextChange(object sender, EventArgs e)
        {
            if (this.sugestionBox.InvokeRequired)
            {
                if (!this.sugestionBox.IsDisposed)
                {
                    AppHandler_OnTextChangeCallback d = new AppHandler_OnTextChangeCallback(AppHandler_OnTextChange);
                    sugestionBox.Invoke(d, new object[] {sender,e});
                }
                return;
            }
            //sugestionBox.SetTop();
            textInfo = appHandler.GetActiveWord();
            if (textInfo != null&&textInfo.text != null && textInfo.text.Any(x=>char.IsLetter(x)))
            {
                var loc = textInfo.boundingBox.BottomLeft;
                string word = textInfo.text;
                sugestionBox.Left = (int)loc.X;
                sugestionBox.Top = (int)loc.Y;
                if (word == null)
                    MessageBox.Show("word was null");
                List<string> values = trie.GetCompletions(word, 10, 1E-307);// tested to 1E-324, minimum value of a float is ~ 2E-308
                sugestionBox.Suggestions = values;
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
                    var settingsMenu = new SettingsMenu(sugestionBox,trie);
                    settingsMenu.Show();
                    foreach (int i in settingsMenu.getHandlesToIgnore())
                    {
                        appHandler.ignorehandles.Add(i);
                    }
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
