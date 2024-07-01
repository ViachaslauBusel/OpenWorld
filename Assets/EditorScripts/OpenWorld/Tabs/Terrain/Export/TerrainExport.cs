#if UNITY_EDITOR
using OpenWorld.DATA;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor
{

    public class TerrainExport
    {
        public static void Export(Map map)
        {
           
            float totalProgress = 0; 

          
            using (BinaryWriter stream_out = new BinaryWriter(File.Open(@"Export/terrain.dat", FileMode.Create)))
            { 
                int maxTile = map.Size * map.TilesPerBlock;
                float maxProgress = maxTile * maxTile;
                stream_out.Write(map.Size);
                stream_out.Write(map.TilesPerBlock);//Тайлов на километр
                stream_out.Write(map.HeightmapResolution);

                for (int y = 0; y < maxTile; y++)
                {
                  
                    for (int x = 0; x < maxTile; x++)
                    {
                        totalProgress = (y * maxTile) + x;
                        EditorUtility.DisplayProgressBar("OpenWorld", "Export terrain.dat", totalProgress / maxProgress);


                        int xKM = x / map.TilesPerBlock;
                        int yKM = y / map.TilesPerBlock;
                        int xTR = x % map.TilesPerBlock;
                        int yTR = y % map.TilesPerBlock;
                       
                        Write(xKM,yKM, xTR, yTR, map, stream_out); 
                    }
                }
            }
            EditorUtility.ClearProgressBar();
        }
      
        private static void Write(int xKM, int yKM, int xTR, int yTR, Map map, BinaryWriter stream_out)
        {


            string path = map.GetPath(xKM, yKM, xTR, yTR);
                    Tile mapElement = AssetDatabase.LoadAssetAtPath<Tile>(path);
           
            if (mapElement != null)
            {
              //  Debug.Log(mapElement.terrainData.heightmapResolution);
                int size = (mapElement.terrainData.heightmapResolution * mapElement.terrainData.heightmapResolution) * sizeof(float);
                stream_out.Write(size);//Размер массива высот
                float[,] heights = mapElement.terrainData.GetHeights(0, 0, mapElement.terrainData.heightmapResolution, mapElement.terrainData.heightmapResolution);
                for (int i = 0; i < mapElement.terrainData.heightmapResolution; i++)
                {
                    for (int j = 0; j < mapElement.terrainData.heightmapResolution; j++)
                    {
                        stream_out.Write(heights[i, j] * mapElement.terrainData.size.y);
                    }
                }
            }
           
        }
    }
}
#endif