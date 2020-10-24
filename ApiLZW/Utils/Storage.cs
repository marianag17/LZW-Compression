using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLZW.Utils
{
    public class Storage
    {
        private static Storage _instance = null;

        public static Storage Instance
        {

            get
            {
                if (_instance == null) _instance = new Storage();
                return _instance;
            }
        }

        public List<Compressions> listaCompresiones = new List<Compressions>();
      
    }
}
