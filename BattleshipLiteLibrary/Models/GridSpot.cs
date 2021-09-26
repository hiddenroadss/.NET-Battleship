using System.Collections.Generic;

namespace BattleshipLiteLibrary.Models
{
    public class GridSpot
    {
        public string GridLetter { get; set; }
        public int GridNumber { get; set; }
        public GridSpotStatus Status { get; set; } = GridSpotStatus.Empty;
    }
}