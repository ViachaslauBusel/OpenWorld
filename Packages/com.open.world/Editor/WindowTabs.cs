#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor
{
    public enum Tab { None = 0, Terrain, Monsters, NPC, Resources, Machine, Objects, Export, Setting }
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
                updateTab?.Invoke(_activeTab);
            }
        }
        public static event Action<Tab> updateTab;

        public static void Restore()
        {
            ActiveTab = (Tab)PlayerPrefs.GetInt("ActiveTabMapEditor", (int)Tab.None);
        }

        public static void Draw()
        {

           
            GUILayout.Space(25.0f);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            //Отрисовка и редактирование террейна
            GUI.enabled = ActiveTab != Tab.Terrain;
            if(GUILayout.Button(EditorGUIUtility.IconContent("Terrain Icon"), EditorStyles.miniButtonLeft, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {
                ActiveTab = Tab.Terrain;
            }

            //Отрисовка и редактирование монстров
            GUI.enabled = ActiveTab != Tab.Monsters;
            if(GUILayout.Button(EditorGUIUtility.IconContent("Avatar Icon"), EditorStyles.miniButtonMid, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {
                ActiveTab = Tab.Monsters;
            }

            //Отрисовка и редактирование NPC
            GUI.enabled = ActiveTab != Tab.NPC;
            if (GUILayout.Button(EditorGUIUtility.IconContent("Avatar Icon"), EditorStyles.miniButtonMid, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {
                ActiveTab = Tab.NPC;
            }

            //Отрисовка и редактирование ресурсов
            GUI.enabled = ActiveTab != Tab.Resources;
            if (GUILayout.Button(EditorGUIUtility.IconContent("Avatar Icon"), EditorStyles.miniButtonMid, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {
                ActiveTab = Tab.Resources;
            }

            //Отрисовка и редактирование станков
            GUI.enabled = ActiveTab != Tab.Machine;
            if (GUILayout.Button(EditorGUIUtility.IconContent("Avatar Icon"), EditorStyles.miniButtonMid, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {
                ActiveTab = Tab.Machine;
            }

            //Отрисовка и редактирование обьектов на карте
            GUI.enabled = ActiveTab != Tab.Objects;
            if(GUILayout.Button(EditorGUIUtility.IconContent("Prefab Icon"), EditorStyles.miniButtonMid, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {
                ActiveTab = Tab.Objects;
            }

            //Отрисовка вкладки экспорта данных на сервер
            GUI.enabled = ActiveTab != Tab.Export;
            if (GUILayout.Button(EditorGUIUtility.IconContent("BuildSettings.Web"), EditorStyles.miniButtonMid, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {
                ActiveTab = Tab.Export;
            }

            //Настройка редактора, путей до ассетов
            GUI.enabled = ActiveTab != Tab.Setting;
            if (GUILayout.Button(EditorGUIUtility.IconContent("SettingsIcon"), EditorStyles.miniButtonMid, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {
                ActiveTab = Tab.Setting;
            }

            GUI.enabled = true;
            //Отключить все
            if (GUILayout.Button(EditorGUIUtility.IconContent("PauseButton"), EditorStyles.miniButtonRight, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {
                ActiveTab = Tab.None;
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

        }
    }
}
#endif