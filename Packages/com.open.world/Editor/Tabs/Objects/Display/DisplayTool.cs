#if UNITY_EDITOR
using OpenWorld.DATA;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenWorld.Tools.Objects
{
    class DisplayTool
    {
        private static Vector2 scrollPos = Vector2.zero;
        /// <summary>Выделенный обьект на сцене</summary>
        private static GameObject m_selectOBJ = null;
        /// <summary>Список объектов прикреплённых к выделенному тайлу</summary>
        private static List<DisplayObject> m_displays = new List<DisplayObject>();
        /// <summary>Выделенная строка с обьектом из списка обьектов прикрепленных к тайлу</summary>
        private static DisplayObject m_selectDisplay;
        /// <summary>список выделенных тайлов</summary>
        private static List<EditorTile> m_selectTiles = new List<EditorTile>();
        public static void Draw()
        {
            if (Selection.activeObject == null) return;

            GameObject _obj = Selection.activeObject as GameObject;
            if (_obj == null) return;
            //Если был выделен другой объект на сцене
            if (_obj != m_selectOBJ)
            {
                m_displays.Clear();
                m_selectOBJ = _obj;
                m_selectTiles.Clear();
                foreach(GameObject selectOBJ in Selection.gameObjects)
                {
                    EditorTile tile = selectOBJ.GetComponentInParent<EditorTile>();
                    if (tile != null) m_selectTiles.Add(tile);
                }

                //Тайлы небыли выделены
              //  if (m_selectTiles.Count != 0) { 

                foreach (EditorTile tile in m_selectTiles)
                {
                    foreach (MapObject mapObject in tile.Data.Objects)
                    {
                        DisplayObject displayObject = DisplayObject.Create(
                        tile.Data,
                        mapObject,
                        mapObject.Prefab?.editorAsset as GameObject,
                        tile.transform.Find(mapObject.GetHashCode().ToString())?.gameObject
                        );
                        if (displayObject != null) m_displays.Add(displayObject);
                    }
                }
            }


            if (GUILayout.Button("Detach All"))
            {
                if (EditorUtility.DisplayDialog("Detach Objects", $"Вы уверены что хотите открепить {m_displays.Count} обьектов от карты?", "Да", "Нет"))
                {
                    m_displays.ForEach((d) => d.Detach());
                    m_displays.Clear();
                }
            }

            if (m_displays.Count == 0) return;



            scrollPos = GUILayout.BeginScrollView(scrollPos);
            {
                for (int i=m_displays.Count-1; i>=0; i--)
                {
                    GUILayout.Space(10.0f);
                    GUILayout.BeginHorizontal();
                    //GameObject prefab = 

                    GUILayout.Label(m_displays[i]?.Prefab.name ?? "Не удалось найти префаб");


                    GUI.enabled = m_selectDisplay != m_displays[i];
                    if (GUILayout.Button("Select"))
                    {
                        m_selectDisplay = m_displays[i];
                        Selection.activeObject = m_displays[i].ObjectOnScene;
                    }
                    GUI.enabled = true;

                    if (GUILayout.Button("Detach"))
                    {
                        
                            m_displays[i].Detach();

                            //Удалить обькут из списка отрисовки UI
                            m_displays.RemoveAt(i);
                    }

                    if (GUILayout.Button("Delet"))
                    {
                        m_displays[i].Delet();

                        //Удалить обькут из списка отрисовки UI
                        m_displays.RemoveAt(i);
                    }

                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();

        }
    }
    public class DisplayObject
    {
        /// <summary>Данные с информацией об тайле на котором находиться обьект</summary>
        public Tile TileData { get; private set; }
        /// <summary>Данные с информацией об обьекте</summary>
        public MapObject ObjectData { get; private set; }
        /// <summary>Префаб обьекта</summary>
        public GameObject Prefab { get; private set; }
        /// <summary>Загруженный объект на сцене</summary>
        public GameObject ObjectOnScene { get; private set; }

        private DisplayObject(Tile tileData, MapObject objectData, GameObject prefab, GameObject sceneObject)
        {
            TileData = tileData;
            ObjectData = objectData;
            Prefab = prefab;
            ObjectOnScene = sceneObject;
        }
           

        /// <summary>Открепить обьект от карты</summary>
        public void Detach()
        {
            //Открепить обьект от загруженной карты
           ObjectOnScene.transform.SetParent(null);

            //Удалить обьект из префаба
            TileData.RemoveObject(ObjectData);
            EditorUtility.SetDirty(TileData);
        }

        public void Delet()
        {
            //Удалить обьект со сцены
            GameObject.DestroyImmediate(ObjectOnScene);

            //Удалить обьект из префаба
            TileData.RemoveObject(ObjectData);
            EditorUtility.SetDirty(TileData);
        }

        internal static DisplayObject Create(Tile tileData, MapObject objectData, GameObject prefab, GameObject sceneObject)
        {
            if(tileData == null) { Debug.LogError($"DisplayObject: Не удалось найти данные тайла"); return null; }
            if (objectData == null) { Debug.LogError($"DisplayObject: Не удалось найти данные обьекта закрепленного на тайле"); return null; }
            if (prefab == null) { Debug.LogError($"DisplayObject: Не удалось найти префаб обьекта закрепленного на тайле.");  }
            if (sceneObject == null) { Debug.LogError($"DisplayObject: Не удалось найти обьект на сцене");  }

            return new DisplayObject(tileData, objectData, prefab, sceneObject);
        }
    }
}
#endif