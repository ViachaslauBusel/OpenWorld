#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using OpenWorld;
using OpenWorldEditor.Tools.Terrain;

namespace OpenWorldEditor
{


  
    public class TabTerrain
    {
        private enum Tools { 
            /// <summary>
            /// Вкладка с инструментами генерации океана по заданной высоте
            /// </summary>
            Water,
            /// <summary>
            /// Вкладка с инструментами наложения текстуры
            /// </summary>
            Texture,
            /// <summary>
            /// Вкладка с инструментами для наложение текстуры карты высот на террейн
            /// </summary>
            Heightmap,
            Light
        }

        private static Tools activeTool = Tools.Water;
        

        public static void DrawToolsIcon()
        {
            GUILayout.Space(20.0f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUI.enabled = activeTool != Tools.Water;
            if (GUILayout.Button(EditorGUIUtility.IconContent("TerrainInspector.TerrainToolTrees"), EditorStyles.miniButtonLeft)) { activeTool = Tools.Water; }//Генерация уровня воды
            GUI.enabled = activeTool != Tools.Texture;
            if (GUILayout.Button(EditorGUIUtility.IconContent("TerrainInspector.TerrainToolPlants"), EditorStyles.miniButtonMid)) { activeTool = Tools.Texture; }//Задать текстуру террейна
            GUI.enabled = activeTool != Tools.Heightmap;
            if (GUILayout.Button(EditorGUIUtility.IconContent("TerrainInspector.TerrainToolSettings"), EditorStyles.miniButtonRight)) { activeTool = Tools.Heightmap; }//Применить карту высот
            GUI.enabled = activeTool != Tools.Light;
            if (GUILayout.Button(EditorGUIUtility.IconContent("TerrainInspector.TerrainToolSettings"), EditorStyles.miniButtonRight)) { activeTool = Tools.Light; }
            GUI.enabled = true;

       

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void Draw()
        {
            if(TabSetting.Map == null) 
            {
                EditorGUILayout.Space(10.0f);
                EditorGUILayout.LabelField("Не указаны необходимые компоненты для работы вкладки: Map");
                return;
            }

            DrawToolsIcon();

            switch (activeTool)
            {
                case Tools.Water:
                    WaterTool.Draw();
                    break;
                case Tools.Texture:
                    TextureLayerTool.Draw();
                    break;
                case Tools.Heightmap:
                     HeightmapTool.Draw();
                    break;
                case Tools.Light:
                    HeightmapTool.Draw();
                    break;
            }
        }

    }
}
#endif