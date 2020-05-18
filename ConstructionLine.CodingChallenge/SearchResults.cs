using System;
using System.Collections.Generic;

namespace ConstructionLine.CodingChallenge
{
    public class SearchResults
    {
        public List<Shirt> Shirts { get; internal set; }


        public List<SizeCount> SizeCounts { get; internal set; }


        public List<ColorCount> ColorCounts { get; internal set; }
    }


    public class SizeCount
    {
        public Size Size { get; internal set; }

        public int Count { get; internal set; }
    }


    public class ColorCount
    {
        public Color Color { get; internal set; }

        public int Count { get; internal set; }
    }
}