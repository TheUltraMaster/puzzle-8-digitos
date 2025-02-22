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

    public int CompareTo(Node? other)
    {
        if (other == null) return 1; // Manejo seguro en caso de null
        return (Cost + Heuristic).CompareTo(other.Cost + other.Heuristic);
    }
}

}