using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public static class Util
    {
        public static int ConvertToInt<Tag>(Tag tag)
        {
            return (int)System.Convert.ChangeType(tag, typeof(int));
        }
    }
}
