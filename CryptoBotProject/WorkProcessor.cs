using CryptoBotProject.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoBotProject
{
    class WorkProcessor
    {
        static WorkProcessor instance;
        public static WorkProcessor Instance 
        { 
            get 
            { 
                instance ??= new WorkProcessor();
                return instance; 
            } 
        }

        protected WorkProcessor()
        {

        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}
