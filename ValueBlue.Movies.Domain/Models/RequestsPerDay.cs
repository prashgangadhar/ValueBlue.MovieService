using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueBlue.Movies.Domain.Models
{
    public class SearchRequestsPerDay
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
