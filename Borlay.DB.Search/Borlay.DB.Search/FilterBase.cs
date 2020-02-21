using System;
using System.Collections.Generic;
using System.Text;

namespace Borlay.DB.Search
{
    public class FilterBase : IOrderBy, ISkipTake
    {
        public string OrderColumn { get; set; }

        public Direction OrderDirection { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; } = 10;
    }
}
