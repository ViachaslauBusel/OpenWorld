using OpenWorldEditor.Tools.Terrain;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor
{
    public class TabTerrain
    {
        private enum Tools
        {
            /// <summary>
            /// Tab with tools for generating the ocean at a given height.
            /// </summary>
            Water,
            /// <summary>
            /// Tab with tools for applying texture.
            /// </summary>
            Texture,
            /// <summary>
            /// Tab with tools for applying a heightmap texture to the terrain.
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

            DrawToolButton(Tools.Water, "TerrainInspector.TerrainToolTrees", EditorStyles.miniButtonLeft, "Generate water level");
            DrawToolButton(Tools.Texture, "TerrainInspector.TerrainToolPlants", EditorStyles.miniButtonMid, "Set terrain texture");
            DrawToolButton(Tools.Heightmap, "TerrainInspector.TerrainToolSettings", EditorStyles.miniButtonMid, "Apply heightmap");
            DrawToolButton(Tools.Light, "TerrainInspector.TerrainToolSettings", EditorStyles.miniButtonRight, "Light settings");

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private static void DrawToolButton(Tools tool, string icon, GUIStyle style, string tooltip)
        {
            GUI.enabled = activeTool != tool;
            if (GUILayout.Button(new GUIContent(EditorGUIUtility.IconContent(icon).image, tooltip), style, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {
                activeTool = tool;
            }
        }

        public static void Draw()
        {
            if (TabSetting.Map == null)
            {
                EditorGUILayout.Space(10.0f);
                EditorGUILayout.LabelField("Required components for the tab are not specified: Map");
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
                    //LightTool.Draw();
                    break;
            }
        }
    }
}
