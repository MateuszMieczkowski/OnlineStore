using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersBase.Server.Services.Exceptions
{
    public class DuplicateSizeException : DuplicateException
    {
        public DuplicateSizeException() : base(message: "This size already exists")
        {

        }
        public DuplicateSizeException(string message) : base(message)
        {

        }
    }
}
