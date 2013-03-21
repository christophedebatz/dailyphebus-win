using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPhebus
{
   public class BusStop
    {

        public string name { get; private set; }
        public string code { get; private set; }

        public BusStop(string name, string code)
        {
            this.name = name;
            this.code = code;
        }


        public override string ToString()
        {
            return this.name;
        }
    }
}
