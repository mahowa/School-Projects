using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using System.Dynamic;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text;
using Boggle;
using System.IO;

namespace BoggleService
{
    /// <summary>
    /// Provides a way to start and stop the IIS web server from within the test
    /// cases.  If something prevents the test cases from stopping the web server,
    /// subsequent tests may not work properly until the stray process is killed
    /// manually.
    /// </summary>
    public class IISAgent
    {
        // Reference to the running process
        private static Process process = null;

        // Location of IIS Express
        private const string IIS_EXECUTABLE = @"C:\Program Files \IIS Express\iisexpress.exe";

        /// <summary>
        /// Starts IIS
        /// </summary>
        public static void Start(string arguments)
        {
            if (process == null)
            {
                ProcessStartInfo info = new ProcessStartInfo(IIS_EXECUTABLE, arguments);
                info.WindowStyle = ProcessWindowStyle.Minimized;
                process = Process.Start(info);
            }
        }

        /// <summary>
        ///  Stops IIS
        /// </summary>
        public static void Stop()
        {
            if (process != null)
            {
                process.Kill();
            }
        }
    }

    [TestClass]
    public class BoggleServiceTest
    {


        /// <summary>
        /// Creates a generic client for communicating with the ToDoList service.
        /// Your port number may differ and will need to be changed.
        /// </summary>
        public static HttpClient CreateClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:3000/");
            return client;
        }

