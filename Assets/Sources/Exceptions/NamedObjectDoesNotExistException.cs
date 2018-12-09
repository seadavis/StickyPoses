using Assets.Sources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Sources.Exceptions
{
    /// <summary>
    /// Use when sometries to find a named object and it doesn't exist.
    /// </summary>
    public class NamedObjectDoesNotExistException : Exception
    {


        public NamedObjectDoesNotExistException(string name) : base(String.Format("The Object: {0} does not exist", name))
        {

        }

        public NamedObjectDoesNotExistException(INamedObject named): this(named.Name)
        {

        }

    }
}
