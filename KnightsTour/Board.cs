
using System.Collections.Generic;

namespace KnightsTour
{
    public class Board
    {
        private readonly BoardNode[][] boardNodes;
        public Board(IBoardRepresentation boardRepresentation)
        {
            Rows = boardRepresentation.Rows;
            Columns = boardRepresentation.Columns;
            boardNodes = new BoardNode[Rows][];
            for (uint i = 0; i < Rows; i++)
            {
                boardNodes[i] = new BoardNode[Columns];
                for (uint j = 0; j < Columns; j++)
                {
                    boardNodes[i][j] = new BoardNode(boardRepresentation.GetValue(i, j));
                }
            }
        }

        public BoardNode GetNode(uint row, uint col)
        {
            if (row < boardNodes.Length && col < boardNodes[row].Length)
            {
                return boardNodes[row][col];
            }

            return null;
        }

        public uint Rows
        {
            get;
            private set;
        }

        public uint Columns
        {
            get;
            private set;
        }
    }
}
