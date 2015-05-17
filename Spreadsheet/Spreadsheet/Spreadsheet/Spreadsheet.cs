using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using Dependencies;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<String, Cell> Cells;     //CELLS CONTAIN A NAME + Content valued pair Key holds value
        private DependencyGraph ssDep;              //Dependency graph to track our dependency
        private HashSet<String> temp;               //Temporary hasset that will hold variables of a formula
        private Regex IsValid;

        /// <summary>
        /// Construct a Spreadsheet object that contains a empty cell
        /// </summary>
        public Spreadsheet()
            : this(new Regex(".*")){ }                 //Regex expression that accepts all strings (default)
        

        public Spreadsheet(Regex IsValid)
        {
            this.IsValid = IsValid;                         
            this.Cells = new Dictionary<string, Cell>();
            this.ssDep = new DependencyGraph();

        }

        //TODO add exceptin handeling
        public Spreadsheet(TextReader Source)
        {
            this.ssDep = new DependencyGraph();
            this.Cells = new Dictionary<string, Cell>();

            string name="";
            using (XmlReader reader = XmlReader.Create(Source))
            {
                try
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":                                //GET REGEX FROM XML ATTRIBUTE
                                    this.IsValid = new Regex(reader["isvalid"]);
                                    break;

                                case "name":                                       //GET CELL NAME
                                    reader.Read();
                                    name = reader.Value;
                                    break;

                                case "contents":                                   //GET CELL CONTENT -> THEN ADD TO CELL DICTIONARY
                                    reader.Read();
                                    SetContentsOfCell(name, reader.Value);
                                    break;
                            }
                        }
                    }
                }
                catch { throw new SpreadsheetReadException("Spreadsheet file may be corrupt"); }
            }
            Changed = false;
        }

        public override IEnumerable<String> GetNamesOfAllNonemptyCells()
        {
            foreach (var c in Cells)
                if (c.Value.Content.ToString().Length > 0)  //CHECK IF CELL CONTENT CONTAINS DATA
                    yield return c.Key;                     //RETURN CELL NAME
        }

        public override object GetCellContents(String name)
        {
            //EXCEPTION HANDELING\\
            if (System.String.IsNullOrWhiteSpace(name))
                throw new InvalidNameException();
            if (!isValid(name))
                throw new InvalidNameException();

            name = name.ToUpper();
            Cell v;
            if (Cells.TryGetValue(name, out v))             //GET RIGHT CELL THEN RETRUN ITS CONTENTS
                return v.Content;
            else
            {
                Cells.Add(name, new Cell(name));
                return "";
            }
        }

        /// <summary>
        /// private lookup function to use as delegate for GetCellValue method
        /// </summary>
        private double lookup(String s)
        {
            s = s.ToUpper();                    //NORMALIZE VARIABLE
            Cell v;
            if (Cells.TryGetValue(s, out v))    //GET THE RIGHT CELL
                return (double)v.Value;
            else
                throw new Exception();
        }

        public override object GetCellValue(String name)
        {
            //EXCEPTION HANDELING\\
            if (!isValid(name))
                throw new InvalidNameException();
            if (System.String.IsNullOrWhiteSpace(name))
                throw new InvalidNameException();

            name = name.ToUpper();
            Cell v;
            if (Cells.TryGetValue(name, out v))                 //GET THE RIGHT CELL
                return v.Value;
            else
                Cells.Add(name, new Cell(name));
            return "";
        }

        public override void Save(System.IO.TextWriter dest)
        { 
            using (XmlWriter writer = XmlWriter.Create(dest))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("isvalid", IsValid.ToString());      //regex.tostring
                foreach (var c in Cells)
                {
                    writer.WriteStartElement("cell");
                    writer.WriteElementString("name", c.Key);          //name
                    if (c.Value.Content is Formula)       //check for  formula
                        writer.WriteElementString("contents", "=" + c.Value.Content.ToString());      //contents.tostring
                    else
                        writer.WriteElementString("contents",c.Value.Content.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            Changed = false;
        }

        public override bool Changed { get; protected set; }
        public override ISet<String> SetContentsOfCell(String name, String content)
        {
            //Exception handeling\\
            if (String.IsNullOrWhiteSpace(name))
                 throw new InvalidNameException();
            if (content==null)
               throw new ArgumentNullException();
            
            name = name.ToUpper();                                  //Normalize name
            double result;
            if (content == "")                                      //IF CONTENT IS BLANK 
                return SetCellContents(name, content);              

            if (Double.TryParse(content, out result))               //CHECK IF CONTENT IS A DOUBLE
               return SetCellContents(name, result);
            else if (content.ToCharArray()[0] == '=')               //CHECK IF CONTENT HAS AN = INDICATING A FORMULA
            {
                int charsize = content.ToCharArray().Count();
                string formula = null;
                for (int i = 1; i < charsize; i++)
                    formula = formula + content.ToCharArray()[i];

                //CHECK FOR NULL FORMULA
                if (formula == null) { throw new ArgumentNullException(); } 

                Formula f = new Formula(formula, s => s.ToUpper(), s => true);      //CREATE FORMULA AND CHECK FOR FORMATTING
                return SetCellContents(name, f);             
            }
            else
               return SetCellContents(name, content);                               //SET CONTENTS OF CELL TO CONTENT IF IT IS NOT A FORMULA OR NUMBER

        }


        protected override ISet<String> SetCellContents(String name, double number)
        {
            //EXCEPTION HANDELING (POSSIBLY REDUNDENT)
            if (System.String.IsNullOrWhiteSpace(name))
                throw new InvalidNameException();
            if (!isValid(name))
                throw new InvalidNameException();


            ISet<string> set = Set(name, number);
            foreach (string s in set)
                calculateV(s);
            return set;
        }

        protected override ISet<String> SetCellContents(String name, String text)
        {
            //EXCEPTION HANDELING\\
            if (System.String.IsNullOrWhiteSpace(name))
                throw new InvalidNameException();
            if (!isValid(name))
                throw new InvalidNameException();
            if (text == null)
                throw new ArgumentNullException();
            
            ISet<string> set = Set(name, text);
            foreach (string s in set)
                calculateV(s);
            return set;
        }

        protected override ISet<String> SetCellContents(String name, Formula formula)
        {
            //EXCEPTION HANDELING\\
            if (System.String.IsNullOrWhiteSpace(name))
                throw new InvalidNameException();
            if (!isValid(name))
                throw new InvalidNameException();
            if (formula == null)
                throw new ArgumentNullException();

            this.temp = new HashSet<string>();
            foreach (string s in formula.GetVariables())        //GET ALL THE VARIABLES FROM THE FORMULA AND ADD THEM TO A SET
            {
                if (!isValid(s))
                    throw new FormulaFormatException("One or more variables names do not follow the proper syntax expected");
                this.temp.Add(s);
            }

            foreach (string s in this.temp)                     //TRACK DEPENDENCIES
            {
                if(ssDep.HasDependents(s))
                    foreach (string ss in ssDep.GetDependents(s))
                        if (ss == s || ss == name)
                            throw new CircularException();
                ssDep.AddDependency(name, s);
            }
            Cell v;
            if (Cells.TryGetValue(name, out v))                 //Get the right cell
                if (v.Content is Formula)
                {
                    Formula check = (Formula)v.Content;
                    foreach (string s in check.GetVariables())
                        ssDep.RemoveDependency(name, s);        //Remove old dependencies
                }
            ISet<string> set = Set(name, formula);              
            foreach (string s in set)                           //Recalculate Cells that depend on new cells content
                calculateV(s);
            return set;
        }

        protected override IEnumerable<String> GetDirectDependents(String name){return ssDep.GetDependees(name);}


        /// <summary>
        /// Checks this Instruction: A string is a cell name if and only if it consists of one or more letters, 
        /// followed by a non-zero digit, followed by zero or more digits & matches the IsVaid Regex
        /// </summary>
        private bool isValid(String n)
        {
            string valid = @"^[a-zA-Z]+[1-9][0-9]*$";  //checks that a string has 1 or more letters followed by 1 or more digits
            if (Regex.IsMatch(n, valid))
                if (Regex.IsMatch(n, IsValid.ToString()))
                    return true;
                else
                    return false;
            else
                return false;
        }

        /// <summary>
        /// Given a value pair populates a Cell
        /// Checks for dependents and dependees of the key
        /// returns them in a set
        /// </summary>
        private ISet<String> Set(String key, object o)
        {
            Changed = true;
            Cell cell = new Cell(key, o);
            if (!Cells.ContainsKey(key))                                    //If no cell exists with key as name
                Cells.Add(key, cell);                                       //Add it
            ////If Cell Exists\\\\\
            else
            {
                if (o is Formula)                                           //Check for formula 
                    ssDep.ReplaceDependents(key, temp);                     //Replace all the dependents of that key
                else
                {
                    Cell v;
                    if (Cells.TryGetValue(key, out v))
                        if (v.Content is Formula)                           //CHECK IF OLD CONTENT WAS A FORMULA
                        {    
                            Formula check = (Formula)v.Content;
                            foreach (string s in check.GetVariables())
                                ssDep.RemoveDependency(key, s);             //REMOVE THE OLD FORMULA DEPENDENCIES
                        }
                }
                Cells.Remove(key);                                          //Remove old cell
                Cells.Add(key, cell);                                       //Add the cell back with new content
            }

            //Return the set of cells that directly or indirectly relate to the cell we are working on
            HashSet<string> set = new HashSet<string>();
            foreach (string s in GetCellsToRecalculate(key))
                set.Add(s);
            return set;
        }

        private void calculateV(String name)
        {
            Cell v;
            if (Cells.TryGetValue(name, out v))                 //GET THE RIGHT CELL
            {
                if (v.Content is Formula)                       //CHECK IF ITS A FORMULA
                {
                    Formula f = (Formula)v.Content;
                    try{v.Value = f.Evaluate(lookup);}          //EVALUATE THE FORMULA
                    catch{v.Value = new FormulaError();}   
                }
                else if (v.Content is double)                   //CHECK IF CONTENT A NUMBER
                    v.Value = v.Content;
                else
                    v.Value = v.Content;                        //ELSE ITS A STRING
            }
            else
                Cells.Add(name, new Cell(name));                //IF NAME DOESNT EXIST IN DICTIONARY ADD IT
        }
    }
}
