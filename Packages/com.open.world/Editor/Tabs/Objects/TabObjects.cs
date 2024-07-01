#if UNITY_EDITOR
using OpenWorld;
using OpenWorld.Tools.Objects;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OpenWorldEditor
{
    public class TabObjects
    {
        private enum Tools { None, AddObject, DisplayObject }
        private static Tools activeTool = Tools.None;




        public static void DrawTabIcon()
        {

            GUILayout.Space(15.0f);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            //Добавление обьекта на карту
            GUI.enabled = activeTool != Tools.AddObject;
            if (GUILayout.Button(EditorGUIUtility.IconContent("Animation.AddEvent"), EditorStyles.miniButtonLeft, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {
  
                activeTool = Tools.AddObject;
            }

            //Добавление обьекта в список префабов
            GUI.enabled = activeTool != Tools.DisplayObject;
            if (GUILayout.Button(EditorGUIUtility.IconContent("UnityLogo"), EditorStyles.miniButtonRight, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {

                activeTool = Tools.DisplayObject;
            }


            GUI.enabled = true;
           GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

        }


        public static void Draw()
        {
            if (TabSetting.Map == null)
            {
                EditorGUILayout.Space(10.0f);
                EditorGUILayout.LabelField("Не указаны необходимые компоненты для работы вкладки: Map");
                return;
            }

            DrawTabIcon();

            switch (activeTool)
            {
                case Tools.AddObject:
                    AttachTool.Draw();
                    break;
                case Tools.DisplayObject:
                    OpenWorld.Tools.Objects.DisplayTool.Draw();
                    break;
                case Tools.None:
                    break;
            }
        }
       
    }
}
#endif