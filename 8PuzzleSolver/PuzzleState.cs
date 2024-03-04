namespace _8PuzzleSolver
{
    /// <summary>
    /// Represents the state of the 8-puzzle being solved at some specific point in time
    /// </summary>
    internal class PuzzleState
    {
        /// <summary>
        /// A dictionary whose keys are indexes of the puzzle's tiles and whose values are an array of ints 
        /// corresponding to the index of adjacent, non-diagonal tiles for the key index.
        /// </summary>
        public static Dictionary<int, int[]> TileNeighbors = new()
        {
            {0, [1,3] },
            {1, [0,2,4] },
            {2, [1,5] },
            {3, [0,4,6] },
            {4, new int[] {1,3,5,7} },
            {5, new int[] {2,4,8} },
            {6, new int[] {3,7} },
            {7, new int[] {4,6,8} },
            {8, new int[] {5,7} }
        };

        /// <summary>
        /// Represents the arrangement of the tiles of the puzzle. The tiles array is always 9 ints in length
        /// and always contains ints 0 to 8. The index is the tile's position and
        /// the value represents the number on the tile, with the value of 0 being the blank tile. 
        /// </summary>

        /*  The puzzle's three rows are flattened to 1 dimension so that the array [1,2,3,4,5,6,7,8,0]
            would represent the tile state 
            
                1 2 3
                4 5 6
                7 8 -
        */
        public int[] Tiles = new int[9];

        /// <summary>
        /// Represents how many previous puzzle states have been selected and used for subsequent decisions
        /// since the initial state (inclusive).
        /// </summary>
        public int Depth = 1;

        /// <summary>
        /// The index of the entry in the Tiles array whose value is 0 (i.e. the location of the blank tile)
        /// </summary>
        public int BlankIndex => Array.IndexOf(Tiles, 0);

        /// <summary>
        /// Manhattan Distance is the primary hueristic measure used by the A* search algorithm for our solution.
        /// A puzzle state's Manhattan distance is the sum of the absolute difference between where a value is located
        /// in the goal state and where that value actually is in the current state for every value in the state's Tiles array.
        /// </summary>
        public int ManhattanDistance => GetManhattanDistance();

        /// <summary>
        /// Represents if the current arrangement of Tiles for the puzzle state can be solved.
        /// </summary>
        public bool IsSolveable => GetInversionCount() % 2 == 0;

        /// <summary>
        /// Creates a new <see cref="PuzzleState"/> with a Depth of 1 and a randomized Tiles array 
        /// </summary>
        public PuzzleState()
        {
            var random = new Random();

            Tiles = [0, 1, 2, 3, 4, 5, 6, 7, 8];

            //Randomize the tile values for this new puzzle state in-place.
            random.Shuffle(Tiles);
        }

        /// <summary>
        /// Creates a <see cref="PuzzleState"/> whose Tiles array is a copy of the <paramref name="parent"/>'s
        /// and whose Depth is equal to the <paramref name="parent"/>'s  + 1
        /// </summary>
        /// <param name="parent">The <see cref="PuzzleState"/> to use to determine the Tiles arrangement and Depth for the
        /// new <see cref="PuzzleState"/></param>
        public PuzzleState(PuzzleState parent)
        {
            //Copy the tiles and increment the depth value by one
            Tiles = [.. parent.Tiles];
            Depth = parent.Depth + 1;
        }

        /// <summary>
        /// Finds the Manhattan Distance value for the <see cref="PuzzleState"/> Tiles array.
        /// </summary>
        /// <returns>The Manhattan Distance value for the calling <see cref="PuzzleState"/>.</returns>
        private int GetManhattanDistance()
        {
            int distance = 0;
            int goalValue, currentRow, currentCol, goalRow, goalCol;

            for (int i = 0; i < 9; i++)
            {
                // Skip the blank tile
                if (Tiles[i] == 0) continue;

                // Calculate current row and column from index
                currentRow = i / 3;
                currentCol = i % 3;

                // Calculate goal row and column for the current tile value
                goalValue = Tiles[i] - 1; // Subtract 1 to align with zero-indexed array
                goalRow = goalValue / 3;
                goalCol = goalValue % 3;

                // Calculate the Manhattan distance for this tile and add it to the total distance
                distance += Math.Abs(currentRow - goalRow) + Math.Abs(currentCol - goalCol);
            }

            return distance;
        }

        //Included for convenience 
        /// <summary>
        /// Swaps the value located at index <paramref name="idx1"/> of the <see cref="PuzzleState"/>'s Tiles array
        /// With the value located at index <paramref name="idx2"/>
        /// </summary>
        /// <param name="idx1">The index of the first of the two items to swap in the Tiles array.</param>
        /// <param name="idx2">The index of the second of the two items to swap in the Tiles array.</param>
        public void SwapTiles(int idx1, int idx2) => (Tiles[idx1], Tiles[idx2]) = (Tiles[idx2], Tiles[idx1]);

        /// <summary>
        /// Determines the number of inversions in the Tiles array, which is used to determine if the current state of the puzzle is solveable.
        /// </summary>
        /// <returns>The count of inversions in the Tiles array.</returns>
        public int GetInversionCount()
        {
            int inversions = 0;

            for (int i = 0; i < 9; i++)
            {
                // Skip the blank tile for inversion count
                if (Tiles[i] == 0) continue;

                for (int j = i + 1; j < 9; j++)
                {
                    // Compare only with non-blank tiles
                    if (Tiles[j] == 0) continue;

                    // If a pair is an inversion, increment the count
                    if (Tiles[i] > Tiles[j])
                    {
                        inversions++;
                    }
                }
            }

            return inversions;
        }
    }
}
