using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Boggle;
using System.IO;


namespace BoggleService
{
    public class BoggleService : IBoggleService  
    {
        public static  Dictionary<String, Player> Users = new Dictionary<String, Player>();     //Database of Users 
        public static Dictionary<String, Game> Games = new Dictionary<String, Game>();          //Database of games
        private List<string> Words = new List<string>();                                        //Populates List with words from dictionary

        //TODO need to get right file location this is temporary
        public BoggleService()
        {}

        /// <summary>
        /// Creates a new user and adds them to the user database
        /// returns the new USER ID
        /// </summary>
        public String MakeUser(Player p)
        {
            if (p == null)
            {
                SetResponse("NF");
                return null;
            }
            SetResponse("Success");
            Guid userID = Guid.NewGuid();               //New GUID for a Unique player ID
            p.userToken = userID.ToString();
            p.userToken = userID.ToString();
            //p.wordsPlayed = new Dictionary<string, int>();
            Users.Add(userID.ToString(), p);            //Add a User with unique ID to database
            return p.userToken;           
        }

        /// <summary>
        /// Finds a waiting game and adds the player to the game and returns Game ID
        /// Otherwise,
        /// Creates new game and adds it to the database
        /// returns the Game ID
        /// </summary>
        public String JoinGame(Player p)
        {
            if ( p == null)
            {
                SetResponse("NF");
                return null;
            }
            SetResponse("Success");                             //default response
            if (String.IsNullOrEmpty(p.userToken))
            {
                SetResponse("player");                          //403 Forbidden Response(invalid player token)
                return null;
            }
           // p.wordsPlayed = new Dictionary<string, int>();
            if (Games.Count > 0)                                //Check if there are games in the database
            {
                foreach (Game g in Games.Values)
                {
                    if (g.gameStatus == "Waiting")               //Check for Game with status "Waiting"
                    {
                        if (g.Player1.userToken == p.userToken)
                        {
                            SetResponse("same");                //409 Conflict Response(player already in game)
                            return null;
                        }
                        else
                        {
                            g.Player2 = p;
                            g.gameStatus = "Playing";            //Change gamestate to playing
                            return g.gameToken.ToString();
                        }
                    }
                }
                return NewGame(p);                              //If no game in waiting creates a new game
            }
            else
                return NewGame(p);                              //Make first game in database   
        }

        /// <summary>
        /// New game helper. Adds first player to game and then returns the Game ID
        /// </summary>
        private string NewGame(Player p)
        {
            if ( p == null)
            {
                SetResponse("NF");
                return null;
            }
            SetResponse("Success");
            Guid gameToken = Guid.NewGuid();
            BoggleBoard b = new BoggleBoard();
            Dictionary<string, int> player1w = new Dictionary<string, int>();
            Dictionary<string, int> player2w = new Dictionary<string, int>();

            //Creates a game in waiting for Player 1
            Game game = new Game(gameToken.ToString(), "Waiting", p, b.ToString(), player1w, player2w);           
            Games.Add(gameToken.ToString(), game);
            return gameToken.ToString();
            
        }

        /// <summary>
        /// Remove game from database
        /// </summary>
        public void DeleteGame(Delete game)
        {
            SetResponse("Success");                         //default response
            if (game.gameToken == null || game.userToken == null)
            {
                SetResponse("NF");
                return;
            }
            if (String.IsNullOrEmpty(p))
            {
                SetResponse("player");                      //403 Forbidden Response(invalid player token)
                return;
            }
            if (Games.ContainsKey(game.gameToken))                  //Check if game is in databse
            {
                Game g = Games[game.gameToken];
                if(!(g.Player1.userToken == game.userToken))
                {
                    SetResponse("na");                      //403 Forbidden Response (not authorized to cancel)
                    return;
                }
                if (g.gameStatus == "Waiting")
                {
                    g.gameStatus = "Deleted";                //assign gamestate to deleted
                    SetResponse("delete");
                    return;
                }
                else
                {
                    SetResponse("nw");                      //403 Forbidden Response (invalid gamestate)
                    return;
                }
            }
            else
            {
                SetResponse("gt");                          //403 Forbidden Response (invalid game token)
                return;
            }
        }
        

