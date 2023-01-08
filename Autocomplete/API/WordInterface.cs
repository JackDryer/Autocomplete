using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Text;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
namespace Autocomplete.API
{
    internal class WordInterface
    {
        private Word.Application objWord;
        public WindowsInterface windowsInterface;
        public void Insert(string word)
        {
            Thread thread = new Thread(() => {
                objWord.ScreenUpdating = false; // more astheticaly pleasing
                var rangetoreplace = GetCurrentWord();
                rangetoreplace.Text = word;
                objWord.Selection.Move(WdUnits.wdWord, 1);
                objWord.ScreenUpdating = true;

            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
        public void Latch()
        {
            objWord = Marshal.GetActiveObject("Word.Application") as Word.Application;
        }
        private Range GetCurrentWord()
        {
            //if (objWord.Selection.Start== objWord.Selection.End) // selection is 0 char wide
            var range = objWord.Selection.Previous();
            range.Expand(WdUnits.wdWord);
            if (range.Text.EndsWith(" "))
                range.MoveEnd(WdUnits.wdCharacter, -1);
            return range;
        }

       public TextInformation GetActiveWord()
        {
            var range = GetCurrentWord();
            int left, top, width, height;

            var scale = GetWindowsScaling();
            Console.WriteLine(scale);
            objWord.ActiveWindow.GetPoint(out left, out top, out width, out height, range);
            left = (int)(left/scale);
            top = (int)(top / scale);
            width = (int)(width / scale);
            height = (int)(height / scale);
            // Calculate the bounding box for the range
            System.Windows.Rect boundingBox = new System.Windows.Rect(left, top, width, height);

            return new TextInformation { text = range.Text, boundingBox = boundingBox };
        }
        public static double GetWindowsScaling() // this feature exists due to a bug in MS word when scalling.
        {
            return SystemParameters.PrimaryScreenWidth/ Screen.PrimaryScreen.Bounds.Width;
            //return (double)Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth;
        }
    }
}
namespace Autocomplete
{
    internal class TextInformation
    {
        public string text;
        public System.Windows.Rect boundingBox;
    }
}