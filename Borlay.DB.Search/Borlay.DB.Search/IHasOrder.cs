using System;
using System.Collections.Generic;
using System.Text;

namespace Borlay.DB.Search
{
    public interface IHasOrder
    {
        OrderBy Order { get; }
    }
}
