using Assets.Sources.Graph;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Sources.Exceptions
{
    /// <summary>
    /// Use this exception if the two edges are not connected, by a path.
    /// and the argument assumed they were.
    /// </summary>
    public class EdgesNotConnectedException : Exception
    {

        public EdgesNotConnectedException(CharacterGraphNode source, CharacterGraphNode target)
            : base(String.Format("There is no path in the graph from {0} to {1}", source.Name, target.Name))
        {

        }

    }
}
