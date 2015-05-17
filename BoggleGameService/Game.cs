using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Web;
using System.Timers;
using System.Diagnostics;

namespace BoggleService
{
    /// <summary>
    /// This is the data format that is used to represent items in the ToDo list
    /// in both the server and the client.
    /// </summary>

    public class Game
    {
        public Game(string GT, string GS, Player P1, string GB,
            Dictionary<String, int> P1W, Dictionary<String, int> P2W)       //Constuct a new game object
        {
            gameToken = GT;
            gameStatus = GS;
            Player1 = P1;
            board = GB;
            Player1Words = P1W;
            Player2Words = P2W;
            timelimit = 120;
            timeleft = 120;
            Player1.Score = 0;
            Player1Words = new Dictionary<string, int>();
            Player2Words = new Dictionary<string, int>();
        }

        private Stopwatch stopWatch = new Stopwatch();
        private BriefGame _BG = new BriefGame();
   
        public BriefGame BG                         //Creates a brief game summery
        {
            get
            {
                _BG.gameStatus = gameStatus;
                _BG.score1 = Score1;
                _BG.score2 = Score2;
                _BG.timeleft = timeleft;
                return _BG;
            }
        }

        private FullGame _FG = new FullGame();
        
        public FullGame FG                          //Creates a full game summery
        {
            get
            {
                _FG.gameStatus = gameStatus;
                _FG.timeleft = timeleft;

                //_FG.player1.nickname = Player1.nickname;
                _FG.player1.Score = Player1.Score;

                //_FG.player2.nickname = Player2.nickname;
                _FG.player2.Score = Player2.Score;

                if (gameStatus == "Finished")
                {
                    _FG.player1.wordsPlayed = Player1Words;
                    _FG.player2.wordsPlayed = Player2Words;
                }
                else
                {
                    _FG.player1.wordsPlayed = new Dictionary<string, int>();
                    _FG.player2.wordsPlayed = new Dictionary<string, int>();
                }

                _FG.board = board;
                _FG.timeleft = timeleft;
                _FG.timelimit = timelimit;
                _FG.board = board;
                return _FG;
            }
        }


        public string gameToken { get; private set; }         //Unique game token

        //TODO When gamestate is set start the timer
        public string gameStatus { get; set; }                             // "Waiting" , "Playing", "Finished", "Canceled"

        private Player _Player1;

        public Player Player1                               //Player one unique token
        {
            get
            {
                int total = 0;                              //Recalculate Player 1 score
                //TODO check which score is important Unique list or total words played???
                if (gameStatus == "Playing" || gameStatus == "Finished")
                {
                    if (Player1Words != null)
                        if (Player1Words.Count > 0)
                            foreach (int score in Player1Words.Values)
                                total = total + score;

                }
                _Player1.Score = total;
                _FG.player1.nickname = _Player1.nickname;
                return _Player1;
            }
            private set
            {
                _Player1 = value;
            }
        }

        private Player _Player2;

        public Player Player2                                //Player two unique token
        {
            get
            {
                int total = 0;                             //Recalculate Player 2 score
                //TODO check which score is important Unique list or total words played???
                if (gameStatus == "Playing" || gameStatus == "Finished")
                    if (Player2Words != null)
                        if (Player2Words.Count > 0)
                            foreach (int score in Player2Words.Values)
                                total = total + score;

                _Player2.Score = total;
                _FG.player2.nickname = _Player2.nickname;
                return _Player2;
            }
            set { _Player2 = value; }
        }

        private string _GameBoard;

        public string board                             //Represents letters on the board
        {
            get
            {
                if (gameStatus == "Waiting" || gameStatus == "Canceled")
                    return String.Empty;
                return _GameBoard;
            }
            set
            {
                _GameBoard = value;
            }
        }

        private int _TimeLimit;

        public int timelimit                                //Represents time limit in "seconds"
        {
            get
            {
                if (gameStatus == "Waiting" || gameStatus == "Canceled")
                    return 0;
                return _TimeLimit;
            }
            private set { _TimeLimit = value; }
        }

        private double _TimeLeft;

        public double timeleft                           //Represents time remaining in "seconds"
        {
            get
            {
                if (gameStatus == "Playing")
                {
                    if (!stopWatch.IsRunning)
                        stopWatch.Start();
                    _TimeLeft = 120000 - stopWatch.ElapsedMilliseconds;
                }
                if (_TimeLeft < 0)
                    _TimeLeft = 0;
                if (_TimeLeft == 0 && gameStatus == "Playing")
                {
                    stopWatch.Stop();
                    gameStatus = "Finished";
                }
                return _TimeLeft;
            }
            private set { _TimeLeft = value; }            //TODO need to check requirments for gamestate time left


        }


        public int Score1 { get { return Player1.Score; } }                  //Player one score

        public int Score2 { get { return Player2.Score; } }             //Player two score

        public Dictionary<String, int> Player1Words { get; set; }       //Player one list of word score pair


        public Dictionary<String, int> Player2Words { get; set; }       //Player two list of word score pair

    }
}