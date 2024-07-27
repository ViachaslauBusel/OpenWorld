#if UNITY_EDITOR
using OpenWorld.DATA;
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
        private static List<IMapUtility> _utilites = new();

        static TabUtilities()
        {
            //Find all classes that implement the IMapUtilite interface
            var types = typeof(TabUtilities).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IMapUtility)));
            foreach (var type in types)
            {
                if (type.IsAbstract || type.IsInterface) continue;
                var instance = (IMapUtility)Activator.CreateInstance(type);
                if (instance != null)
                {
                    _utilites.Add(instance);
                }
            }
        }

        public static void Draw()
        {
            foreach (var utilite in _utilites)
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
                    ExecuteWithExceptionHandling(mapTileUtilite.Name, mapTileUtilite.BeginExecution, mapTileUtilite.Execute, mapTileUtilite.EndExecution);
                    break;
                case IMapUtilityForMapEntity mapEntityUtilite:
                    ExecuteWithExceptionHandling(mapEntityUtilite.Name, mapEntityUtilite.BeginExecution, mapTile =>
                    {
                        bool isDirty = false;
                        foreach (var entity in mapTile.Entities)
                        {
                           isDirty = mapEntityUtilite.Execute(entity) | isDirty;
                        }
                        return isDirty;
                    }, mapEntityUtilite.EndExecution);
                    break;
            }
        }

        private static void ExecuteWithExceptionHandling(string utiliteName, Action<GameMap> beginExecution, Func<MapTile, bool> executeAction, Action<bool> endExecution)
        {
            bool success = true;
            try
            {
                beginExecution(TabSetting.Map);
                ExecuteUtiliteWithProgress(utiliteName, executeAction);
            }
            catch (Exception e)
            {
                success = false;
                Debug.LogError(e);
            }
            endExecution(success);
        }

        private static void ExecuteUtiliteWithProgress(string utiliteName, Func<MapTile, bool> executeAction)
        {
            EditorUtility.DisplayProgressBar("OpenWorld", "Executing " + utiliteName, 0.0f);
            int totalTiles = TabSetting.Map.MapSizeKilometers * TabSetting.Map.MapSizeKilometers * TabSetting.Map.TilesPerKilometer * TabSetting.Map.TilesPerKilometer;
            int currentTile = 0;
            foreach (var mapElement in TabSetting.Map.EnumerateAllTiles())
            {
                EditorUtility.DisplayProgressBar("OpenWorld", "Executing " + utiliteName, ++currentTile / (float)totalTiles);
                bool isDirty = executeAction(mapElement);
                if(isDirty) EditorUtility.SetDirty(mapElement);
            }
            EditorUtility.ClearProgressBar();
            if (totalTiles != currentTile) Debug.LogError($"Total tiles and current tile count mismatch: {currentTile}/{totalTiles}");
        }
    }
}
#endif