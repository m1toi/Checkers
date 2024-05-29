using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Models
{
    public class GameConfiguration
    {
        public EPiece[] ePieces { get; set; }
        public int BlackPieces { get; set; }
        public int WhitePieces { get; set; }
        public EPiece CurrentPlayer { get; set; }
        public EGameState GameState { get; set; }
        public bool MultipleCaptures { get; set; }
    }
}
