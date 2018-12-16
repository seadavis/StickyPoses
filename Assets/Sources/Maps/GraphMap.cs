using Assets.Sources.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Sources.Maps
{
    /// <summary>
    /// Maps a set of edges, to a given set of edges.
    /// </summary>
    public class GraphMap : Map<CharacterGraphEdge, List<CharacterGraphEdge>>
    {
        public GraphMap(MapEntry<List<CharacterGraphEdge>, List<CharacterGraphEdge>>[] mapEntries) : base(mapEntries)
        {
        }

        /// <summary>
        /// Updates the outputs to have the same rotation as the given set of inputs.
        /// </summary>
        /// <param name="inputs">the list of graph edges that have updates, calculates the edge from them.</param>
        public override void Update(List<CharacterGraphEdge> inputs)
        {
            MapEntry<List<CharacterGraphEdge>, List<CharacterGraphEdge>>[] entries = this.GetEntries(inputs);

            foreach(MapEntry<List<CharacterGraphEdge>, List<CharacterGraphEdge>> entry in entries)
            {
                List<CharacterGraphEdge> subset = this.Subset(entry, inputs);
                Vector3 sourceAverage = subset.Select(edge => edge.Source.Transformation.Position).Average();
                Vector3 targetAverage = subset.Select(edge => edge.Target.Transformation.Position).Average();

                foreach(CharacterGraphEdge edge in entry.Output)
                {
                    edge.Source.Transformation.Position = sourceAverage;
                    edge.Target.Transformation.Position = targetAverage;
                }
            }

        }

    }
}
