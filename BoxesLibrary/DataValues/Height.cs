using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesLibrary
{
    class Height : IComparable<Height>
    {
        public double HeightValue { get; set; }

        public int Count { get; set; }

        public Linked_List<TimeData>.Node timeListNodeRef { get; set; }

        public Height(double heightValue) //Dummy constructor
        {
            HeightValue = heightValue;
        }

        public Height(double heightValue, int count)
        {
            HeightValue = heightValue;
            Count = count;
        }

        public int CompareTo(Height otherHeightData)
        {
            if (HeightValue > otherHeightData.HeightValue)
                return 1;
            else if (HeightValue < otherHeightData.HeightValue)
                return -1;
            else
                return 0;
        }
    }
}
