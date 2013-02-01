using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBookStore
{
    class Auth
    {
        private static bool isWorker { get { return isWorker; } set { isWorker = value; } }
        private static Auth instance = null;
        private Auth() { 
            
        }

        public static Auth GetInstance() {
            if (instance == null) { 
                instance = new Auth();
            }

            return instance;
        }
    }
}