        /// <summary>
        /// Creates user with the nickname parameter and 
        /// returns new user unique user id
        /// </summary>
        public static async Task<UserToken> MakeUser(Player p)
        {
            using (HttpClient client = CreateClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("/Boggle.svc/makeuser", content);
                if (response.IsSuccessStatusCode)
                {
                    String result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<UserToken>(result);
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Adds player to game and 
        /// returns the games unique game id
        /// </summary>
        public static async Task<GameToken> JoinGame(Player p1)
        {
            using (HttpClient client = CreateClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(p1), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("/Boggle.svc/joingame", content);
                if (response.IsSuccessStatusCode)
                {
                    String result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GameToken>(result);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Deletes specific game and 
        /// returns true if successfull
        /// </summary>
        public static async Task<bool> DeleteGame(string gt, string ut)
        {
            using (HttpClient client = CreateClient())
            {
                String url = String.Format("/Boggle.svc/joingame/{0}/{1}", gt, ut);
                HttpResponseMessage response = await client.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Returns the full status of a game
        /// </summary>
        public static async Task<FullGame> GetStatus(string gameId)
        {
            using (HttpClient client = CreateClient())
            {
                String url = String.Format("/Boggle.svc/status?gameToken={0}", gameId);
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    String result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<FullGame>(result);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns the Brief Status of a game
        /// </summary>
        public static async Task<BriefGame> GetBriefStatus(string gameId)
        {
            using (HttpClient client = CreateClient())
            {
                String url = String.Format("/Boggle.svc/briefstatus?gameToken={0}", gameId);
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    String result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<BriefGame>(result);
                }
                else
                    return null;
            }
        }
        /// <summary>
        /// Adds the word to the players played word list and score and 
        /// returns the words score
        /// </summary>
        public static async Task<WordValue> PlayWord(PlayWord w)
        {
            using (HttpClient client = CreateClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(w), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("/Boggle.svc/playword", content);
                if (response.IsSuccessStatusCode)
                {
                    String result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<WordValue>(result);
                }
                else
                    return null;
            }
        }


        /// <summary>
        /// This is automatically run prior to all the tests to start the server
        /// </summary>
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            IISAgent.Start("/site:\"BoggleService\" /apppool:\"Clr4IntegratedAppPool\"");
            string filesdir = Directory.GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).ToString()).ToString()).ToString();
            string dictionary = filesdir + @"\dictionary.txt";
            string boggle = filesdir + @"\boggle.xml";
            File.Copy(boggle,"boggle.xml",true);
            File.Copy(dictionary, "dictionary.txt",true);

        }
                        

        /// <summary>
        /// This is automatically run when all tests have completed to stop the server
        /// </summary>
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            IISAgent.Stop();
        }

        [TestMethod]
        public void ComplexTest()
        {
            Player player1 = new Player() { nickname = "test" };
            player1.userToken = MakeUser(player1).Result.userToken;

            Player player2 = new Player() { nickname = "test3" };
            player2.userToken = MakeUser(player2).Result.userToken;

            String gameToken = JoinGame(player1).Result.gameToken;
            BriefGame bg = GetBriefStatus(gameToken).Result;
            Assert.AreEqual(bg.gameStatus, "waiting");

            Assert.AreEqual(gameToken, JoinGame(player2).Result.gameToken);
            Assert.AreEqual(GetBriefStatus(gameToken).Result.gameStatus, "playing");
        }
        private const int USERS = 10;
        private const int WORDS = 30;
        private string[] playWords1 = { "crayon", "ear", "car", "cade", "cades", "dee", "dees", "rag", "rags", "cry", "car", "help", "stop", "for", "at", "step", "ads", "fig", "drags", "drag", "cay", "cray", "life", "dog", "cat", "kite","fad","are","ant","pet" };
        private int? score = 0;
        private int? score2 = 0;
        private const int GAMES = 5;
        public string gameboard = "cdeerasoygfushis";
        // NOTE: The two tests cannot be run simultaneously, since they are accessing
        // the same server.

        /// <summary>
        /// Takes 2 minutes to complete. Tests the data structure to make sure everythin works
        /// internally.
        /// </summary>
        [TestMethod]
        public void SimpleDataStructureTest()
        {
            BoggleService Boggle = new BoggleService();
            Player[] uT = new Player[USERS];
            string[] gT = new string[GAMES];
            for (int i = 0; i < USERS; i++)
            {
                string nn = i.ToString();
                Player p = new Player { nickname = nn };
                uT[i] = new Player() { userToken = MakeUser(p).Result.userToken };
            }
            int gc = 0;                 //counter
            int pc = 0;
            foreach (Player tempP in uT)
            {
                if (pc % 2 == 0)
                {
                    gT[gc] = JoinGame(tempP).Result.gameToken;
                    Assert.AreEqual(GetBriefStatus(gT[gc]).Result.gameStatus, "waiting");
                    Assert.AreEqual(GetStatus(gT[gc]).Result.gameStatus, "waiting");
                    Assert.AreEqual(GetStatus(gT[gc]).Result.player1.Score, 0);
                    Assert.AreEqual(GetStatus(gT[gc]).Result.board, "");
                    gc++;
                }
                else
                {
                    string gt = JoinGame(tempP).Result.gameToken;
                    Assert.AreEqual(GetBriefStatus(gT[gc-1]).Result.gameStatus, "playing");
                    Assert.AreEqual(GetStatus(gT[gc - 1]).Result.gameStatus, "playing");
                }
                pc++;
            }

            for (int i = 0; i < 30; i++)
            {
                int uc = 0;                 //counter
                foreach (string s in gT)
                {
                    if (GetBriefStatus(s).Result.gameStatus == "playing")
                    {
                        BoggleBoard boggleboard = new BoggleBoard(GetStatus(s).Result.board);
                        gameboard = GetStatus(s).Result.board;
                        PlayerStatus p1 = GetStatus(s).Result.player1;
                        PlayWord pw = new PlayWord() { gameToken = s, playerToken = uT[uc].userToken, word = playWords1[i] };
                        uT[uc].Score = uT[uc].Score + (int)PlayWord(pw).Result.wordScore;
                    }
                    else
                        Assert.Fail("Game in waiting");
                    Assert.AreEqual(GetStatus(s).Result.player1.Score, uT[uc].Score);
                    Assert.AreEqual(GetBriefStatus(s).Result.score1, uT[uc].Score);
                    uc = uc + 2;
                }
            }
        }
        [TestMethod]
        public void DeleteTest()
        {
            BoggleService Boggle = new BoggleService();
            Player[] uT = new Player[USERS];
            string[] gT = new string[10];
            for (int i = 0; i < USERS; i++)
            {
                string nn = i.ToString();
                Player p = new Player { nickname = nn };
                uT[i] = new Player() { userToken = MakeUser(p).Result.userToken };
            }
            int gc = 0;                 //counter
            foreach (Player tempP in uT)
            {
                gT[gc] = JoinGame(tempP).Result.gameToken;
                string gamestatus = GetStatus(gT[gc]).Result.gameStatus;
                Assert.AreEqual(gamestatus, "waiting");
                if (!DeleteGame(gT[gc], tempP.userToken).Result)
                    Assert.Fail("couldnt delete game");
                gamestatus = GetStatus(gT[gc]).Result.gameStatus;
                Assert.AreEqual(gamestatus, "canceled");
                Assert.AreEqual(GetStatus(gT[gc]).Result.gameStatus, "canceled");
                gc++;
            }
        }
        [TestMethod]
        public void ResponseTest()
        {
            Player p = null;
            string gt;
            string ut = MakeUser(p).Result.userToken;
            if (!String.IsNullOrEmpty(ut))
                Assert.Fail("Not failing");

            gt = JoinGame(p).Result.gameToken;
            if (!String.IsNullOrEmpty(gt))
                Assert.Fail("Not failing");

            p = new Player() { userToken = null };
            gt = JoinGame(p).Result.gameToken;
            if (!String.IsNullOrEmpty(gt))
                Assert.Fail("Not failing");

            p = new Player() { userToken = "0" };
            gt = JoinGame(p).Result.gameToken;
            if (!String.IsNullOrEmpty(gt))
                Assert.Fail("Not failing");
            p = new Player() { nickname = "test" };
            ut = MakeUser(p).Result.userToken;
            p.userToken = ut;
            gt = JoinGame(p).Result.gameToken;
            gt = JoinGame(p).Result.gameToken;
            if (!String.IsNullOrEmpty(gt))
                Assert.Fail("Not failing");
            //TODO these tests are not working need to figure out why???
            if (DeleteGame("", null).Result)
                Assert.Fail("delete game nulll user token respones not working");
            if (DeleteGame( null,"").Result)
                Assert.Fail("delete game nulll user game respones not working");
            if (DeleteGame("", "").Result)
                Assert.Fail("delete game couldnt find gametoken in games list response not working");

            FullGame fg = GetStatus(null).Result;
            if (fg != null)
                Assert.Fail("full game null respones not working");

            fg = GetStatus("").Result;
            if (fg != null)
                Assert.Fail("full game not game with game token respones not working");

            BriefGame bg = GetBriefStatus(null).Result;
            if (bg != null)
                Assert.Fail("brief game null respones not working");

            bg = GetBriefStatus("").Result;
            if (bg != null)
                Assert.Fail("brief game not game with game token respones not working");

            PlayWord w = null;
            int? pw = PlayWord(w).Result.wordScore;
            if (pw != null)
                Assert.Fail("playword respones failing");
            w = new PlayWord() { playerToken = "" };
            pw = PlayWord(w).Result.wordScore;
            if (pw != null)
                Assert.Fail("playword respones failing");
        }

    }
}
