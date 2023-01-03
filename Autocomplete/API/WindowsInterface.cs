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
    class WindowsInterface : ApplicationListener
    {
        private TextPattern textPattern;

        public void ReplaceWord(string text,TextPatternRange rangeToReplace)
        {
            int start = GetRangeIndex(rangeToReplace);
            int end = start + rangeToReplace.GetText(-1).Length;
            ReplaceTextUsingDll(activeWindow, start, end, text);
        }
        public int GetRangeIndex(TextPatternRange range)
        {
            return range.CompareEndpoints(TextPatternRangeEndpoint.Start, textPattern.DocumentRange, TextPatternRangeEndpoint.Start);
        }

        public event EventHandler OnTextChange;
        public void Unlatch()
        {
            if (activeWindow != null)
            {
                Automation.RemoveAutomationEventHandler(TextPattern.TextSelectionChangedEvent, activeWindow, handleTextChange);
                activeWindow = null;
            }
            OnTextChange = null;
        }
        public void Latch()
        {
            if (activeWindow != null)
            {
                throw new Exception("Not unlatched");
            }
            activeWindow = AutomationElement.FocusedElement;
            string className = activeWindow.Current.ClassName;
            object textob = null;
            if (activeWindow.TryGetCurrentPattern(TextPattern.Pattern, out textob))//Editables.Contains(className)
            {
                textPattern = activeWindow.GetCurrentPattern(TextPattern.Pattern) as TextPattern;
                Automation.AddAutomationEventHandler(TextPattern.TextSelectionChangedEvent, activeWindow, TreeScope.Element, handleTextChange);
            }
        }
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetGUIThreadInfo(uint hTreadID, ref GUITHREADINFO lpgui);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int iLeft;
            public int iTop;
            public int iRight;
            public int iBottom;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct GUITHREADINFO
        {
        public int cbSize;
        public int flags;
        public IntPtr hwndActive;
        public IntPtr hwndFocus;
        public IntPtr hwndCapture;
        public IntPtr hwndMenuOwner;
        public IntPtr hwndMoveSize;
        public IntPtr hwndCaret;
        public RECT rectCaret;
        }
        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point position);
        public Point getCaretPositon() //loosely based on https://www.codeproject.com/Articles/34520/Getting-Caret-Position-Inside-Any-Application
        {
            var guiInfo = new GUITHREADINFO();
            guiInfo.cbSize = Marshal.SizeOf(guiInfo);
            
            // Get GuiThreadInfo into guiInfo
            GetGUIThreadInfo(0, ref guiInfo);
            var caretPosition = new Point();
            caretPosition.X = (int)guiInfo.rectCaret.iLeft;// + 25;
            caretPosition.Y = (int)guiInfo.rectCaret.iBottom; // + 25;
            ClientToScreen(guiInfo.hwndCaret, ref caretPosition);
            return caretPosition;
        }

        public TextPatternRange GetActiveWord()
        {
            try
            {
                var p = getCaretPositon();
                var range = textPattern.RangeFromPoint(new System.Windows.Point((int)p.X, (int)p.Y - 5));
                range.ExpandToEnclosingUnit(TextUnit.Word);
                if (!range.GetText(-1).Any(x => char.IsLetter(x)))
                {
                    range.Move(TextUnit.Character, -1);
                    range.ExpandToEnclosingUnit(TextUnit.Word);
                }
                //range.Select();
                return range;
            }
            catch (System.ArgumentException) { 
                return null; }
            catch (COMException)
            {
                return null;
            }
            catch (ElementNotAvailableException)
            { return null; }
        }
        private void handleTextChange(object src, AutomationEventArgs e)
        {
            string text = textPattern.DocumentRange.GetText(-1);
            OnTextChange?.Invoke(this, new EventArgs());
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
