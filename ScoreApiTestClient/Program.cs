using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KinectLight.ScoreApi;

namespace ScoreApiTestClient
{
    public class Program
    {
        private static IScoreApi scoreApi;
        private static Uri baseUri = new Uri("http://176.58.105.78:3000");
        public static void Main(string[] args)
        {
            scoreApi = new ScoreClient();

            WriteSingleGame(); //WriteSingleUser är samma
            WriteDelimiter();
            
            WriteScoresForSingleGame();
            WriteDelimiter();
            
            WriteHighScoreForSingleGame();
            WriteDelimiter();
            
            WriteHighScore();
            WriteDelimiter();
            
            WriteUsers();
            WriteDelimiter();

            WriteScoreForUserInGame();
            WriteDelimiter();

            CreateGame();
            WriteDelimiter();

            PostScore();
            WriteDelimiter();
            Console.ReadLine();
        }

        private static void WriteSingleGame()
        {
            var singleGame = scoreApi.GetAsync<GameDto>(new Uri(baseUri,"/games/androidgame")).Result;
            Console.WriteLine(singleGame.name);
            Console.WriteLine();
        }

        private static void WriteScoresForSingleGame()
        {
            var score = scoreApi.GetAsync<List<ScoreDto>>(new Uri(baseUri, "/games/androidgame/score")).Result;
            score.ForEach(s => Console.WriteLine(s.game + ":" + s.points));
            Console.WriteLine();
        }

        private static void WriteHighScoreForSingleGame()
        {
            var highscore = scoreApi.GetAsync<List<HighscoreDto>>(new Uri(baseUri, "/games/androidgame/highscore")).Result;
            foreach(var h in highscore)
            {
                Console.WriteLine(h.user);
            }
        }

        private static void WriteScoreForUserInGame()
        {
            var score = scoreApi.GetAsync<List<ScoreDto>>(new Uri(baseUri, "/games/androidgame/score/marcus")).Result;
            score.ForEach(s => Console.WriteLine(s.game + ":" + s.points));
            Console.WriteLine();
        }

        private static void CreateGame()
        {
            Console.WriteLine("Creating game...");
            HttpResponseMessage createTask = scoreApi.PostAsync<GameDto>(new Uri(baseUri, "/games"), new GameDto { name = "kinectgame2", _id = "" }).Result;
            if (createTask.IsSuccessStatusCode)
                Console.WriteLine("Game created.");
            else
                Console.WriteLine("Failed to create game.");
        }

        private static void PostScore()
        {
            Console.WriteLine("Posting score...");
            HttpResponseMessage createTask = scoreApi.PostAsync<ScoreDto>(new Uri(baseUri, "/games/kinectgame2/score/anton"), new ScoreDto { points = "42", game = "kinectgame2" }).Result;
            if (createTask.IsSuccessStatusCode)
                Console.WriteLine("Score posted.");
            else
                Console.WriteLine("Failed to post score.");
        }

        private static void WriteDelimiter()
        {
            Console.WriteLine("---------");
        }


        public static void WriteUsers()
        {
            var users = scoreApi.GetAsync<List<UserDto>>(new Uri(baseUri, "/users")).Result;
            users.ForEach(u => Console.WriteLine("name:" + u.name + " _id:" + u._id));
        }

        public static void WriteHighScore()
        {
            var list = scoreApi.GetAsync<List<HighscoreDto>>(new Uri(baseUri, "/highscore")).Result;
            foreach (var highscore in list)
            {
                Console.WriteLine("name:" + highscore.user);
                highscore.game_score.ToList().ForEach(g => { Console.Write(g.game + ":" + g.points); });
                Console.WriteLine();
            }
        }

    }
}
