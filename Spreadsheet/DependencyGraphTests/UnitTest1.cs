using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;
using System.Collections;
using System.Collections.Generic;

namespace DependencyGraphTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]  //Simple constructor test
        public void TestConstructor()
        {
            DependencyGraph f = new DependencyGraph();
        }

        [TestMethod]  //Test Has methods TRUE CONDITION
        public void TestHasDependes()
        {
            DependencyGraph f = new DependencyGraph();
            f.AddDependency("a", "b");
            Assert.IsTrue(f.HasDependents("a"));
            Assert.IsTrue(f.HasDependees("b"));
        }

        [TestMethod]    //Test Has methods False CONDITION
        public void TestHasDependes2()
        {
            DependencyGraph f = new DependencyGraph();
            f.AddDependency("a", "b");
            Assert.IsFalse(f.HasDependents("b"));
            Assert.IsFalse(f.HasDependees("a"));
        }

        [TestMethod]   //Test if we can get dependents
        public void TestGetDependents()
        {
            DependencyGraph f = new DependencyGraph();
            f.AddDependency("a", "b");
            f.AddDependency("a", "b");
            f.AddDependency("c", "b");
            f.AddDependency("b", "d");
            foreach (string n in f.GetDependents("a"))
            {
                if (n == "b") break;
                else Assert.Fail();
            }
            foreach (string n in f.GetDependents("c"))
            {
                if (n == "b") break;
                else Assert.Fail();
            }

        }
        [TestMethod]  //Test if we can get dependees.
        public void TestGetDependees()
        {
            DependencyGraph f = new DependencyGraph();
            f.AddDependency("a", "b");
            f.AddDependency("c", "b");
            f.AddDependency("b", "d");
            foreach (string n in f.GetDependees("b"))
            {
                switch (n)
                {
                    case "a":
                        break;
                    case "c":
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
            foreach (string n in f.GetDependents("d"))
            {
                if (n == "b") break;
                else Assert.Fail();
            }
            foreach (string n in f.GetDependees("a"))
            {
                if (n == null)
                    break;
                else
                    Assert.Fail();
            }

        }
        [TestMethod]  //Test if size is correct.
        public void TestSize()
        {
            DependencyGraph f = new DependencyGraph();
            f.AddDependency("a", "b");
            f.AddDependency("c", "b");
            f.AddDependency("b", "d");
            if (f.Size != 3) Assert.Fail();
        }
        [TestMethod]  //Test remove dependency and size still works
        public void TestSize2()
        {
            DependencyGraph f = new DependencyGraph();
            f.AddDependency("c", "b");
            f.RemoveDependency("c", "b");
            if (f.Size != 0) Assert.Fail();
        }

          [TestMethod]  //Test remove dependency and size still works
        public void TestReplaceDependent()
        {
            DependencyGraph f = new DependencyGraph();
            f.AddDependency("a", "b");
            f.AddDependency("c", "b");
            f.AddDependency("b", "d");
            HashSet<string> temp = new HashSet<string>();
            temp.Add("f");
            temp.Add("g");
            f.ReplaceDependents("a", temp);
            foreach (string n in f.GetDependees("a"))
            {
                switch (n)
                {
                    case "f":
                        break;
                    case "g":
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
            f.ReplaceDependents("l", temp);
        }
          [TestMethod]  //Test remove dependency and size still works
          public void TestReplaceDependees()
          {
              DependencyGraph f = new DependencyGraph();
              f.AddDependency("a", "b");
              f.AddDependency("c", "b");
              f.AddDependency("b", "d");
              HashSet<string> temp = new HashSet<string>();
              temp.Add("f");
              temp.Add("g");
              f.ReplaceDependees("b", temp);
              foreach (string n in f.GetDependents("a"))
              {
                  switch (n)
                  {
                      case "f":
                          break;
                      case "g":
                          break;
                      default:
                          Assert.Fail();
                          break;
                  }
              }
              f.ReplaceDependees("l", temp);

          }
      

    }
}
