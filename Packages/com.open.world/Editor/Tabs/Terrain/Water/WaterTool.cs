#if UNITY_EDITOR
using OpenWorld;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tools.Terrain
{
    public class WaterTool
    {
        private static float height;
        public static void Draw()
        {
            height = EditorGUILayout.FloatField("Water level: ", height);

            if (GUILayout.Button("Generation Water"))
            {
                WaterGeneration.Generation(height, TabSetting.Map);
            }
        }
    }
}
#endif