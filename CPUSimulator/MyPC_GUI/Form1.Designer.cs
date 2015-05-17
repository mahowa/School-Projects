namespace MyPC_GUI
{
    partial class MyGUI
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
            this.sortedBox = new System.Windows.Forms.TextBox();
            this.sortBtn = new System.Windows.Forms.Button();
            this.generateBtn = new System.Windows.Forms.Button();
            this.numGenBox = new System.Windows.Forms.TextBox();
            this.Results = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sWITCHSIMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sortedBox
            // 
            this.sortedBox.Location = new System.Drawing.Point(21, 539);
            this.sortedBox.Multiline = true;
            this.sortedBox.Name = "sortedBox";
            this.sortedBox.ReadOnly = true;
            this.sortedBox.Size = new System.Drawing.Size(511, 242);
            this.sortedBox.TabIndex = 10;
            // 
            // sortBtn
            // 
            this.sortBtn.Location = new System.Drawing.Point(193, 460);
            this.sortBtn.Name = "sortBtn";
            this.sortBtn.Size = new System.Drawing.Size(167, 50);
            this.sortBtn.TabIndex = 9;
            this.sortBtn.Text = "Sort";
            this.sortBtn.UseVisualStyleBackColor = true;
            this.sortBtn.Click += new System.EventHandler(this.sortBtn_Click);
            // 
            // generateBtn
            // 
            this.generateBtn.Location = new System.Drawing.Point(141, 376);
            this.generateBtn.Name = "generateBtn";
            this.generateBtn.Size = new System.Drawing.Size(270, 53);
            this.generateBtn.TabIndex = 8;
            this.generateBtn.Text = "Generate New Numbers";
            this.generateBtn.UseVisualStyleBackColor = true;
            this.generateBtn.Click += new System.EventHandler(this.generateBtn_Click);
            // 
            // numGenBox
            // 
            this.numGenBox.Location = new System.Drawing.Point(21, 108);
            this.numGenBox.Multiline = true;
            this.numGenBox.Name = "numGenBox";
            this.numGenBox.ReadOnly = true;
            this.numGenBox.Size = new System.Drawing.Size(511, 242);
            this.numGenBox.TabIndex = 7;
            // 
            // Results
            // 
            this.Results.Location = new System.Drawing.Point(580, 108);
            this.Results.Multiline = true;
            this.Results.Name = "Results";
            this.Results.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Results.Size = new System.Drawing.Size(890, 673);
            this.Results.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(763, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(177, 48);
            this.button1.TabIndex = 12;
            this.button1.Text = "CACHE";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(580, 54);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(177, 48);
            this.button2.TabIndex = 13;
            this.button2.Text = "Main";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.mainBtn);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sWITCHSIMToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1538, 40);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // sWITCHSIMToolStripMenuItem
            // 
            this.sWITCHSIMToolStripMenuItem.Name = "sWITCHSIMToolStripMenuItem";
            this.sWITCHSIMToolStripMenuItem.Size = new System.Drawing.Size(200, 38);
            this.sWITCHSIMToolStripMenuItem.Text = "Show other CPU";
            this.sWITCHSIMToolStripMenuItem.Click += new System.EventHandler(this.sWITCHSIMToolStripMenuItem_Click);
            // 
            // MyGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1538, 800);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Results);
            this.Controls.Add(this.sortedBox);
            this.Controls.Add(this.sortBtn);
            this.Controls.Add(this.generateBtn);
            this.Controls.Add(this.numGenBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MyGUI";
            this.Text = "MY CPU DESIGN";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sortedBox;
        private System.Windows.Forms.Button sortBtn;
        private System.Windows.Forms.Button generateBtn;
        private System.Windows.Forms.TextBox numGenBox;
        private System.Windows.Forms.TextBox Results;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sWITCHSIMToolStripMenuItem;
    }
}

