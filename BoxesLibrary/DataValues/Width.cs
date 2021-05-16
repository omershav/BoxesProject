using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesLibrary
{
    class Width : IComparable<Width>
    {
        public double WidthValue { get; set; }

        public BST<Height> HeightTree { get; set; }

        public Width(double width) //Dummy constructor
        {
            WidthValue = width;
        }

        public Width(double width, Height height)
        {
            WidthValue = width;

            HeightTree = new BST<Height>();

            HeightTree.Add(height);
        }

        public int CompareTo(Width otherWidthData)
        {
            if (WidthValue > otherWidthData.WidthValue)
                return 1;
            else if (WidthValue < otherWidthData.WidthValue)
                return -1;
            else
                return 0;
        }
    }
}
