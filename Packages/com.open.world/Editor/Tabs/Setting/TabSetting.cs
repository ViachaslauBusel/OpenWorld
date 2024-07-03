using OpenWorld.DATA;
using OpenWorldEditor.SceneWindow;
using OpenWorldEditor.Tabs.Setting;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor
{
    public static class TabSetting
    {
        private static SettingReferenceSaver<Map> _mapSaver = new SettingReferenceSaver<Map>("mapASFVS");
        private static SettingReferenceSaver<MapSettings> _mapSettingsSaver = new SettingReferenceSaver<MapSettings>("mapSettingsDGVVC");
        private static Vector2 _scrollPosition = Vector2.zero;

        public static Map Map => _mapSaver.Object;
        public static MapSettings MapSettings => _mapSettingsSaver.Object;

        public static void Draw()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, true);

            DrawSection("Map", ref _mapSaver);
            DrawSection("Map Settings", ref _mapSettingsSaver);

            GUILayout.Space(15.0f);

            if (GUILayout.Button("Save"))
            {
                SaveSettings();
            }

            EditorGUILayout.EndScrollView();
            GUI.enabled = true;
        }

        private static void DrawSection<T>(string label, ref SettingReferenceSaver<T> saver) where T : UnityEngine.Object
        {
            GUILayout.Space(15.0f);
            GUILayout.Label(label);
            GUILayout.Space(5.0f);
            saver.Object = EditorGUILayout.ObjectField(saver.Object, typeof(T), false) as T;
        }

        private static void SaveSettings()
        {
            SettingsRegistry.Settings.ForEach(c => c.Save());
            WorldLoader.Reload();
        }
    }
}
