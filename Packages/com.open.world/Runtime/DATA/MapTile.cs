using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorld.DATA
{
    /// <summary>
    /// Container for storing data of a single tile
    /// </summary>
    public class MapTile : ScriptableObject
    {
        [SerializeField] TerrainData _terrainData;
        [SerializeField] Mesh _waterTile;
        [SerializeField] List<MapEntity> _objects;

        public TerrainData TerrainData => _terrainData;
        public Mesh WaterTile => _waterTile;
        public IReadOnlyCollection<MapEntity> Entities => _objects;


        public void AddEntity(MapEntity obj)
        {
            _objects.Add(obj);
        }

        public void RemoveEntity(MapEntity obj)
        {
            _objects.Remove(obj);
        }

        public void SetTerrainData(TerrainData terrainData)
        {
            _terrainData = terrainData;
        }

        public void SetWater(Mesh waterTile)
        {
            _waterTile = waterTile;
        }
    }
}