using System;
using System.Collections.Generic;
using System.Text;

namespace Borlay.DB.Search.Tests
{
    public class VartotojaiFilter : FilterBase
    {
        [Filter]
        public string Name { get; set; }

        [Filter(Operation = Operation.GreaterOrEqual)]
        public int? Id { get; set; }

        [Filter("Name", Operation = Operation.Contains)]
        public string NameContains { get; set; }

        [Filter("Name")]
        public string[] NameIn { get; set; }
    }
}
