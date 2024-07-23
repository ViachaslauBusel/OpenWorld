using OpenWorld;
using OpenWorld.DATA;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.MapObjectTab.Display
{
    public class DisplayObject
    {
        /// <summary>Data with information about the tile on which the object is located</summary>
        public MapTile TileData { get; private set; }
        /// <summary>Data with information about the object</summary>
        public MapEntity EntityData { get; private set; }
        /// <summary>Object's prefab</summary>
        public GameObject Prefab { get; private set; }
        /// <summary>Loaded object on the scene</summary>
        public GameObject ObjectOnScene { get; private set; }
        public string LayerName { get; }

        private DisplayObject(MapTile tileData, MapEntity objectData, GameObject prefab, GameObject sceneObject, string layerName)
        {
            TileData = tileData;
            EntityData = objectData;
            Prefab = prefab;
            ObjectOnScene = sceneObject;
            LayerName = layerName;
        }


        /// <summary>Detach the object from the map</summary>
        public void Detach()
        {
            // Delete the object from the scene
            int entityIdentifierID = ObjectOnScene.GetComponent<MapEntityIdentifier>()?.ID ?? 0;
            GameObject.DestroyImmediate(ObjectOnScene);

            //Create object with link to prefab
            GameObject newObject = PrefabUtility.InstantiatePrefab(EntityData.Prefab.editorAsset) as GameObject;

            // Set the object's position, rotation, and scale
            newObject.transform.position = EntityData.Position;
            newObject.transform.rotation = EntityData.Rotation;
            newObject.transform.localScale = EntityData.Scale;

            if(entityIdentifierID != 0)
            {
                var mapEntityIdentifier = newObject.AddComponent<MapEntityIdentifier>();
                mapEntityIdentifier?.Initialize(entityIdentifierID);
                mapEntityIdentifier?.NotifyOnDestroyIdentifier();
            }

            // Remove the object from the prefab
            TileData.RemoveEntity(EntityData);
            EditorUtility.SetDirty(TileData);
        }

        public void Delete()
        {
            // Delete the object from the scene
            GameObject.DestroyImmediate(ObjectOnScene);

            // Remove the object from the prefab
            TileData.RemoveEntity(EntityData);
            EditorUtility.SetDirty(TileData);
        }

        internal static DisplayObject Create(MapTile tileData, MapEntity objectData, GameObject prefab, GameObject sceneObject, string layerName)
        {
            if (tileData == null) { Debug.LogError("DisplayObject: Tile data not found"); return null; }
            if (objectData == null) { Debug.LogError("DisplayObject: Object data on tile not found"); return null; }
            if (prefab == null) { Debug.LogError("DisplayObject: Prefab of object on tile not found."); }
            if (sceneObject == null) { Debug.LogError("DisplayObject: Object on scene not found"); }

            return new DisplayObject(tileData, objectData, prefab, sceneObject, layerName);
        }
    }
}