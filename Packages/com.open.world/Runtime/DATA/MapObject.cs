using DATA;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace OpenWorld.DATA
{
    /// <summary>
    /// Data for saving a single object on the map
    /// </summary>
    [System.Serializable]
    public class MapObject
    {
        [SerializeField] int _layer;
        [SerializeField] Vector3 _position;
        [SerializeField] Quaternion _rotation;
        [SerializeField] Vector3 _scale;
        [SerializeField] AssetReference _prefab;

        public int Layer => _layer;
        public Vector3 Position => _position;
        public Quaternion Rotation => _rotation;
        public Vector3 Scale => _scale;
        public AssetReference Prefab => _prefab;

#if UNITY_EDITOR
        public MapObject(int layer, GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(prefab, out string guid, out long _);

            _layer = layer;
            _prefab = new AssetReference(guid);
            _position = position;
            _rotation = rotation;
            _scale = scale;
        }
#endif
    }
}