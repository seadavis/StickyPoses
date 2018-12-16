using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Sources.Maps
{
    public class MapEntry<TInput, TOutput>
    {
        #region Properties

        public TInput Input
        {
            get;
            set;
        }

        public TOutput Output
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a map entry with the input and output.
        /// </summary>
        /// <param name="input">the input element of the map entry.</param>
        /// <param name="output">the output element of the map entry.</param>
        public MapEntry(TInput input, TOutput output)
        {
            this.Input = input;
            this.Output = output;
        }

        #endregion

      

    }
}
