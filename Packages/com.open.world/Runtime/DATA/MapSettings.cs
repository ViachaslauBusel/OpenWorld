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
        [SerializeField, HideInInspector]
        private int _areaVisible = 1;
        [SerializeField] Material _waterMaterial;
        [SerializeField] Material _terrainMaterial;

        public ObjectLayerMask ObjectLayerMask { get => _objectLayerMask; set => _objectLayerMask = value; }
        public int AreaVisible { get => _areaVisible; set => _areaVisible = value; }
        public Material WaterMaterial => _waterMaterial;
        public Material TerrainMaterial => _terrainMaterial;
    }
}
