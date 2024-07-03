using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor
{
    public enum Tab { None = 0, Terrain, Objects, Export, Setting }
    public class WindowTabs
    {
        private static Tab _activeTab;
        public static Tab ActiveTab
        {
            get => _activeTab;
            private set
            {
                _activeTab = value;
                PlayerPrefs.SetInt("ActiveTools", (int)_activeTab);
                OnUpdateTab?.Invoke(_activeTab);
            }
        }
        public static event Action<Tab> OnUpdateTab;

        public static void Restore()
        {
            ActiveTab = (Tab)PlayerPrefs.GetInt("ActiveTabMapEditor", (int)Tab.None);
        }

        public static void Draw()
        {
            GUILayout.Space(25.0f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            DrawTabButton(Tab.Terrain, "Terrain Icon", EditorStyles.miniButtonLeft);
            DrawTabButton(Tab.Objects, "Prefab Icon", EditorStyles.miniButtonMid);
            DrawTabButton(Tab.Export, "BuildSettings.Web", EditorStyles.miniButtonMid);
            DrawTabButton(Tab.Setting, "SettingsIcon", EditorStyles.miniButtonMid);
            DrawTabButton(Tab.None, "PauseButton", EditorStyles.miniButtonRight);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private static void DrawTabButton(Tab tab, string icon, GUIStyle style)
        {
            GUI.enabled = ActiveTab != tab;
            if (GUILayout.Button(EditorGUIUtility.IconContent(icon), style, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {
                ActiveTab = tab;
            }
        }
    }
}