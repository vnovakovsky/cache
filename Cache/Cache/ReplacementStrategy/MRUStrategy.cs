﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache.ReplacementStrategy
{
    public class MRUStrategy<Tag> : IReplacementStrategy<Tag>
    {
        public Set<Tag> SelectVictim(Tag tag)
        {
            throw new NotImplementedException();
        }

        public void SetRecentWord(Tag tag)
        {
            throw new NotImplementedException();
        }
    }
}