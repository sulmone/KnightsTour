
namespace KnightsTour
{
    public interface IBoardRepresentation
    {
        uint Rows
        {
            get;
        }

        uint Columns
        {
            get;
        }

        char GetValue(uint row, uint column);
    }
}
