using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace githubConnect
{
    public class Languages
    {
        public Languages(string t, string l)
        {
            type = t;
            lines = l;
        }
        public string type { get; private set; }
        public string lines { get; private set; }
    }
}
