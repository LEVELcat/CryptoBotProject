using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoBotProject.WebParse
{
    class CoinUpdater
    {
        static CoinUpdater instance;
        public static CoinUpdater Instance
        {
            get
            {
                instance ??= new CoinUpdater();
                return instance;
            }
        }

        protected CoinUpdater()
        {

        }

        public void Start()
        {
            throw new NotImplementedException();
        }

    }
}
