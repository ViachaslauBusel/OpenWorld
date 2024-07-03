using OpenWorld.DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWorld.Helpers
{
    internal static class ObjectLayerMaskHelper
    {
        internal static bool ContainsLayer(this ObjectLayerMask layermask, int layer)
        {
            return ((int)layermask & (1 << layer)) != 0;
        }
    }
}
