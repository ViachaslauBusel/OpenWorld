using OpenWorld.DATA;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenWorld.Helpers
{
    public static class MapHelper
    {
        public static TileLocation CalculateLocation(this GameMap map, Vector3 startPosition)
        {
            TileLocation startLocation = new TileLocation(map)
            {
                Xkm = (int)(startPosition.x / 1000.0f),
                Ykm = (int)(startPosition.z / 1000.0f),
                Xtr = (int)((startPosition.x % 1000.0f) / map.TileSize),
                Ytr = (int)((startPosition.z % 1000.0f) / map.TileSize)
            };
            return startLocation;
        }

        public static Vector3 ClampPosition(this GameMap map, Vector3 position)
        {
            float x = Mathf.Clamp(position.x, 0f, map.MapSizeKilometers * map.TilesPerKilometer * map.TileSize);
            float z = Mathf.Clamp(position.z, 0, map.MapSizeKilometers * map.TilesPerKilometer * map.TileSize);
            return new Vector3(x, position.y, z);
        }

        public static bool IsLocationValid(this GameMap map, TileLocation location)
        {
            return location.Xkm >= 0 && location.Ykm >= 0 && location.Xkm < map.MapSizeKilometers && location.Ykm < map.MapSizeKilometers;
        }


        /// <summary>
        /// Transform position from world space to map space
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector3 WorldToMapPoint(this GameMap map, Vector3 position)
        {
            position -= map.WorldStartPoint;
            position.x = Mathf.Clamp(position.x, 0, map.MapSizeKilometers * GameMap.SIZE_KMBLOCK);
            position.z = Mathf.Clamp(position.z, 0, map.MapSizeKilometers * GameMap.SIZE_KMBLOCK);
            return position;
        }


        /// <summary>
        /// Enumerates all tile locations within the map.
        /// </summary>
        /// <param name="map">The map to enumerate tile locations for.</param>
        /// <returns>An enumerable of all possible tile locations within the map.</returns>
        public static IEnumerable<TileLocation> EnumerateAllTileLocations(this GameMap map)
        {
            for (int kilometerY = 0; kilometerY < map.MapSizeKilometers; kilometerY++)
            {
                for (int kilometerX = 0; kilometerX < map.MapSizeKilometers; kilometerX++)
                {
                    for (int tileRow = 0; tileRow < map.TilesPerKilometer; tileRow++)
                    {
                        for (int tileColumn = 0; tileColumn < map.TilesPerKilometer; tileColumn++)
                        {
                            yield return new TileLocation(map)
                            {
                                Xkm = kilometerX,
                                Ykm = kilometerY,
                                Xtr = tileColumn,
                                Ytr = tileRow
                            };
                        }
                    }
                }
            }
        }

        public static IEnumerable<(MapTile Tile, TileLocation Location)> EnumerateAllTiles(this GameMap map)
        {
#if UNITY_EDITOR
            foreach (TileLocation location in map.EnumerateAllTileLocations())
            {
                yield return (AssetDatabase.LoadAssetAtPath<MapTile>(location.Path), location);
            }
#else
            throw new NotImplementedException();
#endif
        }

        public static IEnumerable<MapEntity> EnumerateAllMapEntities(this GameMap map)
        {
            foreach ((MapTile Tile, TileLocation Location) tile in map.EnumerateAllTiles())
            {
                foreach (MapEntity mapObject in tile.Tile.Entities)
                {
                    yield return mapObject;
                }
            }
        }
    }
}
