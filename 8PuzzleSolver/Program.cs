using _8PuzzleSolver;

internal class Program
{
    private static void Main()
    {
        //Initialize the Puzzle solver engine and display an initial status to the user
        var solverEngine = new PuzzleSolverEngine();
        solverEngine.Print();

        while (true)
        {
            //Prompt and receive instructions from the user on which action to perform as an int.
            Console.WriteLine("Please select an option: 1.) Continue \t2.) Solve\t3.) Reset\t4.) Exit");
            string? userSelectString = Console.ReadLine();

            //If an invalid value is provided, inform the user that it is invalid and start back at prompting them.
            if (string.IsNullOrWhiteSpace(userSelectString) ||!int.TryParse(userSelectString, out int userSelection) 
                || userSelection < 1 || userSelection > 4)
            {
                Console.WriteLine("\nInvalid input entered. Please select one of the provided options.\n");
                continue;
            }

            //Based on user input, invoke the appropriate functionality from the PuzzleSolverEngine
            switch (userSelection)
            {
                case 1:
                    solverEngine.Continue();
                    break;
                case 2:
                    solverEngine.Solve();
                    break;
                case 3:
                    solverEngine.Reset();
                    Console.Clear();
                    solverEngine.Print();
                    break;
                case 4:
                    Console.WriteLine("Exiting program.");
                    return;
            }
        }
    }
}