namespace GraphTraversal
{
   using System;
   using System.Linq;

   /// <summary>
   /// The main program.
   /// </summary>
   internal class Program
   {
      /// <summary>
      /// Defines the entry point of the application.
      /// </summary>
      private static void Main()
      {
         // GraphDemo();
         FindPathOwnTry();

         Console.ReadKey();
      }

      /// <summary>
      /// Graph demo method.
      /// </summary>
      private static void GraphDemo()
      {
         var map = ImmutableDirectedGraph<string, string>.Empty
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
      }

      /// <summary>
      /// Own try for the find path method.
      /// </summary>
      private static void FindPathOwnTry()
      {
         var map = ImmutableDirectedGraph<string, string>.Empty
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

         foreach (var path in map.AllPathsFrom("Troll Room"))
         {
            Console.WriteLine("All paths: " +
                              path.Aggregate(string.Empty, (existing, newPath) => existing + " -> " + newPath.Key + " " + newPath.Value + ")")
                                  .TrimStart(' ', '-', '>'));
            Console.WriteLine();
         }

         foreach (var path in map.FindPaths("Troll Room", "Maintenance Room"))
         {
            Console.WriteLine("Found paths: " +
                              path.Aggregate(string.Empty, (existing, newPath) => existing + " -> " + newPath.Key + " (" + newPath.Value + ")")
                                  .TrimStart(' ', '-', '>'));
            Console.WriteLine();
         }
      }
   }
}