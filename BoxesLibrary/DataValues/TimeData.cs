using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesLibrary
{
    class TimeData
    {
        public DateTime DateLastBought { get; set; }

        public double WidthValue { get; set; }

        public double HeightValue { get; set; }

        public TimeData(double widthValue, double heightValue)
        {
            WidthValue = widthValue;
            HeightValue = heightValue;
            DateLastBought = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            var timeData = obj as TimeData;

            if (timeData == null) return false;

            return (timeData.HeightValue == HeightValue && timeData.WidthValue == WidthValue);
        }
    }
}
