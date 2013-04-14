using System;
namespace KnightsTour
{
    class Program
    {
        static void Main(string[] args)
        {
            uint maxLength = 10;
            uint maxVowels = 2;

            var argsLength = args.Length;

            if (argsLength > 0)
            {
                uint.TryParse(args[0], out maxLength);
            }

            if (argsLength > 1)
            {
                uint.TryParse(args[1], out maxVowels);
            }

            var board = new Board(new KnightBoard());
            var movable = new KnightMovable();
            var solver = new BoardSolver(board, movable, maxLength, maxVowels);
            Console.Out.WriteLine("Number of valid sequences: " + solver.Solve());

            Console.In.ReadLine();
        }
    }
}
