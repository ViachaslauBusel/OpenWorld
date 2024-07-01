#if UNITY_EDITOR
using OpenWorld.DATA;
using OpenWorld.Tabs.Terrain.Export;
using OpenWorldEditor.Tabs.Monsters;
using OpenWorldEditor.Tabs.Res;
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
        
                                foreach(MapObject obj in mapElement.objects)
                                {
                                    if(obj.Prefab.Refresh())
                                    {
                                        Debug.Log("Refresh prefab");
                                        EditorUtility.SetDirty(mapElement);
                                    }
                                }
                            }
                        }
                    }
                }
                EditorUtility.ClearProgressBar();

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                BuildPipeline.BuildAssetBundles("Export/AssetBundles/Win", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
                BuildPipeline.BuildAssetBundles("Export/AssetBundles/Android", BuildAssetBundleOptions.None, BuildTarget.Android);
                BuildPipeline.BuildAssetBundles("Export/AssetBundles/IOS", BuildAssetBundleOptions.None, BuildTarget.iOS);
            }
            if (GUILayout.Button("Export Spawn Point"))
            {
                float maxProgress = map.MapSizeKilometers * map.MapSizeKilometers;
                float totalProgress = 0.0f;

                string folder;
                List<Vector3> spawnPoint = new List<Vector3>();
                for (int x = 0; x < map.MapSizeKilometers; x++)
                {
                    EditorUtility.DisplayProgressBar("OpenWorld", "Export Spawn Point", totalProgress / maxProgress);
                    for (int y = 0; y < map.MapSizeKilometers; y++)
                    {

                        totalProgress = (x * map.MapSizeKilometers) + y;

                        folder = map.MapName + "/KMObject_" + x + '_' + y;
                        spawnPoint.AddRange(GetPoint(folder));
                    }
                }
                EditorUtility.ClearProgressBar();

                Export(spawnPoint);

                Resources.UnloadUnusedAssets();
            }



            //if (GUILayout.Button("Export monsters.dat"))
            //{
            //    if (TabSetting.WorldMonsterList == null || TabSetting.MonstersList == null)
            //    {
            //        EditorUtility.DisplayDialog("Export monsters.dat", "Ошибка экспорта", "ok");
            //        return;
            //    }
            //    MonsterExport.Export(TabSetting.WorldMonsterList, TabSetting.MonstersList);
            //    EditorUtility.DisplayDialog("Export monsters.dat", "Экспорт выполнен", "ok");
            //}
            //if (GUILayout.Button("Export npcs.dat"))
            //{
            //    if (TabSetting.WorldNPCs == null || TabSetting.NPCs == null)
            //    {
            //        EditorUtility.DisplayDialog("Export npcs.dat", "Ошибка экспорта", "ok");
            //        return;
            //    }
            //    NPCsExport.Export(TabSetting.WorldNPCs, TabSetting.NPCs);
            //    EditorUtility.DisplayDialog("Export npcs.dat", "Экспорт выполнен", "ok");
            //}
            //if (GUILayout.Button("Export resources.dat"))
            //{
            //    if (TabSetting.WorldResources == null || TabSetting.Resources == null)
            //    {
            //        EditorUtility.DisplayDialog("Export resources.dat", "Ошибка экспорта", "ok");
            //        return;
            //    }
            //    ResourceExport.Export(TabSetting.WorldResources, TabSetting.Resources);
            //    EditorUtility.DisplayDialog("Export resources.dat", "Экспорт выполнен", "ok");
            //}
           
            /*    if (GUILayout.Button("Export npc.dat"))
                {
                    if (WindowSetting.WorldNPCList == null || WindowSetting.NPCList == null)
                    {
                        EditorUtility.DisplayDialog("Export npc.dat", "Ошибка экспорта", "ok");
                        return;
                    }
                    NPCExport.Export(WindowSetting.WorldNPCList, WindowSetting.NPCList);
                    EditorUtility.DisplayDialog("Export npc.dat", "Экспорт выполнен", "ok");
                }
                if (GUILayout.Button("Export resources.dat"))
                {
                    if (WindowSetting.WorldResourcesList == null || WindowSetting.ResourcesList == null)
                    {
                        EditorUtility.DisplayDialog("Export resources.dat", "Ошибка экспорта", "ok");
                        return;
                    }
                    ResourcesExport.Export(WindowSetting.WorldResourcesList, WindowSetting.ResourcesList);
                    EditorUtility.DisplayDialog("Export monsters.dat", "Экспорт выполнен", "ok");
                }*/
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

            /*   if (GUILayout.Button("Export machines.dat"))
               {
                   if (WindowSetting.MachineList == null)
                   {
                       EditorUtility.DisplayDialog("Export machines.dat", "Ошибка экспорта", "ok");
                       return;
                   }
                   MachinesExport.Export(WindowSetting.MachineList);
                   EditorUtility.DisplayDialog("Export machines.dat", "Экспорт выполнен", "ok");
               }*/

            /*  if (GUILayout.Button("fix TerrainData"))
              {
                  float maxProgress = map.mapSize * map.mapSize;
                  float totalProgress = 0.0f;

                  string folder;
                  for (int x = 0; x < map.mapSize; x++)
                  {
                      EditorUtility.DisplayProgressBar("OpenWorld", "fix terrain", totalProgress / maxProgress);
                      for (int y = 0; y < map.mapSize; y++)
                      {

                          totalProgress = (x * map.mapSize) + y;

                          folder = map.mapName + "/KMBlock_" + x + '_' + y;
                          Fix(folder);
                      }
                  }
                  EditorUtility.ClearProgressBar();
              }*/

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
                        mapElement.terrainData.wavingGrassSpeed = 0.2f;
                        mapElement.terrainData.wavingGrassAmount = 0.31f;
                        mapElement.terrainData.wavingGrassStrength = 0.3f;
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

        private static void Remove(string folder)
        {
            for (int x = 0; x < map.TilesPerKilometer; x++)
            {
                for (int y = 0; y < map.TilesPerKilometer; y++)
                {
                    string path = folder + "/TRBlock_" + x + '_' + y;
                    Tile mapElement = Resources.Load<Tile>(path);

                    if (mapElement != null)
                    {
                        TreePrototype[] tree =  mapElement.terrainData.treePrototypes;
                        List<TreePrototype> treesSave = new List<TreePrototype>();
                        foreach(TreePrototype treePrototype in tree)
                        {
                            if (treePrototype.prefab != null)
                            {
                                treesSave.Add(treePrototype);
                            }
                        }
                        TreePrototype[] prototype = treesSave.ToArray();
                        mapElement.terrainData.treePrototypes = prototype;

                        TreeInstance[] treeInstance = mapElement.terrainData.treeInstances;
                        List<TreeInstance> listTrees = new List<TreeInstance>();
                        foreach(TreeInstance instance in treeInstance)
                        {
                            if(instance.prototypeIndex > 0 || instance.prototypeIndex <  prototype.Length)
                            {
                                listTrees.Add(instance);
                            }
                        }
                        mapElement.terrainData.treeInstances = listTrees.ToArray();


                       DetailPrototype[] details = mapElement.terrainData.detailPrototypes;
                        List<DetailPrototype> detailsSave = new List<DetailPrototype>();
                        foreach (DetailPrototype detailPrototype in details)
                        {
                            if (detailPrototype.prototype != null)
                            {
                                detailsSave.Add(detailPrototype);
                            }
                        }
                        DetailPrototype[] detailPrototypes = detailsSave.ToArray();
                        mapElement.terrainData.detailPrototypes = detailPrototypes;

                     //   mapElement.terrainData.GetDetailLayer


                       mapElement.terrainData.RefreshPrototypes();
                        AssetDatabase.Refresh();
                        EditorUtility.SetDirty(mapElement);
                        AssetDatabase.SaveAssets();
                        Resources.UnloadAsset(mapElement);
                    }


                }
            }
        }

        private static void Export(List<Vector3> spawnPoint)
        {
            using (BinaryWriter stream_out = new BinaryWriter(File.Open(@"Export/spawnPoint.dat", FileMode.Create)))
            {
                foreach (Vector3 point in spawnPoint)
                {
                    stream_out.Write(point.x);
                    stream_out.Write(point.y);
                    stream_out.Write(point.z);
                }
            }
        }
        private static List<Vector3> GetPoint(string folder)
        {
            return null;
            //    List<Vector3> spawnPoint = new List<Vector3>();
            //    for (int x = 0; x < map.terrainsCount; x++)
            //    {
            //        for (int y = 0; y < map.terrainsCount; y++)
            //        {
            //          string path = folder + "/TRObject_" + x + '_' + y;
            //            TileObjects objectElement = Resources.Load<TileObjects>(path);

            //            if (objectElement != null && objectElement.mapObjects != null)
            //            {
            //                for (int i = 0; i < objectElement.mapObjects.Count; i++)
            //                {
            //                    MapObject mapObject = objectElement.mapObjects[i];
            //                    if (mapObject == null || mapObject.prefabGUID == null) continue;
            //                 /*   SpawnPoint _point = mapObject.prefab.GetComponent<SpawnPoint>();
            //                    if (_point != null)
            //                    {

            //                        mapObject.prefab.transform.position = mapObject.postion;
            //                        mapObject.prefab.transform.rotation = mapObject.orientation;
            //                        mapObject.prefab.transform.localScale = mapObject.scale;
            //                        Vector3 vector = _point.GetPoint();
            //                        spawnPoint.Add(vector);
            //                        Debug.Log("spawn : " + vector);

            //                    }*/
            //                }
            //            }


            //        }
            //    }
            //            return spawnPoint;
            //}
        }
    }
}
#endif