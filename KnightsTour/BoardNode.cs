
using System.Collections.Generic;
using System.Linq;

namespace KnightsTour
{
    public class BoardNode
    {
        private BoardNode[] movableNodes;

        public IEnumerable<BoardNode> MovableNodes
        {
            get
            {
                return movableNodes;
            }
        }

        public void InitializeMovableNodes(IMovableObject movableObject)
        {
            var originalPosition = movableObject.BoardPosition;
            var movablePositions = movableObject.PossibleMoves.ToArray();
            movableNodes = new BoardNode[movablePositions.Length];
            for (int i = 0; i < movablePositions.Length; i++)
            {
                movableObject.BoardPosition = movablePositions[i];
                movableNodes[i] = movableObject.BoardNode;
            }
            movableObject.BoardPosition = originalPosition;
        }

        private readonly char nodeCharacter;

        public char NodeCharacter
        {
            get
            {
                return nodeCharacter;
            }
        }

        public BoardNode(char nodeCharacter)
        {
            this.nodeCharacter = nodeCharacter;

            IsNull = nodeCharacter == '\0';
            IsVowel = Vowels.Contains(nodeCharacter);
        }

        public static readonly HashSet<char> Vowels = new HashSet<char>(new[] { 'A', 'E', 'I', 'O', 'U' });


        public bool IsNull
        {
            get;
            private set;
        }

        public bool IsVowel
        {
            get;
            private set;
        }
    }
}
