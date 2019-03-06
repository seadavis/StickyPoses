using Assets.Sources.Interfaces;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Sources.Graph
{
    [Serializable]
    public class CharacterGraphEdge : IEdge<CharacterGraphNode>, INamedObject
    {
        #region Private Variables

        private CharacterGraphNode _nodeIn;
        private CharacterGraphNode _nodeOut;
        private Tuple<float, float> _angleRange;
        private bool _isInverted;

        #endregion

        #region Properties

        /// <summary>
        /// The direction that the character has in a "neutral" pose.
        /// </summary>
        public Vector3 NeutralDirection
        {
            get;
            set;
        }

        public bool IsInverted
        {
            get
            {
                return this._isInverted;
            }
            set
            {
                this._isInverted = value;
            }
        }

        public CharacterGraphNode Source
        {
            get
            {
                return this._nodeIn;
            }

            set
            {
                this._nodeIn = value;
            }
        }

        public CharacterGraphNode Target
        {
            get { return this._nodeOut; }

            set { this._nodeOut = value; }
        }

        public Tuple<float, float> AngleRange
        {
            get { return this._angleRange; }
            set { this._angleRange = value; }
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
            this.NeutralDirection = Vector3.UnitY;
            this._angleRange = new Tuple<float, float>(0, 360);
            this._isInverted = false;
        }

        #endregion

    }
}
