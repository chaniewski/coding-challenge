using ConstructionLine.CodingChallenge.Impl;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly IEnumerable<Shirt> _shirts;
        private readonly PredicateBuilder _predicateBuilder;

        public SearchEngine(IEnumerable<Shirt> shirts)
        {
            // normally would be injected from a container
            _predicateBuilder = new PredicateBuilder();
            _shirts = shirts;
        }


        public SearchResults Search(SearchOptions options)
        {
            var resultBuilder = new ResultBuilder();
            var shirtMatchesOptions = _predicateBuilder.GetSearchPredicate(options);
            // Note: I've changed the type of _shirts field to IEnumerable<Shirt>
            // This way, if we get the shirts in the form of a database query (EF IQueryable<Shirt>, for example),
            // then the query wouldn't get materialised until the enumeration below happens - and the predicate will 
            // be executed against the database, not in-memory. For large data set it should improve the performance a lot.
            // Of course this class can still work on in-memory collections, as demonstrated by the tests.
            foreach(var shirt in _shirts.Where(shirtMatchesOptions))
            {
                resultBuilder.AddShirt(shirt);
            }

            return resultBuilder.Build();
        }
    }
}