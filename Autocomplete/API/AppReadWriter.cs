using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation.Text;
using Word = Microsoft.Office.Interop.Word;

namespace Autocomplete
{
    public enum ActiveApplicationLike
    {
        Notepad,
        Word
    }
    internal class AppReadWriter
    {
        private ApplicationListener Listener;
        private Word.Application objWord;

        private TypingListener typingListener;
        private WindowsInterface windowsInterface;

        public event EventHandler OnTextChange;

        // these 3 events are to do with when the app focus changes, this is not the job of this class
        public event EventHandler<string> OnUneditableWindow { add => Listener.OnUneditableWindow += value; remove => Listener.OnUneditableWindow -= value; }
        public event EventHandler OnAppChange { add => Listener.OnAppChange+=value; remove => Listener.OnAppChange -=value; }
        public HashSet<int> ignorehandles { get => Listener.ignorehandles; set => Listener.ignorehandles =value; }

        public Func<TextPatternRange> GetActiveWord;
        public Action<string, TextPatternRange> ReplaceWord; //(string text, TextPatternRange rangeToReplace)
        public Action FocusActiveWindow;
        public AppReadWriter()
        {
            windowsInterface = new WindowsInterface();
            typingListener = new TypingListener();
            Listener = new ApplicationListener();
            Listener.OnAppChange += Listener_OnAppChange;
            FocusActiveWindow = Listener.FocusActiveWindow;
        }

        private void InsertWindows(string word, TextPatternRange range)
        {
            Thread thread = new Thread(() => {
                objWord.ScreenUpdating = false; // more astheticaly pleasing
                //if (objWord.Selection.Start== objWord.Selection.End)
                var rangetoreplace = objWord.Selection.Previous();
                rangetoreplace.Expand(WdUnits.wdWord);
                if (rangetoreplace.Text.EndsWith(" "))
                    rangetoreplace.MoveEnd(WdUnits.wdCharacter,-1);
                rangetoreplace.Text = word;
                objWord.Selection.Move(WdUnits.wdWord, 1);
                objWord.ScreenUpdating = true;

            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
        private void Listener_OnAppChange(object sender, EventArgs e)
        {
            typingListener.Unlatch();
            windowsInterface.Unlatch();
            if (Process.GetProcessById(Listener.GetProcessId()).ProcessName == "WINWORD")
            {
                // app is like word
                windowsInterface.Latch();
                objWord = Marshal.GetActiveObject("Word.Application") as Word.Application; //equivilent to latch
                this.GetActiveWord = windowsInterface.GetActiveWord;
                this.ReplaceWord = InsertWindows;
                typingListener.OnTextChange += OnTextChange;
                
            }
            else
            {
                // app is like notepad
                windowsInterface.Latch();
                this.GetActiveWord = windowsInterface.GetActiveWord;
                this.ReplaceWord = windowsInterface.ReplaceWord;
                windowsInterface.OnTextChange += OnTextChange;
            }
        }

    }
}
