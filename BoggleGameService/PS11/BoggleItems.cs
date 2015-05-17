using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Web;
using System.Timers;
using System.Diagnostics;

namespace BoggleService
{
    

    [DataContract]
    public class Player
    {
        [DataMember]
        public string nickname { get; set; }                        //Nickname of the player

        [DataMember]
        public string userToken { get; set; }                     //Unique player ID

        [DataMember]
        public int Score { get; set; }                              //Player score

    }


    [DataContract]
    public class FullGame
    {
        [DataMember]
        public string gameStatus { get; set; }           // "waiting" , "playing", "finished", "canceled"\

        [DataMember]
        public PlayerStatus player1 { get; set; }           //Player one score
        [DataMember]
        public PlayerStatus player2 { get; set; }           //Player two score

        [DataMember]
        public string board { get; set; }           //Represents letters on the board

        [DataMember]
        public int timelimit { get; set; }              //Represents time limit in "seconds"

        [DataMember]
        public int timeleft { get; set; }           //Represents time remaining in "seconds"

    }

    [DataContract]
    public class BriefGame
    {
        [DataMember]
        public string gameStatus { get; set; }           // "waiting" , "playing", "finished", "canceled"\

        [DataMember]
        public int timeleft { get; set; }            //Represents time remaining in "seconds"
        
        [DataMember]
        public int score1 { get; set; }           //Player one score
        [DataMember]
        public int score2 { get; set; }           //Player two score
    }

    [DataContract]
    public class PlayWord
    {
        [DataMember]
        public string word { get; set; }

        [DataMember]
        public string gameToken { get; set; }

        [DataMember]
        public string playerToken { get; set; }
    }

    [DataContract]
    public class PlayerStatus
    {
        [DataMember]
        public string nickname { get; set; }                        //Nickname of the player

        [DataMember]
        public int Score { get; set; }                              //Player score

        [DataMember]
        public List<WordScore> wordsPlayed { get; set; }                              //Words played


    }

    [DataContract]
    public class WordScore
    {
                                            //word: string, score: int
        [DataMember]
        public string word { get; set; }

        [DataMember]
        public int score { get; set; }
    }

    [DataContract]
    public class UserToken
    {
        [DataMember]
        public string userToken { get; set; }
    }

    [DataContract]
    public class GameToken
    {
        [DataMember]
        public string gameToken { get; set; }
    }

    [DataContract]
    public class WordValue
    {
        [DataMember]
        public int wordScore { get; set; }
    }


}