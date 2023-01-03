using System;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.CodeDom;
using Microsoft.Office.Interop.Word;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Automation;
using Autocomplete.Properties;
using System.Runtime;

namespace Autocomplete
{
    public partial class SettingsMenu : Form
    {
        DropDown mainDropDown;
        Trie mainTrie;
        Trie exampleTrie;
        public SettingsMenu(DropDown dropDown, Trie trie)
        {
            InitializeComponent();
            this.mainDropDown = dropDown;
            this.mainTrie = trie;
            exampleTrie = Trie.LoadFromFile();
            exampleOutBox.Suggestions = exampleTrie.GetCompletions("", 10, 1E-307);
            string fileName = "settings.json";
            string jsonString = File.ReadAllText(fileName);
            Settings settings = JsonSerializer.Deserialize<Settings>(jsonString);
            SettingsConfiguration = settings;
            exampleOutBox.ApplySettings(SettingsConfiguration);
            exampleOutBox.WhenClicked += ExampleOutBox_WhenClick;

            //colorDialog1.ShowDialog();
        }

        private void ExampleOutBox_WhenClick(object sender, EventArgs e)
        {
            inputBox.Text = exampleOutBox.MouseSelection;
            inputBox.SelectionStart = inputBox.Text.Length;
        }

        const int expSliderMax = 60000, expSliderMin = 1, baseNumber = 1000;
        public int logSliderFrequncy // frequency slider is between 0 1nd 100
        {
            get
            {
                double scaleTransformed = (double)this.frequncySlider.Value / (double)this.frequncySlider.Maximum;
                double expValue = Math.Pow(baseNumber, scaleTransformed);

                // transforming to be in the bound of the slider
                double untransformedExpMin = 1;// x^1 = x and x^0 = 1

                return (int)TransformPointInRange(expValue, untransformedExpMin, baseNumber, expSliderMin, expSliderMax);
            }
            set
            {
                double expValue = TransformPointInRange(value, expSliderMin, expSliderMax, 1, baseNumber);
                double scaleTransformed = Math.Log(expValue, baseNumber);
                frequncySlider.Value = (int)(scaleTransformed * frequncySlider.Maximum);
            }
        }
        public double TransformPointInRange(double value, double originalMin, double originalMax, double newMin, double newMax)
        {
            double originalRange = originalMax - originalMin;
            double newRange = newMax - newMin;
            return ((value - originalMin) / originalRange * newRange) + newMin;
        }
        private void ApplySettings()
        {
            exampleOutBox.ApplySettings(SettingsConfiguration);
            SaveSettings();
            mainDropDown.LoadSettings();
        }
        private Settings SettingsConfiguration
        {
            get
            {
                return new Settings
                {
                    textSize = TextSizeupdown.Value,
                    textColour = buttonTextColour.BackColor.ToArgb(),
                    backgroundColour = buttonBgColour.BackColor.ToArgb(),
                    highlightColour = buttonhighlightColour.BackColor.ToArgb(),
                    highlightBackgroundColour = buttonHiglightBgColour.BackColor.ToArgb()
                };
            }
            set
            {
                TextSizeupdown.ValueChanged -= TextSizeupdown_ValueChanged; // so that the settings not get set accidentally
                TextSizeupdown.Value = value.textSize;
                TextSizeupdown.ValueChanged += TextSizeupdown_ValueChanged;
                buttonTextColour.BackColor = Color.FromArgb(value.textColour);
                buttonBgColour.BackColor = Color.FromArgb(value.backgroundColour);
                buttonhighlightColour.BackColor = Color.FromArgb(value.highlightColour);
                buttonHiglightBgColour.BackColor = Color.FromArgb(value.highlightBackgroundColour);
            }
        }
        private void SaveSettings()
        {
            string fileName = "settings.json";
            string jsonString = JsonSerializer.Serialize(SettingsConfiguration);

            //File.WriteAllText(fileName, jsonString);
        }

