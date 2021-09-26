using System;
using System.Collections.Generic;
using System.Linq;
using BattleshipLiteLibrary.Models;

namespace BattleshipLiteLibrary
{
    public class GameLogic
    {
        public static void InitializeGrid(Player player)
        {
            List<string> letters = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            List<int> numbers = new List<int>
            {
                1,
                2,
                3,
                4,
                5
            };

            foreach (string letter in letters)
            {
                foreach (int num in numbers)
                {
                    AddGridSpot(player, letter, num);
                }
            }
        }

        public static bool PlaceShip(Player player, string spotToPlace)
        {
            bool output = false;
            (string letter, int number) = DestructureSpotName(spotToPlace);
            bool isValidSpot = ValidateGridLocation(player, letter, number);
            bool isOpenSpot = ValidateShipLocation(player, letter, number);
            if (isValidSpot && isOpenSpot)
            {
                player.ShipLocations.Add(new GridSpot{ GridLetter = letter.ToUpper(), GridNumber = number, Status = GridSpotStatus.Ship});
                output = true;
            }
            return output;

        }

        private static bool ValidateShipLocation(Player player, string letter, int number)
        {
            bool isValid = true;
            foreach (var shipSpot in player.ShipLocations)
            {
                if (shipSpot.GridLetter == letter.ToUpper() && shipSpot.GridNumber == number)
                {
                    isValid = false;
                }
            }

            return isValid;
        }

        private static bool ValidateGridLocation(Player player, string letter, int number)
        {
            bool isValid = false;
            foreach (var spot in player.ShotGrid)
            {
                if (spot.GridLetter == letter.ToUpper() && spot.GridNumber == number)
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        private static void AddGridSpot(Player model, string letter, int number)
        {
            GridSpot spot = new GridSpot
            {
                GridLetter = letter.ToUpper(),
                GridNumber = number,
                Status = GridSpotStatus.Empty
            };
            model.ShotGrid.Add(spot);
        }


        public static bool CheckIfStillHasShipAfloat(Player player)
        {
            bool hasShips = false;
            foreach (var shipSpot in player.ShipLocations)
            {
                if (shipSpot.Status != GridSpotStatus.Sunk)
                {
                    hasShips = true;
                }
            }

            return hasShips;
        }

        public static int GetShotCount(Player player)
        {
            int shots = 0;
            foreach (var spot in player.ShotGrid)
            {
                if (spot.Status != GridSpotStatus.Empty)
                {
                    shots += 1;
                }
            }

            return shots;
        }

        public static (string, int) DestructureSpotName(string shotSpot)
        {
            string letter = "";
            int number = 0;
            if (shotSpot.Length != 2)
            {
                throw new ArgumentException("Your spot is not valid", nameof(shotSpot));
            }

            char[] spotArray = shotSpot.ToArray();
            letter = spotArray[0].ToString();
            number = int.Parse(spotArray[1].ToString());
            return (letter, number);
        }

        public static bool ValidateShot(Player player, string row, int column)
        {
            bool isValid = false;
            foreach (var spot in player.ShotGrid)
            {
                if (spot.GridLetter == row && spot.GridNumber == column)
                {
                    if (spot.Status == GridSpotStatus.Empty)
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }

        public static bool IdentifyShotResult(Player opponent, string row, int column)
        {
            bool isAHit = false;
            foreach (var shipSpot in opponent.ShipLocations)
            {
                if (shipSpot.GridLetter == row.ToUpper() && shipSpot.GridNumber == column)
                {
                    isAHit = true;
                    shipSpot.Status = GridSpotStatus.Sunk;
                }
            }

            return isAHit;
        }

        public static void MarkShotResult(Player player, string row, int column, bool isAHit)
        {
            foreach (var spot in player.ShotGrid)
            {
                if (spot.GridLetter == row.ToUpper() && spot.GridNumber == column)
                {
                    spot.Status = isAHit ? GridSpotStatus.Hit : GridSpotStatus.Miss;
                }
            }
        }
    }
}