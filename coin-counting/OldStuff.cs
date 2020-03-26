using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coin_counting
{
    public class OldStuff
    {
        // this is used to determine the most efficient order of buying items cross-world
        public static List<IEnumerable<Listing>> GetMostEfficientPurchases(List<Listing> listings, int needed)
        {
            var target = Enumerable.Range(1, listings.Count)
                .SelectMany(p => listings.Combinations(p))
                .OrderBy(p => Math.Abs((int)p.Select(x => x.Quantity).Sum() - needed)) // sort by number of listings - fewest orders possible
                .ThenBy(x => x.Sum(y => y.Price * y.Quantity))
                .Where(x => x.Sum(y => y.Quantity) >= needed).Take(5).ToList(); // sort by total price of listing - typically leans towards more orders but cheaper overall

                return target;
        }
    }

    // for use with the GetMostEfficientPurchases command, to build order lists
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
                elements.SelectMany((e, i) =>
                    elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }
    }
}
