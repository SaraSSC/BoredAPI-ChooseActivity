using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SecondTryTest
{
    public class Activities
    {
        public int Key { get; set; }

        public string Activity { get; set; }

        public string Type { get; set; }

        public int Participants { get; set; }


       /* public override string ToString()
        *{
        *   return $"{ActivityName} - {Type} - {Participants}";
        *}
        */
    }
}
