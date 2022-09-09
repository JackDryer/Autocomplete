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
            this.wordsBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // wordsBox
            // 
            this.wordsBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wordsBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.wordsBox.Location = new System.Drawing.Point(0, 0);
            this.wordsBox.Name = "wordsBox";
            this.wordsBox.ReadOnly = true;
            this.wordsBox.ShortcutsEnabled = false;
            this.wordsBox.Size = new System.Drawing.Size(275, 130);
            this.wordsBox.TabIndex = 0;
            this.wordsBox.Text = "";
            this.wordsBox.Click += new System.EventHandler(this.wordsBox_Click);
            this.wordsBox.MouseLeave += new System.EventHandler(this.wordsBox_MouseLeave);
            this.wordsBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.wordsBox_MouseMove);
            // 
            // DropDown
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(275, 130);
            this.Controls.Add(this.wordsBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DropDown";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "DropDown";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox wordsBox;
    }
}