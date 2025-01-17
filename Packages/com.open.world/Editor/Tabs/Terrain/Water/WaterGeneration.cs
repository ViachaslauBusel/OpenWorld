﻿#if UNITY_EDITOR
using OpenWorld;
using OpenWorld.DATA;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tools.Terrain
{
    public class WaterGeneration
    {
        public static void Generation(float waterLevel, GameMap editMap)
        {
            GameMap map = editMap;
            map.WaterLevel = waterLevel;
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(map);
            AssetDatabase.Refresh();

            float maxProgress = map.MapSizeKilometers * map.MapSizeKilometers;


            for (int yKM = 0; yKM < map.MapSizeKilometers; yKM++)
            {
                for (int xKM = 0; xKM < map.MapSizeKilometers; xKM++)
                {
                    float totalProgress = (yKM * map.MapSizeKilometers) + xKM;
                    EditorUtility.DisplayProgressBar("OpenWorld", "Water Generation", totalProgress / maxProgress);
                    for (int y = 0; y < map.TilesPerKilometer; y++)
                    {
                        for (int x = 0; x < map.TilesPerKilometer; x++)
                        {
                            string path = map.GetPath(xKM, yKM, x, y);

                            MapTile mapElement = AssetDatabase.LoadAssetAtPath<MapTile>(path);

                            if (mapElement.WaterTile != null)
                            {
                                AssetDatabase.RemoveObjectFromAsset(mapElement.WaterTile);
                            }

                            Mesh waterTile = CreateTileWater(mapElement.TerrainData, TileLocation.GetPostion(map, xKM, yKM, x, y), waterLevel);
                            if (waterTile != null)
                            {

                                AssetDatabase.AddObjectToAsset(waterTile, mapElement);
                            }
                            mapElement.SetWater(waterTile);
                        }
                    }


                }
            }
            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static Mesh CreateTileWater(TerrainData terrainData, Vector3 position, float waterLevel)
        {
            float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);


            waterLevel /= terrainData.size.y;

            Vertices vertices = new Vertices(heights.GetLength(0), heights.GetLength(1));
            float disX = terrainData.size.x / (vertices.GetLength(0) - 1);
            float disY = terrainData.size.z / (vertices.GetLength(1) - 1);
            //    Debug.Log("Water: " + heights[0, 0]);
            //waterLevel = heights[0, 0] * 1.006f;
            //      Debug.Log("Water level: " + waterLevel);
            //   Debug.Log("D:" + heights.GetLength(0));
            //     Debug.Log("Vertex: " + (heights.GetLength(0)* heights.GetLength(1)));

            for (int y = 0; y < vertices.GetLength(1); y++)
            {
                for (int x = 0; x < vertices.GetLength(0); x++)
                {

                    if (ContainsWater(heights, waterLevel, x, y)) vertices.Add(new Vector3(x * disX, 1.0f, y * disY), x, y);
                    //  else if( x != 0 && ContainsWater(heights, waterLevel, x-1, y)) vertices.Add(new Vector3(x * disX, 0.0f, y * disY), x, y);
                    //  else if (y != 0 && ContainsWater(heights, waterLevel, x, y-1)) vertices.Add(new Vector3(x * disX, 0.0f, y * disY), x, y);
                    //else if (x != 0 && y != 0 && ContainsWater(heights, waterLevel, x-1, y - 1)) vertices.Add(new Vector3(x * disX, 0.0f, y * disY), x, y);

                }
            }



            List<int> triangles = new List<int>();
            for (int y = 0; y < vertices.GetLength(1) - 1; y++)
            {
                for (int x = 0; x < vertices.GetLength(0); x++)
                {
                    if (vertices.NotEmpty(x, y))
                    {
                        if (x != 0 && vertices.NotEmpty(x, y + 1) && vertices.NotEmpty(x - 1, y))
                        {
                            triangles.Add(vertices.GetIndex(x, y));
                            triangles.Add(vertices.GetIndex(x - 1, y));
                            triangles.Add(vertices.GetIndex(x, y + 1));

                        }
                        if (x < vertices.GetLength(0) - 1 && vertices.NotEmpty(x + 1, y + 1) && vertices.NotEmpty(x, y + 1))
                        {
                            triangles.Add(vertices.GetIndex(x, y));
                            triangles.Add(vertices.GetIndex(x, y + 1));
                            triangles.Add(vertices.GetIndex(x + 1, y + 1));

                        }
                    }

                }
            }

            Vector3[] normals = new Vector3[vertices.Count];
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = Vector3.up;
            }

            if (triangles.Count > 0)
            {
                Mesh waterMesh = new Mesh();
                waterMesh.SetVertices(vertices.ToList());
                waterMesh.SetNormals(normals.ToList());
                waterMesh.SetColors(vertices.ToList().Select((v) => Color.black).ToList());
                waterMesh.SetUVs(0, vertices.ToList().Select((v) => new Vector2(v.x, v.z) + new Vector2(position.x, position.z)).ToList());
                waterMesh.SetTriangles(triangles.ToList(), 0);

                // update
                waterMesh.RecalculateBounds();
                waterMesh.RecalculateTangents();
                //waterMesh.RecalculateNormals();
                return waterMesh;
            }
            return null;
        }

        private static bool ContainsWater(float[,] heights, float waterLevel, int xStart, int yStart)
        {
            if (xStart < 0 || xStart >= heights.GetLength(0) || yStart < 0 || yStart >= heights.GetLength(1)) return false;
            return heights[xStart, yStart] < waterLevel;
        }

        public class Vertices
        {
            private Vertex[,] vertices;
            private int index = 0;
            private List<Vector3> listVertices = new List<Vector3>();
            public Vertices(int sizeX, int sizeY)
            {
                vertices = new Vertex[sizeX, sizeY];
            }

            public void Add(Vector3 vertex, int x, int y)
            {
                vertices[x, y] = new Vertex(vertex, index++);
                listVertices.Add(vertex);
            }

            public int Count
            {
                get { return listVertices.Count; }
            }

            public int GetIndex(int x, int y)
            {
                return vertices[x, y].index;
            }

            public bool NotEmpty(int x, int y)
            {
                return vertices[x, y] != null;
            }

            public int GetLength(int dimension)
            {
                return vertices.GetLength(dimension);
            }
            public Vector3[] ToArray()
            {
                return listVertices.ToArray();
            }
            public List<Vector3> ToList()
            {
                return listVertices;
            }
            private class Vertex
            {
                public Vector3 vertex;
                public int index;
                public Vertex(Vector3 vertex, int index)
                {
                    this.vertex = vertex;
                    this.index = index;
                }
            }
        }
    }
}
#endif