        /// <summary>
        /// Returns the Full game status of a game with the particular GAME ID
        /// </summary>
        public FullGame GetStatus(string gameId)
        {
            SetResponse("Success");
            if (String.IsNullOrEmpty(gameId))
            {
                SetResponse("gt");
                return null;
            }
            Game g;
            if (Games.ContainsKey(gameId))          //Get game from database
            {
                Games.TryGetValue(gameId, out g);
                return g.FG;                        //Return Full game status object
            }
            else
            {
                SetResponse("gt");
                return null;
            }
            
        }

        /// <summary>
        /// Returns the Brief game status of a game with a particular GAME ID
        /// </summary>
        public BriefGame GetBriefStatus(string gameId)
        {
            if (gameId == null )
            {
                SetResponse("NF");
                return null;
            }
            SetResponse("Success");
            Game g;
            if (Games.ContainsKey(gameId))          //Get game from database
            {
                Games.TryGetValue(gameId, out g);
                return g.BG;                        //Return Brief game status
            }
            else
            {
                SetResponse("gt");
                return null;
            }
        }

        /// <summary>
        /// Takes a word and adds it to a players list while increasing their score in a game with a particular game ID
        /// Returns the words value
        /// </summary>
        public int? PlayWord(PlayWord w)
        {
            if ( w == null) 
            {
                SetResponse("NF");
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

            SetResponse("Success");
            int value = WordLookUp(w.word);
            Game game;
            if (Games.TryGetValue(w.gameToken, out game))                                 //Find game
            {
                BoggleBoard bb = new BoggleBoard(game.board);
                if (!bb.CanBeFormed(w.word))                                                           //TODO check to make sure its not negative points
                    value = 0;
                if (game.Player1.userToken == w.playerToken)                                           //Check if player is Player1
                {
                    if (value > 0 || value < 1)                                                        //If word value is greater than 0
                        if (!game.Player1Words.ContainsKey(w.word))                                    //If Players word list does not contain the word being played
                        {
                            game.Player1Words.Add(w.word, value);                                      //Add it to player word list
                            //game.Player1.wordsPlayed.Add(word, value);                               //Add to word to player words played list
                            //WordListComparer(g.gameToken);                                           //Check if Player lists are unique and removes like words
                        }
                }
                else if (game.Player2.userToken == w.playerToken)                                      //Repeat previous if the player is player two
                {
                    if (value > 0 || value < 1)
                        if (!game.Player2Words.ContainsKey(w.word))
                        {
                            game.Player2Words.Add(w.word, value);
                            //game.Player2.wordsPlayed.Add(word, value);                               //Add to words to word played
                            //WordListComparer(g.gameToken);
                        }
                }
                else
                {
                    SetResponse("player");                                      //403 Forbidden Response(invalid player token)
                    return null;
                }
            }                
            return value;
        }

        /// <summary>
        /// Helper method to get the point value of the word in question
        /// </summary>
        private int WordLookUp(string word)
        {
            char[] charArr = { };               
            int value = 0;
            word = word.ToUpper();
           // if()
            if(Words.Contains(word))                    //Check if word exists 
            {                                           //TODO add a way to check if word exists on board!!!
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
                    return;
                case "same":
                    response.StatusCode = System.Net.HttpStatusCode.Conflict;
                    response.StatusDescription = "Invalid Partner Request";
                    return;
                case "na":
                    response.StatusCode = System.Net.HttpStatusCode.Forbidden;
                    response.StatusDescription = "Not Authorized";
                    return;
                case "nw":
                    response.StatusCode = System.Net.HttpStatusCode.Conflict;
                    response.StatusDescription = "Game in Progress: Cancellation Request Denied";
                    return;
                case "delete":
                    response.StatusCode = System.Net.HttpStatusCode.NoContent;
                    response.StatusDescription = "Game Deleted";
                    return;
                case "gt":
                    response.StatusCode = System.Net.HttpStatusCode.Forbidden;
                    response.StatusDescription = "Invalid Game Token";
                    return;
                case "word":
                    response.StatusCode = System.Net.HttpStatusCode.Forbidden;
                    response.StatusDescription = "Invalid Word Request";
                    return;
                case "NF":
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    response.StatusDescription = "Missing Parameters";
                    return;

                default:

                    return;
            }
          
        }
        public Dictionary<string, Player> GetUsers()
        {
            return Users;
        }

        public Dictionary<string, Game> GetGames()
        {
            return Games;
        }
    }
    
}
