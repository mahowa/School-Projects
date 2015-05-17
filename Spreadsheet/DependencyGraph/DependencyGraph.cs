// Skeleton implementation written by Joe Zachary for CS 3500, January 2015.
// Version 1.1 (1/28/15 7:00 p.m.): Changed name of namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.  (Recall that sets never contain duplicates.  If an attempt
    /// is made to add an element to a set, and the element is already in the set, the set remains unchanged.)
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If s is a string, the set of all strings t such that the dependency (t,s) is in DG 
    ///    is called the dependees of s, which we will denote as dependees(s).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of the class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///
    /// IMPLEMENTATION NOTE:  The simplest way to describe a DependencyGraph is as a set of dependencies.
    /// This is neither the simplest nor the most efficient way to implement a DependencyGraph, though.  Choose
    /// a representation that is both easy to work with and acceptably efficient.  Some of the test cases
    /// with which you will be graded will create massive DependencyGraphs.
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<string, HashSet<string>> Dependents; //Holds all dependees as keys and the dependants as values
        private Dictionary<string, HashSet<string>> Dependees;  //Holds all dependants as keys and dependees as valuse
        private int size;
        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            this.Dependents = new Dictionary<string, HashSet<string>>();
            this.Dependees = new Dictionary<string, HashSet<string>>();
            this.size = 0;
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size{get { return this.size; }}

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (Dependents.ContainsKey(s))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (Dependees.ContainsKey(s))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (Dependents.ContainsKey(s))
                return Dependents[s];
            else
                return new HashSet<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (Dependees.ContainsKey(s))
                return Dependees[s];
            else
                return new HashSet<string>();
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph
        /// This has no effect if (s,t) already belongs to this DependencyGraph
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (!Dependents.ContainsKey(s))
            {
                Dependents.Add(s, new HashSet<string>());
                Dependents[s].Add(t); 
                this.size++;
            }
            else
                if (!Dependents[s].Contains(t)) { this.size++; }
                    Dependents[s].Add(t);
            if (!Dependees.ContainsKey(t))
            {
                Dependees.Add(t, new HashSet<string>());
                Dependees[t].Add(s);
            }
            else
                Dependees[t].Add(s);
           
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (Dependents.ContainsKey(s))
            {
                if (Dependents[s].Contains(t))
                {
                    Dependents[s].Remove(t);
                    this.size--;
                    if (Dependents[s].Count == 0)
                        Dependents.Remove(s);
                    Dependees[t].Remove(s);
                    if (Dependees[t].Count == 0)
                        Dependees.Remove(t);
                }
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (Dependents.ContainsKey(s))
            {
                foreach (string n in Dependents[s].ToList())
                    RemoveDependency(s, n);
                foreach (string m in newDependents)
                    AddDependency(s, m);
            }
            else
                foreach (string m in newDependents)
                    AddDependency(s, m);
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,s).  Then, for each 
        /// t in newDependees, adds the dependency (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (Dependees.ContainsKey(s))
            {
                foreach (string n in Dependees[s].ToList())
                    RemoveDependency(n, s);
                foreach (string m in newDependees)
                    AddDependency(m, s);
            }
            else
                foreach (string m in newDependees)
                    AddDependency(m, s);
        }
    }
}
