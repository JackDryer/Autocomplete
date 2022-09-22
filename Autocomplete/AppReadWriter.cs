using System;
using System.Collections.Generic;
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
        private Word.Application objWord;
        private ActiveApplicationLike activeApplicationLike;

        private LowLevelKeyBoardListener lowListener;
        private WindowsInterface windowsInterface;

        public event EventHandler OnTextChange;
        public event EventHandler<string> OnUneditableWindow;
        public event EventHandler OnAppChange { add => windowsInterface.OnAppChange+=value; remove => windowsInterface.OnAppChange -=value; }

        public HashSet<int> ignorehandles { get => windowsInterface.ignorehandles; set => windowsInterface.ignorehandles =value; }
        public void ReplaceWord(string text, TextPatternRange rangeToReplace)
        {
            switch (activeApplicationLike)
            {
                case ActiveApplicationLike.Word:
                    objWord.Selection.InsertAfter(text);
                    break;
                case ActiveApplicationLike.Notepad:

                    break;
            }
        }
        public AppReadWriter()
        {
            lowListener = new LowLevelKeyBoardListener();
            lowListener.OnKeyPressed += LowListener_OnKeyPressed;
            lowListener.HookKeyboard();
            windowsInterface = new WindowsInterface();
            windowsInterface.OnTextChange += WindowsInterface_OnTextChange;
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
