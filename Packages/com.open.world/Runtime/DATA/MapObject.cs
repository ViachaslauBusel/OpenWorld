using DATA;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenWorld.DATA
{
    /// <summary>
    /// Data for saving a single object on the map
    /// </summary>
    [System.Serializable]
    public class MapObject
    {
        [SerializeField] Vector3 _position;
        [SerializeField] Quaternion _rotation;
        [SerializeField] Vector3 _scale;
        [SerializeField] Prefab<GameObject> _prefab;

        public Vector3 Position => _position;
        public Quaternion Rotation => _rotation;
        public Vector3 Scale => _scale;
        public Prefab<GameObject> Prefab => _prefab;

#if UNITY_EDITOR
        public MapObject(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            _prefab = new Prefab<GameObject>(prefab);
            _position = position;
            _rotation = rotation;
            _scale = scale;
        }
#endif
    }
}