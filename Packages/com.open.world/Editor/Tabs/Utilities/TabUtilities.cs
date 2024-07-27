#if UNITY_EDITOR
using OpenWorld.Helpers;
using OpenWorld.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace OpenWorldEditor
{
    internal static class TabUtilities
    {
        private static List<IMapUtility> _utilities = new();

        static TabUtilities()
        {
            // Find all classes that implement the IMapUtility interface in all assemblies
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IMapUtility)));
                foreach (var type in types)
                {
                    if (type.IsAbstract || type.IsInterface) continue;
                    var instance = (IMapUtility)Activator.CreateInstance(type);
                    if (instance != null)
                    {
                        _utilities.Add(instance);
                    }
                }
            }
        }

        public static void Draw()
        {
            foreach (var utilite in _utilities)
            {
                if (GUILayout.Button(utilite.Name))
                {
                    ExecuteUtilite(utilite);
                    AssetDatabase.SaveAssets();
                }
            }
        }

        private static void ExecuteUtilite(IMapUtility utilite)
        {
            switch (utilite)
            {
                case IMapUtilityForGameMap gameMapUtilite:
                    gameMapUtilite.Execute(TabSetting.Map);
                    break;
                case IMapUtilityForMapTile mapTileUtilite:
                    ExecuteForMapTile(mapTileUtilite);
                    break;
                case IMapUtilityForMapEntity mapEntityUtilite:
                    ExecuteForMapEntity(mapEntityUtilite);
                    break;
            }
        }

        private static void ExecuteForMapEntity(IMapUtilityForMapEntity mapEntityUtilite)
        {
            bool success = true;
            try
            {
                mapEntityUtilite.BeginExecution(TabSetting.Map);
                EditorUtility.DisplayProgressBar("OpenWorld", "Executing " + mapEntityUtilite.Name, 0.0f);
                int totalTiles = TabSetting.Map.MapSizeKilometers * TabSetting.Map.MapSizeKilometers * TabSetting.Map.TilesPerKilometer * TabSetting.Map.TilesPerKilometer;
                int currentTile = 0;
                foreach (var mapElement in TabSetting.Map.EnumerateAllTiles())
                {
                    EditorUtility.DisplayProgressBar("OpenWorld", "Executing " + mapEntityUtilite.Name, ++currentTile / (float)totalTiles);
                    bool isDirty = false;
                    foreach (var entity in mapElement.Tile.Entities)
                    {
                        isDirty = mapEntityUtilite.Execute(entity) || isDirty;
                    }
                    if (isDirty) EditorUtility.SetDirty(mapElement.Tile);
                }

                if (totalTiles != currentTile) Debug.LogError($"Total tiles and current tile count mismatch: {currentTile}/{totalTiles}");
            }
            catch (Exception e)
            {
                success = false;
                Debug.LogError(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                mapEntityUtilite.EndExecution(success);
            }
        }


        private static void ExecuteForMapTile(IMapUtilityForMapTile mapTileUtilite)
        {
            bool success = true;
            try
            {
                mapTileUtilite.BeginExecution(TabSetting.Map);
                EditorUtility.DisplayProgressBar("OpenWorld", "Executing " + mapTileUtilite.Name, 0.0f);
                int totalTiles = TabSetting.Map.MapSizeKilometers * TabSetting.Map.MapSizeKilometers * TabSetting.Map.TilesPerKilometer * TabSetting.Map.TilesPerKilometer;
                int currentTile = 0;
                foreach (var mapElement in TabSetting.Map.EnumerateAllTiles())
                {
                    EditorUtility.DisplayProgressBar("OpenWorld", "Executing " + mapTileUtilite.Name, ++currentTile / (float)totalTiles);
                    bool isDirty = mapTileUtilite.Execute(mapElement.Tile, mapElement.Location);
                    if (isDirty) EditorUtility.SetDirty(mapElement.Tile);
                }

                if (totalTiles != currentTile) Debug.LogError($"Total tiles and current tile count mismatch: {currentTile}/{totalTiles}");
            }
            catch (Exception e)
            {
                success = false;
                Debug.LogError(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                mapTileUtilite.EndExecution(success);
            }
        }
    }
}
#endif