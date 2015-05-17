using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS
{
    public class Cell
    {
        public Cell(string name)                                //cell w/ no content
            : this(name, "", ""){ }

        public Cell(string name, object content)                // cell with no value
            : this(name, content, ""){ }

        public Cell(string name, object content, object value)  //all cell attributes
        {
            Name = name;
            Content = content;
            Value = Value;
        }

        public String Name { get; set; }
        public object Content { get; set; }
        public object Value { get; set; }

    }
}
