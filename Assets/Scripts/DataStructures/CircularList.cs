using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularList<T>
{
    public int size;
    public NodeCList<T> head;
    public CircularList(NodeCList<T> head, int size)
    {
        this.size = size;
        this.head = head;
    }
   
}
public class NodeCList<T>
{
    public T value;
    public NodeCList<T> next = null;
    public NodeCList(T value)
    {
        this.value = value;
    }
    public NodeCList(T value, NodeCList<T> next)
    {
        this.value = value;
        this.next = next;
    }

    public static CircularList<T> CreateCList(T[] values, int sizeOfList)
    {
        NodeCList<T> old = new NodeCList<T>(values[0]);
        NodeCList<T> head = old;
        for (int i = 1; i < sizeOfList; i++)
        {
            NodeCList<T> current = new NodeCList<T>(values[i]);
            old.next = current;
            old = current;
        }
        old.next = head;
        return new CircularList<T>(head, sizeOfList);
    }

}

