using System;
using System.IO;
using System.Text;

namespace TASerialization
{
    public class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public ListRand()
        {
            Head = null;
            Tail = null;
            Count = 0;
        }

        public void Add(ListNode node)
        {
            if (Count == 0)
            {
                Head = node;
                Tail = node;
            }
            else
            {
                node.Prev = Tail;
                Tail.Next = node;
                Tail = node;
            }
            Count++;
        }

        public void Serialize(FileStream s)
        {
            if (s.CanWrite)
            {
                s.Write(BitConverter.GetBytes(Count));
                if (Count > 0)
                {
                    RedBlackBinarySearchTree<int> tree = new RedBlackBinarySearchTree<int>();
                    ListNode node = Head;
                    int counter = 0;
                    while (node != null)
                    {
                        int nodeValue = node.GetHashCode();
                        tree.Insert(nodeValue, counter);

                        node = node.Next;
                        counter++;
                    }
                    node = Head;
                    while (node != null)
                    {
                        int rand;
                        if (node.Rand != null)
                        {
                            int randomNodeValue = node.Rand.GetHashCode();
                            rand = tree.Find(randomNodeValue);
                        }
                        else rand = -1;
                        s.Write(BitConverter.GetBytes(rand));

                        byte[] nodeData = Encoding.UTF8.GetBytes(node.data);
                        int length = nodeData.Length;
                        s.Write(BitConverter.GetBytes(length));
                        s.Write(nodeData, 0, nodeData.Length);

                        node = node.Next;
                    }
                }
            }
        }

        public void Deserialize(FileStream s)
        {
            if (s.CanRead)
            {
                byte[] intContainer = new byte[4];
                s.Read(intContainer, 0, 4);
                Count = BitConverter.ToInt32(intContainer);
                if (Count == 0)
                {
                    Head = null;
                    Tail = null;
                }
                else
                {
                    int[] randIndices = new int[Count];
                    ListNode[] nodes = new ListNode[Count];

                    for (int i = 0; i < Count; ++i)
                    {
                        s.Read(intContainer, 0, 4);
                        randIndices[i] = BitConverter.ToInt32(intContainer);

                        s.Read(intContainer, 0, 4);
                        int stringLength = BitConverter.ToInt32(intContainer);

                        byte[] stringBytes = new byte[stringLength];
                        s.Read(stringBytes, 0, stringLength);
                        string nodeData = Encoding.UTF8.GetString(stringBytes);

                        nodes[i] = new ListNode(nodeData);
                    }

                    Head = nodes[0];
                    if (randIndices[0] > -1) Head.Rand = nodes[randIndices[0]];

                    for (int i = 1; i < Count; ++i)
                    {
                        nodes[i].Prev = nodes[i - 1];
                        nodes[i - 1].Next = nodes[i];
                        if (randIndices[i] > -1) nodes[i].Rand = nodes[randIndices[i]];
                    }
                    Tail = nodes[Count - 1];
                }
            }
        }

        public void Show()
        {
            if (Count > 0)
            {
                ListNode node = Head;
                while (node != null)
                {
                    if (node.Rand != null)
                        Console.WriteLine($"Node data: {node.data}  Random node data: {node.Rand.data}");
                    else Console.WriteLine($"Node data: {node.data}");
                    node = node.Next;
                }
            }
            else Console.WriteLine("ListRand is empty :(");
        }
    }
}