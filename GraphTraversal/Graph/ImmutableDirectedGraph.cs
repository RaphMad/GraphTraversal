namespace GraphTraversal
{
   using System.Collections.Generic;
   using System.Collections.Immutable;

   /// <summary>
   /// Represents an immutable directed graph.
   /// </summary>
   /// <typeparam name="TNode">The type of the nodes.</typeparam>
   /// <typeparam name="TEdge">The type of the edges.</typeparam>
   internal struct ImmutableDirectedGraph<TNode, TEdge>
   {
      /// <summary>
      /// The empty graph.
      /// </summary>
      public readonly static ImmutableDirectedGraph<TNode, TEdge> Empty =
         new ImmutableDirectedGraph<TNode, TEdge>(ImmutableDictionary<TNode, ImmutableDictionary<TEdge, TNode>>.Empty);

      /// <summary>
      /// Stores the actual grpah.
      /// </summary>
      private readonly ImmutableDictionary<TNode, ImmutableDictionary<TEdge, TNode>> _graph;

      /// <summary>
      /// Initializes a new instance of the <see cref="ImmutableDirectedGraph{TNode, TEdge}" /> struct.
      /// </summary>
      /// <param name="graph">The graph.</param>
      private ImmutableDirectedGraph(ImmutableDictionary<TNode, ImmutableDictionary<TEdge, TNode>> graph)
      {
         _graph = graph;
      }

      /// <summary>
      /// Adds a new node to the graph.
      /// </summary>
      /// <param name="node">The node.</param>
      /// <returns>A new graph with the node added.</returns>
      public ImmutableDirectedGraph<TNode, TEdge> AddNode(TNode node)
      {
         ImmutableDirectedGraph<TNode, TEdge> newGraph = this;

         if (!_graph.ContainsKey(node))
         {
            newGraph = new ImmutableDirectedGraph<TNode, TEdge>(_graph.Add(node, ImmutableDictionary<TEdge, TNode>.Empty));
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
      public ImmutableDirectedGraph<TNode, TEdge> AddEdge(TNode start, TNode finish, TEdge edge)
      {
         // make sure both start and finish are present
         ImmutableDirectedGraph<TNode, TEdge> adjustedGraph = AddNode(start).AddNode(finish);

         return new ImmutableDirectedGraph<TNode, TEdge>(adjustedGraph._graph.SetItem(start, adjustedGraph._graph[start].SetItem(edge, finish)));
      }

      /// <summary>
      /// Gets the edges starting at a specified node.
      /// </summary>
      /// <param name="node">The node.</param>
      /// <returns>The outgoing edges of the node, or the empty set if the node is not in the graph.</returns>
      public IReadOnlyDictionary<TEdge, TNode> Edges(TNode node)
      {
         return _graph.ContainsKey(node) ? _graph[node] : ImmutableDictionary<TEdge, TNode>.Empty;
      }
   }
}