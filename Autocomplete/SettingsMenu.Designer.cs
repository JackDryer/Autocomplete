namespace Autocomplete
{
    partial class SettingsMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsMenu));
            this.TextSizeupdown = new System.Windows.Forms.NumericUpDown();
            this.labelTxtSize = new System.Windows.Forms.Label();
            this.labelTxtColour = new System.Windows.Forms.Label();
            this.labelBgColour = new System.Windows.Forms.Label();
            this.labelHighlightColour = new System.Windows.Forms.Label();
            this.labelHighlightBgColour = new System.Windows.Forms.Label();
            this.buttonTextColour = new System.Windows.Forms.Button();
            this.buttonBgColour = new System.Windows.Forms.Button();
            this.buttonhighlightColour = new System.Windows.Forms.Button();
            this.buttonHiglightBgColour = new System.Windows.Forms.Button();
            this.inputBox = new System.Windows.Forms.TextBox();
            this.labelAddupdate = new System.Windows.Forms.Label();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.labelFrequency = new System.Windows.Forms.Label();
            this.frequncySlider = new System.Windows.Forms.TrackBar();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.buttonUpadate = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.lableCommon = new System.Windows.Forms.Label();
            this.lableRare = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.frequncyToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelFrequencyUnits = new System.Windows.Forms.Label();
            this.lblWordPressent = new System.Windows.Forms.Label();
            this.exampleOutBox = new Autocomplete.HighlightedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.TextSizeupdown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequncySlider)).BeginInit();
            this.SuspendLayout();
            // 
            // TextSizeupdown
            // 
            this.TextSizeupdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextSizeupdown.Location = new System.Drawing.Point(614, 28);
            this.TextSizeupdown.Name = "TextSizeupdown";
            this.TextSizeupdown.Size = new System.Drawing.Size(46, 26);
            this.TextSizeupdown.TabIndex = 6;
            this.TextSizeupdown.ValueChanged += new System.EventHandler(this.TextSizeupdown_ValueChanged);
            // 
            // labelTxtSize
            // 
            this.labelTxtSize.AutoSize = true;
            this.labelTxtSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTxtSize.Location = new System.Drawing.Point(530, 30);
            this.labelTxtSize.Name = "labelTxtSize";
            this.labelTxtSize.Size = new System.Drawing.Size(78, 20);
            this.labelTxtSize.TabIndex = 2;
            this.labelTxtSize.Text = "Text Size:";
            // 
            // labelTxtColour
            // 
            this.labelTxtColour.AutoSize = true;
            this.labelTxtColour.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTxtColour.Location = new System.Drawing.Point(530, 63);
            this.labelTxtColour.Name = "labelTxtColour";
            this.labelTxtColour.Size = new System.Drawing.Size(93, 20);
            this.labelTxtColour.TabIndex = 3;
            this.labelTxtColour.Text = "Text Colour:";
            // 
            // labelBgColour
            // 
            this.labelBgColour.AutoSize = true;
            this.labelBgColour.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBgColour.Location = new System.Drawing.Point(530, 97);
            this.labelBgColour.Name = "labelBgColour";
            this.labelBgColour.Size = new System.Drawing.Size(149, 20);
            this.labelBgColour.TabIndex = 4;
            this.labelBgColour.Text = "Background Colour:";
            // 
            // labelHighlightColour
            // 
            this.labelHighlightColour.AutoSize = true;
            this.labelHighlightColour.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHighlightColour.Location = new System.Drawing.Point(530, 132);
            this.labelHighlightColour.Name = "labelHighlightColour";
            this.labelHighlightColour.Size = new System.Drawing.Size(159, 20);
            this.labelHighlightColour.TabIndex = 5;
            this.labelHighlightColour.Text = "Highlight Text Colour:";
            // 
            // labelHighlightBgColour
            // 
            this.labelHighlightBgColour.AutoSize = true;
            this.labelHighlightBgColour.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHighlightBgColour.Location = new System.Drawing.Point(530, 165);
            this.labelHighlightBgColour.Name = "labelHighlightBgColour";
            this.labelHighlightBgColour.Size = new System.Drawing.Size(215, 20);
            this.labelHighlightBgColour.TabIndex = 6;
            this.labelHighlightBgColour.Text = "Highlight Background Colour:";
            // 
            // buttonTextColour
            // 
            this.buttonTextColour.BackColor = System.Drawing.Color.Black;
            this.buttonTextColour.Location = new System.Drawing.Point(627, 60);
            this.buttonTextColour.Name = "buttonTextColour";
            this.buttonTextColour.Size = new System.Drawing.Size(28, 28);
            this.buttonTextColour.TabIndex = 7;
            this.buttonTextColour.UseVisualStyleBackColor = false;
            this.buttonTextColour.Click += new System.EventHandler(this.buttonTextColour_Click);
            // 
            // buttonBgColour
            // 
            this.buttonBgColour.BackColor = System.Drawing.Color.Black;
            this.buttonBgColour.Location = new System.Drawing.Point(685, 94);
            this.buttonBgColour.Name = "buttonBgColour";
            this.buttonBgColour.Size = new System.Drawing.Size(28, 28);
            this.buttonBgColour.TabIndex = 8;
            this.buttonBgColour.UseVisualStyleBackColor = false;
            this.buttonBgColour.Click += new System.EventHandler(this.buttonBgColour_Click);
            // 
            // buttonhighlightColour
            // 
            this.buttonhighlightColour.BackColor = System.Drawing.Color.Black;
            this.buttonhighlightColour.Location = new System.Drawing.Point(695, 129);
            this.buttonhighlightColour.Name = "buttonhighlightColour";
            this.buttonhighlightColour.Size = new System.Drawing.Size(28, 28);
            this.buttonhighlightColour.TabIndex = 9;
            this.buttonhighlightColour.UseVisualStyleBackColor = false;
            this.buttonhighlightColour.Click += new System.EventHandler(this.buttonhighlightColour_Click);
            // 
            // buttonHiglightBgColour
            // 
            this.buttonHiglightBgColour.BackColor = System.Drawing.Color.Black;
            this.buttonHiglightBgColour.Location = new System.Drawing.Point(751, 165);
            this.buttonHiglightBgColour.Name = "buttonHiglightBgColour";
            this.buttonHiglightBgColour.Size = new System.Drawing.Size(28, 28);
            this.buttonHiglightBgColour.TabIndex = 10;
            this.buttonHiglightBgColour.UseVisualStyleBackColor = false;
            this.buttonHiglightBgColour.Click += new System.EventHandler(this.buttonHiglightBgColour_Click);
            // 
            // inputBox
            // 
            this.inputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputBox.Location = new System.Drawing.Point(12, 12);
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(228, 26);
            this.inputBox.TabIndex = 1;
            this.inputBox.TextChanged += new System.EventHandler(this.inputBox_TextChanged);
            this.inputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputBox_KeyDown);
            this.inputBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.inputBox_PreviewKeyDown);
            // 
            // labelAddupdate
            // 
            this.labelAddupdate.AutoSize = true;
            this.labelAddupdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAddupdate.Location = new System.Drawing.Point(23, 298);
            this.labelAddupdate.Name = "labelAddupdate";
            this.labelAddupdate.Size = new System.Drawing.Size(137, 20);
            this.labelAddupdate.TabIndex = 12;
            this.labelAddupdate.Text = "Add/Update word:";
            // 
            // searchBox
            // 
            this.searchBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchBox.Location = new System.Drawing.Point(176, 295);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(177, 26);
            this.searchBox.TabIndex = 11;
            this.searchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchBox_KeyDown);
            // 
            // labelFrequency
            // 
            this.labelFrequency.AutoSize = true;
            this.labelFrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFrequency.Location = new System.Drawing.Point(23, 335);
            this.labelFrequency.Name = "labelFrequency";
            this.labelFrequency.Size = new System.Drawing.Size(88, 20);
            this.labelFrequency.TabIndex = 14;
            this.labelFrequency.Text = "Frequency:";
            // 
            // frequncySlider
            // 
            this.frequncySlider.Enabled = false;
            this.frequncySlider.LargeChange = 10;
            this.frequncySlider.Location = new System.Drawing.Point(176, 335);
            this.frequncySlider.Maximum = 100;
            this.frequncySlider.Name = "frequncySlider";
            this.frequncySlider.Size = new System.Drawing.Size(177, 45);
            this.frequncySlider.TabIndex = 13;
            this.frequncySlider.TickFrequency = 0;
            this.frequncySlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.frequncySlider.Scroll += new System.EventHandler(this.frequncySlider_Scroll);
            this.frequncySlider.MouseHover += new System.EventHandler(this.frequncySlider_MouseHover);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSearch.Location = new System.Drawing.Point(36, 377);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 28);
            this.buttonSearch.TabIndex = 12;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // buttonUpadate
            // 
            this.buttonUpadate.Enabled = false;
            this.buttonUpadate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUpadate.Location = new System.Drawing.Point(117, 377);
            this.buttonUpadate.Name = "buttonUpadate";
            this.buttonUpadate.Size = new System.Drawing.Size(75, 28);
            this.buttonUpadate.TabIndex = 14;
            this.buttonUpadate.Text = "Update";
            this.buttonUpadate.UseVisualStyleBackColor = true;
            this.buttonUpadate.Click += new System.EventHandler(this.buttonUpadate_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Enabled = false;
            this.buttonDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDelete.Location = new System.Drawing.Point(198, 377);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 28);
            this.buttonDelete.TabIndex = 15;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // lableCommon
            // 
            this.lableCommon.AutoSize = true;
            this.lableCommon.BackColor = System.Drawing.Color.Transparent;
            this.lableCommon.Location = new System.Drawing.Point(350, 351);
            this.lableCommon.Name = "lableCommon";
            this.lableCommon.Size = new System.Drawing.Size(48, 13);
            this.lableCommon.TabIndex = 19;
            this.lableCommon.Text = "Common";
            // 
            // lableRare
            // 
            this.lableRare.AutoSize = true;
            this.lableRare.BackColor = System.Drawing.Color.Transparent;
            this.lableRare.Location = new System.Drawing.Point(152, 351);
            this.lableRare.Name = "lableRare";
            this.lableRare.Size = new System.Drawing.Size(30, 13);
            this.lableRare.TabIndex = 20;
            this.lableRare.Text = "Rare";
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            // 
            // labelFrequencyUnits
            // 
            this.labelFrequencyUnits.AutoSize = true;
            this.labelFrequencyUnits.BackColor = System.Drawing.Color.Transparent;
            this.labelFrequencyUnits.Location = new System.Drawing.Point(24, 355);
            this.labelFrequencyUnits.Name = "labelFrequencyUnits";
            this.labelFrequencyUnits.Size = new System.Drawing.Size(123, 13);
            this.labelFrequencyUnits.TabIndex = 23;
            this.labelFrequencyUnits.Text = "(Occurrences per million)";
            // 
            // lblWordPressent
            // 
            this.lblWordPressent.AutoSize = true;
            this.lblWordPressent.Location = new System.Drawing.Point(280, 387);
            this.lblWordPressent.Name = "lblWordPressent";
            this.lblWordPressent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblWordPressent.Size = new System.Drawing.Size(0, 13);
            this.lblWordPressent.TabIndex = 25;
            // 
            // exampleOutBox
            // 
            this.exampleOutBox.Location = new System.Drawing.Point(12, 45);
            this.exampleOutBox.Name = "exampleOutBox";
            this.exampleOutBox.Size = new System.Drawing.Size(261, 131);
            this.exampleOutBox.Suggestions = ((System.Collections.Generic.List<string>)(resources.GetObject("exampleOutBox.Suggestions")));
            this.exampleOutBox.TabIndex = 24;
            this.exampleOutBox.Enter += new System.EventHandler(this.exampleOutBox_Enter);
            // 
            // SettingsMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblWordPressent);
            this.Controls.Add(this.exampleOutBox);
            this.Controls.Add(this.labelFrequencyUnits);
            this.Controls.Add(this.lableCommon);
            this.Controls.Add(this.lableRare);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonUpadate);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.labelFrequency);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.labelAddupdate);
            this.Controls.Add(this.inputBox);
            this.Controls.Add(this.buttonHiglightBgColour);
            this.Controls.Add(this.buttonhighlightColour);
            this.Controls.Add(this.buttonBgColour);
            this.Controls.Add(this.buttonTextColour);
            this.Controls.Add(this.labelHighlightBgColour);
            this.Controls.Add(this.labelHighlightColour);
            this.Controls.Add(this.labelBgColour);
            this.Controls.Add(this.labelTxtColour);
            this.Controls.Add(this.labelTxtSize);
            this.Controls.Add(this.TextSizeupdown);
            this.Controls.Add(this.frequncySlider);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsMenu";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.TextSizeupdown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequncySlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown TextSizeupdown;
        private System.Windows.Forms.Label labelTxtSize;
        private System.Windows.Forms.Label labelTxtColour;
        private System.Windows.Forms.Label labelBgColour;
        private System.Windows.Forms.Label labelHighlightColour;
        private System.Windows.Forms.Label labelHighlightBgColour;
        private System.Windows.Forms.Button buttonTextColour;
        private System.Windows.Forms.Button buttonBgColour;
        private System.Windows.Forms.Button buttonhighlightColour;
        private System.Windows.Forms.Button buttonHiglightBgColour;
        private System.Windows.Forms.TextBox inputBox;
        private System.Windows.Forms.Label labelAddupdate;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Label labelFrequency;
        private System.Windows.Forms.TrackBar frequncySlider;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Button buttonUpadate;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Label lableCommon;
        private System.Windows.Forms.Label lableRare;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolTip frequncyToolTip;
        private System.Windows.Forms.Label labelFrequencyUnits;
        private HighlightedTextBox exampleOutBox;
        private System.Windows.Forms.Label lblWordPressent;
    }
}

