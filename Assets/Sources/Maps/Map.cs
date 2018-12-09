using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Sources.Maps
{
    /// <summary>
    /// Representa a map.
    /// </summary>
    /// <typeparam name="NamedObject">the type of the inputs to the map.</typeparam>
    /// <typeparam name="TOutput">the type of outputs of the map</typeparam>
    public abstract class Map<NamedObject, TOutput>
    {
        #region Properties

        public MapEntry<List<NamedObject>, TOutput>[] Entries
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a map with a set of entries.
        /// </summary>
        /// <param name="mapEntries">the intial set of entries for the map.</param>
        public Map(MapEntry<List<NamedObject>, TOutput>[] mapEntries)
        {

        }

        #endregion

        #region Public Methods

        public TOutput GetOuput(NamedObject input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>the set of outputs that are currently mapped from the inputs.</returns>
        public TOutput[] GetOuputs()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears all of the output entries in a given map.
        /// </summary>
        public void Clear()
        {

        }


        #endregion

        #region Abstract Methods

        /// <summary>
        /// Updates the outputs of the map entries according to the 
        /// new set of inputs.
        /// </summary>
        /// <param name="inputs">the set of inputs to perform the update operations on</param>
        public abstract void Update(List<NamedObject> inputs);

        #endregion

    }
}
