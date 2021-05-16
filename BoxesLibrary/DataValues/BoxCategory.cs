using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesLibrary
{
    class BoxCategory
    {
        public Width WidthValue { get; set; }
        public Height HeightValue { get; set; }

        public BoxCategory(Width width, Height height)
        {
            WidthValue = width;
            HeightValue = height;
        }
    }
}
