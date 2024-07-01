#if UNITY_EDITOR
using OpenWorldEditor.Tabs.Monsters.Attach;
using OpenWorldEditor.Tabs.Monsters.Display;
using OpenWorldEditor.Tools.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace OpenWorldEditor.Tabs.Monsters
{
    public class SceneMonsters
    {
        public static MonstersLoader MonstersLoader { get; } = new MonstersLoader();
        public static void OnSceneGUI(SceneView sceneView)
        {
            MonstersLoader.OnSceneGUI(sceneView);
            if (TabMonsters.ActiveTool == TabADTool.Attach)
            { MonsterBrush.SceneGUI(sceneView); }

        }

        public static void Start()
        {
            TabMonsters.RegisterEventUpdate(UpdateTabs);
            MonstersLoader.Initial();
        }
        public static void Stop()
        {
            MonsterBrush.Dispose();
            MonstersLoader.Dispose();
            TabMonsters.UnregisterEventUpdate(UpdateTabs);
        }
        public static void UpdateTabs(TabADTool tool)
        {
            MonsterBrush.Dispose();
        }
        internal static void Update()
        {
            MonstersLoader.CalculeteVisibleNPCs();
        }
    }
}
#endif