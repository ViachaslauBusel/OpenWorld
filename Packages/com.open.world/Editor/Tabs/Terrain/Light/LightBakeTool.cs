#if UNITY_EDITOR
using OpenWorld;
using OpenWorld.DATA;
using OpenWorldEditor.SceneWindow;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace OpenWorldEditor.Tools.Terrain
{
    public static class LightBakeTool 
    {

        public static void Bake()
        {

            //Map map = TabSetting.Map;

            //int tiles = map.Size * map.TilesPerBlock;
            //float maxProgress = tiles * tiles;


            //MapLoader mapLoader = WorldLoader.MapEditor;

            //for (int y = 0; y < mapLoader.Tiles.GetLength(1); y++)
            //{
            //    for (int x = 0; x < mapLoader.Tiles.GetLength(0); x++)
            //    {
            //        EditorTile tileEditor = mapLoader._tiles[x, y] as EditorTile;
            //        SetParameters(tileEditor.gameObjects);
            //    }
            //}
            //Lightmapping.Bake();

            //for (int y = 1; y < mapLoader.Tiles.GetLength(1) - 1; y++)
            //{
            //    for (int x = 1; x < mapLoader.Tiles.GetLength(0) - 1; x++)
            //    {
            //        EditorTile tileEditor = mapLoader._tiles[x, y] as EditorTile;
            //        if (tileEditor.Terrain.lightmapIndex >= 0)
            //        {
            //            LightmapData lightmap = LightmapSettings.lightmaps[tileEditor.Terrain.lightmapIndex];

            //            string path = tileEditor.Location.PathToLight;
            //            string directory = Path.GetDirectoryName(path);
            //            if (!Directory.Exists(directory)) { Directory.CreateDirectory(directory); }

            //            AssetDatabase.DeleteAsset(path);

            //            Lightmap lightmapData = ScriptableObject.CreateInstance<Lightmap>();
            //            AssetDatabase.CreateAsset(lightmapData, path);

            //            if (lightmap.lightmapColor != null)
            //            {
            //                lightmapData.lightmapColor = lightmap.lightmapColor;
            //                AssetDatabase.RemoveObjectFromAsset(lightmap.lightmapColor);
            //                AssetDatabase.AddObjectToAsset(lightmapData.lightmapColor, path);
            //            }
            //            if (lightmap.lightmapDir != null)
            //            {
            //                lightmapData.lightmapDir = lightmap.lightmapDir;
            //                AssetDatabase.RemoveObjectFromAsset(lightmap.lightmapDir);
            //                AssetDatabase.AddObjectToAsset(lightmapData.lightmapDir, path);
            //            }
            //            if (lightmap.shadowMask != null)
            //            {
            //                lightmapData.shadowMask = lightmap.shadowMask;
            //                AssetDatabase.RemoveObjectFromAsset(lightmap.shadowMask);
            //                AssetDatabase.AddObjectToAsset(lightmapData.shadowMask, path);
            //            }
            //           // else Debug.LogError("lightmap.lightmapDir == null");

 
            //       /*     if (LightmapSettings.lightProbes != null)
            //            {
            //                lightmapData.lightProbes = LightmapSettings.lightProbes;
            //                AssetDatabase.RemoveObjectFromAsset(LightmapSettings.lightProbes);
            //                AssetDatabase.AddObjectToAsset(lightmapData.lightProbes, path);
            //            }*/

            //            AssetImporter.GetAtPath(path).assetBundleName = map.Name + "/map(Light)";
            //        }
            //    }
            //}


            //EditorUtility.ClearProgressBar();
            //AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();
        }

        private static bool BakeLght(MapLoader mapLoader, TileLocation location)
        {
            //for (int y = 0; y < mapLoader.Tiles.GetLength(1); y++)
            //{
            //    for (int x = 0; x < mapLoader.Tiles.GetLength(0); x++)
            //    {
            //        EditorTile tile = mapLoader._tiles[x, y] as EditorTile;

            //        if (tile.Terrain == null) continue;
            //        //Центральный тайл
            //        if (tile.Location == location)
            //        {
            //         //   Debug.Log($"location:{location} layers:{tile.Terrain.terrainData.terrainLayers.Length}");
            //            //Если отсуствуют текстуры не запекаем
            //            if (tile.Terrain.terrainData.terrainLayers.Length == 0) return false;
            //            tile.Terrain.shadowCastingMode = ShadowCastingMode.On;
            //       //     SetParameters(tile.gameObjects, true);
            //        }
            //        else
            //        {
            //            tile.Terrain.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            //            tile.Terrain.bakeLightProbesForTrees = false;
            //         //   SetParameters(tile.gameObjects, false);
            //        }
            //    }
            //}
            return true;
        }
        private static void SetParameters(List<GameObject> gameObjects)
        {
            foreach (GameObject obj in gameObjects)
            {
                MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer meshRenderer in renderers)
                {
               //     meshRenderer.gameObject.isStatic = true;
                    meshRenderer.shadowCastingMode = ShadowCastingMode.On;// : ShadowCastingMode.ShadowsOnly;
                  //  meshRenderer.receiveShadows = receiveShadows;
                }
            }
        }
    }
}
#endif