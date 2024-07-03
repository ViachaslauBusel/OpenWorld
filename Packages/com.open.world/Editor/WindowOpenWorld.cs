using OpenWorldEditor.MapObjectTab;
using OpenWorldEditor.Tools.EditorScene;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor
{
    /// <summary>
    /// "Open World" control / editing window
    /// </summary>
    public class WindowOpenWorld : EditorWindow
    {
        private static WindowOpenWorld _window;

        [MenuItem("Window/Open World")]
        public static void ShowWindow()
        {
            _window = EditorWindow.GetWindow<WindowOpenWorld>(false, "Open World");
            _window.minSize = new Vector2(200.0f, 100.0f);
           
        }

        private void OnEnable()
        {
            _window = this;
            // Subscribe to the editor scene redraw event
            SceneView.duringSceneGui += OnSceneGUI;
            // Restore the operation of the tool responsible for switching control tabs
            WindowTabs.Restore();
            // Subscribe to the tab switching event
            WindowTabs.OnUpdateTab += OnUpdateTab;
        }

        private void OnDisable()
        {
            WindowTabs.OnUpdateTab -= OnUpdateTab;
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        /// <summary>
        /// Handles the tab switching event
        /// </summary>
        private void OnUpdateTab(Tab tab)
        {
            if (tab == Tab.None) MapEditorLoader.Destroy();
            else if (tab != Tab.Setting) MapEditorLoader.CreateOrGetMapLoader();
        }

        private void OnGUI() // Drawing the editor window
        {
            // Draw tab buttons
            WindowTabs.Draw();

            switch (WindowTabs.ActiveTab)
            {
                case Tab.Terrain: // Drawing the content of tabs
                    TabTerrain.Draw();
                    break;
                case Tab.Objects:
                    ObjectTab.Draw();
                    break;
                case Tab.Export:
                    TabExport.Draw();
                    break;
                case Tab.Setting:
                    TabSetting.Draw();
                    break;
            }
        }

        /// <summary>
        /// Handles the scene update event. Drawing tools on the scene.
        /// </summary>
        private void OnSceneGUI(SceneView sceneView)
        {
            MapEditorLoader.OnSceneGUI();
            if (WindowTabs.ActiveTab == Tab.Objects)
            {
                _window.Repaint();
            }
        }
    }

}