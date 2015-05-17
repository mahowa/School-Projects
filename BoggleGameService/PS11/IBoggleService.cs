using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace BoggleService
{
    /// <summary>
    /// The Boggle server keeps track of two sets: a set of users and a set of games.
    /// A user consists of a nickname and a user token.  No two users can share the same token.  A nickname can be any string,
    /// such as "Joe" or "Spike".  A user token must be a string that is extremely difficult to guess.  This implies that it
    /// must be long and randomly generated.  (A good way to generate a token is with Guid.NewGuid.)
    /// A game represents the state of a single Boggle game.
    /// All games have:
    ///  1.  A unique game token.  No two games can share a token.  (Again, Guid.NewGuid is a good way to generate tokens.)
    ///  2.  A game state.  The four possible game states are "waiting" (only one player has joined and the game has
    ///      not been canceled); "playing" (two players have joined a game and time has not yet expired); "finished"
    ///      (two players have joined a game and the time has expired); and "canceled" (only one player has joined
    ///      a game and it has been canceled).
    ///      At most one game can be in the "waiting" state at any given time.  Any number of games may be "playing",
    ///      "finished", or "canceled".
    ///  3.  The user token of the first player to join a game (player 1).
    /// Only games that are "playing" or "finished" have:
    ///  4.  The user token of the second player to join a game (player 2).
    ///  5.  The 16-character string that represents the contents of the game board.
    ///  6.  The time limit of the game, in seconds.
    ///  7.  The time remaining in the game, in seconds.
    ///  8.  The score in the game of player 1
    ///  9.  The score in the game of player 2
    /// 10.  A list of word/score pairs for player 1.  (A word/score pair consists of a word that was played and the
    ///      score received for that word.  The words should appear in the order in which they were played.)
    /// 11.  A list of word/score pairs for player 2.
    /// </summary>
    [ServiceContract]
    public interface IBoggleService
    {
        /// <summary>
        /// Creates a new user and returns the new users
        /// unique token
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/makeuser",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        UserToken MakeUser(Player p);

        /// <summary>
        /// If the user token is invalid, responds with response code 403 (Forbidden).
        /// Otherwise, if there is currently a "waiting" game and the specified player is also player 1 in that game,
        /// responds with response code 409 (Conflict).
        /// Otherwise, if there is currently a "waiting" game, adds the specified player as player 2.  Returns the game's
        /// unique token.
        /// Otherwise, creates a new game that has the specified player 1.  Returns the new game's unique token.
        ///// </summary>
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/joingame",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        GameToken JoinGame(Player p);

        /// <summary>
        /// If the user token is invalid, the game token is invalid, or the user is not a player in the game,
        /// responds with response code 403 (Forbidden).
        /// Otherwise, if the specified game is not "waiting", responds with response code 409 (Conflict).
        /// Otherwise, cancels the specified game and responds with response code 204 (No Content).
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/joingame/{userToken}/{gameToken}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        void DeleteGame(string gameToken, string userToken);

        /// <summary>
        /// If the gameToken is missing or invalid, responds with response code 403 (Forbidden).
        /// Otherwise, returns the structure illustrated below, where:
        ///   1.  gameStatus is one of "waiting", "playing", "finished", or "canceled"
        ///   2.  board is either the empty string (if the state is "waiting" or "canceled") or is the 16-character
        ///       board state (otherwise)
        ///   3.  timelimit is either 0 (if the state is "waiting" or "canceled") or is the length of the game in
        ///       seconds (otherwise)
        ///   4.  timeleft is either the time in seconds remaining in the game (if the state is "playing"), or is
        ///       0 (otherwise)
        ///   5.  player1.nickname is the nickname of player 1
        ///   6.  player1.score is either 0 (if the state is "waiting" or "canceled") or is the score of player 1 (otherwise)
        ///   7.  player1.wordsPlayed is either the word/score list for player 1 (if the state is "finished") or is
        ///       an empty list (otherwise)
        ///   8.  player2.nickname is either the empty string (if the state is "waiting" or "canceled") or is the
        ///       nickname of player 2 (otherwise).
        ///   9.  player2.score is either 0 (if the state is "waiting" or "canceled") or is the score of player 2 (otherwise)
        ///  10.  player2.wordsPlayed is either the word/score list for player 2 (if the state is "finished") or is
        ///       an empty list (otherwise)
        ///Otherwise, returns the second structure illustrated below, where:
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET",
            UriTemplate = "/status?gameToken={gameId}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        FullGame GetStatus(string gameId);

        /// <summary>
        ///If the gameToken is missing or invalid, responds with response code 403 (Forbidden).
        ///Otherwise, returns the structure illustrated below, where:
        ///   1.  gameStatus is one of "waiting", "playing", "finished", or "canceled"
        ///   2.  timeleft is either the time in seconds remaining in the game (if the state is "playing"), or is
        ///       0 (otherwise)
        ///   3.  score1 is either 0 (if the state is "waiting" or "canceled") or is player 1's score (otherwise)
        ///   4.  score2 is either 0 (if the state is "waiting" or "canceled") or is player 2's score (otherwise)
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET",
            UriTemplate = "/briefstatus?gameToken={gameId}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
       BriefGame GetBriefStatus(string gameId);

        /// <summary>
        /// If the game token or the player token is missing or invalid, or if the player is not a participant in
        /// the game, responds with response code 403 (Forbidden).
        ///Otherwise, if the game state is anything other than "playing", responds with response code 409 (Conflict).
        ///Otherwise, records the word as being played by the player and returns the structure illustrated below, where:
        ///    1.  wordScore is the value of the word
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/playword",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle =  WebMessageBodyStyle.Bare)]
        WordValue PlayWord(PlayWord w);

       }

}
