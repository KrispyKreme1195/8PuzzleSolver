# 8-Puzzle Solver Project

## Description

The 8PuzzleSolver Project is designed as a learning exercise to implement a simple C# console application to solve the classic 8-puzzle problem. It features a PuzzleSolverEngine that generates, displays, and solves a randomized (but solvable) 8-puzzle. The project includes three main files: `Program.cs`, which serves as the entry point and user interaction layer; `PuzzleSolverEngine.cs`, which contains the logic to solve the puzzle; and `PuzzleState.cs`, managing individual puzzle states including tile positions and the goal state. The decision logic is an implementation of the A* search algorithm where the current arrangement of the tiles of the 8 puzzle act as the nodes of a graph, and the Manhattan Distance hueristic is applied to determine the next node to expand and navigate to.

## Features

- **Interactive Console Application**: Allows users to interact with the puzzle solver, offering options to continue solving step by step, solve completely, reset the puzzle, or exit the application.
- **Puzzle Solving Engine**: Implements logic to solve the puzzle using the A* search algorithm, considering tile movements and the Manhattan Distance heuristic.
- **State Management**: Keeps track of puzzle states, including the current state, previous states (to avoid cycles), and the goal state.

## How to Use

1. Compile the C# files in your preferred IDE or compiler.
2. Run the `Program.cs` file to start the application.
3. Follow the on-screen instructions to interact with the puzzle solver.

## System Requirements

- .NET Framework or .NET Core compatible environment
- C# compiler/IDE

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request with your proposed changes or improvements. Otherwise, feel free to discuss new ideas and ask questions aout the project, and definitely do not hesitate to reach out if you identify a mistake in the implementation or documentation, since the goal of this repository is to serve as a helpful learning reference for others.
