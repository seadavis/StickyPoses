using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Sources.Interfaces
{
    /// <summary>
    /// An object that has a string name.
    /// </summary>
    public interface INamedObject
    {

        string Name
        {
            get;
        }

    }
}
