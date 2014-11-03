namespace GraphTraversal
{
   using System.Collections.Generic;
   using System.Linq;

   /// <summary>
   /// Contains extension methods on graphs.
   /// </summary>
   internal static class GraphExtensions
   {
      /// <summary>
      /// Finds all paths from the "start" node to the "end" node.
      /// </summary>
      public static IEnumerable<IEnumerable<KeyValuePair<TEdges, TNodes>>> FindPaths<TNodes, TEdges>(
         this ImmutableDirectedGraph<TNodes, TEdges> graph,
         TNodes start,
         TNodes finish)
      {
         // enumerate all paths starting at node "start", filter for the ones ending at node "finish"
         return graph.AllPathsFrom(start)
                     .Where(path => path.Last().Value.Equals(finish));
      }

      /// <summary>
      /// Finds all paths starting at the "start" node.
      /// </summary>
      public static IEnumerable<IEnumerable<KeyValuePair<TEdges, TNodes>>> AllPathsFrom<TNodes, TEdges>(
         this ImmutableDirectedGraph<TNodes, TEdges> graph,
         TNodes start)
      {
         // for each outgoing edge from "start"...
         foreach (var edge in graph.Edges(start))
         {
            // ...the edge itself represents a possible path
            yield return edge.Singleton();

            // ...as well as all paths of the form "edge + path starting at the end of the edge"
            foreach (var path in graph.AllPathsFrom(edge.Value))
               yield return path.Prepend(edge);
         }
      }

      /// <summary>
      /// Prepends an item to a sequence (creates a new sequence with the item as the 1st item).
      /// </summary>
      private static IEnumerable<T> Prepend<T>(this IEnumerable<T> sequence, T itemToPrepend)
      {
         yield return itemToPrepend;

         foreach (var item in sequence)
            yield return item;
      }

      /// <summary>
      /// Produces a singleton sequence consisting of exactly one item.
      /// </summary>
      private static IEnumerable<T> Singleton<T>(this T item)
      {
         yield return item;
      }
   }
}