using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Sources.Graph;
using static PoseNet;

namespace Assets.Sources.Maps
{
    /// <summary>
    /// Defines the mapping from a set of keypoints to a 
    /// character graph node.
    /// </summary>
    public class KeyPointMap : Map<Keypoint, CharacterGraphNode>

    {
        #region Constructor

        public KeyPointMap(MapEntry<List<Keypoint>, CharacterGraphNode>[] mapEntries) : base(mapEntries)
        {
        }

        #endregion

        #region Public Methods

       
        /// <summary>
        /// Updates the list of character graph nodes, so that their,
        /// position matches the position on the keypoints.
        /// </summary>
        /// <param name="inputs">the set of keypoints to update.</param>
        public override void Update(List<Keypoint> inputs)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
