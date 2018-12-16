using Assets.Sources.Exceptions;
using Assets.Sources.Interfaces;
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
    /// <typeparam name="INamedObject">the type of the inputs to the map.</typeparam>
    /// <typeparam name="TOutput">the type of outputs of the map</typeparam>
    public abstract class Map<NamedObject, TOutput> where NamedObject: Interfaces.INamedObject
    {
        #region Properties

        /// <summary>
        /// Holds the entries of named objects to 
        /// </summary>
        private MapEntry<List<NamedObject>, TOutput>[] Entries
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
            string[][] allnames = mapEntries
                         .Select(entry => entry.Input.Select(obj => obj.Name).ToArray()).ToArray();

            for(int i = 0;i <allnames.Length -1;i++)
            {
                IEnumerable<string> intersection = allnames[i].Intersect(allnames[i + 1]);

                if (intersection.Count() > 0)
                {
                    throw new NamedObjectCollision(intersection.First(), this);
                }
            }

            this.Entries = mapEntries;

            
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Finds the map entry such that, each namedobject matches an entirelist
        /// in the entry.
        /// </summary>
        /// <param name="input">the output for the given input. Map to an empty array if the list does not contain at
        /// least one complete input set.</param>
        /// <returns>The output object</returns>
        public TOutput GetOuput(List<NamedObject> input)
        {
            MapEntry<List<NamedObject>, TOutput>[] entries = this.GetEntries(input);
            
            if(entries.Length == 0)
            {
                return default(TOutput);
            }
            else
            {
                return entries.First().Output;
            }
        }

       
        #endregion

        #region Abstract Methods

        /// <summary>
        /// Updates the outputs of the map entries according to the 
        /// new set of inputs.
        /// 
        /// Automatically groups each of the inputs into their output list.
        /// </summary>
        /// <param name="inputs">the set of inputs to perform the update operations on</param>
        public abstract void Update(List<NamedObject> inputs);

        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns the subset of inputs that belongs to the given entry.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="inputs"></param>
        /// <returns>the subset of inputs that belong to the given entry.</returns>
        protected List<NamedObject> Subset(MapEntry<List<NamedObject>, TOutput> entry, List<NamedObject> inputs)
        {
            string[] names = entry.Input.Select(input => input.Name).ToArray();
            List<NamedObject> subSet = new List<NamedObject>();

            foreach (NamedObject name in inputs)
            {
                if (names.Contains(name.Name))
                {
                    subSet.Add(name);
                }
            }

            return subSet;
        }

        /// <summary>
        /// Grabs the map entry that corresponds to the set of inputs given.
        /// </summary>
        /// <param name="inputs">the set of inputs that correspond to the map entry we are getting.</param>
        /// <returns>the map entry that matches the set of inputs.</returns>
        protected MapEntry<List<NamedObject>, TOutput>[] GetEntries(List<NamedObject> inputs)
        {
            List<MapEntry<List<NamedObject>, TOutput>> entries =
                                            new List<MapEntry<List<NamedObject>, TOutput>>();

            MapEntry<List<NamedObject>, TOutput>[] singleListObjects =
                                                this.Entries.Where(entry => entry.Input.Count == 1).ToArray();
            MapEntry<List<NamedObject>, TOutput>[] multipleListObjects =
                                                this.Entries.Where(entry => entry.Input.Count > 1).ToArray();
            string[] inputNames = inputs.Select(input => input.Name).ToArray();
            entries.AddRange(singleListObjects.Where(entry => inputNames.Contains(entry.Input[0].Name)));

            foreach (MapEntry<List<NamedObject>, TOutput> entry in this.Entries)
            {
                if (this.IsSubset(entry, inputs))
                {
                    entries.Add(entry);
                }
            }


            return entries.ToArray();
        }

        #endregion

        #region Private Methods




        /// <summary>
        /// Tests to see if inputs contains a subset of the names.
        /// </summary>
        /// <param name="entry">the entry we are testing inputs on to see if it has a subset of
        /// the names in entry.</param>
        /// <param name="inputs"></param>
        /// <returns>true if inputs contains a subset of the list of entry.</returns>
        private bool IsSubset(MapEntry<List<NamedObject>, TOutput> entry, List<NamedObject> inputs)
        {
            return this.Subset(entry, inputs).Count == entry.Input.Count;
        }


       

        #endregion

    }
}
