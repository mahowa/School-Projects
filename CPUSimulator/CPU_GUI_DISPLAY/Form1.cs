using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CPU_Simulator;
using CPU_MAIN;

namespace CPU_GUI_DISPLAY
{
    public partial class GivenGui : Form
    {
        public GivenGui()
        {
            InitializeComponent();
            sortBtn.Enabled = false;
        }
        private int[] unsorted;
        private CPU_SIMULATOR sim;
        private Random numGen = new Random();
        private void generateBtn_Click(object sender, EventArgs e)
        {
            numGenBox.Clear();
            sortedBox.Clear();
            unsorted =  new int[100];
            sim = new CPU_SIMULATOR();

            for (int i = 0; i < 100; i++)
            {
                int tmp = numGen.Next(0, 200);
                unsorted[i] = tmp;
                numGenBox.Text += (tmp + ", ");
            }
            sortBtn.Enabled = true;
        }

        private void sortBtn_Click(object sender, EventArgs e)
        {
            foreach (int i in unsorted)
                sim.inputController.Enqueue(i);
            sim.SIMULATOR();

            int count = 100;
            while (count-- > 0)
                sortedBox.Text+= sim.OutputController.Dequeue() + ", ";

            IMiss.Text = sim.IFCache.misses.ToString();
            IHits.Text = sim.IFCache.hits.ToString();
            DMiss.Text = sim.DataCache.misses.ToString();
            DHit.Text = sim.DataCache.hits.ToString();

            mCycles.Text = ((sim.DataCache.misses + sim.IFCache.misses) * 3).ToString();
            totalCycles.Text = sim.Cycles.ToString();

            ReadLatency.Text = sim.input.ToString();
            WriteLatency.Text = sim.output.ToString();

            IFcycles.Text = sim.IFcycles.ToString();
            PCcycles.Text = sim.PCcycles.ToString();
            otherCycles.Text = sim.OPcycles.ToString();
            totalTime.Text = sim.CompletionTime.ToString();
            BranchCycles.Text = sim.BranchCycles.ToString();
            CPI.Text = "CPI: " + sim.CPI.ToString("#.####");
            sortBtn.Enabled = false;
        }

        private void sortedBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void numGenBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void sWITCHSIMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
