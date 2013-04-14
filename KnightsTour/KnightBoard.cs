
namespace KnightsTour
{
    public class KnightBoard : IBoardRepresentation
    {
        private static readonly char[][] knightBoard =
        {
            new[] {'A', 'B', 'C', 'D', 'E'},
            new[] {'F', 'G', 'H', 'I', 'J'},
            new[] {'K', 'L', 'M', 'N', 'O'},
            new[] {'\0', '1', '2', '3', '\0'}
        };

        public uint Columns
        {
            get
            {
                return 5;
            }
        }

        public uint Rows
        {
            get
            {
                return 4;
            }
        }

        public char GetValue(uint row, uint col)
        {
            return knightBoard[row][col];
        }
    }
}
