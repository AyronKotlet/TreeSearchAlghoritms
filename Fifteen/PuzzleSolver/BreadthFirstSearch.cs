﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver
{
    public class BreadthFirstSearch : IPuzzleSolver
    {
        public BreadthFirstSearch()
        {
            searchOrder = new Direction[] { Direction.Left, Direction.Up, Direction.Right, Direction.Down };
        }

        public BreadthFirstSearch(Direction[] searchOrder)
        {
            SearchOrder = searchOrder;
        }

        public BreadthFirstSearch(string searchOrder) : base()
        {           
            for( int i = 0; i < 4; i++ )
            {
                switch(searchOrder[i].ToString())
                {
                    case "L":
                        SearchOrder[i] = Direction.Left;
                        break;
                    case "R":
                        SearchOrder[i] = Direction.Right;
                        break;
                    case "U":
                        SearchOrder[i] = Direction. Up;
                        break;
                    case "D":
                        SearchOrder[i] = Direction.Down;
                        break;
                }
            }
        }

        public Direction[] SearchOrder
        {
            get
            {
                return searchOrder;
            }
            set
            {
                if (value.Length == Enum.GetValues(typeof(Direction)).Length)
                {
                    searchOrder = value;
                }
                else
                {
                    throw new ArgumentException("Wrong number of directions in order");
                }
            }
        }

        private Direction[] searchOrder = new Direction[4];

        private PuzzleSolution solution = null;

        public PuzzleSolution Solve(Puzzle unsolved, Puzzle target)
        {
            solution = new PuzzleSolution();
            DateTime startTime = DateTime.Now;

            Proceed(unsolved, target);

            solution.Duration = DateTime.Now - startTime;
            return solution;
        }

        private void Proceed(Puzzle puzzle, Puzzle target)
        {
            Queue<Puzzle> queue = new Queue<Puzzle>();
            queue.Enqueue(puzzle);

            while (queue.Count != 0)
            {
                solution.RecursionDepth++;
                var currentState = queue.Dequeue();
                solution.Visited.Add(currentState);
                
                var possibleMoves = currentState.GetPossibleMoves();
                for (int moveId = 0; moveId < searchOrder.Length; moveId++)
                {                    
                    if (possibleMoves.Contains(searchOrder[moveId]))
                    {
                        var newState = new Puzzle(currentState.ToMatrix());
                        newState.MoveBlank(searchOrder[moveId]);
                        solution.Solution.Add(searchOrder[moveId]);
                        if (newState.Equals(target))
                        {
                            solution.LastState = newState;
                            solution.IsSolved = true;
                            return;
                        }
                        queue.Enqueue(newState);
                    }
                }
                solution.Processed.Add(currentState);
            }
        }
    }
}
