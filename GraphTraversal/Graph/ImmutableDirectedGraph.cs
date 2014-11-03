namespace GraphTraversal
{
   using System.Collections.Generic;
   using System.Collections.Immutable;

   /// <summary>
   /// Represents an immutable directed graph.
   /// </summary>
   /// <typeparam name="TNodes">The type of the nodes.</typeparam>
   /// <typeparam name="TEdges">The type of the edges.</typeparam>
   internal struct ImmutableDirectedGraph<TNodes, TEdges>
   {
      /// <summary>
      /// The empty graph.
      /// </summary>
      public readonly static ImmutableDirectedGraph<TNodes, TEdges> Empty =
         new ImmutableDirectedGraph<TNodes, TEdges>(ImmutableDictionary<TNodes, ImmutableDictionary<TEdges, TNodes>>.Empty);

      /// <summary>
      /// Stores the actual grpah.
      /// </summary>
      private readonly ImmutableDictionary<TNodes, ImmutableDictionary<TEdges, TNodes>> _graph;

      /// <summary>
      /// Initializes a new instance of the <see cref="ImmutableDirectedGraph{TNodes, TEdges}" /> struct.
      /// </summary>
      /// <param name="graph">The graph.</param>
      private ImmutableDirectedGraph(ImmutableDictionary<TNodes, ImmutableDictionary<TEdges, TNodes>> graph)
      {
         _graph = graph;
      }

      /// <summary>
      /// Adds a new node to the graph.
      /// </summary>
      /// <param name="node">The node.</param>
      /// <returns>A new graph with the node added.</returns>
      public ImmutableDirectedGraph<TNodes, TEdges> AddNode(TNodes node)
      {
         ImmutableDirectedGraph<TNodes, TEdges> newGraph = this;

         if (!_graph.ContainsKey(node))
         {
            newGraph = new ImmutableDirectedGraph<TNodes, TEdges>(_graph.Add(node, ImmutableDictionary<TEdges, TNodes>.Empty));
         }

         return newGraph;
      }

      /// <summary>
      /// Adds a new edge to the graph.
      /// </summary>
      /// <param name="start">The start node.</param>
      /// <param name="finish">The finish node.</param>
      /// <param name="edge">The edge to add.</param>
      /// <returns>A new graph with the edge added.</returns>
      public ImmutableDirectedGraph<TNodes, TEdges> AddEdge(TNodes start, TNodes finish, TEdges edge)
      {
         // make sure both start and finish are present
         ImmutableDirectedGraph<TNodes, TEdges> adjustedGraph = AddNode(start).AddNode(finish);

         return new ImmutableDirectedGraph<TNodes, TEdges>(adjustedGraph._graph.SetItem(start, adjustedGraph._graph[start].SetItem(edge, finish)));
      }

      /// <summary>
      /// Gets the edges starting at a specified node.
      /// </summary>
      /// <param name="node">The node.</param>
      /// <returns>The outgoing edges of the node, or the empty set if the node is not in the graph.</returns>
      public IReadOnlyDictionary<TEdges, TNodes> Edges(TNodes node)
      {
         return _graph.ContainsKey(node) ? _graph[node] : ImmutableDictionary<TEdges, TNodes>.Empty;
      }
   }
}