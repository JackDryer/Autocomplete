using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Automation.Text;
using Word = Microsoft.Office.Interop.Word;
namespace Autocomplete.API
{
    internal class WordInterface
    {
        private Word.Application objWord;
        public WindowsInterface windowsInterface;
        public void Insert(string word, TextPatternRange range)
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

            public TextPatternRange GetActiveWord()
        {
            var range = GetCurrentWord();
            int left, top, width, height;

            objWord.ActiveWindow.GetPoint(out left, out top, out width, out height, range);
            // Calculate the bounding box for the range
            System.Windows.Rect boundingBox = new System.Windows.Rect(left, top, width, height);
            Console.WriteLine($"shape: {boundingBox.BottomLeft.X},{boundingBox.BottomLeft.Y}");
            var other = windowsInterface.GetActiveWord();
            var loc = other.GetBoundingRectangles()[0].BottomLeft;
            Console.WriteLine($"UI API: {loc.X},{loc.Y}");
            return null;
        }
    }
}