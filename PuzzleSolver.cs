using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _8digitos
{


class PuzzleSolver
{
    private static readonly int[][] Moves = {
        new int[] {1, 3},    // Posición 0 puede moverse a 1, 3
        new int[] {0, 2, 4}, // Posición 1 puede moverse a 0, 2, 4
        new int[] {1, 5},    // Posición 2 puede moverse a 1, 5
        new int[] {0, 4, 6}, // Posición 3 puede moverse a 0, 4, 6
        new int[] {1, 3, 5, 7}, // Posición 4 puede moverse a 1, 3, 5, 7
        new int[] {2, 4, 8}, // Posición 5 puede moverse a 2, 4, 8
        new int[] {3, 7},    // Posición 6 puede moverse a 3, 7
        new int[] {4, 6, 8}, // Posición 7 puede moverse a 4, 6, 8
        new int[] {5, 7}     // Posición 8 puede moverse a 5, 7
    };

    private const string TargetState = "123456780";

    public static void Solve(string initialState)
    {
        if (initialState.Length != 9 || !initialState.All(char.IsDigit))
        {
            Console.WriteLine("❌ Error: El estado debe contener exactamente 9 caracteres numéricos.");
            return;
        }

        if (!IsSolvable(initialState))
        {
            Console.WriteLine("🚫 Este estado del puzzle no tiene solución.");
            return;
        }

        SortedSet<Node> openSet = new SortedSet<Node>();
        HashSet<string> visited = new HashSet<string>();

        openSet.Add(new Node(initialState, null, 0, CalculateHeuristic(initialState)));
        visited.Add(initialState);

        while (openSet.Count > 0)
        {
            Node current = openSet.First();
            openSet.Remove(current);

            if (current.State == TargetState)
            {
                PrintSolution(current);
                return;
            }

            int zeroIndex = current.State.IndexOf('0');

            foreach (int move in Moves[zeroIndex])
            {
                string newState = Swap(current.State, zeroIndex, move);
                if (!visited.Contains(newState))
                {
                    openSet.Add(new Node(newState, current, current.Cost + 1, CalculateHeuristic(newState)));
                    visited.Add(newState);
                }
            }
        }

        Console.WriteLine("No se encontró solución.");
    }

    private static int CalculateHeuristic(string state)
    {
        int sum = 0;
        string goal = "123456780";

        for (int i = 0; i < 9; i++)
        {
            if (state[i] != '0')
            {
                int goalPos = goal.IndexOf(state[i]);
                sum += Math.Abs(i / 3 - goalPos / 3) + Math.Abs(i % 3 - goalPos % 3);
            }
        }
        return sum;
    }

    private static string Swap(string state, int i, int j)
    {
        char[] array = state.ToCharArray();
        (array[i], array[j]) = (array[j], array[i]);
        return new string(array);
    }

    private static bool IsSolvable(string state)
    {
        int[] puzzle = state.Select(c => c - '0').ToArray();
        int inversions = 0;

        for (int i = 0; i < 9; i++)
        {
            for (int j = i + 1; j < 9; j++)
            {
                if (puzzle[i] > 0 && puzzle[j] > 0 && puzzle[i] > puzzle[j])
                    inversions++;
            }
        }

        return (inversions % 2 == 0);
    }

    private static void PrintSolution(Node node)
    {
        Stack<string> path = new Stack<string>();
        while (node != null)
        {
            path.Push(node.State);
            node = node.Parent;
        }

        int step = 0;
        while (path.Count > 0)
        {
            Console.Clear();
            Console.WriteLine($"==== Paso {step} ====");
            PrintState(path.Pop());
            Console.WriteLine("\nPresiona ENTER para continuar...");
            Console.ReadLine();
            step++;
        }
    }

    private static void PrintState(string state)
    {
        Console.WriteLine("+---+---+---+");
        for (int i = 0; i < 9; i++)
        {
            Console.Write("| " + (state[i] == '0' ? " " : state[i].ToString()) + " ");
            if (i % 3 == 2) Console.WriteLine("|\n+---+---+---+");
        }
    }
}

}