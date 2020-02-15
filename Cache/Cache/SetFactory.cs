using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    class SetFactory
    {
        public static Cache.Set.ISet Create(int numberOfLines, int wordsInLine, int wordSize)
        {
            Cache.Set.ISet setProxy = null;
            string setImplConfig = ConfigurationManager.AppSettings["SetImplementation"];
            if (setImplConfig.Equals("CLI"))
            {
                setProxy = new CacheSet.SetProxy(numberOfLines, wordsInLine, wordSize);
            }
            else if (setImplConfig.Equals("C#"))
            {
                setProxy = new Cache.Set.Cs.SetProxy(numberOfLines, wordsInLine, wordSize);
            }
            else
            {
                throw new ArgumentOutOfRangeException("incorrect app config for SetProxy");
            }
            return setProxy;
        }
    }
}
