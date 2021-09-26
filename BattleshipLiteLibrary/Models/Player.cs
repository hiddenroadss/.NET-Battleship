using System.Collections.Generic;


namespace BattleshipLiteLibrary.Models
{
    public class Player
    {
        public string Name { get; set; }
        public List<GridSpot> ShipLocations { get; set; } = new List<GridSpot>();
        public List<GridSpot> ShotGrid { get; set; } = new List<GridSpot>();
    }
}