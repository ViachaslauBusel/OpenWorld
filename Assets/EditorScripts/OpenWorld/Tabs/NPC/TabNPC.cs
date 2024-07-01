#if UNITY_EDITOR
using OpenWorldEditor;
using OpenWorldEditor.Tools.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace OpenWorld.Tabs.NPC
{

    public class TabNPC
    {
      
        private static TabAD m_tab = new TabAD();

        public static TabADTool ActiveTool => m_tab.ActiveTool;
        public static void RegisterEventUpdate(Action<TabADTool> action) => m_tab.update += action;
        public static void UnregisterEventUpdate(Action<TabADTool> action) => m_tab.update -= action;


        public static void Draw()
        {
            if (TabSetting.Map == null)//|| TabSetting.NPCs == null || TabSetting.WorldNPCs == null
            {
                EditorGUILayout.Space(10.0f);
                EditorGUILayout.LabelField("Не указаны необходимые компоненты для работы вкладки: Map, NPCs, WorldNPCs");
                return;
            }

            m_tab.DrawToolsIcon();

            switch (m_tab.ActiveTool)
            {
                case TabADTool.Attach:
                    AttachNPC.Draw();
                    break;
                case TabADTool.Display:
                    DisplayNPC.Draw();
                    break;
            }
        }
    }
}
#endif