using Autocomplete.API;
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
        private WordInterface wordInterface;

        private TypingListener typingListener;
        private WindowsInterface windowsInterface;

        public event EventHandler OnTextChange;

        // these 3 events are to do with when the app focus changes, this is not the job of this class
        public event EventHandler<string> OnUneditableWindow { add => Listener.OnUneditableWindow += value; remove => Listener.OnUneditableWindow -= value; }
        public event EventHandler OnAppChange { add => Listener.OnAppChange+=value; remove => Listener.OnAppChange -=value; }
        public HashSet<int> ignorehandles { get => Listener.ignorehandles; set => Listener.ignorehandles =value; }

        public Func<TextInformation> GetActiveWord;
        public Action<string> Insert; //(string text, TextPatternRange rangeToReplace)

        public AppReadWriter()
        {
            windowsInterface = new WindowsInterface();
            typingListener = new TypingListener();
            Listener = new ApplicationListener();
            wordInterface= new WordInterface();
            wordInterface.windowsInterface = windowsInterface;
            Listener.OnAppChange += Listener_OnAppChange;
        }
        private void Listener_OnAppChange(object sender, EventArgs e)
        {
            typingListener.Unlatch();
            windowsInterface.Unlatch();
            if (Process.GetProcessById(Listener.GetProcessId()).ProcessName == "WINWORD")
            {
                // app is like word
                wordInterface.Latch();
                windowsInterface.Latch();
                this.GetActiveWord = windowsInterface.GetActiveWordInfo;//windowsInterface.GetActiveWordInfo;
                this.Insert = wordInterface.Insert;
                typingListener.OnTextChange += OnTextChange;// no need to latch as unlatch just removes all events
                
            }
            else
            {
                // app is like notepad
                windowsInterface.Latch();
                this.GetActiveWord = windowsInterface.GetActiveWordInfo;
                this.Insert = windowsInterface.InsertWord;
                windowsInterface.OnTextChange += OnTextChange;
            }
        }

    }
}
