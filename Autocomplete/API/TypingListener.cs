using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocomplete
{
    internal class TypingListener
    {
        private LowLevelKeyBoardListener lowLevel;
        private string currentWord;

        public event EventHandler OnTextChange;
        public TypingListener()
        {
            lowLevel = new LowLevelKeyBoardListener();
            lowLevel.OnKeyPressed += LowLevel_OnKeyPressed;
            lowLevel.HookKeyboard();
        }

        private void LowLevel_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            if (e.ToString() == " ")
            {
                currentWord = "";
            }
            else if (e.ToString().Length == 1)
            {
                currentWord+=e.ToString();
            }
            OnTextChange?.Invoke(this, e);
        }

        public string GetActiveWord()
        {
            return currentWord;
        }
        public void Unlatch()
        {
            OnTextChange = null;
        }
    }
}
