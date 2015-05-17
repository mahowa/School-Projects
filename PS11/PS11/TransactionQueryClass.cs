using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BoggleService
{
    public class TransactionQuery
    {
        private Command C { get; set; }
        private CommandQue CQ { get; set; }
        private MySqlConnection conn;
        private MySqlCommand command;
        public TransactionQuery(MySqlConnection connection)
        {
            conn = connection;
            C = new Command();
            CQ = new CommandQue();
        }
        public bool Transaction()
        {
            Update();
            conn.Open();
            using (MySqlTransaction trans = conn.BeginTransaction())
            {
                Queue<Queue<Tuple<string,bool>>> readParms = C.readparms;
                command = conn.CreateCommand();
                try
                {
                    //Itterate throw transactions
                    foreach (string commandString in C.commands.Keys)
                    {
                        //Set command
                        command.CommandText = commandString;

                        //Set Parameters
                        foreach (var parms in C.commands[commandString])
                        {
                            if (parms.Item1 != "")
                                command.Parameters.AddWithValue(parms.Item1, parms.Item2);
                        }

                        //Execute Query
                        if (command.ExecuteNonQuery() == 0)
                            throw new Exception("Query failed");

                        Queue<string> readResults = new Queue<string>();
                        bool condition = false;
                        //For SELECT commands places results on the que to be dealt with where implemented
                        if (commandString.Contains("select"))
                        {
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                Queue<Tuple<string,bool>> ls = readParms.Dequeue();
                                int count = ls.Count;
                               
                                while (reader.Read())
                                {
                                    for (int i = 0; i < count; i++ )
                                    {
                                        Tuple<string, bool> t = ls.Dequeue();
                                        readResults.Enqueue(reader.GetString(t.Item1));
                                        if (t.Item2 == true)
                                            condition = true;
                                            
                                    }
                                }
                            }
                            if (condition == true)
                            {
                                Conditional Condition = CQ.conditions.Dequeue();
                                if (!Condition(readResults, command))
                                    goto BreakLabel;
                            }
                        }
                    }
                BreakLabel:
                    try
                    {  
                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        // Set Repsone code to 500.
                        trans.Rollback();
                        return false;
                    }
                    finally
                    {
                        conn.Close();
                        Clear();
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
        private Tuple<string, List<Tuple<string, string>>> commandParms(string command, string[] parm1, string[] parm2)
        {
            List<Tuple<string, string>> tuplelist = new List<Tuple<string, string>>();
            if (parm1.Length == 0)
            {
                Tuple<string, string> t = new Tuple<string, string>("", "");
                tuplelist.Add(t);
                return new Tuple<string, List<Tuple<string, string>>>(command, tuplelist);
            }
            for (int i = 0; i < parm1.Length; i++)
            {
                Tuple<string, string> t = new Tuple<string, string>(parm1[i], parm2[i]);
                tuplelist.Add(t);
            }
            return new Tuple<string, List<Tuple<string, string>>>(command, tuplelist);
        }
        private Queue<Tuple<string,bool>> readParms(string[] readparms, bool[] checks)
        {
            Queue<Tuple<string, bool>> ls = new Queue<Tuple<string, bool>>();

            for (int i = 0; i < readparms.Length; i++)
            {
                Tuple<string,bool> t = new Tuple<string,bool>(readparms[i], checks[i]);
                ls.Enqueue(t);
            }
            return ls;
        }

        private void Update()
        {
            int count = CQ.commandStrings.Count;
            for (int i = 0; i < count; i++)
            {
                //GET OUR DATA IN THE RIGHT FORM AND ADDS IT TO THE DICTIONARY
                Tuple<string, List<Tuple<string, string>>> t = commandParms(CQ.commandStrings.Dequeue(), CQ.parmsQue1.Dequeue(), CQ.parmsQue2.Dequeue());
                C.commands.Add(t.Item1, t.Item2);
            }
            C.readparms = CQ.readparmQue;
        }

        private void Clear()
        {
            C = new Command();
            CQ = new CommandQue();
        }

        public void addParms(string[] sqlParms, string[] injectParm)
        {
            CQ.parmsQue1.Enqueue(sqlParms);
            CQ.parmsQue2.Enqueue(injectParm);
        }

        public void addReadParms(string[] parms, bool[] checks)
        {
            Queue<Tuple<string, bool>> ls = readParms(parms, checks);
            CQ.readparmQue.Enqueue(ls);
        }

        public void addCommand(string command)
        {
            CQ.commandStrings.Enqueue(command);
        }

        public void addConditions(Conditional c)
        {
            CQ.conditions.Enqueue(c);
        }
        public void addRepeatParm()
        {
            CQ.parmsQue1.Enqueue(new string[] { });
            CQ.parmsQue2.Enqueue(new string[] { });
        }

        public bool injectComand(string cLine, string[,] parms, string[]readParms, bool read, out Queue<string> returnQ)
        {
            returnQ = new Queue<string>();
            command.CommandText = cLine;
            int parmNum = parms.GetLength(0);
            for(int i = 0; i< parms.GetLength(0); i++)
                command.Parameters.AddWithValue(parms[i,0], parms[i,1]);

            if (command.ExecuteNonQuery() == 0)
                return false;

            if (read)
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        for (int i = 0; i < readParms.Length; i++)
                            returnQ.Enqueue(reader.GetString(readParms[i]));
                }
            }
            return true;
        }
    }

    public delegate bool Conditional(Queue<string> readResults, MySqlCommand command);
    public class Command
    {
        public Command()
        {
            commands = new Dictionary<string, List<Tuple<string, string>>>();
            readparms = new Queue<Queue<Tuple<string, bool>>>();
        }
        public Dictionary<string, List<Tuple<string, string>>> commands { get; set; }
        public Queue<Queue<Tuple<string, bool>>> readparms { get; set; }

    }
    public class CommandQue
    {
        public CommandQue()
        {
            commandStrings = new Queue<string>();
            parmsQue1 = new Queue<string[]>();
            parmsQue2 = new Queue<string[]>();
            readparmQue = new Queue<Queue<Tuple<string,bool>>>();
            conditions = new Queue<Conditional>();
        }

        public Queue<string> commandStrings { get; set; }
        public Queue<string[]> parmsQue1 { get; set; }
        public Queue<string[]> parmsQue2 { get; set; }
        public Queue<Queue<Tuple<string,bool>>> readparmQue { get; set; }
        public Queue<Conditional> conditions { get; set; }
    }
}

