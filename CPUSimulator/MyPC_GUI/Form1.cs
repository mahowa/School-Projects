using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using CPU_Simulator;
using CPU_GUI_DISPLAY;

namespace MyPC_GUI
{
    public partial class MyGUI : Form
    {
        public MyGUI()
        {
            InitializeComponent();
            sortBtn.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;

        }

        private int[] unsorted;
        private CPU_SIMULATOR2 sim;
        private Random numGen = new Random();
        private void generateBtn_Click(object sender, EventArgs e)
        {
            numGenBox.Clear();
            sortedBox.Clear();
            Results.Clear();
            unsorted = new int[100];
            sim = new CPU_SIMULATOR2();

            for (int i = 0; i < 100; i++)
            {
                int tmp = numGen.Next(0, 200);
                unsorted[i] = tmp;
                numGenBox.Text += (tmp + ", ");
            }
            sortBtn.Enabled = true;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void sortBtn_Click(object sender, EventArgs e)
        {
            foreach (int i in unsorted)
                sim.inputController.Enqueue(i);
            sim.SIMULATOR();

            int count = 100;
            while (count-- > 0)
                sortedBox.Text += sim.outputController.Dequeue() + ", ";

            mainBtn(sender, e);
            sortBtn.Enabled = false;
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Results.Clear();
            int stalls = sim.TB.stalls;
            Results.AppendText("Type of Cache used:\tTWO BIT BRANCH PREDICTOR");
            Results.AppendText(Environment.NewLine);
            Results.AppendText(sim.TB.stallstring);
            Results.AppendText(Environment.NewLine + Environment.NewLine);
            Results.AppendText("Stalls due to mispredicted branch instructions: \t" + stalls);
            Results.AppendText(Environment.NewLine + "Cost in Cycles:\t" + stalls/2);
            Results.AppendText(Environment.NewLine + "Cost in Nano Seconds:\t" + (stalls/2) * 9);
        }

        private void mainBtn(object sender, EventArgs e)
        {
            Results.Clear();

            Results.AppendText("Pipelined processor design");
            Results.AppendText(Environment.NewLine);
            Results.AppendText("Clock Period = 9 nano seconds");
            Results.AppendText(Environment.NewLine);
            Results.AppendText("Total cycles to sort the number set: "+ sim.TotalCycles);
            Results.AppendText(Environment.NewLine);
            Results.AppendText("Cycles due to mispredicted branch instructions: " + (sim.TB.stalls/2));
            Results.AppendText(Environment.NewLine);
            Results.AppendText("Total time to finish the sort: " + sim.TimeSpan + " nano seconds");
            Results.AppendText(Environment.NewLine);
            Results.AppendText("CPI:\t"+ sim.CPI);
            Results.AppendText(Environment.NewLine);
        }

        private void sWITCHSIMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GivenGui GG = new GivenGui();
            GG.Show();

        }
    }
}
