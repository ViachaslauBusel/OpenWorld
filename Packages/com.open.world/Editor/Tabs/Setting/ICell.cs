using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWorldEditor.Tabs.Setting
{
    public interface ICell
    {
        bool IsSave { get; }

        void Save();
    }
}
