using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueBlue.Movies.Application.Exceptions
{
    public class MovieSearchFailedException : Exception
    {
        public MovieSearchFailedException() { } //For unit tests
        
        public MovieSearchFailedException(string message) : base(message) { }
    }
}
