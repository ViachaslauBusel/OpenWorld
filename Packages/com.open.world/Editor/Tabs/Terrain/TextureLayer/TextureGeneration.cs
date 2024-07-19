#if UNITY_EDITOR
using OpenWorld.DATA;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tools.Terrain
{
    public class TextureGeneration
    {
        public static void Generation(float height, TerrainLayer terrainLayer, GameMap editMap)
        {
            GameMap map = editMap;

            float maxProgress = map.MapSizeKilometers * map.MapSizeKilometers;
            float totalProgress = 0.0f;

           

            for (int yKM = 0; yKM < map.MapSizeKilometers; yKM++)
            {
                for (int xKM = 0; xKM < map.MapSizeKilometers; xKM++)
                {
                    totalProgress = (yKM * map.MapSizeKilometers) + xKM;
                    EditorUtility.DisplayProgressBar("OpenWorld", "assign initial texture", totalProgress / maxProgress);
                    for (int y = 0; y < map.TilesPerKilometer; y++)
                    {
                        for (int x = 0; x < map.TilesPerKilometer; x++)
                        {
                            string path = map.GetPath(xKM, yKM, x, y);
                            MapTile mapElement = AssetDatabase.LoadAssetAtPath<MapTile>(path);

                            if (mapElement.TerrainData.terrainLayers == null || mapElement.TerrainData.terrainLayers.Length == 0)
                            {
                                if (!ConstainsHeight(mapElement.TerrainData, height)) continue;
                                TerrainLayer[] terrainLayers = new TerrainLayer[1];
                                for (int k = 0; k < 1; k++)
                                {
                                    terrainLayers[k] = terrainLayer;
                                }

                                mapElement.TerrainData.terrainLayers = terrainLayers;
                                mapElement.TerrainData.SetAlphamaps(0, 0, GetMapTexture(mapElement.TerrainData.alphamapHeight, mapElement.TerrainData.alphamapWidth, terrainLayers.Length));
                            }
                        }
                    }


                }
            }
            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static bool ConstainsHeight(TerrainData terrainData, float height)
        {
            height /= terrainData.size.y;
            float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
            for (int y = 0; y < heights.GetLength(1); y++)
            {
                for (int x = 0; x < heights.GetLength(0); x++)
                {
                    if (heights[x, y] < height) return true;
                }
            }
            return false;
        }

        private static float[,,] GetMapTexture(int x, int y, int z)
        {

            float[,,] map = new float[x, y, z];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    map[i, j, 0] = 1.0f;
                }
            }
            return map;
        }
    }
}
#endif