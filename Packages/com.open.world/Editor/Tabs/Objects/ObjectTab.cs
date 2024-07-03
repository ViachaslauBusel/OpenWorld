using OpenWorldEditor.MapObjectTab.Attach;
using OpenWorldEditor.MapObjectTab.Display;
using OpenWorldEditor.MapObjectTab.Settings;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.MapObjectTab
{
    public static class ObjectTab
    {
        private enum Tools { None, AddObject, DisplayObject, Settings }
        private static Tools activeTool = Tools.None;

        public static void DrawTabIcon()
        {
            GUILayout.Space(15.0f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Add an object to the map
            DrawToolButton(Tools.AddObject, "Animation.AddEvent", EditorStyles.miniButtonLeft, "Add object to map");

            // Add an object to the prefab list
            DrawToolButton(Tools.DisplayObject, "UnityLogo", EditorStyles.miniButtonRight, "Add object to prefab list");

            GUI.enabled = true;
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

            DrawTabIcon();

            switch (activeTool)
            {
                case Tools.AddObject:
                    MapObjectAttachTool.Draw();
                    break;
                case Tools.DisplayObject:
                    MapObjectDisplayTool.Draw();
                    break;
                case Tools.Settings:
                    ObjectSettingsTool.Draw();
                    break;
            }
        }
    }
}
