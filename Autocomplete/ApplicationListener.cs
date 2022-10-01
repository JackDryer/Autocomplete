using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
namespace Autocomplete
{
    internal class ApplicationListener
    {
        internal AutomationElement activeWindow;
        public HashSet<int> ignorehandles;

        public event EventHandler<string> OnUneditableWindow;
        public event EventHandler OnAppChange;
        public ApplicationListener()
        {
            activeWindow = null;
            ignorehandles = new HashSet<int>();
            AutomationFocusChangedEventHandler focusHandler = new AutomationFocusChangedEventHandler(OnFocusChange);
            Automation.AddAutomationFocusChangedEventHandler(focusHandler);
        }
        private void OnFocusChange(object src, AutomationEventArgs e)
        {
            //Console.WriteLine(AutomationElement.FocusedElement.Current.Name);
            //Console.WriteLine(AutomationElement.FocusedElement.Current.NativeWindowHandle);
            //Console.WriteLine(String.Join(",", ignorehandles));
            //Console.WriteLine(Process.GetProcessById(AutomationElement.FocusedElement.Current.ProcessId).ProcessName);
            if (ignorehandles.Contains(AutomationElement.FocusedElement.Current.NativeWindowHandle) || AutomationElement.FocusedElement == activeWindow)
            {
                return;
            }
            activeWindow = AutomationElement.FocusedElement;
            if (!activeWindow.TryGetCurrentPattern(TextPattern.Pattern, out object textob))//Editables.Contains(className)
            {
                string className = activeWindow.Current.ClassName;
                OnUneditableWindow?.Invoke(this, className);
            }
            OnAppChange?.Invoke(this, new EventArgs());
        }
        public int GetProcessId()
        { 
            return activeWindow.Current.ProcessId;
        }

    }
}
