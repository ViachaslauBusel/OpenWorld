using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorld.DATA
{
    /// <summary>
    /// Container for storing data of a single tile
    /// </summary>
    public class Tile : ScriptableObject
    {
        [SerializeField] TerrainData _terrainData;
        [SerializeField] Mesh _waterTile;
        [SerializeField] List<MapObject> _objects;

        public TerrainData TerrainData => _terrainData;
        public Mesh WaterTile => _waterTile;
        public IReadOnlyCollection<MapObject> Objects => _objects;


        public void AddObject(MapObject obj)
        {
            _objects.Add(obj);
        }

        public void RemoveObject(MapObject obj)
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