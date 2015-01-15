using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vestorly
{
    /// <summary>
    /// Invalid Email or Misformatted Email Addresses 
    /// </summary>
    public class VestorlyInvalidEmailException : VestorlyException
    {
        internal VestorlyInvalidEmailException(String message)
            : base(message)
        {
        }

        internal VestorlyInvalidEmailException()
            : base("Duplicate Username Found")
        {
        }


    }
}
