using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWorldEditor.Tabs.Setting
{
    public abstract class Container
    {
        public static List<ICell> Cells { get; } = new List<ICell>();
    }
}
