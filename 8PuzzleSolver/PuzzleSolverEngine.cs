using System.Text;

namespace _8PuzzleSolver
{
    /// <summary>
    /// Defines functionality to generate, display, and solve a randomized 8-puzzle.
    /// </summary>
    internal class PuzzleSolverEngine
    {
        /// <summary>
        /// Represents all previous <see cref="PuzzleState"/>s that have been navigated to previously by the engine 
        /// while attempting to solve the current puzzle
        /// </summary>
        private List<PuzzleState> StateHistory { get; set; } = [];
        /// <summary>
        /// The current <see cref="PuzzleState"/> being used by the engine to find possible subsequent states
        /// </summary>
        private PuzzleState CurrentState { get; set; }
        /// <summary>
        /// Represents the desired arrangement of tiles for the 8-puzzle as a 1 dimensional array. 
        /// </summary>
        private static int[] GoalState { get; set; } = [1, 2, 3, 4, 5, 6, 7, 8, 0];
        /// <summary>
        /// Represents whether the current 8-puzzle has already been solved or not.
        /// </summary>
        private bool DoneSolving => IsSolved(CurrentState);
        /// <summary>
        /// Creates a new <see cref="PuzzleSolverEngine"/> with an empty StateHistory and an initial <see cref="PuzzleState"/>
        /// which has a randomized Tiles array and is known to be solveable.
        /// </summary>
        public PuzzleSolverEngine()
        {
            //Make sure we start with a solveable puzzle.

            do
            {
                CurrentState = new PuzzleState();
            }
            while (!CurrentState.IsSolveable);

            StateHistory.Add(CurrentState);
        }

        //Included for convenience 

        /// <summary>
        /// Determines if a <see cref="PuzzleState"/>'s Tiles array matches the values for the 1D representation of 
        /// a solved 8-puzzle's tiles element-wise.
        /// </summary>
        /// <param name="state">The <see cref="PuzzleState"/> </param>
        /// <returns></returns>
        private bool IsSolved(PuzzleState state) => state.Tiles.SequenceEqual(GoalState);


        /// <summary>
        /// Finds the list of all possible puzzle states which can be found as a result of swapping the blank tile 
        /// of the given puzzle's state with an adjacent, non-diagonal tile.
        /// </summary>
        /// <param name="state">The <see cref="PuzzleState"/> whose Tiles array will be used as the basis for finding
        /// possible descendant <see cref="PuzzleState"/>s</param>
        /// <returns>A <see cref="List{PuzzleState}"/> of all possible descendant <see cref="PuzzleState"/>s</returns>
        private static List<PuzzleState> GetDescendantStates(PuzzleState state)
        {
            var descendants = new List<PuzzleState>();

            foreach (int index in PuzzleState.TileNeighbors[state.BlankIndex])
            {
                //Build the new state with the same Tiles but increment its depth. 
                var descendant = new PuzzleState(state);

                //Swap the blank tile with the value of one of the adjacent tiles non-diagonally.
                descendant.SwapTiles(index, descendant.BlankIndex);

                //Add it to the list of descendants
                descendants.Add(descendant);
            }

            return descendants;
        }

        /// <summary>
        /// Advances the engine's attempt to solve the given puzzle by finding the best of possible subsequent states and selecting that 
        /// as the next CurrentState
        /// </summary>
        public void Continue()
        {
            //Find the list of solvable descendants sorted by if they are the goal state, then by the hueristic
            //Manhattan Distance value defined for the A* search algorithm in ascending order

            if (DoneSolving)
            {
                Console.WriteLine("\nThe current puzzle is already solved. Please reset the puzzle and solve another or exit the program\n");
                return;
            }

            var descendants = GetDescendantStates(CurrentState)
                .Where(x => x.IsSolveable && !StateHistory.Any(y => y.Tiles.SequenceEqual(x.Tiles)))
                .OrderBy(x => x.ManhattanDistance + CurrentState.Depth)
                .ToList();

            //If we don't find any valid descendants, step back and try again.
            if (descendants.Count == 0)
            {
                Console.WriteLine("No valid moves found, reverting to previous state and trying another approach.");
                //Go back to the state before the last one in the state history (the current state)
                CurrentState = StateHistory[^2];
                return;
            }

            //Check if we've found a solution.
            if (descendants.Any(IsSolved))
            {
                //If so, select the solved puzzle as the current state and inform the user the puzzle is solved.
                CurrentState = descendants.Where(IsSolved).First();
                Print();
                return;
            }

            //Otherwise, select the next state from the list of descendants that is closest to the goal and update the user
            CurrentState = descendants.First();
            StateHistory.Add(CurrentState);
            Print();
        }

        /// <summary>
        /// Continuously advances the engine's attempts to solve the puzzle until it arrives at the solved, goal state.
        /// </summary>
        public void Solve()
        {
            do 
            {
                Continue();
            }
            while (!DoneSolving);
        }

        /// <summary>
        /// Resets the engine to consider a new, solveable randomized <see cref="PuzzleState"/> 
        /// And resets the StateHistory to not contain any previous states.
        /// </summary>
        public void Reset()
        {
            //Similar to in the constructor, set the current state to be a new random, solveable state
            do
            {
                CurrentState = new PuzzleState();
            }
            while (!CurrentState.IsSolveable);

            //Clear the old state history and add the new current state as the first state in the new history.
            StateHistory.Clear();
            StateHistory.Add(CurrentState);
        }

        /// <summary>
        /// Prints a message to the user displaying information about the current status of the puzzle the engine will attempt to solve.
        /// </summary>
        public void Print()
        {
            string header;
            if (DoneSolving)
            {
                header = "\nPuzzle solved!\n\n";
            }
            else
            {
                header = CurrentState.Depth == 1 ? "Initial State:\n\n" : "\nCurrent State:\n\n";
            }

            var builder = new StringBuilder(header);

            builder.AppendLine($"\t{CurrentState.Tiles[0]}\t{CurrentState.Tiles[1]}\t{CurrentState.Tiles[2]}");
            builder.AppendLine($"\t{CurrentState.Tiles[3]}\t{CurrentState.Tiles[4]}\t{CurrentState.Tiles[5]}");
            builder.AppendLine($"\t{CurrentState.Tiles[6]}\t{CurrentState.Tiles[7]}\t{CurrentState.Tiles[8]}\n");

            builder.AppendLine($"Current Depth: {CurrentState.Depth}\t\tManhattan Distance: {CurrentState.ManhattanDistance}\n");

            Console.WriteLine(builder.ToString());
        }
    }
}
