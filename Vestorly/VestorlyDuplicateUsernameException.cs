using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vestorly
{
    /// <summary>
    /// Thrown when there already exists a duplicate email in the system
    /// </summary>
    public class VestorlyDuplicateUsernameException : VestorlyException
    {
        internal VestorlyDuplicateUsernameException(String message)
            : base(message)
        {
        }

        internal VestorlyDuplicateUsernameException()
            : base("Duplicate Username Found")
        {
        }


    }
}
