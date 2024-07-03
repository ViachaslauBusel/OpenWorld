#if UNITY_EDITOR
using OpenWorld.DATA;
using OpenWorld.Tabs.Terrain.Export;
using OpenWorldEditor.Tools.Terrain;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace OpenWorldEditor
{
    public class TabExport
    {
        private static Map map;
        public static void Draw()
        {
            if (GUILayout.Button("Bake Light"))
            {
                LightBakeTool.Bake();
            }
                /*    if (GUILayout.Button("FIX"))
                    {
                        Map map = TabSetting.Map;
                        float maxProgress = map.mapSize * map.mapSize + map.terrainsCount * map.terrainsCount;
                        for (int yKM = 0; yKM < map.mapSize; yKM++)
                        {
                            for (int xKM = 0; xKM < map.mapSize; xKM++)
                            {
                                for (int yTR = 0; yTR < map.terrainsCount; yTR++)
                                {
                                    for (int xTR = 0; xTR < map.terrainsCount; xTR++)
                                    {

                                        EditorUtility.DisplayProgressBar("OpenWorld", "attach objects", ((yKM * map.mapSize + xKM) + (yTR * map.terrainsCount + xTR)) / maxProgress);

                                        MapElement mapElement = AssetDatabase.LoadAssetAtPath<MapElement>(map.GetPath(xKM, yKM, xTR, yTR));

                                        mapElement.terrainData.SetDetailResolution(64, 64);
                                        EditorUtility.SetDirty(mapElement);
                                    }
                                }
                            }
                        }
                        EditorUtility.ClearProgressBar();

                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }*/

                if (GUILayout.Button("build asset bundles"))
            {
                if (!Directory.Exists("Export/AssetBundles/Win"))
                    Directory.CreateDirectory("Export/AssetBundles/Win");
               

                if (TabSetting.Map == null) return;
                Map map = TabSetting.Map;
                string mapName = map.MapName.ToLower();

                if (!Directory.Exists("Export/AssetBundles/Win/" +mapName))
                    Directory.CreateDirectory("Export/AssetBundles/Win/" + mapName);
                if (!Directory.Exists("Export/AssetBundles/Android/" + mapName))
                    Directory.CreateDirectory("Export/AssetBundles/Android/" + mapName);
                if (!Directory.Exists("Export/AssetBundles/IOS/" + mapName))
                    Directory.CreateDirectory("Export/AssetBundles/IOS/" + mapName);

                //TerrainLayer[] terrainLayers = null;

                float maxProgress = map.MapSizeKilometers * map.MapSizeKilometers + map.TilesPerKilometer * map.TilesPerKilometer;
                for (int yKM = 0; yKM < map.MapSizeKilometers; yKM++)
                {
                    for (int xKM = 0; xKM < map.MapSizeKilometers; xKM++)
                    {
                        for (int yTR = 0; yTR < map.TilesPerKilometer; yTR++)
                        {
                            for (int xTR = 0; xTR < map.TilesPerKilometer; xTR++)
                            {

                                EditorUtility.DisplayProgressBar("OpenWorld", "attach objects", ((yKM * map.MapSizeKilometers + xKM) + (yTR * map.TilesPerKilometer + xTR)) / maxProgress);

                                Tile mapElement = AssetDatabase.LoadAssetAtPath<Tile>(map.GetPath(xKM, yKM, xTR, yTR));
        
                                //foreach(MapObject obj in mapElement.Objects)
                                //{
                                //    if(obj.Prefab.Refresh())
                                //    {
                                //        Debug.Log("Refresh prefab");
                                //        EditorUtility.SetDirty(mapElement);
                                //    }
                                //}
                            }
                        }
                    }
                }
                EditorUtility.ClearProgressBar();

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

               // BuildPipeline.BuildAssetBundles("Export/AssetBundles/Win", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
                BuildPipeline.BuildAssetBundles("Asset/StreamingAssets/Android", BuildAssetBundleOptions.None, BuildTarget.Android);
               //BuildPipeline.BuildAssetBundles("Export/AssetBundles/IOS", BuildAssetBundleOptions.None, BuildTarget.iOS);
            }
            if (GUILayout.Button("Export terrain.dat"))
            {
                if (TabSetting.Map == null)
                {
                    EditorUtility.DisplayDialog("Export terrain.dat", "Ошибка экспорта", "ok");
                    return;
                }
                TerrainExport.Export(TabSetting.Map);
                EditorUtility.DisplayDialog("Export terrain.dat", "Экспорт выполнен", "ok");
            }

            if (GUILayout.Button("Export collision.dat"))
            {
                if (TabSetting.Map == null)
                {
                    EditorUtility.DisplayDialog("Export collision.dat", "Ошибка экспорта", "ok");
                    return;
                }
                CollisionExport.Export(TabSetting.Map);
                EditorUtility.DisplayDialog("Export collision.dat", "Экспорт выполнен", "ok");
            }
            if (GUILayout.Button("Remove missing Tree/Grass"))
            {
                float maxProgress = map.MapSizeKilometers * map.MapSizeKilometers;
                float totalProgress = 0.0f;

                string folder;
                for (int x = 0; x < map.MapSizeKilometers; x++)
                {
                    EditorUtility.DisplayProgressBar("OpenWorld", "fix terrain", totalProgress / maxProgress);
                    for (int y = 0; y < map.MapSizeKilometers; y++)
                    {

                        totalProgress = (x * map.MapSizeKilometers) + y;

                        folder = map.MapName + "/KMBlock_" + x + '_' + y;
                    //    Remove(folder);
                        Fix(folder);
                    }
                }
                EditorUtility.ClearProgressBar();
            }
        }

        private static void Fix(string folder)
        {
            for (int x = 0; x < map.TilesPerKilometer; x++)
            {
                for (int y = 0; y < map.TilesPerKilometer; y++)
                {
                    string path = folder + "/TRBlock_" + x + '_' + y;
                    Tile mapElement = Resources.Load<Tile>(path);

                    if (mapElement != null)
                    {
                        mapElement.TerrainData.wavingGrassSpeed = 0.2f;
                        mapElement.TerrainData.wavingGrassAmount = 0.31f;
                        mapElement.TerrainData.wavingGrassStrength = 0.3f;
                        //  mapElement.terrainData.baseMapResolution = 64;
                        //  mapElement.terrainData.SetDetailResolution(128, 32);
                        AssetDatabase.Refresh();
                        EditorUtility.SetDirty(mapElement);
                        AssetDatabase.SaveAssets();
                        Resources.UnloadAsset(mapElement);
                    }


                }
            }
        }
    }
}
#endif