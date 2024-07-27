using OpenWorld;
using OpenWorld.DATA;
using OpenWorld.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWorldEditor
{
    internal class FixMissingMapElement : IMapUtilityForMapTile
    {
        public string Name => "Remove missing Tree/Grass";

        public void BeginExecution(GameMap map)
        {
        }

        public bool Execute(MapTile tile, TileLocation location)
        {
            //mapElement.TerrainData.wavingGrassSpeed = 0.2f;
            //mapElement.TerrainData.wavingGrassAmount = 0.31f;
            //mapElement.TerrainData.wavingGrassStrength = 0.3f;
            //  mapElement.terrainData.baseMapResolution = 64;
            //  mapElement.terrainData.SetDetailResolution(128, 32);
            bool isNeedFix = tile.TerrainData.treeInstances.Any(t => t.prototypeIndex >= tile.TerrainData.treePrototypes.Length);
            if(isNeedFix) tile.TerrainData.treeInstances = tile.TerrainData.treeInstances.Where(t => t.prototypeIndex < tile.TerrainData.treePrototypes.Length).ToArray();
            return isNeedFix;
        }

        public void EndExecution(bool success)
        {
        }
    }
}
