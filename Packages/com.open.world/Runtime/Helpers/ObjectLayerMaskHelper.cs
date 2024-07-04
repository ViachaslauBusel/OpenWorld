using OpenWorld.DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWorld.Helpers
{
    public static class ObjectLayerMaskHelper
    {
        public static bool ContainsLayer(this ObjectLayerMask layermask, int layer)
        {
            return ((int)layermask & (1 << layer)) != 0;
        }
    }
}
