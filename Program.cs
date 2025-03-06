using System;
using System.Collections.Generic;
using System.Linq;
using _8digitos;



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
