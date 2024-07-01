#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tabs.Res
{
    public enum ResourcesTools { None, Attach, Display }
    class TabResources
    {
        public static ResourcesTools ActiveTool { get; private set; } = ResourcesTools.Display;
        public static event Action<ResourcesTools> update;

        private static void DrawToolsIcon()
        {
            GUILayout.Space(15.0f);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            //Добавление обьекта на карту
            GUI.enabled = ActiveTool != ResourcesTools.Attach;
            if (GUILayout.Button(EditorGUIUtility.IconContent("Animation.AddEvent"), EditorStyles.miniButtonLeft, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {
                ActiveTool = ResourcesTools.Attach;
                update?.Invoke(ActiveTool);
            }

            //Просмотр списка мобов
            GUI.enabled = ActiveTool != ResourcesTools.Display;
            if (GUILayout.Button(EditorGUIUtility.IconContent("UnityLogo"), EditorStyles.miniButtonRight, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {

                ActiveTool = ResourcesTools.Display;
                update?.Invoke(ActiveTool);
            }


            GUI.enabled = true;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        public static void Draw()
        {
            if (TabSetting.Map == null )// || TabSetting.Resources == null || TabSetting.WorldResources == null
            {
                EditorGUILayout.Space(10.0f);
                EditorGUILayout.LabelField("Не указаны необходимые компоненты для работы вкладки: Map, Resources, WorldResources");
                return;
            }

            DrawToolsIcon();

            switch (ActiveTool)
            {
                case ResourcesTools.Attach:
                    AttachResourceTool.Draw();
                    break;
                case ResourcesTools.Display:
                    DisplayResourcesTool.Draw();
                    break;
                case ResourcesTools.None:
                    break;
            }
        }
    }
}
#endif