using OpenWorldEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OpenWorld.DATA
{
    [CreateAssetMenu(fileName = "Map", menuName = "Map Data", order = 50), System.Serializable]
    public class Map : ScriptableObject
    {
        public const float SIZE_KMBLOCK = 1000.0f;
        private static readonly int[] RESOLUTIONS = { 16, 32, 64, 128, 256, 512, 1024 };


        [SerializeField] string m_mapName = null;
        /// <summary>Размер карты в километрах</summary>
        [SerializeField] int m_mapSize;
        /// <summary>Количество тайлов на 1 километр</summary>
        [SerializeField] int m_tilesPerBlock;
        /// <summary>Размер тайла в мировых единицах</summary>
        [SerializeField] float m_tileSize;
        [SerializeField] Vector2 m_startWorld;
        [SerializeField] Vector2 m_endWorld;
        /// <summary>Максимальная высота мира</summary>
        [SerializeField] float m_maxHeight;
        [SerializeField] int m_heightmapResolutionIndex = 2;
        [SerializeField] int m_alphamapResolutionIndex = 3;
        [SerializeField] int m_baseMapResolutionIndex = 3;
        [SerializeField] float m_waterLevel = 0;

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------PUBLIC---------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public string Name => m_mapName;
        public new string name => Name;
        /// <summary>Размер карты в километрах</summary>
        public int Size => m_mapSize;
        /// <summary>Количество тайлов на 1 километр</summary>
        public int TilesPerBlock => m_tilesPerBlock;
        /// <summary>Размер тайла(террейна) в мировых единицах</summary>
        public float TileSize => m_tileSize;
        /// <summary>Максимальная высота мира</summary>
        public float MaxHeight => m_maxHeight;
        /// <summary>Разрешеник карты высот на террейне(тайл)</summary>
        public int HeightmapResolution => RESOLUTIONS[m_heightmapResolutionIndex] + 1;
        /// <summary>Разрешеник текстуры на террейне(тайл)</summary>
        public int AlphamapResolution => RESOLUTIONS[m_alphamapResolutionIndex];
        public int BaseMapResolution => RESOLUTIONS[m_baseMapResolutionIndex];
        public float WaterLevel { get => m_waterLevel; set { m_waterLevel = value; } }

        public Vector3 StartWorld => new Vector3(m_startWorld.x, 0, m_startWorld.y);
        public Vector3 EndWorld => new Vector3(m_endWorld.x, 0, m_endWorld.y);

        public string GetPath(int xKM, int yKM, int xTR, int yTR) => "Assets/MapData/" + m_mapName + "/KMBlock_" + xKM + '_' + yKM + "/Tile_" + xTR + '_' + yTR + ".asset";

        public string GetPathToLight(int xKM, int yKM, int xTR, int yTR) => "Assets/MapData/" + m_mapName + "(Light)/KMBlock_" + xKM + '_' + yKM + "/TRBlock_" + xTR + '_' + yTR + ".asset";

        /// <summary>
        /// Transform position from world space to map space
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        internal Vector3 WorldToMapPoint(Vector3 position)
        {
            position -= StartWorld;
            position.x = Mathf.Clamp(position.x, 0, Size * SIZE_KMBLOCK);
            position.z = Mathf.Clamp(position.z, 0, Size * SIZE_KMBLOCK);
            return position;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR

        private string m_inputName = "";
        private int m_error = 0;

        /// <summary>
        /// Высота мира по умолчанию
        /// </summary>
        private float m_inputHeight;
        public void DrawGUI()
        {
            ShowError();

            if (string.IsNullOrEmpty(m_mapName))//Карта не создана
            {
                EditorGUILayout.LabelField("Карта не создана");
                m_inputName = EditorGUILayout.TextField("Имя:", m_inputName);
                m_mapSize = EditorGUILayout.IntField("Размер карты в километрах:", m_mapSize);
                m_tilesPerBlock = EditorGUILayout.IntSlider("Тайлов на километр:", m_tilesPerBlock, 1, 100);
                //  m_startWorld = EditorGUILayout.Vector2Field("Начало мира:", m_startWorld);
                m_maxHeight = EditorGUILayout.FloatField("Максимальная высота мира:", m_maxHeight);
                m_inputHeight = EditorGUILayout.FloatField("Заданная высота мира:", m_inputHeight);

                m_heightmapResolutionIndex = EditorGUILayout.Popup("Heightmap Resolution", m_heightmapResolutionIndex, RESOLUTIONS.Select((i) => $"{i + 1} x {i + 1}").ToArray());
                m_alphamapResolutionIndex = EditorGUILayout.Popup("Control Texture Resolution", m_alphamapResolutionIndex, RESOLUTIONS.Select((i) => $"{i} x {i}").ToArray());
                m_baseMapResolutionIndex = EditorGUILayout.Popup("Base Texture Resolution", m_baseMapResolutionIndex, RESOLUTIONS.Select((i) => $"{i} x {i}").ToArray());
                //   selectAssetBundle = EditorGUILayout.Popup("Asset Bundle", selectAssetBundle, AssetDatabase.GetAllAssetBundleNames());


                if (GUILayout.Button("Создать"))
                {


                    if (m_inputName.Length < 3) { m_error = 1; return; }
                    if (m_mapSize <= 0) { m_error = 2; return; }
                    if (m_maxHeight <= 10.0f) { m_error = 5; return; }
                    if (m_inputHeight < 0.0f || m_inputHeight > m_maxHeight) { m_error = 6; return; }
                    if (Directory.Exists("Assets/MapData/" + m_inputName)) { m_error = 3; return; }
                    if (AssetDatabase.GetAllAssetBundleNames().Any((str) => str.Equals(m_mapName))) { m_error = 7; return; }


                    m_mapName = m_inputName;
                    m_tileSize = SIZE_KMBLOCK / m_tilesPerBlock;
                    m_startWorld = new Vector2(-(m_mapSize * SIZE_KMBLOCK / 2.0f), -(m_mapSize * SIZE_KMBLOCK / 2.0f));
                    m_endWorld = new Vector2(m_mapSize * SIZE_KMBLOCK / 2.0f, m_mapSize * SIZE_KMBLOCK / 2.0f);

                    try
                    {
                        MapGeneration.GenerationWorld(this, m_inputHeight);
                    }
                    catch (Exception e) { Debug.LogError(e); m_mapName = null; m_error = 4; return; }
                    // else {   return; }
                    EditorUtility.SetDirty(this);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), m_mapName);
                    m_error = 0;
                }


            }
            else
            {

                EditorGUILayout.LabelField($"Имя : {m_mapName}");
                EditorGUILayout.LabelField($"Размер карты: {m_mapSize}X{m_mapSize}км");
                EditorGUILayout.LabelField($"Размер тайла: {m_tileSize}X{m_tileSize}м");
                EditorGUILayout.LabelField("Начало мира: " + m_startWorld.ToString());
                EditorGUILayout.LabelField("Максимальная высота мира: " + m_maxHeight.ToString());
                EditorGUILayout.LabelField("Heightmap Resolution: " + HeightmapResolution.ToString());
                m_alphamapResolutionIndex = EditorGUILayout.Popup("Control Texture Resolution", m_alphamapResolutionIndex, RESOLUTIONS.Select((i) => $"{i} x {i}").ToArray());
                m_baseMapResolutionIndex = EditorGUILayout.Popup("Base Texture Resolution", m_baseMapResolutionIndex, RESOLUTIONS.Select((i) => $"{i} x {i}").ToArray());

                if (GUILayout.Button("Применить"))
                {
                    if (EditorUtility.DisplayDialog("Применение настроек", "При выставлении Control Texture Resolution более низкого разрешения, качество будет безвозвратно утеряно", "продолжить", "отмена"))
                    {


                        foreach (Tile tile in this)
                        {
                            if (tile.terrainData.alphamapResolution != AlphamapResolution)
                            { tile.terrainData.alphamapResolution = AlphamapResolution; }
                            if (tile.terrainData.baseMapResolution != BaseMapResolution)
                            { tile.terrainData.baseMapResolution = BaseMapResolution; }
                            //  if (tile.terrainData.alphamapLayers != 0) Debug.Log($"tile.terrainData.alphamapLayers:{tile.terrainData.alphamapLayers}");
                            EditorUtility.SetDirty(tile);
                        }
                    }

                    EditorUtility.SetDirty(this);
                    AssetDatabase.SaveAssets();
                }

                if (GUILayout.Button("Удалить"))
                {
                    //m_mapName = "TestWorld"; m_mapSize = 2; m_tilesPerBlock = 20; m_tileSize = 50.0f; m_maxHeight = 900.0f; m_heightmapResolution = 33;
                    //EditorUtility.SetDirty(this);
                    //AssetDatabase.SaveAssets();
                    //return;
                    if (EditorUtility.DisplayDialog("Удаление карты", "Удалить карту: " + m_mapName + "?", "Да", "Нет"))
                    {
                        FileUtil.DeleteFileOrDirectory($"Assets/MapData/{m_mapName}/");

                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
                        AssetDatabase.Refresh();
                    }
                }

            }
        }
        private void ShowError()
        {
            switch (m_error)
            {
                case 0:
                    break;
                case 1:
                    EditorGUILayout.HelpBox("Неверное имя", MessageType.Error);
                    break;
                case 2:
                    EditorGUILayout.HelpBox("Неверный размер", MessageType.Error);
                    break;
                case 3:
                    EditorGUILayout.HelpBox("Имя уже используется", MessageType.Error);
                    break;
                case 4:
                    EditorGUILayout.HelpBox("Ошибка при создании карты", MessageType.Error);
                    break;
                case 5:
                    EditorGUILayout.HelpBox("Незадана высота", MessageType.Error);
                    break;
                case 6:
                    EditorGUILayout.HelpBox("Неправильная задаваемая высота", MessageType.Error);
                    break;
                case 7:
                    EditorGUILayout.HelpBox("Не удалось задать Asset Bundle", MessageType.Error);
                    break;
                default:
                    EditorGUILayout.HelpBox("Неизвестная ошибка", MessageType.Error);
                    break;
            }
        }

        public IEnumerator GetEnumerator()
        {
            float maxProgress = Size * Size + TilesPerBlock * TilesPerBlock;
            for (int yKM = 0; yKM < Size; yKM++)
            {
                for (int xKM = 0; xKM < Size; xKM++)
                {


                    for (int y = 0; y < TilesPerBlock; y++)
                    {
                        for (int x = 0; x < TilesPerBlock; x++)
                        {
                            EditorUtility.DisplayProgressBar("OpenWorld", "processing", ((yKM * Size + xKM) + (y * TilesPerBlock + x)) / maxProgress);
                            string path = GetPath(xKM, yKM, x, y);

                            Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(path);


                            yield return tile;
                        }
                    }


                }
            }
            EditorUtility.ClearProgressBar();
        }
#endif

    }
}