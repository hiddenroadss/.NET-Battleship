using System;
using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;

namespace BattleshipLite
{
    class Program
    {
        static void Main(string[] args)
        {
            WelcomePlayer();
            Player activePlayer = CreatePlayer("Player 1");
            Player opponent = CreatePlayer("Player 2");
            Player winner = null;

            do
            {
                DisplayShotGrid(activePlayer);

                RecordPlayerShot(activePlayer, opponent);

                bool hasShips = GameLogic.CheckIfStillHasShipAfloat(opponent);
                if (hasShips)
                {
                    (activePlayer, opponent) = (opponent, activePlayer);
                }
                else
                {
                    winner = activePlayer;
                }

            } while (winner == null);

            CongratulateWinner(winner);
        }

        private static void CongratulateWinner(Player winner)
        {
            Console.WriteLine($"Congratulations to {winner.Name}!");
            Console.WriteLine($"{winner.Name} took { GameLogic.GetShotCount(winner)} shots to win");
        }

        private static void RecordPlayerShot(Player activePlayer, Player opponent)
        {
            var isValidShot = false;
            var row = "";
            var column = 0;
            do
            {
                var shotSpot = AskForShotSpot(activePlayer);
                try
                {
                    (row, column) = GameLogic.DestructureSpotName(shotSpot);
                }
                catch (ArgumentException err)
                {
                    Console.WriteLine("Please,enter valid shot spot.");
                }
                isValidShot = GameLogic.ValidateShot(activePlayer, row, column);
                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid shot location.Please try again");
                }

            } while (isValidShot == false);

            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);
        }

        private static string AskForShotSpot(Player player)
        {
            Console.Write($"{player.Name},where do you want to shoot?: ");
            return Console.ReadLine();
        }

        private static void DisplayShotGrid(Player activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].GridLetter;
            foreach (GridSpot gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.GridLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.GridLetter;
                }
                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($" {gridSpot.GridLetter}{gridSpot.GridNumber} ");
                } 
                else if (gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X  ");
                }
                else if (gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O  ");
                }
                else
                {
                    Console.Write(" ?  ");
                }
            }

            Console.WriteLine();
        }

        private static void WelcomePlayer()
        {
            Console.WriteLine("Welcome to Battleship Lite!");
            Console.WriteLine("Created by hiddenroadss");
            Console.WriteLine();
        }

        private static Player CreatePlayer(string playerTitle)
        {
            Player output = new Player();

            Console.WriteLine($"Player info for {playerTitle}");
            output.Name = AskPlayerName();
            GameLogic.InitializeGrid(output);
            
            PlaceShips(output);
            
            Console.Clear();
            
            return output;
        }

        private static void PlaceShips(Player model)
        {
            do
            {
                Console.Write($"Where do you want to place your ship number {model.ShipLocations.Count + 1}?:");
                var spotToPlace = Console.ReadLine();
                var isValidSpot = false;
                try
                {
                    isValidSpot = GameLogic.PlaceShip(model, spotToPlace);

                }
                catch (ArgumentException)
                {
                    Console.WriteLine("This is invalid spot to place.Choose another.");
                }

                if (isValidSpot == false)
                {
                    Console.WriteLine("This spot is taken.Choose another.");
                }

            } while (model.ShipLocations.Count < 5);
        }

        private static string AskPlayerName()
        {
            string output = "";
            Console.Write("What is your name? ");
            output = Console.ReadLine();
            return output;
        }
    }
}