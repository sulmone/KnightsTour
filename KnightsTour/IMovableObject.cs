
using System.Collections.Generic;

namespace KnightsTour
{
    public interface IMovableObject
    {
        Board Board
        {
            get;
            set;
        }

        BoardPosition BoardPosition
        {
            get;
            set;
        }

        BoardNode BoardNode
        {
            get;
        }

        IEnumerable<BoardPosition> PossibleMoves
        {
            get;
        }
    }
}
