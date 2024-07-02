#if UNITY_EDITOR
using OpenWorld.DATA;
using OpenWorldEditor;
using OpenWorldEditor.SceneWindow;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OpenWorld.Tools.Objects
{
    class AttachTool
    {
        public static void Draw()
        {
            GUILayout.Space(15.0f);
            if (GUILayout.Button("Attach select Object"))
            {
                AttachObjectOnMap(Selection.gameObjects);
            }
            GUILayout.Space(10.0f);
            if (GUILayout.Button("Attach all Objects"))
            {


                List<GameObject> rootObjects = new List<GameObject>();
                Scene scene = SceneManager.GetActiveScene();
                scene.GetRootGameObjects(rootObjects);

                AttachObjectOnMap(rootObjects.ToArray());
            }
        }

        private static void AttachObjectOnMap(GameObject[] gameObjects)
        {
            List<AttachObject> attachObjects = new List<AttachObject>();
            foreach (GameObject obj in gameObjects)
            {
                if (obj == null || obj.transform.parent != null) continue;//Если обьект является Child

                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource<GameObject>(obj);
                if (prefab == null) continue;//Если у обьекта нет префаба

                string pathToPrefab = AssetDatabase.GetAssetPath(prefab);
                if (string.IsNullOrEmpty(pathToPrefab)) continue;//Если у обьекта нет префаба

                string guid = AssetDatabase.AssetPathToGUID(pathToPrefab);

                // Check if the prefab is attached to the correct Addressable group
                var settings = AddressableAssetSettingsDefaultObject.Settings;
                var entry = settings.FindAssetEntry(guid);
                if (entry == null)
                {
                    Debug.LogError($"The added object is not attached to the Addressable group");
                    continue;
                }

                ////Если префаб не прекреплен к нужному бандлу
                //if (!AssetImporter.GetAtPath(pathToPrefab).assetBundleName.StartsWith(TabSetting.Map.MapName.ToLower())) { Debug.LogError("Добавляемый объект не прикреплён к бандлу или находиться в неправильном бандле"); continue; }

              
                if (string.IsNullOrEmpty(guid)) { Debug.LogError("Не удалось найти guid"); continue; }

                attachObjects.Add(new AttachObject(obj, prefab));//Добавить обьект в массив на добовление на карту  
            }

            if (attachObjects.Count == 0) return;//Если добовляемых обьектов нет

            string names = "";
            for (int i = 0; i < attachObjects.Count; i++) { if (i != 0) names += ", "; names += attachObjects[i].Object.name; }


            if (EditorUtility.DisplayDialog("Attach Object", "Добавить " + names + " на карту?", "Да", "Нет"))
            {
                foreach (AttachObject attachObject in attachObjects)
                {
                    int xKMBlock = (int)(attachObject.Object.transform.position.x / 1000.0f);
                    int yKMBlock = (int)(attachObject.Object.transform.position.z / 1000.0f);
                    int xTRBlock = (int)((attachObject.Object.transform.position.x % 1000.0f) / TabSetting.Map.TileSize);
                    int yTRBlock = (int)((attachObject.Object.transform.position.z % 1000.0f) / TabSetting.Map.TileSize);


                    Tile mapElement = AssetDatabase.LoadAssetAtPath<Tile>(TabSetting.Map.GetPath(xKMBlock, yKMBlock, xTRBlock, yTRBlock));

                    if (mapElement == null)
                    {
                        Debug.LogError("Не удалось загрузить тайл");
                        return;
                    }
                    MapObject mapObject = new MapObject(
                        attachObject.Prefab,
                        attachObject.Object.transform.position,
                        attachObject.Object.transform.rotation,
                        attachObject.Object.transform.localScale
                        );

                   
                    mapElement.AddObject(mapObject);
                    EditorUtility.SetDirty(mapElement);
                   

                   // MapEditLoader.UpdateTileObject(WorldLoader.LoadMap(), xKMBlock, yKMBlock, xTRBlock, yTRBlock);
                    GameObject.DestroyImmediate(attachObject.Object);
                }
                WorldLoader.Reload();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
#endif