#if UNITY_EDITOR
using OpenWorld.Tabs.NPC;
using OpenWorldEditor.Tools.Tabs;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tabs.NPCs
{
    public class SceneNPCs 
    {
        public static NPCsLoader NPCsLoader { get; } = new NPCsLoader();
        public static void OnSceneGUI(SceneView sceneView)
        {
            NPCsLoader.OnSceneGUI(sceneView);
            if (TabNPC.ActiveTool == TabADTool.Attach)
            { NPCBrush.SceneGUI(sceneView); }

        }

        public static void Start()
        {
            TabNPC.RegisterEventUpdate(UpdateTabs);
            NPCsLoader.Initial();
        }
        public static void Stop()
        {
            NPCBrush.Dispose();
            NPCsLoader.Dispose();
            TabNPC.UnregisterEventUpdate(UpdateTabs);
        }
        public static void UpdateTabs(TabADTool tool)
        {
           NPCBrush.Dispose();
        }
        internal static void Update()
        {
            NPCsLoader.CalculeteVisibleNPCs();
        }
    }
}
#endif