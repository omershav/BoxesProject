using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BoxesLibrary
{
    class Linked_List<T> : IEnumerable<T>
    {
        Node start;
        Node end;

        internal Node Start { get => start; }
        internal Node End { get => end; }

        public bool IsEmpty() => start == null;

        public void AddFirst(T item)
        {
            Node n = new Node(item);
            n.next = start;
            start = n;
            if (end == null)
            {
                end = n;
                return;
            }

            start.next.prev = start;
        }

        public void AddLast(T item)
        {
            if (start == null)
            {
                AddFirst(item);
                return;
            }
            Node n = new Node(item);
            n.prev = end;
            end.next = n;
            end = n;
        }

        public bool RemoveFirst(out T saveFirstValue)
        {
            saveFirstValue = default(T);
            if (start == null) return false;

            saveFirstValue = start.data;
            start = start.next;
            if (start == null)
            {
                end = null;
            }
            else
            {
                start.prev = null;
            }

            return true;
        }

        public bool RemoveLast(out T saveLastValue)
        {
            saveLastValue = default(T);
            if (start == null) return false;

            saveLastValue = end.data;
            end = end.prev;
            if (end == null)
            {
                start = null;
            }
            else
            {
                end.next = null;
            }

            return true;
        }

        public bool GetAt(int position, out T value)
        {
            int counter = 0;
            value = default(T);
            Node temp = start;

            while (counter <= position && temp != null)
            {
                if (counter == position)
                {
                    value = temp.data;
                    return true;
                }

                temp = temp.next;
                counter++;
            }

            return false;
        }

        public bool AddAt(int position, T value)
        {
            if (position == 0)
            {
                AddFirst(value);
                return true;
            }

            int counter = 0;
            Node newValue = new Node(value); 
            Node temp = start;

            while (counter <= position && temp != null)
            {
                if (counter == position)
                {
                    temp.prev.next = newValue;
                    newValue.prev = temp.prev;
                    temp.prev = newValue;
                    newValue.next = temp;
                    return true;
                }

                temp = temp.next;
                counter++;
            }

            if (temp == null && counter == position)
            {
                AddLast(value);
                return true;
            }
            else return false;
        }

        public void MoveToEnd(Node itemToMove)
        {
            if (itemToMove.next == null) return;

            Node temp;

            if (itemToMove.prev != null)
            {
                temp = itemToMove.prev;
                temp.next = itemToMove.next;
                itemToMove.next.prev = temp;
            }
            else
            {
                start = itemToMove.next;
                start.prev = null;
            }

            itemToMove.next = null;
            end.next = itemToMove;
            itemToMove.prev = end;
            end = itemToMove;
        }

        public void RemoveByNode(Node itemToDelete)
        {
            if (itemToDelete.next == null)
            { 
                RemoveLast(out itemToDelete.data);
                return;
            }
            if (itemToDelete.prev == null)
            {
                RemoveFirst(out itemToDelete.data);
                return;
            }

            Node temp;
            temp = itemToDelete.prev; 
            temp.next = itemToDelete.next;
            temp.next.prev = temp;
        }

        public override string ToString()
        {
            Node temp = start;
            StringBuilder st = new StringBuilder();

            while (temp != null)
            {
                st.AppendLine(temp.data.ToString());
                temp = temp.next;
            }

            return st.ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node currentNode = start;
            while (currentNode != null)
            {
                yield return currentNode.data;
                currentNode = currentNode.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal class Node
        {
            public T data;
            public Node prev;
            public Node next;

            public Node(T data)
            {
                this.data = data;
                next = null;
                prev = null;
            }
        }
    }
}
