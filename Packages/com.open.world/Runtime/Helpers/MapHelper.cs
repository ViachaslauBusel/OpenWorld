using Bundles;
using OpenWorld.DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenWorld.Helpers
{
    public static class MapHelper
    {
        public static TileLocation CalculateLocation(this Map map, Vector3 startPosition)
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

        public static Vector3 ClampPosition(this Map map, Vector3 position)
        {
            float x = Mathf.Clamp(position.x, 0f, map.MapSizeKilometers * map.TilesPerKilometer * map.TileSize);
            float z = Mathf.Clamp(position.z, 0, map.MapSizeKilometers * map.TilesPerKilometer * map.TileSize);
            return new Vector3(x, position.y, z);
        }

        public static bool IsLocationValid(this Map map, TileLocation location)
        {
            return location.Xkm >= 0 && location.Ykm >= 0 && location.Xkm < map.MapSizeKilometers && location.Ykm < map.MapSizeKilometers;
        }


        /// <summary>
        /// Transform position from world space to map space
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector3 WorldToMapPoint(this Map map, Vector3 position)
        {
            position -= map.WorldStartPoint;
            position.x = Mathf.Clamp(position.x, 0, map.MapSizeKilometers * Map.SIZE_KMBLOCK);
            position.z = Mathf.Clamp(position.z, 0, map.MapSizeKilometers * Map.SIZE_KMBLOCK);
            return position;
        }
    }
}
