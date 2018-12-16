﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        #region Private Variables
        private float zValue;
        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapEntries"></param>
        /// <param name="zValue">the z-value to put all of the keypoints into.</param>
        public KeyPointMap(MapEntry<List<Keypoint>, CharacterGraphNode>[] mapEntries, float zValue) : base(mapEntries)
        {
            this.zValue = zValue;
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
            MapEntry<List<Keypoint>, CharacterGraphNode>[] entries = this.GetEntries(inputs);
          
            foreach(MapEntry<List<Keypoint>, CharacterGraphNode> entry in entries)
            {
                List<Keypoint> subset = this.Subset(entry, inputs);
                Vector3[] keyPointVectors = subset.Select(keypoint => keypoint.position)
                                                  .Select(v => new Vector3(v.X, v.Y, this.zValue))
                                                  .ToArray();
                entry.Output.Transformation.Position = keyPointVectors.Average();

            }
                
        }

        #endregion
    }
}
