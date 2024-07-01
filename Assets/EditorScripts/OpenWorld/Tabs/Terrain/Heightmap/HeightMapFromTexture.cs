#if UNITY_EDITOR
using OpenWorld.DATA;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tools.Terrain
{
    public class HeightMapFromTexture
    {
        public static void Apply(Map map, HeightMapSetting heightMapSetting)
        {
            int heightMapPoint =  map.Size * map.TilesPerBlock * (map.HeightmapResolution - 1) + 1;
            Color[] mapColors = heightMapSetting.texture.GetPixels(0, 0, heightMapSetting.texture.width, heightMapSetting.texture.height);


            float[] grayScale = new float[heightMapPoint * heightMapPoint];

            float pixelX = heightMapSetting.texture.width / (float)heightMapPoint;
            float pixelY = heightMapSetting.texture.height / (float)heightMapPoint;

          
            float maxProgress = heightMapPoint * heightMapPoint;

            for (int y = 0; y < heightMapPoint; y++)
            {
                EditorUtility.DisplayProgressBar("OpenWorld", "HeightMap Generation", (y * heightMapPoint) / maxProgress);
                for (int x = 0; x < heightMapPoint; x++)
                {
                   
                    float xSmooth = x * pixelX; 
                    float ySmooth = y * pixelY;
                    int xPos = (int)xSmooth;
                    int yPos = (int)ySmooth;
                    xSmooth -= xPos;
                    ySmooth -= yPos;

                    int cColor = xPos + yPos * heightMapSetting.texture.width;
                    int xColor = (xPos + 1) + yPos * heightMapSetting.texture.width;
                    int yColor = xPos + (yPos + 1) * heightMapSetting.texture.width;
//if(cColor >= mapColors.Length) cColor = mapColors.Length - 1;
                    if (xColor >= mapColors.Length) xColor = mapColors.Length - 1;
                    if (yColor >= mapColors.Length) yColor = mapColors.Length - 1;
                    grayScale[x + y * heightMapPoint] = Color.Lerp(Color.Lerp(mapColors[cColor], mapColors[xColor], xSmooth), Color.Lerp(mapColors[cColor], mapColors[yColor], ySmooth), 0.5f).grayscale;
                }
            }
            EditorUtility.ClearProgressBar();
            float setHeight = heightMapSetting.maxHeight - heightMapSetting.minHeight;
            maxProgress = map.Size * map.Size + map.TilesPerBlock * map.TilesPerBlock;
            for (int yKM = 0; yKM < map.Size; yKM++)
            {
                for (int xKM = 0; xKM < map.Size; xKM++)
                {


                    for (int y = 0; y < map.TilesPerBlock; y++)
                    {
                        for (int x = 0; x < map.TilesPerBlock; x++)
                        {
                            EditorUtility.DisplayProgressBar("OpenWorld", "HeightMap apply", ((yKM * map.Size + xKM) + (y * map.TilesPerBlock + x)) / maxProgress);
                            string path = map.GetPath(xKM, yKM, x, y) ;

                            Tile resource = AssetDatabase.LoadAssetAtPath<Tile>(path);
                            TerrainData terrainData = (resource as Tile).terrainData;

                            int xStart = (xKM * map.TilesPerBlock + x) * (map.HeightmapResolution - 1);
                            int yStart = (yKM * map.TilesPerBlock + y) * (map.HeightmapResolution - 1);
                            SetHeight(terrainData, xStart, yStart, heightMapSetting.minHeight, setHeight, map.MaxHeight, grayScale, heightMapPoint);
                        }
                    }


                }
            }
            EditorUtility.ClearProgressBar();
        }


        private static void SetHeight(TerrainData terrainData, int xStart, int yStart, float minHeight, float setHeight, float mapHeight, float[] grayScale, int heightMapPoint)
        {
            float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
         

            for (int y = 0; y < heights.GetLength(1); y++)
            {
                for (int x = 0; x < heights.GetLength(0); x++)
                {
                    float scale = grayScale[xStart + x + ((yStart + y) * heightMapPoint)];
                    heights[y, x] = (minHeight + setHeight * scale)/mapHeight;
                }
            }

            terrainData.SetHeights(0, 0, heights);

        }


    }
}
#endif