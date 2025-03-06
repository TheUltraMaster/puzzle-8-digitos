using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _8digitos
{
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
}