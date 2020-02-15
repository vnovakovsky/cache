using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    class Comparator<Tag> where Tag : unmanaged, IComparable
    {
        public Comparator(Set<Tag> set)
        {
        }
    }
}
