using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using Formulas;
using System.Collections.Generic;

namespace SpreadsheetTests
{
    [TestClass]
    public class SSTESTS
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SS_Exceptions()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SS_Exceptions2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("3A", "Text");
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SS_Exceptions4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("3A", 5);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SS_Exceptions5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula f = new Formula("5-2");
            sheet.SetContentsOfCell("3A", f);
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SS_Exceptions6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula f = new Formula("D1 + C1");
            sheet.SetContentsOfCell("A1", f);
            Formula g = new Formula("C1 + A1");
            sheet.SetContentsOfCell("A1", f);
            sheet.SetContentsOfCell("D1", g);
        }


        [TestMethod]
        public void SS_Construct()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
        }

        [TestMethod]
        public void SS_Construct2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A5", "5");
            foreach (string s in sheet.GetNamesOfAllNonemptyCells())
            {
                if (s != "A5")
                    Assert.Fail();
            }

            Assert.AreEqual("5", sheet.GetCellContents("A5"));
        }

        [TestMethod]
        public void SS_Construct3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula f = new Formula("5*4");
            sheet.SetContentsOfCell("A5", f);
            foreach (string s in sheet.GetNamesOfAllNonemptyCells())
            {
                if (s != "A5")
                    Assert.Fail();
            }

            Assert.AreEqual(f, sheet.GetCellContents("A5"));
        }

        [TestMethod]
        public void SS_Construct4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            double num = 5;
            sheet.SetContentsOfCell("A5", num);
            foreach (string s in sheet.GetNamesOfAllNonemptyCells())
            {
                if (s != "A5")
                    Assert.Fail();
            }

            Assert.AreEqual(5.0, sheet.GetCellContents("A5"));
        }

         [TestMethod]
        public void SS_Dependencys()   
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula f = new Formula("B1 + 7");
            Formula g = new Formula("C1+ B1");
            sheet.SetContentsOfCell("A1", f);
            sheet.SetContentsOfCell("D1", g);

            HashSet<string> valueSet = new HashSet<string>();
            valueSet.Add("A1");
            valueSet.Add("B1");
            valueSet.Add("C1");
            valueSet.Add("D1");
            valueSet.Add("F1");
            valueSet.Add("G1");
            Formula h = new Formula("A1+ F1");
            foreach (string s in sheet.SetContentsOfCell("G1", h))
                if (!valueSet.Contains(s))
                    Assert.Fail();
            sheet.SetContentsOfCell("G1", h);

        }
         [TestMethod]
         public void SS_Dependencys2()   
         {
             AbstractSpreadsheet sheet = new Spreadsheet();
             Formula f = new Formula("B1 + 7");
             Formula g = new Formula("C1+ B1");
             sheet.SetContentsOfCell("A1", f);
             sheet.SetContentsOfCell("D1", g);

             HashSet<string> valueSet = new HashSet<string>();
             valueSet.Add("A1");
             valueSet.Add("B1");
             valueSet.Add("C1");
             valueSet.Add("D1");
             valueSet.Add("F1");
             valueSet.Add("G1");
             Formula h = new Formula("A1+ F1");
             foreach (string s in sheet.SetContentsOfCell("G1", h))
                 if (!valueSet.Contains(s))
                     Assert.Fail();
             sheet.SetContentsOfCell("G1", h);
             foreach (string s in sheet.SetContentsOfCell("G1", "hello"))
                 if (!valueSet.Contains(s))
                     Assert.Fail();


         }

        
    }
}
