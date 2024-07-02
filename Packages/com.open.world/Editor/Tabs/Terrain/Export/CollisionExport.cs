#if UNITY_EDITOR
using OpenWorld.DATA;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace OpenWorld.Tabs.Terrain.Export
{
    public static class CollisionExport
    {
        public static void Export(Map map)
        {

            float totalProgress = 0;

            Dictionary<string, Mesh> meshColliders = new Dictionary<string, Mesh>();
            using (BinaryWriter stream_out = new BinaryWriter(File.Open(@"Export/collision.dat", FileMode.Create)))
            {
                int maxTile = map.MapSizeKilometers * map.TilesPerKilometer;
                float maxProgress = maxTile * maxTile;

                for (int y = 0; y < maxTile; y++)
                {

                    for (int x = 0; x < maxTile; x++)
                    {
                        totalProgress = (y * maxTile) + x;
                        EditorUtility.DisplayProgressBar("OpenWorld", "Export collision.dat", totalProgress / maxProgress);


                        int xKM = x / map.TilesPerKilometer;
                        int yKM = y / map.TilesPerKilometer;
                        int xTR = x % map.TilesPerKilometer;
                        int yTR = y % map.TilesPerKilometer;

                        Write(xKM, yKM, xTR, yTR, map, stream_out, in meshColliders);
                    }
                }
            }
            EditorUtility.ClearProgressBar();


            using (BinaryWriter stream_out = new BinaryWriter(File.Open(@"Export/terrainMeshColliders.dat", FileMode.Create)))
            {
                stream_out.Write(meshColliders.Count);
                foreach (var keyValue in meshColliders)
                {
                    stream_out.Write(keyValue.Key);

                    Vector3[] vertices = keyValue.Value.vertices;
                  
                    stream_out.Write(vertices.Length);
                    for(int v = 0; v < vertices.Length; v++)
                    {
                        stream_out.Write(vertices[v].x);
                        stream_out.Write(vertices[v].y);
                        stream_out.Write(vertices[v].z);
                    }

                    int[] triangles = keyValue.Value.triangles;
                    stream_out.Write(triangles.Length);
                    for (int t = 0; t < triangles.Length; t++)
                    {
                        stream_out.Write(triangles[t]);
                    }
                }
            }

        }

        private static void Write(int xKM, int yKM, int xTR, int yTR, Map map, BinaryWriter stream_out, in Dictionary<string, Mesh> meshColliders)
        {

            string path = map.GetPath(xKM, yKM, xTR, yTR);
            Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(path);

            if (tile != null)
            {
                foreach(MapObject mapOBJ in tile.Objects)
                {
                    GameObject prefabOBJ = mapOBJ.Prefab.Asset;
                    Collider[] collidersArray = prefabOBJ.GetComponentsInChildren<Collider>();
                    stream_out.Write(collidersArray.Length);//Количество Collider на обьекте
                    foreach (Collider collider in collidersArray)
                    {
                        prefabOBJ.transform.position = mapOBJ.Position;
                        prefabOBJ.transform.rotation = mapOBJ.Rotation;
                        prefabOBJ.transform.localScale = mapOBJ.Scale;




                        //Рассчитывает точное положение коллайдера в мировом пространстве.
                        //Позиция обьекта на карте + смещение центра коллайдера
                        Func<Vector3, Vector3> positionOnCenter = (center) =>
                        {
                            return collider.transform.position + (collider.transform.rotation * new Vector3(center.x * collider.transform.lossyScale.x,
                                                                                                            center.y * collider.transform.lossyScale.y,
                                                                                                            center.z * collider.transform.lossyScale.z));
                        };

                        Vector3 position = collider switch
                        {
                            CapsuleCollider capsuleCollider => positionOnCenter.Invoke(capsuleCollider.center),// - capsuleCollider.transform.up * capsuleCollider.height * capsuleCollider.transform.lossyScale.y,
                            BoxCollider boxCollider => positionOnCenter.Invoke(boxCollider.center),
                            SphereCollider sphereCollider => positionOnCenter.Invoke(sphereCollider.center),
                            MeshCollider meshCollider => collider.transform.position,
                            _ => Vector2.zero
                        };

                      //  Debug.Log($"position:{position} collider.transform.position:{collider.transform.position}");


                        stream_out.Write(position.x);
                        stream_out.Write(position.y);
                        stream_out.Write(position.z);


                        stream_out.Write(collider.transform.rotation.w);
                        stream_out.Write(collider.transform.rotation.x);
                        stream_out.Write(collider.transform.rotation.y);
                        stream_out.Write(collider.transform.rotation.z);


                        switch (collider)
                        {
                            case BoxCollider box:
                                stream_out.Write((byte)1); //<<<<<box <- 1
                                Vector3 size = new Vector3(box.size.x * collider.transform.lossyScale.x,
                                                           box.size.y * collider.transform.lossyScale.y,
                                                           box.size.z * collider.transform.lossyScale.z);
                                stream_out.Write(size.x);
                                stream_out.Write(size.y);
                                stream_out.Write(size.z);
                                break;
                            case SphereCollider sphere:
                                stream_out.Write((byte)2); //<<<<<sphere <- 2
                                float sphereRadius = sphere.radius * Mathf.Max(collider.transform.lossyScale.x, collider.transform.lossyScale.y, collider.transform.lossyScale.z);
                                stream_out.Write(sphereRadius);
                                break;
                            case CapsuleCollider capsule:
                                stream_out.Write((byte)3); //<<<<<capsule <- 3
                                float capsuleRadius = capsule.radius * Mathf.Max(collider.transform.lossyScale.x, collider.transform.lossyScale.z);
                                stream_out.Write(capsuleRadius);
                                float capsuleHeight = capsule.height * collider.transform.lossyScale.y * 0.5f;
                                stream_out.Write(capsuleHeight);
                                break;
                            case MeshCollider mesh:
                                stream_out.Write((byte)4); //<<<<<mesh <- 4

                                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(mesh.sharedMesh.GetInstanceID(), out string guid, out long _))
                                {
                                    stream_out.Write(guid);
                                    meshColliders.Add(guid, mesh.sharedMesh);
                                }
                                else
                                {
                                    stream_out.Write("");
                                    Debug.Log("MeshCollider: Не удалось найти guid");
                                }
                                stream_out.Write(collider.transform.lossyScale.x);
                                stream_out.Write(collider.transform.lossyScale.y);
                                stream_out.Write(collider.transform.lossyScale.z);
                                break;
                        }
                       

                      
                    }
                }
            }

        }
    }
}
#endif