using System;
using System.Collections.Generic;
using System.Text;

namespace Borlay.DB.Search
{
    public interface IOrderBy
    {
        string OrderColumn { get; }
        Direction OrderDirection { get; }
    }
}
