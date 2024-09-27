using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenWorld.DATA
{
    [CreateAssetMenu(fileName = "MapSettings", menuName = "OpenWorld/MapSettings", order = 1)]
    public class MapSettings : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private ObjectLayerMask _objectLayerMask;
        [SerializeField]
        private Material _waterMaterial;
        [SerializeField]
        private Material _terrainMaterial;
        [SerializeField, HideInInspector]
        private int _areaVisible = 1;
        // Нужно сделать выбор только 1 слоя
        [SerializeField, HideInInspector]
        private int _terrainLayer;
        
        public ObjectLayerMask ObjectLayerMask { get => _objectLayerMask; set => _objectLayerMask = value; }
        public int AreaVisible { get => _areaVisible; set => _areaVisible = value; }
        public Material WaterMaterial => _waterMaterial;
        public Material TerrainMaterial => _terrainMaterial;
        public int TerrainLayer { get => _terrainLayer; set => _terrainLayer = value; }
    }
}