        private void TextSizeupdown_ValueChanged(object sender, EventArgs e)
        {
            ApplySettings();
        }
        //taken form https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.colordialog?view=windowsdesktop-7.0
        private void buttonTextColour_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = buttonTextColour.BackColor;
            // Update the text box color and the exampleBox if the user clicks OK 
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                buttonTextColour.BackColor = colorDialog1.Color;
                ApplySettings();
            }
        }

        private void buttonBgColour_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = buttonBgColour.BackColor;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                buttonBgColour.BackColor = colorDialog1.Color;
                ApplySettings();
            }
        }

        private void buttonhighlightColour_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = buttonhighlightColour.BackColor;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                buttonhighlightColour.BackColor = colorDialog1.Color;
                ApplySettings();
            }
    }

        private void buttonHiglightBgColour_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = buttonHiglightBgColour.BackColor;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                buttonHiglightBgColour.BackColor = colorDialog1.Color;
                ApplySettings();
            }
        }
        private void frequncySlider_MouseHover(object sender, EventArgs e)
        {
            frequncyToolTip.SetToolTip(frequncySlider, logSliderFrequncy.ToString());
        }
        private void frequncySlider_Scroll(object sender, EventArgs e)
        {
            frequncySlider_MouseHover(sender, e);
            exampleTrie.UpdateFrequency(searchBox.Text, logSliderFrequncy);
            inputBox_TextChanged(sender, e);
            //Console.WriteLine(exampleTrie.GetFrequency(searchBox.Text));

        }

        private void inputBox_TextChanged(object sender, EventArgs e)
        {
            exampleOutBox.Suggestions = exampleTrie.GetCompletions(inputBox.Text, 10, 1E-307);
        }

        private void inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    exampleOutBox.moveSelecionUp(sender, e);
                    break;
                case Keys.Down:
                    exampleOutBox.moveSelecionDown(sender, e);
                    break;
                case Keys.Tab:
                    inputBox.Text = exampleOutBox.Selection;
                    inputBox.SelectionStart = inputBox.Text.Length;
                    e.SuppressKeyPress = true;
                    break;
            }
        }                    

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (mainTrie.Contains(searchBox.Text))
            {
                logSliderFrequncy = (int)mainTrie.GetFrequency(searchBox.Text);
                setWordOptionsUpdate();
                lblWordPressent.Text = "Word is present in wordlist";
                exampleTrie.UpdateFrequency(searchBox.Text, logSliderFrequncy);
            }
            else
            {
                logSliderFrequncy = 1;
                setWordOptionsAdd();
                lblWordPressent.Text = "Word was not found in wordlist";
                exampleTrie.Add(searchBox.Text, logSliderFrequncy);
            }
            inputBox_TextChanged(sender, e);
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSearch_Click(sender, e);
                e.SuppressKeyPress = true;
            }
            else 
                setWordOptionsDissabled();
        }

        private void buttonUpadate_Click(object sender, EventArgs e)
        {
            if (mainTrie.Contains(searchBox.Text))
            {
                mainTrie.UpdateFrequency(searchBox.Text, logSliderFrequncy);

                // replace the text in the file
                string text = File.ReadAllText(MainProgram.pathToWordList);

                var pattern = $"(^{searchBox.Text}:)(\\d+)";
                Regex rgx = new Regex(pattern, RegexOptions.Multiline);
                var replacePattern = $"{searchBox.Text}:{logSliderFrequncy}";//$1 references the first group
                text = rgx.Replace(text, replacePattern,1);

                File.WriteAllText(MainProgram.pathToWordList, text);
                lblWordPressent.Text = "Word Updated";
            }
            else
            {

                mainTrie.Add(searchBox.Text, logSliderFrequncy);
                // replace the text in the file
                File.AppendAllText(MainProgram.pathToWordList, $"\n{searchBox.Text}:{logSliderFrequncy}");
                lblWordPressent.Text = "Word Added";
                setWordOptionsUpdate();
            }

        }
        public int[] getHandlesToIgnore()
        {
            int[] handles = { (int)inputBox.Handle, (int)searchBox.Handle, exampleOutBox.GetHandle() };
            return handles;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            mainTrie.Delete(searchBox.Text);
            exampleTrie.UpdateFrequency(searchBox.Text,1);
            logSliderFrequncy = 1;
            inputBox_TextChanged(sender, e);//reload the suggestions


            // replace the text in the file
            string text = File.ReadAllText(MainProgram.pathToWordList);

            var pattern = $"^({searchBox.Text}:)(\\d+)\\n?";
            Regex rgx = new Regex(pattern,RegexOptions.Multiline);
            var replacePattern = "";//$1 references the first group
            text = rgx.Replace(text, replacePattern).Trim( '\r', '\n' );;

            File.WriteAllText(MainProgram.pathToWordList, text);
            lblWordPressent.Text = "Word Deleted";
            setWordOptionsAdd();
        }

        private void setWordOptionsDissabled()
        {
            frequncySlider.Enabled= false;
            buttonUpadate.Enabled= false;
            buttonUpadate.Text = "Update";
            buttonDelete.Enabled= false;
            lblWordPressent.Text= "";
        }

        private void exampleOutBox_Enter(object sender, EventArgs e)
        {
            inputBox.Focus();
        }

        private void inputBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;// so it doesn't switch focus for this particular key
            }
        }

        private void setWordOptionsUpdate()
        {
            frequncySlider.Enabled = true;
            buttonUpadate.Enabled = true;
            buttonUpadate.Text = "Update";
            buttonDelete.Enabled = true;
        }
        private void setWordOptionsAdd()
        {
            frequncySlider.Enabled = true;
            buttonUpadate.Enabled = true;
            buttonUpadate.Text = "Add";
            buttonDelete.Enabled = false;
        }
    }

}





