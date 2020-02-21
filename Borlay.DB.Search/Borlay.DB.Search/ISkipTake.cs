using System;
using System.Collections.Generic;
using System.Text;

namespace Borlay.DB.Search
{
    public interface ISkipTake
    {
        int? Skip { get; }

        int? Take { get; }
    }

}
