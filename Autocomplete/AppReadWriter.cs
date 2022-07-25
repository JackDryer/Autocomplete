using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Automation;
using System.Diagnostics;
using System.Windows.Automation.Text;
using System.Runtime.InteropServices;

namespace Autocomplete
{
    public enum ReplaceType
    {
        None,
        Word,
        All
    }
    class AppReadWriter
    {
        private TextPattern textPattern;
        private AutomationElement activeWindow;
        public void writeWord(string text)
        {
            string before = textPattern.DocumentRange.GetText(-1).Trim();
            int start = before.LastIndexOfAny(" \n".ToCharArray());
            ReplaceTextUsingDll(activeWindow, start+1,start+text.Length,text);
        }
        public AppReadWriter()
        {
            activeWindow = null;
            AutomationFocusChangedEventHandler focusHandler = new AutomationFocusChangedEventHandler(OnFocusChange);
            Automation.AddAutomationFocusChangedEventHandler(focusHandler);
        }
        public event EventHandler<string> OnTextChange;
        public event EventHandler<string> OnUneditableWindow;
        private void OnFocusChange(object src,AutomationEventArgs e)
        {
            if (activeWindow != null)
            {
                Automation.RemoveAutomationEventHandler(TextPattern.TextSelectionChangedEvent, activeWindow, handleTextChange);
                activeWindow = null;
            }
            activeWindow = AutomationElement.FocusedElement;
            string className = activeWindow.Current.ClassName;
            object textob = null;
            if (activeWindow.TryGetCurrentPattern(TextPattern.Pattern, out textob))//Editables.Contains(className)
            {
                textPattern = activeWindow.GetCurrentPattern(TextPattern.Pattern) as TextPattern;
                Automation.AddAutomationEventHandler(TextPattern.TextSelectionChangedEvent, activeWindow, TreeScope.Element, handleTextChange);
            }
            else
            {
                OnUneditableWindow?.Invoke(this, className);
            }
        }
        private void handleTextChange(object src, AutomationEventArgs e)
        {
            string text = textPattern.DocumentRange.GetText(-1);
            OnTextChange?.Invoke(this, text);
        }
        const int WM_SETTEXT = 0x000C;

        [DllImport("User32.dll", CharSet = CharSet.Auto)]// SetLastError = true
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParamm, string lParam);
        private static void SetTextUingDll(AutomationElement handle, string text)
        {
            SendMessage(new IntPtr(handle.Current.NativeWindowHandle), WM_SETTEXT, IntPtr.Zero, text);

        }
        const int EM_REPLACESEL = 0x00C2;
        const int EM_SETSEL = 0x00B1;
        [DllImport("User32.dll", CharSet = CharSet.Auto)]// SetLastError = true
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParamm, IntPtr lParam);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]// SetLastError = true
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, bool wParamm, string lParam);
        private static void ReplaceTextUsingDll(AutomationElement handle, int start, int end, string text)
        {
            SendMessage(new IntPtr(handle.Current.NativeWindowHandle), EM_SETSEL, (IntPtr)start,(IntPtr)end);
            SendMessage(new IntPtr(handle.Current.NativeWindowHandle), EM_REPLACESEL, true, text);

        }
        public void InsertTextUsingUIAutomation(AutomationElement element,
                                    string value, ReplaceType replace = ReplaceType.None, bool popup = false)
        {
            try
            {
                // Validate arguments / initial setup
                if (value == null)
                    throw new ArgumentNullException(
                        "String parameter must not be null.");

                if (element == null)
                    throw new ArgumentNullException(
                        "AutomationElement parameter must not be null");

                // A series of basic checks prior to attempting an insertion.
                //
                // Check #1: Is control enabled?
                // An alternative to testing for static or read-only controls
                // is to filter using
                // PropertyCondition(AutomationElement.IsEnabledProperty, true)
                // and exclude all read-only text controls from the collection.
                if (!element.Current.IsEnabled)
                {
                    throw new InvalidOperationException(
                        "The control with an AutomationID of "
                        + element.Current.AutomationId.ToString()
                        + " is not enabled.\n\n");
                }

                // Check #2: Are there styles that prohibit us
                //           from sending text to this control?
                if (!element.Current.IsKeyboardFocusable)
                {
                    throw new InvalidOperationException(
                        "The control with an AutomationID of "
                        + element.Current.AutomationId.ToString()
                        + "is read-only.\n\n");
                }

                // Once you have an instance of an AutomationElement,
                // check if it supports the ValuePattern pattern.
                object valuePattern = null;

                // Control does not support the ValuePattern pattern
                // so use keyboard input to insert content.
                //
                // NOTE: Elements that support TextPattern
                //       do not support ValuePattern and TextPattern
                //       does not support setting the text of
                //       multi-line edit or document controls.
                //       For this reason, text input must be simulated
                //       using one of the following methods.
                //
                if (!element.TryGetCurrentPattern(
                    ValuePattern.Pattern, out valuePattern))
                {
                    if (popup)
                    {
                        MessageBox.Show("The control with an AutomationID of " + element.Current.AutomationId.ToString()
                            + " does not support ValuePattern."
                            + " Using keyboard input.\n");
                    }
                    // Set focus for input functionality and begin.
                    element.SetFocus();

                    switch (replace)
                    {
                        case ReplaceType.All:
                            // Delete existing content in the control and insert new content.
                            SendKeys.SendWait("^{HOME}");   // Move to start of control
                            SendKeys.SendWait("^+{END}");   // Select everything
                            SendKeys.SendWait("{DEL}");     // Delete selection
                            break;
                        case ReplaceType.Word:
                            // Delete existing content in the control and insert new content.
                            SendKeys.SendWait("^{LEFT}");   // Move to start of control
                            SendKeys.SendWait("^+{RIGHT}");   // Select everything
                            SendKeys.SendWait("{DEL}");     // Delete selection
                            break;
                    }
                    SendKeys.SendWait(value);
                }
                // Control supports the ValuePattern pattern so we can
                // use the SetValue method to insert content.
                else
                {
                    if (popup)
                    {
                        MessageBox.Show("The control with an AutomationID of "
                            + element.Current.AutomationId.ToString()
                            + " supports ValuePattern."
                            + " Using ValuePattern.SetValue().\n");
                    }
                    // Set focus for input functionality and begin.
                    element.SetFocus();
                    switch (replace)
                    {
                        case ReplaceType.All:
                            ((ValuePattern)valuePattern).SetValue(value);
                            break;
                        case ReplaceType.None:
                            ((ValuePattern)valuePattern).SetValue(textPattern.DocumentRange.GetText(-1)+value);
                            break;
                        case ReplaceType.Word:
                            string all = textPattern.DocumentRange.GetText(-1);
                            int end = all.Length - 1;
                            if (end == -1)
                                end = 0;
                            else
                            { 
                                while (end>0)
                                {
                                    end--;
                                    if (all[end] == ' ')
                                        break;
                                }
                            }
                            ((ValuePattern)valuePattern).SetValue(all.Substring(0,end)+ value);
                            break;

                    }
                    
                }
            }
            catch (ArgumentNullException exc)
            {
                MessageBox.Show(exc.Message);
            }
            catch (InvalidOperationException exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}
