using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;

namespace TestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended primarily to show you
    /// how to create your own, which we strong recommend that you do!  To run them, pull down
    /// the Test menu and do Run > All Tests.
    /// 
    /// MODIFIED 1/27/2015
    /// BY MATT HOWA
    /// //Added More Tests
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct4()
        {
            Formula f = new Formula("5x");
        }

        [TestMethod]
        public void Construct5()
        {
            Formula f = new Formula("xx55");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct6()
        {
            Formula f = new Formula("5_3");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct7()
        {
            Formula f = new Formula(")1(");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct8()
        {
            Formula f = new Formula("/1");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct9()
        {
            Formula f = new Formula("-1");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct10()
        {
            Formula f = new Formula("+1");
        }
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual(f.Evaluate(s => 0), 5.0, 1e-6);
        }

        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(s => 22.5), 22.5, 1e-6);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x5 + y6");
            f.Evaluate(s => { throw new ArgumentException(); });
        }

        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("(2.5e9)*2");
            Assert.AreEqual(f.Evaluate(s => 0), 5e9, 1e-6);
        }

        [TestMethod]
        public void Evaluate4b()
        {
            Formula f = new Formula("15000/407*3+7");
            Assert.AreEqual(f.Evaluate(s => 0), 117.5651106, 1e-6);
        }

        [TestMethod] 
        public void Evaluate4c()
        {
            Formula f = new Formula("15000/(407)*3");
            Assert.AreEqual(f.Evaluate(s => 0), 110.5651106, 1e-6);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        
        public void Evaluate5()
        {
            Formula f = new Formula("(2.5e9)*10/9/0");
            f.Evaluate(s => { throw new ArgumentException(); });
        }
    }
}
