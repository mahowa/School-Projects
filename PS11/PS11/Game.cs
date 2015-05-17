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
        public Game(string GT, string GS, Player P1, string GB)       //Constuct a new game object
           
        {
            _FG = new FullGame() { player1 = new PlayerStatus(), player2 = new PlayerStatus() };
            _BG = new BriefGame();
            _Player2 = new Player() { nickname = string.Empty };
            gameToken = GT;
            gameStatus = GS;
            Player1 = P1;
            board = GB;
            timelimit = 120;
            timeleft = 120;
            Player1.Score = 0;
        }

        private Stopwatch stopWatch = new Stopwatch();
        private BriefGame _BG;
        public BriefGame BG                         //Creates a brief game summery
        {
            get
            {
                _BG.gameStatus = gameStatus;
                _BG.score1 = Player1.Score;
                _BG.score2 = Player2.Score;
                _BG.timeleft = timeleft;
                return _BG;
            }
        }

        private FullGame _FG; 
        
        public FullGame FG                          //Creates a full game summery
        {
            get
            {
                _FG.gameStatus = gameStatus;
                _FG.timeleft = timeleft;

                _FG.player1.Score = Player1.Score;;
                _FG.player2.Score = Player2.Score;

                if (gameStatus == "finished")
                {
                    _FG.player1.wordsPlayed = Player1Words;
                    _FG.player2.wordsPlayed = Player2Words;
                }
                else
                {
                    _FG.player1.wordsPlayed = new List<WordScore>();
                    _FG.player2.wordsPlayed = new List<WordScore>();
                }

                _FG.board = board;
                _FG.timeleft = timeleft;
                _FG.timelimit = timelimit;
                _FG.board = board;
                _FG.player1.nickname = Player1.nickname;
                _FG.player2.nickname = Player2.nickname;
                return _FG;
            }
        }


        public string gameToken { get; private set; }         //Unique game token

        public string gameStatus { get; set; }                             // "waiting" , "playing", "finished", "canceled"

        private Player _Player1;
        public Player Player1                               //Player one unique token
        {
            get
            {
                int total = 0;                              //Recalculate Player 1 score
                if (gameStatus == "playing" || gameStatus == "finished")
                {
                    if (Player1Words != null)
                        if (Player1Words.Count > 0)
                            foreach (var pw in Player1Words)
                                total = total + pw.score;

                }
                _Player1.Score = total;
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
                if (gameStatus == "playing" || gameStatus == "finished")
                    if (Player2Words != null)
                        if (Player2Words.Count > 0)
                            foreach (WordScore pw in Player2Words)
                                total = total + pw.score;

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
                if (gameStatus == "waiting" || gameStatus == "canceled")
                    return String.Empty;
                return _GameBoard;
            }
            set
            {_GameBoard = value;}
        }

        private int _TimeLimit;
        public int timelimit                                //Represents time limit in "seconds"
        {
            get
            {
                if (gameStatus == "waiting" || gameStatus == "canceled")
                    return 0;
                return _TimeLimit;
            }
            set { _TimeLimit = value; }
        }

        private int _TimeLeft;

        public int timeleft                           //Represents time remaining in "seconds"
        {
            get
            {
                if (gameStatus == "playing")
                {
                    if (!stopWatch.IsRunning)
                        stopWatch.Start();

                    _TimeLeft = (int)((_TimeLimit * 1000) - stopWatch.ElapsedMilliseconds) / 1000;
                }
                else return 0;
                if (_TimeLeft < 0)
                    _TimeLeft = 0;
                if (_TimeLeft == 0 && gameStatus == "playing")
                {
                    stopWatch.Stop();
                    gameStatus = "finished";
                }

                return _TimeLeft;
            }
            private set { _TimeLeft = value; }            


        }

        private List<WordScore> Player1Words { get; set; }
        private List<WordScore> Player2Words{get;set;}
    }
}