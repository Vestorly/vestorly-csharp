using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vestorly
{

    /// <summary>
    /// Generic Exception for all Vestorly errors
    /// </summary>
    public class VestorlyException  : Exception
    {
        internal VestorlyException(String message) : base(message)
        {
        }

        internal VestorlyException() : base() {
        }

       
    }

}
