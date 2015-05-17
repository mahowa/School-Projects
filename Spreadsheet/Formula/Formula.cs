// Skeleton written by Joe Zachary for CS 3500, January 2014
//Modified by Matt Howa for PS2, January 26
//Extensions added 2/10/2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Formulas
{
    /// <summary>
    /// A Lookup function is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an IllegalArgumentException (meaning that the string is unmapped.
    /// Exactly how a Lookup function decides which strings map to doubles and which
    /// don't is up to the implementation of that function.
    /// </summary>
    public delegate double Lookup(string s);
    /// <summary>
    /// A normalizer takes a string as a parameter and returns a string as a result; 
    /// its purpose is to convert variables into a canonical form.
    /// </summary>
    public delegate string Normal(string s);        
    /// <summary>
    /// A validator takes a string as a parameter and returns a boolean as a result; 
    /// its purpose is to impose extra restrictions on the validity of a variable (beyond the ones described above).
    /// </summary>
    public delegate bool Valid(string s);
    
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public class Formula
    {
        private string equation;                                //input equation 
        private HashSet<string> vars = new HashSet<string>();   //hashset for variables
        private Normal Normal;                                  //Normalizer
        /// <summary>
        ///SINGLE PARAMETER Formula 
        /// </summary>
        public Formula(String formula)
            : this(formula, s => s, s => true){}

        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using standard C# syntax for double/int literals), 
        /// variable symbols (one or more letters followed by one or more digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// An example of a valid parameter to this constructor is "2.5e9 + x5 / 17".
        /// Examples of invalid parameters are "x", "-5.3", and "2 5 + 3";
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(String formula, Normal Normalizer, Valid Validater)
        {
            if (String.IsNullOrWhiteSpace(formula)) 
                throw new FormulaFormatException("There must be input in order to compute the formula");
            
            this.Normal = Normalizer;
            this.equation = formula;
            
            
            int check = 0;      //COUNTER FOR TOKENS
            int lparen = 0;     //COUNTER FOR LEFT PARENTHESIS
            int rparen = 0;     //COUNTER FOR RIGHT PARENTHESIS
            string[] arr = {"+", "-", "*", "/"};
            Stack<string> test = new Stack<string>();
            Exception ffex = new FormulaFormatException("You made a syntax error. Any token following a number, variable, or closing parenthesis must be either an operator or a closing parenthesis"); 
            foreach (string s in GetTokens(formula)) 
            {
                test.Push(s);
                double n;
                if (double.TryParse(s, out n))
                    if (check == 1)
                            throw ffex;
                    else check = 1;
                else if(arr.Contains(s))
                    if (check == 2 || check == 0) throw ffex;
                    else check = 2; 
                else
                {
                    switch (s)
                    {
                        case "(":
                            lparen++;
                            check = 2;
                            break;
                        case ")":
                            rparen++;
                            if (rparen > lparen)
                                throw new FormulaFormatException("You have made a syntax error. Please check your Parrenthesis and try again"); 
                            break;
                        ////VARIABLES\\\\
                        default: 
                            if (!Validater(Normalizer(s)))
                                throw new FormulaFormatException("There variables you entered do not follow the correct format or are undefined"); 
                            vars.Add(s);
                            char[] ss = s.ToCharArray();
                            if (!Char.IsLetter(ss[0]) || ss[0] =='_')
                                throw new FormulaFormatException("You have made a syntax error. Variables must be letters followed by numbers");
                            if (check > 0 && check == 1)
                                    throw ffex;
                            else check = 1;
                            break;
                    }
                }
            }
            if (rparen != lparen)
                throw new FormulaFormatException("You have made a syntax error. Check Parenthesis and try again"); 

            //CHECK FIRST AND LAST ELEMENTS FOR ANYTHING OTHER THE THE REQUIRED INFO  
            string[] temp = test.ToArray();
            int nn = temp.Length -1 ;
            if (temp[0] == "(" || temp[0] == "+" || temp[0] == "-" || temp[0] == "*" || temp[0] == "/") 
                throw new FormulaFormatException("The last token must be a number, variable or  opening parenthesis."); 
    }

        /// <summary>
        /// Returns formula string
        /// </summary>
        public override string ToString() { return this.equation; }


        /// <summery>
        /// Given a Formula, enumerates normalized version of each variable that appears
        /// </summery>
        public IEnumerable<string> GetVariables()
        {
             foreach (string s in this.vars)
                yield return Normal(s);
        }
        private Stack<double> values;
        private Stack<string> operators;
        /// <summary>
        /// Evaluates this Formula, using lookup to determine the values of variables.  
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, throw a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
             values = new Stack<double>();        //Holds tokens that translate to a value (i.e. numbers & mapped variables(also numbers))
             operators = new Stack<string>();     //Holds opperators *, /, ), (, +, and -
             foreach (string s in GetTokens(equation)) 
             {
                 double n;
                 //TOKENS THAT ARE NUMBERS
                 if (double.TryParse(s, out n))         
                     if (operators.Count > 0 && values.Count > 0)                  //if there is already a operator then check if there is a value on the value stack
                         MultDiv(operators.Peek(), n);
                     else values.Push(n);
                 //TOKENS THAT ARE VARIABLES\\
                 else if (vars.Contains(s))
                     try
                     {
                         double l = lookup(Normal(s));
                         if (operators.Count > 0 && values.Count > 0)              //if there is already a operator then check if there is a value on the value stack
                             MultDiv(operators.Peek(), l);
                         else values.Push(l);
                     }
                     catch { throw new FormulaEvaluationException("The variable is undefined or 0"); }
                 //TOKENS THAT ARE OPPERATORS
                 else
                     switch (s)
                     {
                         case ")":
                             while (true)
                             {
                                 if (operators.Count > 0)
                                 {
                                     if (operators.Peek() == "(")
                                     {
                                         operators.Pop();
                                         if (operators.Count > 0)
                                             MultDiv(operators.Peek(), values.Pop());
                                         break;
                                     }
                                     else { };
                                 }
                                 else break;
                                 if (operators.Count > 0)
                                     AddSub(operators.Peek());
                                 else break;
                             }
                             break;
                        default:
                             operators.Push(s);
                             break;
                     }

             }
             //PROCESS AND REPORT
             //Reverse order of stacks and process last tokens
             Stack<double> valued = new Stack<double>();
             Stack<string> operatorss = new Stack<string>();
             int count = values.Count;
             while (count > 0)
             {
                 valued.Push(values.Pop());
                 count--;
             }
             count = operators.Count();
             while (count > 0)
             {
                 operatorss.Push(operators.Pop());
                 count--;
             }
             while (operatorss.Count > 0)
             {
                 switch (operatorss.Peek())
                 {
                     case "+":
                         valued.Push(valued.Pop() + valued.Pop());
                         operatorss.Pop();
                         break;
                     case "-":
                         valued.Push(valued.Pop() - valued.Pop());
                         operatorss.Pop();
                         break;
                 }
             }
             return valued.Pop();
        }

        private double doDivision(double num, double den)
        {
            if (den == 0) throw new FormulaEvaluationException("You cannot divide by 0");
            return num / den;
        }

        private void MultDiv(string tok, double n)
        {
            switch (tok)                          //applies operator
            {
                case "*":
                    values.Push(values.Pop() * n);
                    operators.Pop();
                    break;
                case "/":
                    values.Push(doDivision(values.Pop(), n));
                    operators.Pop();
                    break;
                default:
                    values.Push(n);
                    break;
            }
        }
        private void AddSub(string tok)
        {
            switch (tok)
            {
                case "+":
                    values.Push(values.Pop() + values.Pop());
                    operators.Pop();
                    break;
                case "-":
                    double tempr = values.Pop();
                    values.Push(values.Pop() - tempr);
                    operators.Pop();
                    break;
            }
        }

        

       

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of one or more
        /// letters followed by one or more digits, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_][a-zA-Z0-9]*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                    yield return s;
        }
        
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message){}
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
 
        public FormulaEvaluationException(String message)
            : base(message){}
    }
}
