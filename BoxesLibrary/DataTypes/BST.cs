﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxesLibrary
{
    class BST<T> where T : IComparable<T>
    {
        Node root;

        public bool IsEmpty() => root == null;

        public void Add(T newItem)
        {
            Node n = new Node(newItem);
            if (root == null)
            {
                root = n;
                return;
            }
            Node temp = root;
            Node parent = null;

            while (temp != null)
            {
                parent = temp;
                if (newItem.CompareTo(temp.data) < 0) temp = temp.left;
                else temp = temp.right;
            }

            if (newItem.CompareTo(parent.data) < 0) parent.left = n;
            else parent.right = n;
        }

        public bool Remove(T value) //O(log(n))
        {
            Node parent = null;
            Node currentRoot = GetNode(value, out parent);// get the specific node of the value (the first time it appears if there are more than one) 
            if (currentRoot == null) return false; //empty tree or value not exist

            //found (return true anyway)
            if (currentRoot.left == null && currentRoot.right == null) DeleteLeaf(currentRoot, parent); //leaf- delete conection to it
            else if (currentRoot.left == null && currentRoot.right != null) DeleteDegenarateRoot(currentRoot, currentRoot.right, parent); //degenarate root (no left leaf)
            else if (currentRoot.left != null && currentRoot.right == null) DeleteDegenarateRoot(currentRoot, currentRoot.left, parent); //degenarate root (no right leaf) 
            else //current root has two branches
            {
                Node UpdateNode = currentRoot;
                Node NodeToDelete = currentRoot.right; //going to the right branch at first
                UpdateNode.data = GetLeftiestLeaf(ref NodeToDelete, ref currentRoot); //giving the function the right branch and move till the leftiest leaf.
                                                                                      //currentRoot will be the parent of the NodeToDelete
                if (NodeToDelete.right == null) DeleteLeaf(NodeToDelete, currentRoot); //option one- leftiest (NodeToDelete) is a leaf
                else DeleteDegenarateRoot(NodeToDelete, NodeToDelete.right, currentRoot); //option two- NodeToDelete is just a degenerate root (with right branches)            
            }
            return true;
        }

        private void DeleteLeaf(Node leaf, Node parent)
        {
            if (parent == null) //Main root is the only element-private case 
            {
                root = null;
                return;
            }
            if (parent.right == leaf) parent.right = null;
            else parent.left = null;
        }

        private void DeleteDegenarateRoot(Node RootToDelete, Node next, Node parent) //Bring the next available generation to parent 
        {
            if (parent == null) //The root is the main root
            {
                root = next;
                return;
            }
            if (parent.right == RootToDelete) parent.right = next;
            else parent.left = next;
        }

        private Node GetNode(T value, out Node parent)
        {
            parent = null;
            if (root == null) return null; //Empty
            Node temp = root;
            while (temp != null)
            {
                if (value.CompareTo(temp.data) == 0) return temp;
                parent = temp;
                if (value.CompareTo(temp.data) < 0) temp = temp.left;
                else temp = temp.right;
            }
            return null; //Value wasn't found
        }

        private T GetLeftiestLeaf(ref Node startRoot, ref Node parent)
        {
            while (startRoot.left != null)
            {
                parent = startRoot;
                startRoot = startRoot.left;
            }
            return startRoot.data; //By the the time the loop end- startRoot will be a leaf
        }

        public bool Search(T itemToSearch, out T foundItem)
        {
            Node temp = root;
            while (temp != null)
            {
                if (itemToSearch.CompareTo(temp.data) == 0)
                {
                    foundItem = temp.data;
                    return true;
                }
                else if (itemToSearch.CompareTo(temp.data) < 0) temp = temp.left;
                else temp = temp.right;
            }
            foundItem = default;
            return false;
        }

        public bool SearchAndInsert(T itemToSearch, out T foundItem)
        {
            foundItem = default;
            if (root == null)
            {
                root = new Node(itemToSearch);
                return false;
            }
            Node temp = root;
            Node parent = null;

            while (temp != null)
            {
                parent = temp;
                if (itemToSearch.CompareTo(temp.data) == 0)
                {
                    foundItem = temp.data;
                    return true;
                }
                else if (itemToSearch.CompareTo(temp.data) < 0) temp = temp.left;
                else temp = temp.right;
            }

            Node n = new Node(itemToSearch);
            if (itemToSearch.CompareTo(parent.data) < 0) parent.left = n;
            else parent.right = n;
            return false;
        }

        public bool SearchTheClosest(T itemToSearch, out T foundItem)
        {
            Node temp = root;
            foundItem = default;

            while (temp != null)
            {
                if (itemToSearch.CompareTo(temp.data) == 0)
                {
                    foundItem = temp.data;
                    return true;
                }

                if (itemToSearch.CompareTo(temp.data) < 0)
                {
                    foundItem = temp.data;
                    temp = temp.left;
                }
                else temp = temp.right;
            }

            if (foundItem == null) return false;
            return true;
        }

        public void ScanInOrder(Action<T> action) => ScanInOrder(root, action);

        private void ScanInOrder(Node temp, Action<T> action)
        {
            if (temp == null) return;

            ScanInOrder(temp.left, action);
            action(temp.data);
            ScanInOrder(temp.right, action);
        }
        public int GetDepth() => GetDepth(root);

        private int GetDepth(Node temp) => (temp == null) ? 0 : 1 + Math.Max(GetDepth(temp.left), GetDepth(temp.right));

        class Node
        {
            public Node left;
            public Node right;
            public T data;
            public Node(T value)
            {
                data = value;
            }
        }
    }
}
