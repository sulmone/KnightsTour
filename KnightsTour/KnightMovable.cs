using System.Collections.Generic;

namespace KnightsTour
{
    public class KnightMovable : IMovableObject
    {
        public Board Board
        {
            get;
            set;
        }

        public BoardPosition BoardPosition
        {
            get;
            set;
        }

        public IEnumerable<BoardPosition> PossibleMoves
        {
            get
            {
                var position = BoardPosition;
                var col = position.Column;
                var row = position.Row;

                //Moving left checks
                if (col >= 2)
                {
                    if (row >= 1)
                    {
                        yield return new BoardPosition(row - 1, col - 2);
                    }
                    if (row < Board.Rows - 1)
                    {
                        yield return new BoardPosition(row + 1, col - 2);
                    }
                }

                //Moving Right Checks
                if (col < Board.Columns - 2)
                {
                    if (row >= 1)
                    {
                        yield return new BoardPosition(row - 1, col + 2);
                    }
                    if (row < Board.Rows - 1)
                    {
                        yield return new BoardPosition(row + 1, col + 2);
                    }
                }

                //Moving Up Checks
                if (row >= 2)
                {
                    if (col < Board.Columns - 1)
                    {
                        yield return new BoardPosition(row - 2, col + 1);
                    }
                    if (col >= 1)
                    {
                        yield return new BoardPosition(row - 2, col - 1);
                    }
                }

                //Moving Down Checks
                if (row < Board.Rows - 2)
                {
                    if (col >= 1)
                    {
                        yield return new BoardPosition(row + 2, col - 1);
                    }
                    if (col < Board.Columns - 1)
                    {
                        yield return new BoardPosition(row + 2, col + 1);
                    }
                }
            }
        }

        public BoardNode BoardNode
        {
            get
            {
                return Board.GetNode(BoardPosition.Row, BoardPosition.Column);
            }
        }
    }
}
