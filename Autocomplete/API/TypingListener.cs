using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            if (e.KeyPressed.ToString() == " ")
            {
                currentWord = "";
            }
            //else if (e.KeyPressed.ToString() == "Back")
            //{
            //    currentWord = currentWord.Substring(0, currentWord.Length - 1);
            //}
            else if (e.KeyPressed.ToString().Length == 1) 
            {
                currentWord+=e.KeyPressed.ToString();
            }
            Thread thread = new Thread(() => { // in new thread so that program the user is typing into can recive the character pressed before this program reads it
                Thread.Sleep(1);
                OnTextChange?.Invoke(this, e);

            });
            thread.Start();
            
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
