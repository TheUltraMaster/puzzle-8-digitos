using System;
using System.Collections.Generic;
using System.Linq;

class Node : IComparable<Node>
{
    public string State { get; }
    public Node? Parent { get; }
    public int Cost { get; }
    public int Heuristic { get; }

    public Node(string state, Node? parent, int cost, int heuristic)
    {
        State = state;
        Parent = parent;
        Cost = cost;
        Heuristic = heuristic;
    }

    // 🔹 Comparador basado en f(n) = g(n) + h(n) (prioriza nodos con menor costo total)
    public int CompareTo(Node? other)
    {
        if (other == null) return -1;
        int thisFn = Cost + Heuristic;
        int otherFn = other.Cost + other.Heuristic;
        return thisFn.CompareTo(otherFn);
    }
}

// 🔹 Implementación de una Cola de Prioridad
class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> heap = new List<T>();

    public void Enqueue(T item)
    {
        heap.Add(item);
        heap.Sort(); // Ordena para priorizar el menor f(n)
    }

    public T Dequeue()
    {
        T item = heap[0];
        heap.RemoveAt(0);
        return item;
    }

    public bool IsEmpty() => heap.Count == 0;
}

// 🔹 Clase principal que implementa A*
class PuzzleSolver
{
    private static readonly int[][] Moves = {
        new int[] {1, 3},    // Posición 0 → 1, 3
        new int[] {0, 2, 4}, // Posición 1 → 0, 2, 4
        new int[] {1, 5},    // Posición 2 → 1, 5
        new int[] {0, 4, 6}, // Posición 3 → 0, 4, 6
        new int[] {1, 3, 5, 7}, // Posición 4 → 1, 3, 5, 7
        new int[] {2, 4, 8}, // Posición 5 → 2, 4, 8
        new int[] {3, 7},    // Posición 6 → 3, 7
        new int[] {4, 6, 8}, // Posición 7 → 4, 6, 8
        new int[] {5, 7}     // Posición 8 → 5, 7
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
            Console.WriteLine("🚫 El estado ingresado no tiene solución.");
            return;
        }

        PriorityQueue<Node> openSet = new PriorityQueue<Node>();
        HashSet<string> visited = new HashSet<string>();

        openSet.Enqueue(new Node(initialState, null, 0, CalculateHeuristic(initialState)));
        visited.Add(initialState);

        while (!openSet.IsEmpty())
        {
            Node current = openSet.Dequeue();

            // 🔍 DEBUG: Mostrar el estado actual
            Console.WriteLine($"🔍 Explorando estado con f(n) = g(n) + h(n) = {current.Cost} + {current.Heuristic}");
            PrintState(current.State);

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
                    openSet.Enqueue(new Node(newState, current, current.Cost + 1, CalculateHeuristic(newState)));
                    visited.Add(newState);
                }
            }
        }

        Console.WriteLine("❌ No se encontró solución.");
    }

    // 🔹 Calcula la heurística de Manhattan
    private static int CalculateHeuristic(string state)
    {
        int sum = 0;
        int[] goalPositions = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

        for (int i = 0; i < 9; i++)
        {
            if (state[i] != '0') // Ignorar el espacio vacío
            {
                int value = state[i] - '1';
                int goalX = goalPositions[value] % 3;
                int goalY = goalPositions[value] / 3;
                int currX = i % 3;
                int currY = i / 3;

                sum += Math.Abs(goalX - currX) + Math.Abs(goalY - currY);
            }
        }
        return sum;
    }

    // 🔹 Verifica si el estado inicial es resoluble
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

        Console.WriteLine($"🔍 Número de inversiones: {inversions}");
        return (inversions % 2 == 0);
    }

    // 🔹 Función para intercambiar posiciones en el string
    private static string Swap(string state, int i, int j)
    {
        char[] array = state.ToCharArray();
        (array[i], array[j]) = (array[j], array[i]);
        return new string(array);
    }

    // 🔹 Imprime la solución paso a paso
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

    // 🔹 Imprime el estado en formato de recuadro
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

// 📌 Programa principal
class Program
{
    static void Main()
    {
        Console.Write("Ingrese el estado inicial (ejemplo: 283164705): ");
        string? input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("❌ Error: Debes ingresar un estado válido.");
            return;
        }

        PuzzleSolver.Solve(input);
    }
}
