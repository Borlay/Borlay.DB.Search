using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Borlay.DB.Search
{
    public class FilterAttribute : Attribute
    {
        public string Column { get; set; }

        public Operation Operation { get; set; }

        public Conjunction Conjunction { get; set; }

        public FilterAttribute([CallerMemberName] string column = null)
        {
            this.Column = column;
        }
    }
}
