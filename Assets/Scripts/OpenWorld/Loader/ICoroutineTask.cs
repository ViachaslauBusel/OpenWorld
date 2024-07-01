using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWorld.Loader
{
    interface ICoroutineTask
    {
        bool MoveNext();
    }
}
