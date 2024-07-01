using Bundles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWorld
{
    public interface ITile
    {
        string name { get; }
        void Load(BundlesMap bundlesMap, TileLocation location, MapLoader mapLoader);
        void Dispose();
    }
}
