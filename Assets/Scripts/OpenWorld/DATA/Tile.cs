using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorld.DATA
{
    /// <summary>
    /// Контейнер для хранения данных одного тайла
    /// </summary>
    public class Tile : ScriptableObject
    {
        public TerrainData terrainData;
        public Mesh waterTile;
        public List<MapObject> objects;
        [SerializeField] List<MapObject> m_objects = new List<MapObject>();

        public int ObjectsCount => m_objects.Count;
        public MapObject GetObject(int index) => m_objects[index];
        public void AddObject(MapObject obj)
        {
            m_objects.Add(obj);
        }
        public void Remove(MapObject obj)
        {
            m_objects.Remove(obj);
        }

    }
}