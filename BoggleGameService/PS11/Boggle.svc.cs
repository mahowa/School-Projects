using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Boggle;
using System.IO;
using System.Diagnostics;
using System.Xml;
using MySql.Data.MySqlClient;



namespace BoggleService
{
    public class BoggleService : IBoggleService
    {
        private static List<string> Words;                                                  //Populates List with words from dictionary
        private static EventLog appLog;
        private static string TotalTime, pathname, initial;
        public const string connectionString = "server=ftp.howadigitaldesign.com;database=mahowa_boggle;uid=mahowa_01;password=mognarchy0";


        static BoggleService()
        {
            appLog = new System.Diagnostics.EventLog();
            appLog.Source = "Application";

            string basedir = AppDomain.CurrentDomain.BaseDirectory;
            string configFilePath = basedir + @"\boggle.xml";
            TextReader tr = new StreamReader(configFilePath);
            using (XmlReader reader = XmlReader.Create(tr))
            {
                try
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "duration":
                                    reader.Read();
                                    TotalTime = reader.Value;
                                    break;

                                case "pathname":
                                    reader.Read();
                                    pathname = basedir + @"\" + reader.Value;
                                    break;

                                case "initial":
                                    reader.Read();
                                    initial = reader.Value;
                                    break;
                            }
                        }
                    }
                }
                catch { appLog.WriteEntry("BoggleServer: Could not read from XML", EventLogEntryType.FailureAudit); }
            }
            Words = new List<string>(File.ReadAllLines(pathname));
        }
        /// <summary>
        /// Creates a new user and adds them to the user database
        /// returns the new USER ID
        /// </summary>
        public UserToken MakeUser(Player p)
        {
            if (p == null)
            {
                SetResponse("NF");
                return null;
            }
            return MakeUserQuery(p);
        }

        private UserToken MakeUserQuery(Player p)
        {
            string userID = Guid.NewGuid().ToString();               //New GUID for a Unique player ID
            MySqlConnection conn = new MySqlConnection(connectionString);
            TransactionQuery TQ = new TransactionQuery(conn);
            TQ.addCommand("insert into users (UserToken, Nickname) values(?UserToken, ?Nickname)");
            TQ.addParms(new string[] { "UserToken", "Nickname" }, new string[] { userID, p.nickname });

            if (!TQ.Transaction())
            {
                SetResponse("ISE");
                return null;
            }
            appLog.WriteEntry("BoggleServer: Made User", EventLogEntryType.Information);
            return new UserToken() { userToken = userID };
        }

        /// <summary>
        /// Finds a waiting game and adds the player to the game and returns Game ID
        /// Otherwise,
        /// Creates new game and adds it to the database
        /// returns the Game ID
        /// </summary>
        public GameToken JoinGame(Player p)
        {
            if (p == null)
            {
                SetResponse("NF");
                return null;
            }
            if (String.IsNullOrEmpty(p.userToken))
            {
                SetResponse("player");                          //403 Forbidden Response(invalid player token)
                return null;
            }
            return joinGameQuery(p);
        }

        private GameToken joinGameQuery(Player p)
        {
            string queryGameToken = "";
            /////Query for asking if the user is in the Users Table
            MySqlConnection conn = new MySqlConnection(connectionString);
            TransactionQuery TQ = new TransactionQuery(conn);

            TQ.addCommand("select count(*) from users where UserToken=?UserToken");
            TQ.addParms(new string[]{"UserToken"}, new string[]{p.userToken});
            TQ.addReadParms(new string[]{"count(*)"}, new bool[]{true});
            TQ.addConditions((result, command) =>
            {
                if (result.Dequeue().Equals("0"))
                {
                    SetResponse("player");
                    return false;
                }
                else return true;
            });

            TQ.addCommand("select count(*) from games where GameState=?GameState");
            TQ.addParms(new string[]{"GameState"}, new string[]{"waiting"});
            TQ.addReadParms(new string[]{"count(*)"}, new bool[]{true});
            TQ.addConditions((result, command) =>
            {
                if (result.Dequeue() =="0")
                {
                    queryGameToken = Guid.NewGuid().ToString();
                    command.CommandText = "insert into games (GameToken, Player1Token, GameState)"
                                                + "values(?gameToken, ?UserToken,?GameState)";
                    command.Parameters.AddWithValue("gameToken", queryGameToken);
                     //Execute Query
                    if (command.ExecuteNonQuery() == 0)
                        throw new Exception("Did not insert player into new game");

                    return false;
                }
                else return true;
            });

            TQ.addCommand("select GameToken, Player1Token from games where GameState=?GameState");
            TQ.addParms(new string[]{}, new string[]{});
            TQ.addReadParms(new string[]{"GameToken", "Player1Token"}, new bool[]{false, true});
            TQ.addConditions((result, command) =>
            {
                queryGameToken = result.Dequeue();
                if (result.Dequeue() == p.userToken)
                {
                    SetResponse("same");                //409 Conflict Response(player already in game)
                    return false;
                }
                else
                {
                    command.CommandText = "update games set GameState = ?gs where GameToken = ?gt";
                    command.Parameters.AddWithValue("gs", "playing");
                    command.Parameters.AddWithValue("gt", queryGameToken);
                    if (command.ExecuteNonQuery() == 0)
                        throw new Exception("update gamestate failed");
                    BoggleBoard b;
                    if (String.IsNullOrEmpty(initial))
                        b = new BoggleBoard(initial);
                    else
                        b = new BoggleBoard();
                    command.CommandText = "insert into pairedGames (GameToken, Player2Token, Score1, Score2, Board, Duration, StartTime)"
                            + " values(?gt, ?Player2Token, 0, 0, ?Board, 0, ?StartTime)";
                    command.Parameters.AddWithValue("Player2Token", p.userToken);
                    command.Parameters.AddWithValue("Board", b.ToString());
                    command.Parameters.AddWithValue("StartTime", DateTime.Now.ToString());
                    if (command.ExecuteNonQuery() == 0)
                        throw new Exception("update gamestate failed");
                    return true;
                }
            });

            if(!TQ.Transaction())
            {
                SetResponse("ISE");
                return null;
            }

            return new GameToken() { gameToken = queryGameToken };
        }

        /// <summary>
        /// Remove game from database
        /// </summary>
        public void DeleteGame(string gt, string ut)
        {
            if (String.IsNullOrEmpty(ut))
            {
                SetResponse("player");                      //403 Forbidden Response(invalid player token)
                return;
            }
            if (String.IsNullOrEmpty(gt))
            {
                SetResponse("gt");                      //403 Forbidden Response(invalid game token)
                return;
            }

            //Delete the game using a private method that uses the TransactionQuery Class
            DeleteGameQuery(gt, ut);
        }

        private void DeleteGameQuery(string gToken, string uToken)
        {
            //Create a Connection 
            MySqlConnection conn = new MySqlConnection(connectionString);
            TransactionQuery TQ = new TransactionQuery(conn);

            //Get the specific game from the DataBase
            TQ.addCommand("select count(*) from games where  GameToken = ?gameToken");
            TQ.addParms(new string[] { "gameToken" }, new string[] { gToken });
            TQ.addReadParms(new string[] { "count(*)" }, new bool[] { true });
            TQ.addConditions((result, command) =>
            {
                if (result.Dequeue().Equals("0"))
                {
                    SetResponse("gt");
                    return false;
                }
                else
                    return true;
            });

            //Check to make sure the gameStatus is  waiting
            TQ.addCommand("select GameState, Player1Token from games where GameToken = ?gameToken");
            TQ.addRepeatParm();
            TQ.addReadParms(new string[] { "GameState","Player1Token" }, new bool[] { false, true });
            TQ.addConditions((result, command) =>
            {
                if (!result.Dequeue().Equals("waiting"))
                {
                    SetResponse("nw");
                    return false;
                }
                if (result.Dequeue() != uToken)
                {
                    SetResponse("player");
                    return false;
                }

                Queue<string> returnQ;
                if (TQ.injectComand("update games set GameState = 'canceled' where GameToken = ?GameToken",
                    new string[,] { }, new string[] { }, false, out returnQ))
                    SetResponse("delete");
                return true;             
            });

            if (!TQ.Transaction())
            {
                SetResponse("ISE");
                return;
            }
            
        }

        /// <summary>
        /// Returns the Full game status of a game with the particular GAME ID
        /// </summary>
        public FullGame GetStatus(string gt)
        {
            if (gt == null)
            {
                //403 Misssing parameters
                SetResponse("NF");
                return null;
            }

            //Create a Connection and a CreateTransaction Query Object
            MySqlConnection conn = new MySqlConnection(connectionString);
            TransactionQuery TQ = new TransactionQuery(conn);
            Queue<string> returnQ;

            //Variables for holding the strings that the query returns.
            string player2Token = "", player1Token = "", board = "", startTime = "", 
                 score1="0",score2="0", p1Name="", p2Name="",status = "";
            int  duration = 0;
            int Limit = 0;
            List<WordScore> p1Words = new List<WordScore>();
            List<WordScore> p2Words = new List<WordScore>();

            // Query Games table for the status and the Player1Token of the given game token.
            TQ.addCommand("select GameState, Player1Token from games where GameToken = ?gameToken");
            TQ.addParms(new string[]{ "gameToken" }, new string[] { gt });
            TQ.addReadParms(new string[] { "GameState", "Player1Token" }, new bool[] { false, true });
            TQ.addConditions((result, command) =>
            {
                status = result.Dequeue();
                player1Token = result.Dequeue();

                TQ.injectComand("select Nickname from users where UserToken = ?player1Token",
                    new string[,] { { "player1Token", player1Token } }, new string[] { "Nickname" }, true, out returnQ);
                p1Name = returnQ.Dequeue();

                if (status == "waiting" || status == "cancelled")
                    return false;
                if (status == "finished")
                {
                    TQ.injectComand("select Word, Score from words where GameToken = ?gameToken and PlayerToken = ?player1Token",
                         new string[,] { }, new string[] { "Word", "Score" }, true, out returnQ);

                    int count = returnQ.Count / 2;
                    while (count > 0)
                    {
                        p1Words.Add(new WordScore() { word = returnQ.Dequeue(), score = Convert.ToInt32(returnQ.Dequeue()) });
                        count--;
                    }
                }
                return true;
            });

            TQ.addCommand("select Player2Token, Board, StartTime, Score1, Score2 from pairedGames where GameToken = ?gameToken");
            TQ.addReadParms(new string[] { "Player2Token", "Board", "StartTime", "Score1", "Score2" }, new bool[] { false, false, false, false, true });
            TQ.addRepeatParm();
            TQ.addConditions((result, command) =>
            {
                player2Token = result.Dequeue();
                board = result.Dequeue();
                startTime = result.Dequeue();
                score1 = result.Dequeue();
                score2 = result.Dequeue();
                Limit = Convert.ToInt32(TotalTime);
                GetTimeSpan(command, ref duration, ref status, startTime );


                TQ.injectComand("select Nickname from users where UserToken = ?player2Token",
                        new string[,] {{"player2Token", player2Token} }, new string[] {"Nickname" }, true, out returnQ);
                p2Name = returnQ.Dequeue();

                if (status == "finished")
                {
                    TQ.injectComand("select Word, Score from words where GameToken = ?gameToken and PlayerToken = ?player2Token",
                            new string[,] { }, new string[] { "Word", "Score" }, true, out returnQ);
                    int count = returnQ.Count / 2;
                    while (count > 0)
                    {
                        p2Words.Add(new WordScore() { word = returnQ.Dequeue(), score = Convert.ToInt32(returnQ.Dequeue()) });
                        count--;
                    }
                }
                return true;
            });
            
            if (!TQ.Transaction())
            {
                SetResponse("ISE");
                return null;
            }
            if(status=="")
            {
                SetResponse("gt");
                return null;
            }

            // Create return object from extracted data
            PlayerStatus ps1 = new PlayerStatus() { Score = Convert.ToInt32(score1), nickname = p1Name, wordsPlayed = p1Words };
            PlayerStatus ps2 = new PlayerStatus() { Score = Convert.ToInt32(score2), nickname = p2Name, wordsPlayed = p2Words };
            FullGame fg = new FullGame() { player1 = ps1, player2 = ps2, board = board, gameStatus = status, timeleft = duration, timelimit = Limit };
            appLog.WriteEntry("BoggleServer: FullGame successfully returned", EventLogEntryType.Information);
            return fg;
        }

        /// <summary>
        /// Returns the Brief game status of a game with a particular GAME ID
        /// </summary>
        public BriefGame GetBriefStatus(string gt)
        {
            if (gt == null)
            {
                //403 Misssing parameters
                SetResponse("NF");
                return null;
            }

            //Create a Connection and a CreateTransaction Query Object
            MySqlConnection conn = new MySqlConnection(connectionString);
            TransactionQuery TQ = new TransactionQuery(conn);

            //Variable for holding the status of the game, if status returns "", it means the queury didn't work
            string status = "";
            // Query Games table for the status of the given game token.
            TQ.addCommand("select GameState from games where GameToken = ?gameToken");
            TQ.addParms(new string[] { "gameToken" }, new string[] { gt });
            TQ.addReadParms(new string[] { "GameState" }, new bool[] { true });
            TQ.addConditions((result, command) =>
            {
                status = result.Dequeue();
                if (status == "waiting" || status == "finished")
                    return false;
                else
                    return true;
            });

            //Variables for keeping duration, score1 and score2.
            string startTime = "", sc1 = "0", sc2 = "0";
            int duration = 0;
            ///Query the Paried Game for GetBrief status, get the duration, score for player1 and player 2
            TQ.addCommand("select  StartTime, Score1, Score2 from pairedGames where GameToken = ?gameToken");
            TQ.addReadParms(new string[] { "StartTime", "Score1", "Score2" }, new bool[] { false, false, true });
            TQ.addRepeatParm();
            TQ.addConditions((result, command) =>
            {
                startTime = result.Dequeue();
                sc1 = result.Dequeue();
                sc2 = result.Dequeue();
                GetTimeSpan(command, ref duration, ref status, startTime);
                return true;
            });

            

            //Proccess the transactions
            if (!TQ.Transaction())
            {
                SetResponse("ISE");
                return null;
            }
            if (status == "")
            {
                SetResponse("gt");
                return null;
            }
            BriefGame bf = new BriefGame() { gameStatus = status, score1 = Convert.ToInt32(sc1), score2 = Convert.ToInt32(sc2), timeleft = duration };
            appLog.WriteEntry("BoggleServer: FullGame successfully returned", EventLogEntryType.Information);
            return bf;

        }


        /// <summary>
        /// Takes a word and adds it to a players list while increasing their score in a game with a particular game ID
        /// Returns the words value
        /// </summary>
        public WordValue PlayWord(PlayWord w)
        {
            if (w == null)
            {
                SetResponse("NF");                                              //403 forbidden missing parameters
                return null;
            }
            if (String.IsNullOrEmpty(w.playerToken))
            {
                SetResponse("player");                                          //403 Forbidden Response(invalid player token)
                return null;
            }
            if (String.IsNullOrEmpty(w.word))
            {
                SetResponse("word");                                          //403 Forbidden Response(invalid player token)
                return null;
            }
            if (String.IsNullOrEmpty(w.gameToken))
            {
                SetResponse("gt");                                              //403 Forbidden Response (invalid game token)
                return null;
            }
            return PlayWordQuery(w);
        }

        private int points;
        private WordValue PlayWordQuery(PlayWord w)
        {
            //Create a Connection 
            MySqlConnection conn = new MySqlConnection(connectionString);

            //CreateTransaction Query Object
            TransactionQuery TQ = new TransactionQuery(conn);

            int player = 0;
            ///Check for games with gametoken
            TQ.addCommand("select count(*) from games where GameToken = ?gameToken");
            TQ.addParms(new string[] { "gameToken" }, new string[] { w.gameToken });
            TQ.addReadParms(new string[] { "count(*)" }, new bool[] { true });
            TQ.addConditions((result, command) =>
            {
                if (result.Dequeue().Equals("0"))
                {
                    SetResponse("gt");
                    return false;
                }
                else
                {
                    Queue<string> returnQ;
                    TQ.injectComand("select Player1Token from games where GameToken = ?gameToken",
                        new string[,] { }, new string[] { "Player1Token" }, true, out returnQ);
                    if (returnQ.Dequeue() == w.playerToken)
                        player = 1;

                    return true;
                }
            });
            bool status = true;
            string gameState= "";
            ///Check if the gamestate is playing
            TQ.addCommand("select GameState from games where GameToken = ?gameToken");
            TQ.addParms(new string[] { }, new string[] { });
            TQ.addReadParms(new string[] { "GameState" }, new bool[] { true });
            TQ.addConditions((result, command) =>
            {
                gameState = result.Dequeue();
                if (gameState.Equals("playing"))
                    return true;
                else if(player == 1)
                {
                    SetResponse("waitingPlay");
                    status = false;
                    return false;
                }
                else
                {
                    SetResponse("wrongStatus");
                    status = false;
                    return false;
                }
            });


            string board;
            //Get the board and see if word can be played
            TQ.addCommand("select Player2Token, Board from pairedGames where GameToken = ?gameToken");
            TQ.addParms(new string[] {  }, new string[] { });
            TQ.addReadParms(new string[] { "Player2Token", "Board" }, new bool[] { false, true });
            TQ.addConditions((result, command) =>
            {
                if (result.Dequeue() == w.playerToken)
                    player = 2;
                board = result.Dequeue();
                BoggleBoard b = new BoggleBoard(board);
                points = WordLookUp(w.word, board);

                if (player == 0)
                {
                    SetResponse("player");
                    return false;
                }

                return true;    
            });
            int totalScore = 0;
            bool repeat = false;
            ///Check if word is already played 
            TQ.addCommand("select count(Word) from words where GameToken = ?gameToken and PlayerToken = ?playerToken");
            TQ.addParms(new string[] { "playerToken" }, new string[] { w.playerToken });
            TQ.addReadParms(new string[] { "Count(Word)" }, new bool[] { true });
            TQ.addConditions((result, command) =>
            {
                command.CommandText = "select Word, Score from words where GameToken = ?gameToken and PlayerToken = ?playerToken";
                if (command.ExecuteNonQuery() == 0)
                    throw new Exception("GET WORDS FAILED");
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetString("Word") == w.word)
                            points = 0;

                        totalScore += Convert.ToInt32(reader.GetString("Score"));
                    }
                }
                if (!repeat)
                {
                    totalScore += points;
                    command.CommandText = "update pairedGames set Score" + player + "=?Score where GameToken=?gameToken";
                    command.Parameters.AddWithValue("Score", totalScore);
                    if (command.ExecuteNonQuery() == 0)
                        throw new Exception("Update player score failed");
                    command.CommandText = "insert into words (GameToken, PlayerToken, Word, Score) values (?gameToken, ?pt, ?word, ?wordscore)";
                    command.Parameters.AddWithValue("pt", w.playerToken);
                    command.Parameters.AddWithValue("word", w.word);
                    command.Parameters.AddWithValue("wordscore", points);
                    if (command.ExecuteNonQuery() == 0)
                        throw new Exception("insert word score failed");
                    return true;
                }
                else return false;
            });

            if (!TQ.Transaction())
            {
                SetResponse("ISE");
                return null;
            }
            else
            {
                if (status)
                    return new WordValue() { wordScore = points };
                else
                    return null;
            }
            
        }


        /// <summary>
        /// Helper method to get the point value of the word in question
        /// </summary>
        private int WordLookUp(string word, string board)
        {
            char[] charArr = { };
            int value = 0;
            word = word.ToUpper();
            //BoggleBoard b = new BoggleBoard(board);                   
            //if (!b.CanBeFormed(word))                     //TODO check rules on this
            //    return -1;
            if (Words.Contains(word))                    //Check if word exists 
            {
                charArr = word.ToCharArray();           //Create char array
                switch (charArr.Length)                 //Assign point value based on array size
                {
                    case 1:
                    case 2:
                        break;                          //<3 letters = 0
                    case 3:
                    case 4:
                        value = 1;
                        break;                          //3-4 letters = 1
                    case 5:
                        value = 2;
                        break;                          //4 letters = 2
                    case 6:
                        value = 3;
                        break;                          //6 letters = 3
                    case 7:
                        value = 5;
                        break;                          //7 letters = 5
                    default:
                        value = 11;
                        break;                          //8+ letters = 11
                }
            }
            else
                value = -1;                             //If word does not exist value = -1
            return value;                               //return value

        }

        /// <summary>
        /// Sets the response code and description for eachcall
        /// </summary>
        private void SetResponse(String problem)
        {
            OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
            switch (problem)
            {
                case "player":
                    response.StatusCode = System.Net.HttpStatusCode.Forbidden;
                    response.StatusDescription = "Invalid Player token";
                    appLog.WriteEntry("BoggleServer: Invalid Player token", EventLogEntryType.Error);
                    return;
                case "same":
                    response.StatusCode = System.Net.HttpStatusCode.Conflict;
                    response.StatusDescription = "Invalid Partner Request";
                    appLog.WriteEntry("BoggleServer: Invalid Partner Request", EventLogEntryType.Error);
                    return;
                case "na":
                    response.StatusCode = System.Net.HttpStatusCode.Forbidden;
                    response.StatusDescription = "Not Authorized";
                    appLog.WriteEntry("BoggleServer: Not Authorized", EventLogEntryType.Error);
                    return;
                case "nw":
                    response.StatusCode = System.Net.HttpStatusCode.Conflict;
                    response.StatusDescription = "Game in Progress: Cancellation Request Denied";
                    appLog.WriteEntry("BoggleServer: Game in Progress: Cancellation Request Denied", EventLogEntryType.Error);
                    return;
                case "delete":
                    response.StatusCode = System.Net.HttpStatusCode.NoContent;
                    response.StatusDescription = "Game Deleted";
                    appLog.WriteEntry("BoggleServer: Game Deleted", EventLogEntryType.SuccessAudit);
                    return;
                case "gt":
                    response.StatusCode = System.Net.HttpStatusCode.Forbidden;
                    response.StatusDescription = "Invalid Game Token";
                    appLog.WriteEntry("BoggleServer: Invalid Game Token", EventLogEntryType.Error);
                    return;
                case "word":
                    response.StatusCode = System.Net.HttpStatusCode.Forbidden;
                    response.StatusDescription = "Invalid Word Request";
                    appLog.WriteEntry("BoggleServer: Invalid Word Request", EventLogEntryType.Error);
                    return;
                case "NF":
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    response.StatusDescription = "Missing Parameters";
                    appLog.WriteEntry("BoggleServer: Missing Parameters", EventLogEntryType.Error);
                    return;
                case "wrongStatus":
                    response.StatusCode = System.Net.HttpStatusCode.Forbidden;
                    response.StatusDescription = "Cannot Play word";
                    appLog.WriteEntry("BoggleServer: Cannot Play word. Game not playing", EventLogEntryType.Information);
                    return;
                case "waitingPlay":
                    response.StatusCode = System.Net.HttpStatusCode.Conflict;
                    response.StatusDescription = "Cannot Play word";
                    appLog.WriteEntry("BoggleServer: Cannot Play word. Game not playing", EventLogEntryType.Information);
                    return;

                case "ISE":
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    response.StatusDescription = "Internal Server Error";
                    appLog.WriteEntry("BoggleServer: query to database failure", EventLogEntryType.Information);
                    return;

                default:

                    return;
            }

        }

        private void GetTimeSpan(MySqlCommand command, ref int duration, ref string status, string startTime )
        {
            DateTime dt = Convert.ToDateTime(startTime);
            TimeSpan TS = DateTime.Now - dt;
            int Seconds_Span = TS.Seconds;
            int totalTime = Convert.ToInt32(TotalTime);
            if (Seconds_Span >= totalTime)
            {
                duration = 0;
                command.CommandText = "update games set GameState=?finished where GameToken=?gameToken";
                command.Parameters.AddWithValue("finished", "finished");
                if (command.ExecuteNonQuery() == 0)
                    throw new Exception("update gamestate failed");
                status = "finished";
            }
            else
                duration = totalTime - Seconds_Span;
            command.CommandText = "update pairedGames set Duration=?duration where GameToken=?gameToken";
            command.Parameters.AddWithValue("duration", duration);
            if (command.ExecuteNonQuery() == 0)
                throw new Exception("update duration failed");
        }


    }
}
