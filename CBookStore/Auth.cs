using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBookStore
{
    class Auth
    {
        public bool isWorker = false;
        private static Auth instance = null;
        private Auth() { 
            
        }

        public static Auth GetInstance() {
            if (instance == null) { 
                instance = new Auth();
            }

            return instance;
        }

        public void setIfIsWorker(int value) {
            isWorker = (value > 1) ? true : false;
        }

        public bool IsAdmin() {
            return this.isWorker;
        }
    
    }
}
