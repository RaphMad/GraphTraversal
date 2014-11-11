namespace GraphTraversal
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Linq;

   /// <summary>
   /// The main program.
   /// </summary>
   internal class Program
   {
      /// <summary>
      /// The map.
      /// </summary>
      private static readonly ImmutableDirectedGraph<string, string> _map = ImmutableDirectedGraph<string, string>.Empty
                                                .AddEdge("Troll Room", "East West Passage", "East")
                                                .AddEdge("East West Passage", "Round Room", "East")
                                                .AddEdge("East West Passage", "Chasm", "North")
                                                .AddEdge("Round Room", "North South Passage", "North")
                                                .AddEdge("Round Room", "Loud Room", "East")
                                                .AddEdge("Chasm", "Reservoir South", "Northeast")
                                                .AddEdge("North South Passage", "Chasm", "North")
                                                .AddEdge("North South Passage", "Deep Canyon", "Northeast")
                                                .AddEdge("Loud Room", "Deep Canyon", "Up")
                                                .AddEdge("Reservoir South", "Dam", "East")
                                                .AddEdge("Deep Canyon", "Reservoir South", "Northwest")
                                                .AddEdge("Deep Canyon", "Dam", "East")
                                                .AddEdge("Dam", "Dam Lobby", "North")
                                                .AddEdge("Dam Lobby", "Maintenance Room", "East")
                                                .AddEdge("Dam Lobby", "Maintenance Room", "North");

      /// <summary>
      /// Defines the entry point of the application.
      /// </summary>
      private static void Main()
      {
         // FindPathOwnTry();
         // FindTraversals();
         // BinomialCoefficient();
         // BinomialCoefficient2();
         Inevitable();

         Console.ReadKey();
      }

      /// <summary>
      /// Own try for the find path method.
      /// </summary>
      private static void FindPathOwnTry()
      {
         ////Console.WriteLine("All paths (" + map.AllPathsFrom("Troll Room").Count() + "):" + Environment.NewLine);

         ////foreach (var path in map.AllPathsFrom("Troll Room"))
         ////{
         ////   foreach (var edge in path)
         ////   {
         ////      Console.WriteLine(edge.Key + " (@ " + edge.Value + ")");
         ////   }

         ////   Console.WriteLine();
         ////}

         Console.WriteLine(Environment.NewLine + "Found paths (" + _map.FindPaths("Troll Room", "Maintenance Room").Count() + "):" + Environment.NewLine);

         foreach (var path in _map.FindPaths("Troll Room", "Maintenance Room"))
         {
            foreach (var edge in path)
            {
               Console.WriteLine(edge.Key + " (@ " + edge.Value + ")");
            }

            Console.WriteLine();
         }
      }

      /// <summary>
      /// Finds the traversals.
      /// </summary>
      private static void FindTraversals()
      {
         foreach (var path in _map.AllEdgeTraversals("Troll Room"))
         {
            Console.WriteLine(string.Join(" ", path.Select(pair => pair.Key)));
         }
      }

      /// <summary>
      /// Calculates all subsets of size k from a set of size n.
      /// </summary>
      private static void BinomialCoefficient()
      {
         // edge function of the infinite graph encoding the problem
         Func<Tuple<int, int>, IReadOnlyDictionary<string, Tuple<int, int>>> getEdges = latticePoint =>
            {
               ImmutableDictionary<string, Tuple<int, int>> edges = ImmutableDictionary<string, Tuple<int, int>>.Empty;

               if (latticePoint.Item1 > 0)
               {
                  edges = edges.Add("Left", Tuple.Create(latticePoint.Item1 - 1, latticePoint.Item2));
               }

               if (latticePoint.Item2 > 0)
               {
                  edges = edges.Add("Down", Tuple.Create(latticePoint.Item1, latticePoint.Item2 - 1));
               }

               return edges;
            };

         // calculates subsets of size 3 from a set of 5 ("Left" = take item, "Down" = skip item), form of  (n-k, k)
         foreach (var path in GraphExtensions.AllEdgeTraversals(Tuple.Create(3, 2), getEdges))
            Console.WriteLine(string.Join(" ", path.Select(pair => pair.Key)));
      }

      /// <summary>
      /// Different approach to calculates all subsets of size k from a set of size n.
      /// </summary>
      private static void BinomialCoefficient2()
      {
         // edge function of the infinite graph encoding the problem
         Func<Tuple<int, int>, IReadOnlyDictionary<string, Tuple<int, int>>> getEdges = latticePoint =>
         {
            ImmutableDictionary<string, Tuple<int, int>> edges = ImmutableDictionary<string, Tuple<int, int>>.Empty;

            if (latticePoint.Item1 > latticePoint.Item2)
            {
               edges = edges.Add("X", Tuple.Create(latticePoint.Item1 - 1, latticePoint.Item2));
            }

            if (latticePoint.Item2 > 0)
            {
               edges = edges.Add((latticePoint.Item1 - 1).ToString(), Tuple.Create(latticePoint.Item1 - 1, latticePoint.Item2 - 1));
            }

            return edges;
         };

         // calculates subsets of size 3 from a set of 5 ("X" = bit not set, "<number>" = bit set), form of tuple is (n, k)
         foreach (var path in GraphExtensions.AllEdgeTraversals(Tuple.Create(5, 3), getEdges))
            Console.WriteLine(string.Join(" ", path.Select(pair => pair.Key)));
      }

      /// <summary>
      /// Finds inevitable nodes.
      /// </summary>
      private static void Inevitable()
      {
         const string NodeToCheck = "Round Room";

         Console.WriteLine("Inevitable node of " + NodeToCheck + " is: " + _map.ClosesInevitableNodeOptimized(NodeToCheck));
      }
   }
}