
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KnightsTour
{
    public class BoardSolver
    {
        private readonly Board board;
        private readonly IMovableObject movableObject;
        private readonly uint maxLength;
        private readonly uint maxVowels;

        public BoardSolver(Board board, IMovableObject movableObject, uint maxLength, uint maxVowels)
        {
            this.board = board;
            this.movableObject = movableObject;
            this.maxLength = maxLength;
            this.maxVowels = maxVowels;

            movableObject.Board = board;

            //One time initialization of nodes
            for (uint i = 0; i < board.Rows; i++)
            {
                for (uint j = 0; j < board.Columns; j++)
                {
                    var currentNode = board.GetNode(i, j);
                    movableObject.BoardPosition = new BoardPosition(i, j);
                    currentNode.InitializeMovableNodes(movableObject);
                }
            }
        }

        public ulong Solve()
        {
            if (maxLength == 0)
            {
                return 0;
            }

            return SolvePieces().Result;
        }

        private async Task<ulong> SolvePieces()
        {
            var solutionTasks = new List<Task<ulong>>();
            for (uint i = 0; i < board.Rows; i++)
            {
                for (uint j = 0; j < board.Columns; j++)
                {
                    var newKey = new SolutionKey(board.GetNode(i, j), maxLength, maxVowels, this);
                    solutionTasks.Add(Task.Factory.StartNew(() => SolveKey(newKey)));
                }
            }

            var solutions = await Task.WhenAll(solutionTasks);
            ulong totalSum = 0;
            for (int i = 0; i < solutions.Length; i++)
            {
                totalSum += solutions[i];
            }

            return totalSum;
        }

        private readonly ConcurrentDictionary<SolutionKey, SolutionNode> solutionDictionary =
            new ConcurrentDictionary<SolutionKey, SolutionNode>(new SolutionKeyComparer());

        private ulong SolveKey(SolutionKey solutionKey)
        {
            var solutionNode = solutionDictionary.GetOrAdd(solutionKey, NodeCreator);

            return solutionNode.Result;
        }

        private SolutionNode NodeCreator(SolutionKey solutionKey)
        {
            return new SolutionNode(solutionKey);
        }

        private class SolutionNode
        {
            private readonly SolutionKey solutionKey;

            public SolutionNode(SolutionKey solutionKey)
            {
                this.solutionKey = solutionKey;
            }

            private int calculating;
            private readonly TaskCompletionSource<ulong> calculationResult = new TaskCompletionSource<ulong>();

            public ulong Result
            {
                get
                {
                    if (Interlocked.CompareExchange(ref calculating, 1, 0) == 0)
                    {
                        CalculateResult();
                    }

                    return calculationResult.Task.Result;
                }
            }
            private void CalculateResult()
            {
                if (solutionKey.BoardNode.IsNull)
                {
                    calculationResult.SetResult(0);
                    return;
                }

                var vowelsLeft = solutionKey.VowelsLeft;
                if (solutionKey.BoardNode.IsVowel)
                {
                    if (solutionKey.VowelsLeft == 0)
                    {
                        calculationResult.SetResult(0);
                        return;
                    }
                    else
                    {
                        vowelsLeft -= 1;
                    }
                }

                if (solutionKey.LengthLeft == 1)
                {
                    calculationResult.SetResult(1);
                    return;
                }

                //Move piece to all spots and then create keys for each and solve
                var movableNodes = solutionKey.BoardNode.MovableNodes;
                ulong solutionSum = 0;
                foreach (var movableNode in movableNodes)
                {
                    var newSubproblem = new SolutionKey(movableNode, solutionKey.LengthLeft - 1, vowelsLeft, solutionKey.BoardSolver);
                    solutionSum += solutionKey.BoardSolver.SolveKey(newSubproblem);
                }

                calculationResult.SetResult(solutionSum);
            }
        }

        private class SolutionKey
        {
            private readonly BoardNode node;
            private readonly uint lengthLeft;
            private readonly uint vowelsLeft;
            private readonly BoardSolver boardSolver;

            public BoardNode BoardNode
            {
                get
                {
                    return node;
                }
            }

            public uint VowelsLeft
            {
                get
                {
                    return vowelsLeft;
                }
            }

            public uint LengthLeft
            {
                get
                {
                    return lengthLeft;
                }
            }

            public BoardSolver BoardSolver
            {
                get
                {
                    return boardSolver;
                }
            }

            public SolutionKey(BoardNode node, uint lengthLeft, uint vowelsLeft, BoardSolver boardSolver)
            {
                this.node = node;
                this.lengthLeft = lengthLeft;
                this.vowelsLeft = vowelsLeft;
                this.boardSolver = boardSolver;
            }
        }

        private class SolutionKeyComparer : IEqualityComparer<SolutionKey>
        {
            public bool Equals(SolutionKey x, SolutionKey y)
            {
                return (x.BoardNode.NodeCharacter == y.BoardNode.NodeCharacter) && (x.LengthLeft == y.LengthLeft) && (x.VowelsLeft == y.VowelsLeft);
            }

            public int GetHashCode(SolutionKey obj)
            {
                return obj.BoardNode.NodeCharacter.GetHashCode() ^ obj.LengthLeft.GetHashCode() ^
                       obj.VowelsLeft.GetHashCode();
            }
        }
    }
}
