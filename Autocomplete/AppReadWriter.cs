using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        private ActiveApplicationLike activeApplicationLike;

        private LowLevelKeyBoardListener lowListener;
        private WindowsInterface windowsInterface;

        public event EventHandler OnTextChange;

        public event EventHandler<string> OnUneditableWindow;
        public event EventHandler OnAppChange { add => Listener.OnAppChange+=value; remove => Listener.OnAppChange -=value; }
        public HashSet<int> ignorehandles { get => Listener.ignorehandles; set => Listener.ignorehandles =value; }
        public void ReplaceWord(string text, TextPatternRange rangeToReplace)
        {
            switch (activeApplicationLike)
            {
                case ActiveApplicationLike.Word:
                    objWord.Selection.InsertAfter(text);
                    break;
                case ActiveApplicationLike.Notepad:
                    windowsInterface.ReplaceWord(text, rangeToReplace);
                    break;
            }
        }
        public AppReadWriter()
        {
            lowListener = new LowLevelKeyBoardListener();
            lowListener.OnKeyPressed += LowListener_OnKeyPressed;
            lowListener.HookKeyboard();
            Listener = new ApplicationListener();
            Listener.OnAppChange += Listener_OnAppChange;

            windowsInterface.OnTextChange += WindowsInterface_OnTextChange;
        }

        private void Listener_OnAppChange(object sender, EventArgs e)
        {

            if (Process.GetProcessById(Listener.GetProcessId()).ProcessName == "WINWORD")
            {
                return ActiveApplicationLike.Word;
            }
            else
            {
                return ActiveApplicationLike.Notepad;
            }
        }

        private void WindowsInterface_OnTextChange(object sender, string e)
        {
            if (activeApplicationLike == ActiveApplicationLike.Notepad)
            {
                OnTextChange?.Invoke(this, new EventArgs());
            }
        }

        private void LowListener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            if (activeApplicationLike == ActiveApplicationLike.Word)
            {
                OnTextChange?.Invoke(this, new EventArgs());
            }
        }
        public TextPatternRange GetActiveWord()
        {
            return windowsInterface.GetActiveWord();
        }
    }
}
