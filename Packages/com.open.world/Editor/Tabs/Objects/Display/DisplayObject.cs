using OpenWorld.DATA;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.MapObjectTab.Display
{
    public class DisplayObject
    {
        /// <summary>Data with information about the tile on which the object is located</summary>
        public Tile TileData { get; private set; }
        /// <summary>Data with information about the object</summary>
        public MapObject ObjectData { get; private set; }
        /// <summary>Object's prefab</summary>
        public GameObject Prefab { get; private set; }
        /// <summary>Loaded object on the scene</summary>
        public GameObject ObjectOnScene { get; private set; }
        public string LayerName { get; }

        private DisplayObject(Tile tileData, MapObject objectData, GameObject prefab, GameObject sceneObject, string layerName)
        {
            TileData = tileData;
            ObjectData = objectData;
            Prefab = prefab;
            ObjectOnScene = sceneObject;
            LayerName = layerName;
        }


        /// <summary>Detach the object from the map</summary>
        public void Detach()
        {
            // Detach the object from the loaded map
            ObjectOnScene.transform.SetParent(null);

            // Remove the object from the prefab
            TileData.RemoveObject(ObjectData);
            EditorUtility.SetDirty(TileData);
        }

        public void Delete()
        {
            // Delete the object from the scene
            GameObject.DestroyImmediate(ObjectOnScene);

            // Remove the object from the prefab
            TileData.RemoveObject(ObjectData);
            EditorUtility.SetDirty(TileData);
        }

        internal static DisplayObject Create(Tile tileData, MapObject objectData, GameObject prefab, GameObject sceneObject, string layerName)
        {
            if (tileData == null) { Debug.LogError("DisplayObject: Tile data not found"); return null; }
            if (objectData == null) { Debug.LogError("DisplayObject: Object data on tile not found"); return null; }
            if (prefab == null) { Debug.LogError("DisplayObject: Prefab of object on tile not found."); }
            if (sceneObject == null) { Debug.LogError("DisplayObject: Object on scene not found"); }

            return new DisplayObject(tileData, objectData, prefab, sceneObject, layerName);
        }
    }
}