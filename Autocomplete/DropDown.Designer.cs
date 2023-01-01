namespace Autocomplete
{
    partial class DropDown
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DropDown));
            this.selectionBox = new Autocomplete.HighlightedTextBox();
            this.SuspendLayout();
            // 
            // selectionBox
            // 
            this.selectionBox.AutoSize = true;
            this.selectionBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectionBox.Location = new System.Drawing.Point(0, 0);
            this.selectionBox.Name = "selectionBox";
            this.selectionBox.Size = new System.Drawing.Size(275, 130);
            this.selectionBox.Suggestions = ((System.Collections.Generic.List<string>)(resources.GetObject("selectionBox.Suggestions")));
            this.selectionBox.TabIndex = 0;
            // 
            // DropDown
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(275, 130);
            this.Controls.Add(this.selectionBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DropDown";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "DropDown";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HighlightedTextBox selectionBox;
    }
}