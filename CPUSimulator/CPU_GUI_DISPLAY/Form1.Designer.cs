namespace CPU_GUI_DISPLAY
{
    partial class GivenGui
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.WriteLatency = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ReadLatency = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.mCycles = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.DMiss = new System.Windows.Forms.Label();
            this.DHit = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.IMiss = new System.Windows.Forms.Label();
            this.IHits = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numGenBox = new System.Windows.Forms.TextBox();
            this.generateBtn = new System.Windows.Forms.Button();
            this.sortBtn = new System.Windows.Forms.Button();
            this.sortedBox = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.PCcycles = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.totalCycles = new System.Windows.Forms.Label();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.IFcycles = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.otherCycles = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.totalTime = new System.Windows.Forms.Label();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BranchCycles = new System.Windows.Forms.Label();
            this.CPI = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(29, 271);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(621, 246);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "I/O";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.WriteLatency);
            this.groupBox3.Location = new System.Drawing.Point(358, 62);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(233, 155);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Write Cycles";
            // 
            // WriteLatency
            // 
            this.WriteLatency.AutoSize = true;
            this.WriteLatency.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WriteLatency.Location = new System.Drawing.Point(67, 70);
            this.WriteLatency.Name = "WriteLatency";
            this.WriteLatency.Size = new System.Drawing.Size(0, 37);
            this.WriteLatency.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ReadLatency);
            this.groupBox2.Location = new System.Drawing.Point(24, 62);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(233, 155);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Read Cycles";
            // 
            // ReadLatency
            // 
            this.ReadLatency.AutoSize = true;
            this.ReadLatency.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReadLatency.Location = new System.Drawing.Point(67, 70);
            this.ReadLatency.Name = "ReadLatency";
            this.ReadLatency.Size = new System.Drawing.Size(0, 37);
            this.ReadLatency.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox7);
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(29, 553);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(621, 246);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Cache";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.mCycles);
            this.groupBox7.Location = new System.Drawing.Point(423, 62);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(177, 164);
            this.groupBox7.TabIndex = 5;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "+Memory Cycles";
            // 
            // mCycles
            // 
            this.mCycles.AutoSize = true;
            this.mCycles.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mCycles.Location = new System.Drawing.Point(38, 97);
            this.mCycles.Name = "mCycles";
            this.mCycles.Size = new System.Drawing.Size(0, 37);
            this.mCycles.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.DMiss);
            this.groupBox6.Controls.Add(this.DHit);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Location = new System.Drawing.Point(225, 62);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(192, 164);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Data";
            // 
            // DMiss
            // 
            this.DMiss.AutoSize = true;
            this.DMiss.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DMiss.ForeColor = System.Drawing.Color.Red;
            this.DMiss.Location = new System.Drawing.Point(85, 124);
            this.DMiss.Name = "DMiss";
            this.DMiss.Size = new System.Drawing.Size(0, 37);
            this.DMiss.TabIndex = 3;
            // 
            // DHit
            // 
            this.DHit.AutoSize = true;
            this.DHit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DHit.ForeColor = System.Drawing.Color.Green;
            this.DHit.Location = new System.Drawing.Point(84, 65);
            this.DHit.Name = "DHit";
            this.DHit.Size = new System.Drawing.Size(0, 37);
            this.DHit.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 37);
            this.label5.TabIndex = 1;
            this.label5.Text = "Miss";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 37);
            this.label6.TabIndex = 0;
            this.label6.Text = "Hit";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.IMiss);
            this.groupBox5.Controls.Add(this.IHits);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Location = new System.Drawing.Point(9, 62);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(198, 164);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Instruction";
            // 
            // IMiss
            // 
            this.IMiss.AutoSize = true;
            this.IMiss.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IMiss.ForeColor = System.Drawing.Color.Red;
            this.IMiss.Location = new System.Drawing.Point(82, 124);
            this.IMiss.Name = "IMiss";
            this.IMiss.Size = new System.Drawing.Size(0, 37);
            this.IMiss.TabIndex = 3;
            // 
            // IHits
            // 
            this.IHits.AutoSize = true;
            this.IHits.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IHits.ForeColor = System.Drawing.Color.Green;
            this.IHits.Location = new System.Drawing.Point(77, 65);
            this.IHits.Name = "IHits";
            this.IHits.Size = new System.Drawing.Size(0, 37);
            this.IHits.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-3, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 37);
            this.label2.TabIndex = 1;
            this.label2.Text = "Miss";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hit";
            // 
            // numGenBox
            // 
            this.numGenBox.Location = new System.Drawing.Point(1221, 75);
            this.numGenBox.Multiline = true;
            this.numGenBox.Name = "numGenBox";
            this.numGenBox.ReadOnly = true;
            this.numGenBox.Size = new System.Drawing.Size(511, 242);
            this.numGenBox.TabIndex = 3;
            this.numGenBox.TextChanged += new System.EventHandler(this.numGenBox_TextChanged);
            // 
            // generateBtn
            // 
            this.generateBtn.Location = new System.Drawing.Point(1341, 343);
            this.generateBtn.Name = "generateBtn";
            this.generateBtn.Size = new System.Drawing.Size(270, 53);
            this.generateBtn.TabIndex = 4;
            this.generateBtn.Text = "Generate New Numbers";
            this.generateBtn.UseVisualStyleBackColor = true;
            this.generateBtn.Click += new System.EventHandler(this.generateBtn_Click);
            // 
            // sortBtn
            // 
            this.sortBtn.Location = new System.Drawing.Point(1393, 427);
            this.sortBtn.Name = "sortBtn";
            this.sortBtn.Size = new System.Drawing.Size(167, 50);
            this.sortBtn.TabIndex = 5;
            this.sortBtn.Text = "Sort";
            this.sortBtn.UseVisualStyleBackColor = true;
            this.sortBtn.Click += new System.EventHandler(this.sortBtn_Click);
            // 
            // sortedBox
            // 
            this.sortedBox.Location = new System.Drawing.Point(1221, 506);
            this.sortedBox.Multiline = true;
            this.sortedBox.Name = "sortedBox";
            this.sortedBox.ReadOnly = true;
            this.sortedBox.Size = new System.Drawing.Size(511, 242);
            this.sortedBox.TabIndex = 6;
            this.sortedBox.TextChanged += new System.EventHandler(this.sortedBox_TextChanged);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.PCcycles);
            this.groupBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox8.Location = new System.Drawing.Point(753, 228);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(388, 155);
            this.groupBox8.TabIndex = 2;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "PC Cycles";
            // 
            // PCcycles
            // 
            this.PCcycles.AutoSize = true;
            this.PCcycles.Location = new System.Drawing.Point(91, 70);
            this.PCcycles.Name = "PCcycles";
            this.PCcycles.Size = new System.Drawing.Size(0, 37);
            this.PCcycles.TabIndex = 1;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.totalCycles);
            this.groupBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox9.Location = new System.Drawing.Point(712, 608);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(227, 191);
            this.groupBox9.TabIndex = 3;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Total Cycles";
            // 
            // totalCycles
            // 
            this.totalCycles.AutoSize = true;
            this.totalCycles.Font = new System.Drawing.Font("MV Boli", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalCycles.ForeColor = System.Drawing.SystemColors.GrayText;
            this.totalCycles.Location = new System.Drawing.Point(38, 96);
            this.totalCycles.Name = "totalCycles";
            this.totalCycles.Size = new System.Drawing.Size(0, 41);
            this.totalCycles.TabIndex = 0;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.IFcycles);
            this.groupBox10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox10.Location = new System.Drawing.Point(753, 436);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(388, 152);
            this.groupBox10.TabIndex = 3;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Instruction Fetch Cycles";
            // 
            // IFcycles
            // 
            this.IFcycles.AutoSize = true;
            this.IFcycles.Location = new System.Drawing.Point(106, 70);
            this.IFcycles.Name = "IFcycles";
            this.IFcycles.Size = new System.Drawing.Size(0, 37);
            this.IFcycles.TabIndex = 0;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.otherCycles);
            this.groupBox11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox11.Location = new System.Drawing.Point(753, 55);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(388, 155);
            this.groupBox11.TabIndex = 3;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "ALU OP Cycles";
            // 
            // otherCycles
            // 
            this.otherCycles.AutoSize = true;
            this.otherCycles.Location = new System.Drawing.Point(91, 70);
            this.otherCycles.Name = "otherCycles";
            this.otherCycles.Size = new System.Drawing.Size(0, 37);
            this.otherCycles.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(-1, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(455, 63);
            this.label3.TabIndex = 7;
            this.label3.Text = "Multi-Cycle CPU ";
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.totalTime);
            this.groupBox12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox12.Location = new System.Drawing.Point(958, 608);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(208, 191);
            this.groupBox12.TabIndex = 4;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Total Time (ns)";
            // 
            // totalTime
            // 
            this.totalTime.AutoSize = true;
            this.totalTime.Font = new System.Drawing.Font("MV Boli", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalTime.ForeColor = System.Drawing.SystemColors.GrayText;
            this.totalTime.Location = new System.Drawing.Point(26, 96);
            this.totalTime.Name = "totalTime";
            this.totalTime.Size = new System.Drawing.Size(0, 41);
            this.totalTime.TabIndex = 0;
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.label4);
            this.groupBox13.Controls.Add(this.BranchCycles);
            this.groupBox13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox13.Location = new System.Drawing.Point(264, 110);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(388, 155);
            this.groupBox13.TabIndex = 3;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Branch Cycles";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(372, 26);
            this.label4.TabIndex = 2;
            this.label4.Text = "*Not Independent from other sections";
            // 
            // BranchCycles
            // 
            this.BranchCycles.AutoSize = true;
            this.BranchCycles.Location = new System.Drawing.Point(91, 70);
            this.BranchCycles.Name = "BranchCycles";
            this.BranchCycles.Size = new System.Drawing.Size(0, 37);
            this.BranchCycles.TabIndex = 1;
            // 
            // CPI
            // 
            this.CPI.AutoSize = true;
            this.CPI.Location = new System.Drawing.Point(50, 125);
            this.CPI.Name = "CPI";
            this.CPI.Size = new System.Drawing.Size(0, 25);
            this.CPI.TabIndex = 8;
            // 
            // GivenGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1791, 822);
            this.Controls.Add(this.CPI);
            this.Controls.Add(this.groupBox13);
            this.Controls.Add(this.groupBox12);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox11);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.sortedBox);
            this.Controls.Add(this.sortBtn);
            this.Controls.Add(this.generateBtn);
            this.Controls.Add(this.numGenBox);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Name = "GivenGui";
            this.Text = "CPU SIMULATION";
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label WriteLatency;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label ReadLatency;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label mCycles;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label DMiss;
        private System.Windows.Forms.Label DHit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label IMiss;
        private System.Windows.Forms.Label IHits;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox numGenBox;
        private System.Windows.Forms.Button generateBtn;
        private System.Windows.Forms.Button sortBtn;
        private System.Windows.Forms.TextBox sortedBox;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label totalCycles;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label IFcycles;
        private System.Windows.Forms.Label PCcycles;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Label otherCycles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Label totalTime;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Label BranchCycles;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label CPI;

    }
}

