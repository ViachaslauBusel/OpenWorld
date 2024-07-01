#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace OpenWorldEditor.Tabs.Res
{
    public class SceneResources
    {
        public static ResourcesLoader ResourcesLoader { get; } = new ResourcesLoader();
        public static void OnSceneGUI(SceneView sceneView)
        {
            ResourcesLoader.OnSceneGUI(sceneView);
            if (TabResources.ActiveTool == ResourcesTools.Attach)
                ResourceBrush.SceneGUI(sceneView);

        }

        public static void Start()
        {
            TabResources.update += UpdateTabs;
            ResourcesLoader.Initial();
        }
        public static void Stop()
        {
            ResourceBrush.Dispose();
            ResourcesLoader.Dispose();
            TabResources.update -= UpdateTabs;
        }
        public static void UpdateTabs(ResourcesTools tool)
        {
            ResourceBrush.Dispose();
        }
        internal static void Update()
        {
            ResourcesLoader.CalculeteVisibleNPCs();
        }
    }
}
#endif