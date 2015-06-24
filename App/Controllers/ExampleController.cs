using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;
using App.Models;
using Newtonsoft.Json.Linq;

namespace App.Controllers
{
    public class ExampleController : ApiController
    {

        [Route("move"), HttpGet]
        public HttpResponseMessage OurMove()
        {
            WebApiApplication.CurrentRound++;
            var ourMoveToPlay = Moves.Paper;
            try
            {

               ourMoveToPlay = RandomMoveToPlay();

                    if (WebApiApplication.Game.Enemy.Name.ToLower() == "fatbotslim")
                        if (WebApiApplication.Game.OpponentMoves.LastOrDefault() ==
                            WebApiApplication.Game.OurMoves.LastOrDefault())
                            ourMoveToPlay = Moves.Water;

                    if (WebApiApplication.Game.Enemy.Name.ToLower() != "fatbotslim")
                        if (WebApiApplication.Game.OpponentMoves.LastOrDefault() ==
                            WebApiApplication.Game.OurMoves.LastOrDefault())
                            ourMoveToPlay = Moves.Dynamite;

                    if (WebApiApplication.Game.Enemy.Name.ToLower() == "botulism")
                        ourMoveToPlay = Moves.Scissors;

                    if (WebApiApplication.Game.Enemy.Name.ToLower() == "botswana")
                        ourMoveToPlay = Moves.Paper;


                    if (WebApiApplication.Game.Enemy.Name.ToLower() == "hackdaybase")
                    {
                        if (WebApiApplication.Game.Us.Dynamite > 0)
                            ourMoveToPlay = Moves.Dynamite;
                        else
                        {
                            ourMoveToPlay = RandomMoveToPlay();
                        }
                    }

                    WebApiApplication.Game.OurMoves.Add(ourMoveToPlay);


                    if (ourMoveToPlay == Moves.Dynamite)
                    {
                        WebApiApplication.Game.Us.Dynamite--;
                    }
                    if (ourMoveToPlay == Moves.Dynamite && WebApiApplication.Game.Us.Dynamite == 0)
                    {
                        ourMoveToPlay = RandomMoveToPlay();
                    }
                
                return new HttpResponseMessage()
                {
                    Content = new StringContent(ourMoveToPlay)
                };
            }

            catch (Exception ex)
            {
                using (var writer = File.AppendText((@"C:\temp\results.txt")))
                {
                    writer.WriteLine(string.Format("{0} {1}", ex.Message, ex.StackTrace));
                    writer.Close();
                }

                return new HttpResponseMessage()
                {
                    Content = new StringContent(RandomMoveToPlay())
                };
            }
        }

        private static string RandomMoveToPlay()
        {
            var ourMoveToPlay = "";
            var random = new Random().Next(1, 4);
            switch (random)
            {
                case 1:
                    ourMoveToPlay = Moves.Paper;
                    break;
                case 2:
                    ourMoveToPlay = Moves.Rock;
                    break;
                case 3:
                    ourMoveToPlay = Moves.Scissors;
                    break;
                case 4:
                    ourMoveToPlay = Moves.Dynamite;
                    break;
            }
            return ourMoveToPlay;
        }

        [Route("start"), HttpPost]
        public HttpStatusCode Start([FromBody]JObject json)
        {
            WebApiApplication.CurrentRound = 0;
            WebApiApplication.Game = new Game()
            {
                Enemy = new Player()
                {
                    Name = json["opponentName"].Value<string>(),
                    Dynamite = json["dynamiteCount"].Value<int>(),
                    Points = 0
                },
                Us = new Player()
                {
                    Name = "DAMPBot",
                    Dynamite = json["dynamiteCount"].Value<int>(),
                    Points = 0
                },
                MaxRounds = json["maxRounds"].Value<int>(),
                PointsToWin = json["pointsToWin"].Value<int>(),
                Round = 1,
            };
            return HttpStatusCode.OK;
        }

        [Route("move"), HttpPost]
        public HttpStatusCode Move([FromBody]JObject json)
        {
            var theirMove = json["lastOpponentMove"].Value<string>();
            if (theirMove == "DYNAMITE")
                WebApiApplication.Game.Enemy.Dynamite--;
            WebApiApplication.Game.OpponentMoves.Add(json["lastOpponentMove"].Value<string>());

            try
            {
                using (var writer = File.AppendText((@"C:\temp\results.txt")))
                {
                    writer.WriteLine(string.Format("{0} {1} {2}", WebApiApplication.Game.Enemy.Name, theirMove, WebApiApplication.Game.OurMoves.LastOrDefault()));
                    writer.Close();
                }
            }
            catch (Exception)
            {

            }



            return HttpStatusCode.OK;
        }
    }

    public class GameTheory
    {
        public static string Process(string name)
        {
            return Moves.Paper;
        }
    }

    public class Player
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public int Dynamite { get; set; }
    }

    public class Game
    {
        public Game()
        {
            OpponentMoves = new List<string>();
            OurMoves = new List<string>();
        }

        public List<string> OpponentMoves { get; set; }
        public List<string> OurMoves { get; set; }
        public Player Enemy { get; set; }
        public Player Us { get; set; }
        public int PointsToWin { get; set; }
        public int Round { get; set; }
        public int MaxRounds { get; set; }
    }

    public static class Moves
    {
        public static string Paper = "PAPER";
        public static string Rock = "ROCK";
        public static string Scissors = "SCISSORS";
        public static string Water = "WATERBOMB";
        public static string Dynamite = "DYNAMITE";
    }
}
