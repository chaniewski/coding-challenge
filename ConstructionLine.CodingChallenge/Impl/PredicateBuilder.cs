using System;

namespace ConstructionLine.CodingChallenge.Impl
{
    internal class PredicateBuilder
    {
        public Func<Shirt, bool> GetSearchPredicate(SearchOptions options)
        {
            Func<Shirt, bool> sizePredicate = options.Sizes.Count == 0
                ? (Func<Shirt, bool>)((Shirt s) => true)
                : (Func<Shirt, bool>)((Shirt s) => options.Sizes.Contains(s.Size));

            Func<Shirt, bool> colorPredicate = options.Colors.Count == 0
                ? (Func<Shirt, bool>)((Shirt s) => true)
                : (Func<Shirt, bool>)((Shirt s) => options.Colors.Contains(s.Color));

            return s => sizePredicate(s) && colorPredicate(s);
        }
    }
}
