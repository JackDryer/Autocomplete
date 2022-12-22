using System;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;
using System.Drawing;

namespace Autocomplete
{
    public partial class SettingsMenu : Form
    {
        public SettingsMenu()
        {
            InitializeComponent();
            //colorDialog1.ShowDialog();
        }
        public event EventHandler AppliedSettings;

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            AppliedSettings?.Invoke(this, EventArgs.Empty);


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

        private void SettingsMenu_Load(object sender, EventArgs e)
        {
            string fileName = "settings.json";
            string jsonString = File.ReadAllText(fileName);
            Settings settings = JsonSerializer.Deserialize<Settings>(jsonString);
            TextSizeupdown.Value =settings.textSize;
            buttonTextColour.BackColor= Color.FromArgb(settings.textColour);
            buttonBgColour.BackColor= Color.FromArgb(settings.backgroundColour);
            buttonhighlightColour.BackColor= Color.FromArgb(settings.highlightColour);
            buttonHiglightBgColour.BackColor= Color.FromArgb(settings.highlightBackgroundColour);
        }
    }

}





