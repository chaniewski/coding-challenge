using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge.Impl
{
    internal class ResultBuilder
    {
        private Dictionary<Color, int> _colorStats;
        private Dictionary<Size, int> _sizeStats;
        private List<Shirt> _shirts = new List<Shirt>();

        public ResultBuilder()
        {
            _colorStats = Color.All.ToDictionary(c => c, _ => 0);
            _sizeStats = Size.All.ToDictionary(c => c, _ => 0);
        }

        public void AddShirt(Shirt shirt)
        {
            _shirts.Add(shirt);
            _colorStats[shirt.Color]++;
            _sizeStats[shirt.Size]++;
        }

        public SearchResults Build()
        {
            return new SearchResults
            {
                Shirts = _shirts,
                ColorCounts = _colorStats.Select(c => new ColorCount { Color = c.Key, Count = c.Value }).ToList(),
                SizeCounts = _sizeStats.Select(c => new SizeCount { Size = c.Key, Count = c.Value }).ToList()
            };
        }
    }
}
