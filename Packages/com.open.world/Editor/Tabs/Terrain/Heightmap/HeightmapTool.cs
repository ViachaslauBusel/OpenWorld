#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tools.Terrain
{
    class HeightmapTool
    {
        public static HeightMapSetting heightMapSetting;
        public static void Draw()
        {
            GUILayout.Space(10.0f);

            if (GUILayout.Button("Open PNG"))
            {
                string path = EditorUtility.OpenFilePanel("Open height map png", "", "png");
                if (!string.IsNullOrEmpty(path))
                {
                    byte[] file = File.ReadAllBytes(path);
                    heightMapSetting.texture = new Texture2D(2, 2);
                    heightMapSetting.texture.LoadImage(file);
                }
            }
            GUILayout.BeginHorizontal();
            GUILayout.Label("Height map: ");
            GUILayout.Label(heightMapSetting.texture, GUILayout.Width(100.0f), GUILayout.Height(100.0f));
            GUILayout.EndHorizontal();

            heightMapSetting.minHeight = EditorGUILayout.FloatField("Min Height: ", heightMapSetting.minHeight);
            heightMapSetting.maxHeight = EditorGUILayout.FloatField("Max height: ", heightMapSetting.maxHeight);

            if (GUILayout.Button("Apply"))
            {
                if (heightMapSetting.texture == null) { EditorUtility.DisplayDialog("ERROR", "No heightmap texture found", "OK"); return; }
                if (heightMapSetting.minHeight < 0.0f || heightMapSetting.minHeight >= heightMapSetting.maxHeight) { EditorUtility.DisplayDialog("ERROR", "Incorrect minimum height", "OK"); return; }
                if (heightMapSetting.maxHeight < heightMapSetting.minHeight || heightMapSetting.maxHeight > TabSetting.Map.MaximumWorldHeight) { EditorUtility.DisplayDialog("ERROR", "Incorrect maximum height", "OK"); return; }

                HeightMapFromTexture.Apply(TabSetting.Map, heightMapSetting);
            }
        }
    }
}
#endif