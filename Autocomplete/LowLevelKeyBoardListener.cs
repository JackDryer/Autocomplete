using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Collections.Generic;


namespace Autocomplete
{
    public class LowLevelKeyBoardListener
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        public HashSet<string> blockKeys { get; set; }
        private Dictionary<string,EventHandler> namedEvents;
        public void AddEvent(string keyname, EventHandler func){
            if (namedEvents.ContainsKey(keyname))
            {
                namedEvents[keyname] += func;
            }
            else
            {
                namedEvents[keyname] = func;
            }
        }
        public void AddEventOnce(string keyname, EventHandler func)
        {
            if (!namedEvents.ContainsKey(keyname))
            {
                namedEvents[keyname] = func;
            }
        }
        public bool Hooked { get; private set;}
        public void RemoveEvent(string keyname) { namedEvents.Remove(keyname); }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public event EventHandler<KeyPressedArgs> OnKeyPressed;

        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        public LowLevelKeyBoardListener()
        {
            _proc = HookCallback;
            blockKeys = new HashSet<string>();
            namedEvents = new Dictionary<string, EventHandler>();
        }

        public void HookKeyboard()
        {
            if (!this.Hooked)
            {
                _hookID = SetHook(_proc);
                Hooked = true;
            }
        }

        public void UnHookKeyboard()
        {
            if (this.Hooked)
            {
                UnhookWindowsHookEx(_hookID);
                Hooked = false;
            }
        }
        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
            {
                var tempBlockKeys = new HashSet<string>(this.blockKeys); //cannot be modifed during this event call
                int vkCode = Marshal.ReadInt32(lParam);
                var keyob = KeyInterop.KeyFromVirtualKey(vkCode);
                OnKeyPressed?.Invoke(this, new KeyPressedArgs(keyob));

                if (namedEvents.ContainsKey(keyob.ToString()))
                    namedEvents[keyob.ToString()](this, new EventArgs());

                if (tempBlockKeys.Contains(keyob.ToString()))
                {
                    return (IntPtr)(-1);
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

    }
    public class KeyPressedArgs : EventArgs
    {
        public Key KeyPressed { get; private set; }

        public KeyPressedArgs(Key key)
        {
            KeyPressed = key;
        }
    }
}
