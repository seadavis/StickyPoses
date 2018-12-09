using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Sources.Exceptions
{
    /// <summary>
    /// missing the output for the given mapper.
    /// </summary>
    public class MissingMapperOutputException : Exception
    {
        public MissingMapperOutputException(string input) : base(String.Format("Missing output for input: {0}", input))
        {

        }

    }
}
