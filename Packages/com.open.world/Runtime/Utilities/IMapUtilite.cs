using OpenWorld.DATA;
using UnityEngine;

namespace OpenWorld.Utilities
{
    public interface IMapUtility
    {
        string Name { get; }
    }

    public interface IMapUtilityForGameMap : IMapUtility
    {
        void Execute(GameMap map);
    }

    public interface IMapUtilityForMapTile : IMapUtility
    {
        void BeginExecution(GameMap map);
        bool Execute(MapTile tile, TileLocation location);
        void EndExecution(bool success);
    }

    public interface IMapUtilityForMapEntity : IMapUtility
    {
        void BeginExecution(GameMap map);
        bool Execute(MapEntity mapSettings);
        void EndExecution(bool success);
    }
}
