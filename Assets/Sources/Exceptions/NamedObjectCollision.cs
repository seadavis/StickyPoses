using Assets.Sources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Sources.Exceptions
{
    /// <summary>
    /// Use this exception when there a named object is added to the same
    /// collection twice.
    /// </summary>
    public class NamedObjectCollision : Exception
    {
        public NamedObjectCollision(string name, object owner) :
         base(String.Format("The Named Object: {0} already exists on {1}", name, owner.GetType().Name))
        { }


        public NamedObjectCollision(INamedObject named, object owner) : this(named.Name, owner)
        { }
    }
}
