using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;
using Formulas;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.InteropServices;

namespace SpreadsheetGUI
{
    public partial class SpreadsheetGUI : Form
    {
        private bool savedas = false;
        private TextWriter save;
        private AbstractSpreadsheet ss;
        private string ssName;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;


        //NECISSARY DLL FUNCTIONS for relocating the window-source:http://www.codeproject.com/Articles/11114/Move-window-form-without-Titlebar-in-C
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public SpreadsheetGUI()
        {
            InitializeComponent();
            this.ss = new Spreadsheet();
            docName.Text = "Untitled Spreadsheet";

            int col;
            int row;
            cells.GetSelection(out col, out row);
            string name = getCellName(col, row);
            cellName.Text = name;
            SelectionChanged(cells);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
        }
        /// <summary>
        /// Helper Method to get the right name of each cell. 
        /// </summary>
        private string getCellName(int col, int row)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";         
            char[] alph = alphabet.ToCharArray();
            return alph[col].ToString() + (row + 1).ToString();     
        }
        /// <summary>
        /// Helper Method to get the right col # and row # of a cell
        /// </summary>
        private void getColRow(string name, out int col, out int row)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] alph = alphabet.ToCharArray();
            string pattern = @"([a-z]+)|([0-9]+)";
            string[] result = Regex.Split(name, pattern, RegexOptions.IgnorePatternWhitespace);
            char let = Convert.ToChar(result[0]);
            col = Array.IndexOf(alph, let);
            row = int.Parse(result[1]) - 1;
        }

        /// <summary>
        /// Shows a message to the user asking if they want to save if the document has been modified
        /// outputs a number to be used for directing the code based on user input
        /// </summary>
        private void saveCheck(out int num)
        {
            var result = MessageBox.Show("You have made changes to " + ssName + ".ss \nWould you like to save before proceeding",
                "Save File?",
                MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
                num = 0;
            else if (result == DialogResult.No)
                num = 1;
            else
                num = 2;
        }

        /// <summary>
        /// Button to enter content into cell. 
        /// </summary>
        private void enter_Click(object sender, EventArgs e)
        {
            int col;
            int row;
            cells.GetSelection(out col, out row);                       //Get COL & ROW #
            string value = content.Text;
            string name = getCellName(col, row);                        //From col and row # get the right name

            try
            {
                ISet<string> set = ss.SetContentsOfCell(name, value);   //Sets the contents of cell
                foreach (string s in set)                               //recalculate each cell that depends on the new cells value
                {
                    getColRow(s, out col, out row);
                    cells.SetValue(col, row, ss.GetCellValue(s).ToString());
                }
                cellValue.Text = ss.GetCellValue(name).ToString();      //Show the value in the cell box
            }
            //Display error messages\\
            catch (FormulaFormatException){ MessageBox.Show("You have made a syntax error in your formula"); } 
            catch (CircularException) { MessageBox.Show("It appears you have created a circular formula. Please revise your equation and try again"); }

            docName_TextChanged(sender, e);                     //Display on the gui that it has been modified by *           
        }

        /// <summary>
        /// Whenever a different cell is selected this event fires
        /// Places all the info of the cell in desinated textboxes 
        /// </summary>
        private void SelectionChanged(SpreadsheetPanel sender)
        {
            int col;
            int row;
            cells.GetSelection(out col, out row);                               //get col row
            string name = getCellName(col, row);                                // get the name of the cell
            if (ss.GetCellContents(name) is Formula)                            //Set contentsTextbox, valueTextbox, cellnameTextbox
                content.Text = "=" + ss.GetCellContents(name).ToString();
            else
                content.Text = ss.GetCellContents(name).ToString();
            cellName.Text = name;
            if (ss.GetCellValue(name) != null)
                cellValue.Text = ss.GetCellValue(name).ToString();
            else
                cellValue.Text = "";

            content.Focus();
            content.SelectAll();

        }

        /// <summary>
        /// Changes the name of the document (to be saved)
        /// </summary>
        private void docName_TextChanged(object sender, EventArgs e)
        {
            saveCtrlSToolStripMenuItem.Text = "Save " + docName.Text + " as...";

            if (!ss.Changed && savedas == true)
                ssLabel.Text = docName.Text + ".ss";
            else
                ssLabel.Text = "*" + docName.Text + ".ss";              // '*' indicates the document has been changed
        }

        /// <summary>
        /// Closes the window gracefully, Also checks if the document has been modified and
        /// if user wishes to save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void close_click(object sender, EventArgs e)
        {
            int num = 1;
            if (ss.Changed)
                saveCheck(out num);
            if (num == 0) { save_Click(sender, e); num = 1; }
            if (num == 1)
                this.Dispose();         //Graceful close
        }

        /// <summary>
        /// Handles spreadsheet keydown events.
        /// Any standard commands should be listed below
        /// </summary>
        private void SpreadsheetGUI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)       // Ctrl-S Save
            {
                save_Click(sender, e);
                e.SuppressKeyPress = true;  
            }
            if (e.Control && e.KeyCode == Keys.N)       // Ctrl-N New
            {
                newCtrlNToolStripMenuItem_Click(sender, e);
                e.SuppressKeyPress = true;  
            }
            if (e.Control && e.KeyCode == Keys.Q)       // Ctrl-Q Quit
            {
                close_click(sender, e);
                e.SuppressKeyPress = true; 
            }
            if (e.Control && e.KeyCode == Keys.O)       // Ctrl-Q Quit
            {
                open_Click(sender, e);
                e.SuppressKeyPress = true;  
            }
        }

        /// <summary>
        /// Opens a previously saved document
        /// </summary>
        private void open_Click(object sender, EventArgs e)
        {
            int num = 1;
            if(ss.Changed)                                                                  //check if current document has been modified
                saveCheck(out num);
            if (num == 0) { save_Click(sender, e); num = 1; }        //user wishes to save               
            if(num == 1){                                                                   //open the document
                try{
                    openFileDialog1.Filter = "Spreadsheet Files (.ss)|*.ss|All Files (*.*)|*.*";
                    openFileDialog1.FilterIndex = 1;
                    openFileDialog1.FileName = "";
                    DialogResult result = openFileDialog1.ShowDialog();                     // Show the dialog.
                    if (result == DialogResult.OK)                                          // Test result.
                    {
                        string file = openFileDialog1.FileName;
                        using (TextReader open = new StreamReader(file))
                        {
                            ss = new Spreadsheet(open);
                            cells.Clear();
                            int col, row;
                            foreach (string s in ss.GetNamesOfAllNonemptyCells())           //Calculate all the cell values
                            {
                                getColRow(s, out col, out row);
                                cells.SetValue(col, row, ss.GetCellValue(s).ToString());
                            }
                            docName.Text = System.IO.Path.GetFileNameWithoutExtension(file);    //set document name to opened file name
                            ssName = file;
                            savedas = true;
                        }
                        docName_TextChanged(sender, e);
                    }
                    SelectionChanged(cells);
                }
                    //Error Message\\
                catch (SpreadsheetReadException) { MessageBox.Show("The file you tried to open may be corrupt"); }
            }
        }

        /// <summary>
        /// Save the document 'as'
        /// </summary>
        private void savedas_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "ss";
            saveFileDialog1.FileName = docName.Text;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.Filter = "Spreadsheet Files (.ss)|*.ss|All Files (*.*)|*.*";
            DialogResult result = saveFileDialog1.ShowDialog();                             // Show the dialog.
            if (result == DialogResult.OK)
            {
                string file = saveFileDialog1.FileName;
                using (save = new StreamWriter(file))
                {
                    savedas = true;
                    ss.Save(save);
                    ssName = file;
                }
                MessageBox.Show("Saved");
                docName.Text = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog1.FileName);
                docName_TextChanged(sender, e);
            }
        }

        /// <summary>
        /// save the document
        /// </summary>
        private void save_Click(object sender, EventArgs e)
        {
            if (savedas == true)
            {
               string fileName = ssName;
               string newFileName = fileName.Substring(0, fileName.LastIndexOf('\\')) +
                                       '\\' + docName.Text + fileName.Substring(fileName.LastIndexOf('.'));

                File.Copy(fileName, newFileName);
                File.Delete(fileName);
                ssName = newFileName;
                using (save = new StreamWriter(ssName))
                {
                    ss.Save(save);
                    MessageBox.Show("Saved");
                }
                docName_TextChanged(sender, e);

            }
            else
                savedas_Click(sender, e);
        }

        /// <summary>
        /// Help Message
        /// </summary>
        private void help_Click(object sender, EventArgs e)
        {MessageBox.Show("Help:\n-Change the name of the document by entering it into the box on the top of the form \n-To change the contents of a cell, Select the Cell, then enter the desired information into the content box and press the enter button or hit return on your keyboard \n-If you wish to calculate an equation, start the input of the content box with a = or hit the button to do it for you. \n-You can save your document under any name and then reopen it as you wish.\n-The arrowpad can be used to changed the cell selection. Also hotkeys such as Ctr-S, Ctr-O, Ctr-N, Ctr-Q all corespond to a command. You can reference this in the file menue.");}

        /// <summary>
        /// Adds an equal sign in front of the text in the content
        /// </summary>
        private void EqualsButton_Click(object sender, EventArgs e)
        {
            string contents = content.Text;
            string temp = "";
            char[] charConts = contents.ToCharArray();
            if (charConts.Count() == 0 || charConts[0] != '=')
                content.Text = "=" + contents;
            else
            {
                for (int i = 1; i < charConts.Count(); i++)
                    temp = temp + charConts[i];
                content.Text = temp;
            }
        }

        /// <summary>
        /// x button at top. Gracefully closes the form
        /// </summary>
        private void x_Click(object sender, EventArgs e){this.Dispose();}

        /// <summary>
        /// maximizes the gui
        /// </summary>
        private void maximized_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
            {
                this.TopMost = true;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
        }

        /// <summary>
        /// Minimizes gui
        /// </summary>
        private void pictureBox1_Click(object sender, EventArgs e) {this.WindowState = FormWindowState.Minimized;}

        /// <summary>
        /// Allow user to relocate window(gui) on mousedown
        /// </summary>
        private void menuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            //Implementation of the relocating functions source:http://www.codeproject.com/Articles/11114/Move-window-form-without-Titlebar-in-C
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        /// <summary>
        /// Credits. Event fires when the U-logo is clicked
        /// </summary>
        private void credits_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This software was designed by Professor Joe Zachary for an class project at the University of Utah. I was asked to take a bit of skelaton" +
                            " code, complete it and then design a GUI to implement it. This code was not coppied or changed after seeing finals solutions in anyway." + 
                            "I was the only programmer on this project finishing, implementing and designing the final product. \n\n            Matthew Howa \n\nWebsite\n" + "http://mhit.co.nr");
        }

        /// <summary>
        /// Opens new document
        /// </summary>
        private void newCtrlNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DemoApplicationContext.getAppContext().RunForm(new SpreadsheetGUI());
        }

        /// <summary>
        /// Creates rollover image for x button
        /// </summary>
        private void x_MouseHover(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.x;
            pictureBox3.Refresh();
            pictureBox3.Visible = true;
        }
        /// <summary>
        /// Rollback on rollover image
        /// </summary>
        private void x_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.x_un;
            pictureBox3.Refresh();
            pictureBox3.Visible = true;
        }

        /// <summary>
        /// maximize button rollover image
        /// </summary>
        private void mazimize_MouseHover(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.maximized;
            pictureBox2.Refresh();
            pictureBox2.Visible = true;
        }

        /// <summary>
        /// maximize rollback after rollover
        /// </summary>
        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.maximized_un;
            pictureBox2.Refresh();
            pictureBox2.Visible = true;
        }

        /// <summary>
        /// minimize button rollover image
        /// </summary>
        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.Programming_Minimize_Window_icon_UN;
            pictureBox1.Refresh();
            pictureBox1.Visible = true;
        }

        /// <summary>
        /// minimize rollback after rollover
        /// </summary>
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.Programming_Minimize_Window_icon;
            pictureBox1.Refresh();
            pictureBox1.Visible = true;
        }

        /// <summary>
        /// Allow form to be moved from anywhere the form is visible on mousedown
        /// </summary>
        private void SpreadsheetGUI_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        /// <summary>
        /// keydown events for the spreadshhet cells
        /// </summary>
        private void cells_KeyDown(object sender, KeyEventArgs e)
        {
            int col;
            int row;
            cells.GetSelection(out col, out row);
            switch (e.KeyCode)
            {
                case Keys.Left:
                    enter_Click(sender, e);
                    col--;
                    cells_KeyDownHelper(col, row);
                    break;
                case Keys.Right:
                    enter_Click(sender, e);
                    col++;
                    cells_KeyDownHelper(col, row);
                    break;
                case Keys.Up:
                    enter_Click(sender, e);
                    row--;
                    cells_KeyDownHelper(col, row);
                    break;
                case Keys.Down:
                    enter_Click(sender, e);
                    row++;
                    cells_KeyDownHelper(col, row);
                    break;

            }

        }

        /// <summary>
        /// Helper method for cells_keydown event
        /// </summary>
        private void cells_KeyDownHelper(int col, int row)
        {
            cells.SetSelection(col, row);
            SelectionChanged(cells);
        }



    }
}
