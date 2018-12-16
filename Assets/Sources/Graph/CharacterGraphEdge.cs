using Assets.Sources.Interfaces;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Sources.Graph
{
    public class CharacterGraphEdge : IEdge<CharacterGraphNode>, INamedObject
    {
        #region Private Variables

        private CharacterGraphNode _nodeIn;
        private CharacterGraphNode _nodeOut;

        #endregion

        #region Properties

        public CharacterGraphNode Source
        {
            get
            {
                return this._nodeIn;
            }
        }

        public CharacterGraphNode Target
        {
            get { return this._nodeOut; }
        }

        public string Name
        {
            get
            {
                return String.Format("{0}_{1}", this.Source.Name, this.Target.Name);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an edge with the node the graph starts from, 
        /// and the node the edge points to.
        /// </summary>
        /// <param name="nodeIn"></param>
        /// <param name="nodeOut"></param>
        public CharacterGraphEdge(CharacterGraphNode nodeIn, CharacterGraphNode nodeOut)
        {
            this._nodeIn = nodeIn;
            this._nodeOut = nodeOut;
        }

        #endregion

    }
}
