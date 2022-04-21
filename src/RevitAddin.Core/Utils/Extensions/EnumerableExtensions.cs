using System.Collections;
using System.Collections.Generic;

namespace RevitAddin.Core.Utils.Extensions
{
    /// <summary>
    /// Class EnumerableExtensions.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Gets the count of the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>System.Int32.</returns>
        public static int Count(this IEnumerable source)
        {
            int c = 0;
            var e = source.GetEnumerator();
            while (e.MoveNext())
                c++;
            return c;
        }

        public static List<T> ToList<T>(this IEnumerable set)
        {
            var list = new List<T>();

            foreach (T element in set) list.Add(element);

            return list;
        }
    }
}
