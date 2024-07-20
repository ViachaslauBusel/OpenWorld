#if UNITY_EDITOR
using OpenWorld.DATA;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorld
{
    /// <summary>
    /// Скрипт для загрузки данных тайла в режиме редактора
    /// </summary>
    public class EditorTile : BaseTile
    {
        private MapTile _tileAsset;
        private Dictionary<MapEntity, GameObject> _mapObjectToSceneObject = new Dictionary<MapEntity, GameObject>();

        public MapTile Data => _tileAsset;

        protected override void OnAssetLoaded(MapTile tileAsset)
        {
            _tileAsset = tileAsset;
        }

        protected override void OnEntityInstantiated(GameObject obj, MapEntity mapEntity)
        {
            _mapObjectToSceneObject.Add(mapEntity, obj);
        }

        public GameObject GetSceneObjectByMapObject(MapEntity mapObject)
        {
            if (_mapObjectToSceneObject.ContainsKey(mapObject))
            {
                return _mapObjectToSceneObject[mapObject];
            }
            return null;
        }

        internal override void Destroy()
        {
            DestroyImmediate(gameObject);
        }
    }
}
#endif