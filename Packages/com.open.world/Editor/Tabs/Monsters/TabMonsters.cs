#if UNITY_EDITOR

using OpenWorldEditor.Tabs.Monsters.Attach;
using OpenWorldEditor.Tabs.Monsters.Display;
using OpenWorldEditor.Tools.Tabs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace OpenWorldEditor.Tabs.Monsters {

    public class TabMonsters
    {
        private static TabAD m_tab = new TabAD();

        public static TabADTool ActiveTool => m_tab.ActiveTool;
        public static void RegisterEventUpdate(Action<TabADTool> action) => m_tab.update += action;
        public static void UnregisterEventUpdate(Action<TabADTool> action) => m_tab.update -= action;

        public static void Draw()
        {
            if (TabSetting.Map == null)// || TabSetting.MonstersList == null || TabSetting.WorldMonsterList == null
            {
                EditorGUILayout.Space(10.0f);
                EditorGUILayout.LabelField("Не указаны необходимые компоненты для работы вкладки: Map, MonstersList, WorldMonsterList");
                return;
            }

            m_tab.DrawToolsIcon();

            switch (m_tab.ActiveTool)
            {
                case TabADTool.Attach:
                    AttachMonsterTool.Draw();
                    break;
                case TabADTool.Display:
                    DisplayMonsterTool.Draw();
                    break;
            }
        }
    }
}
#endif