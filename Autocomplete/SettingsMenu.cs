using System;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.CodeDom;

namespace Autocomplete
{
    public partial class SettingsMenu : Form
    {
        DropDown mainDropDown;
        Trie mainTrie;
        DropDown exampleDropDown;
        public SettingsMenu(DropDown dropDown, Trie trie)
        {
            InitializeComponent();
            this.mainDropDown = dropDown;
            this.mainTrie = trie;

            //colorDialog1.ShowDialog();
        }
        const int expSliderMax = 50000, expSliderMin = 1, baseNumber = 1000;
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
        private void ApplyButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            mainDropDown.LoadSettings();


        }
        private void SaveSettings()
        {
            var settings = new Settings
            {
                textSize = TextSizeupdown.Value,
                textColour = buttonTextColour.BackColor.ToArgb(),
                backgroundColour = buttonBgColour.BackColor.ToArgb(),
                highlightColour = buttonhighlightColour.BackColor.ToArgb(),
                highlightBackgroundColour = buttonHiglightBgColour.BackColor.ToArgb()
            };
            string fileName = "settings.json";
            string jsonString = JsonSerializer.Serialize(settings);

            File.WriteAllText(fileName, jsonString);
        }
        //taken form https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.colordialog?view=windowsdesktop-7.0
        private void buttonTextColour_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = buttonTextColour.BackColor;

            // Update the text box color if the user clicks OK 
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                buttonTextColour.BackColor = colorDialog1.Color;
        }

        private void buttonBgColour_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = buttonBgColour.BackColor;

            // Update the text box color if the user clicks OK 
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                buttonBgColour.BackColor = colorDialog1.Color;
        }

        private void buttonhighlightColour_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = buttonhighlightColour.BackColor;

            // Update the text box color if the user clicks OK 
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                buttonhighlightColour.BackColor = colorDialog1.Color;
        }

        private void buttonHiglightBgColour_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = buttonHiglightBgColour.BackColor;

            // Update the text box color if the user clicks OK 
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                buttonHiglightBgColour.BackColor = colorDialog1.Color;
        }

        private void frequncySlider_Scroll(object sender, EventArgs e)
        {
            frequncyToolTip.SetToolTip(frequncySlider, logSliderFrequncy.ToString());
        }

        private void SettingsMenu_Load(object sender, EventArgs e)
        {
            string fileName = "settings.json";
            string jsonString = File.ReadAllText(fileName);
            Settings settings = JsonSerializer.Deserialize<Settings>(jsonString);
            TextSizeupdown.Value = settings.textSize;
            buttonTextColour.BackColor = Color.FromArgb(settings.textColour);
            buttonBgColour.BackColor = Color.FromArgb(settings.backgroundColour);
            buttonhighlightColour.BackColor = Color.FromArgb(settings.highlightColour);
            buttonHiglightBgColour.BackColor = Color.FromArgb(settings.highlightBackgroundColour);
        }
        public int[] getHandlesToIgnore()
        {
            int[] handles = { (int)inputBox.Handle, (int)searchBox.Handle };
            return handles;
        }
    }

}





