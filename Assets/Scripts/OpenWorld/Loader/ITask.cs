using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWorld.Loader
{
    public interface ITask
    {
        bool Completed { get; }
        void Cancel();
       
    }
}
