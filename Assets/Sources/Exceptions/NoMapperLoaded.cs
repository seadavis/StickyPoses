using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Sources.Exceptions
{
    public class NoMapperLoaded : Exception
    {
        public NoMapperLoaded(string modelName) : base(String.Format("Mapper not loaded for model: {0}", modelName))
        {

        }

    }
}
