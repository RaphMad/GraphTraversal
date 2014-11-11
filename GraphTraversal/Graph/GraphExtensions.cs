namespace GraphTraversal
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Linq;

   /// <summary>
   /// Contains extension methods on graphs.
   /// </summary>
   internal static class GraphExtensions
   {
      /// <summary>
      /// Finds all paths from the "start" node to the "end" node.
      /// </summary>
      public static IEnumerable<IEnumerable<KeyValuePair<TEdge, TNode>>> FindPaths<TNode, TEdge>(
         this ImmutableDirectedGraph<TNode, TEdge> graph,
         TNode start,
         TNode finish)
      {
         // enumerate all paths starting at node "start", filter for the ones ending at node "finish"
         return graph.AllPathsFrom(start)
                     .Where(path => path.Last().Value.Equals(finish));
      }

      /// <summary>
      /// Finds all paths starting at the "start" node.
      /// </summary>
      public static IEnumerable<IEnumerable<KeyValuePair<TEdge, TNode>>> AllPathsFrom<TNode, TEdge>(
         this ImmutableDirectedGraph<TNode, TEdge> graph,
         TNode start)
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
      /// Calculates all edge traversals of the given graph.
      /// </summary>
      /// <typeparam name="TEdge">The type of the edge.</typeparam>
      /// <typeparam name="TNode">The type of the node.</typeparam>
      /// <param name="graph">The graph.</param>
      /// <param name="start">The start node.</param>
      /// <returns>All found edge traversals.</returns>
      public static IEnumerable<ImmutableStack<KeyValuePair<TEdge, TNode>>> AllEdgeTraversals<TEdge, TNode>(
         this ImmutableDirectedGraph<TNode, TEdge> graph,
         TNode start)
      {
         return AllEdgeTraversals(start, graph.Edges);
      }

      /// <summary>
      /// Calculates all edge traversals based on the given Edge function.
      /// </summary>
      /// <typeparam name="TEdge">The type of the edge.</typeparam>
      /// <typeparam name="TNode">The type of the node.</typeparam>
      /// <param name="start">The start.</param>
      /// <param name="getEdges">The edge function.</param>
      /// <returns>All found edge traversals.</returns>
      public static IEnumerable<ImmutableStack<KeyValuePair<TEdge, TNode>>> AllEdgeTraversals<TEdge, TNode>(
         TNode start,
         Func<TNode, IReadOnlyDictionary<TEdge, TNode>> getEdges)
      {
         var edges = getEdges(start);

         if (edges.Count == 0)
         {
            yield return ImmutableStack<KeyValuePair<TEdge, TNode>>.Empty;
         }
         else
         {
            foreach (var pair in edges)
               foreach (var path in AllEdgeTraversals(pair.Value, getEdges))
               {
                  yield return path.Push(pair);
               }
         }
      }

      /// <summary>
      /// Calculates the closes inevitable node of a given node.
      /// </summary>
      /// <typeparam name="TEdge">The type of the edge.</typeparam>
      /// <typeparam name="TNode">The type of the node.</typeparam>
      /// <param name="graph">The graph.</param>
      /// <param name="nodeToCheck">The node to check.</param>
      /// <returns>
      /// The closes inevitable node from the given node.
      /// </returns>
      public static TNode ClosesInevitableNode<TEdge, TNode>(this ImmutableDirectedGraph<TNode, TEdge> graph, TNode nodeToCheck)
      {
         var allTraversals = graph.AllEdgeTraversals(nodeToCheck).ToList();

         // take candidate nodes from first traversal, benefit: they are already ordered by distance
         var candidateNodes = allTraversals.First().Select(edge => edge.Value);

         // return the first candidate that is contained in all other traversals
         return candidateNodes.FirstOrDefault(candidateNode => allTraversals.Skip(1).All(traversal => traversal.Any(edge => edge.Value.Equals(candidateNode))));
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