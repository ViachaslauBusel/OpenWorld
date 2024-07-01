#if UNITY_EDITOR

using OpenWorld.Tabs.NPC;
using OpenWorldEditor.SceneWindow;
using OpenWorldEditor.Tabs.Monsters;
using OpenWorldEditor.Tabs.NPCs;
using OpenWorldEditor.Tabs.Res;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor
{
    /// <summary>
    /// "Open World" control / editing window
    /// </summary>
    public class WindowOpenWorld : EditorWindow
    {
        private static WindowOpenWorld m_window;

        [MenuItem("Window/Open World")]
        public static void ShowWindow()
        {
            m_window = EditorWindow.GetWindow<WindowOpenWorld>(false, "Open World");
            m_window.minSize = new Vector2(200.0f, 100.0f);
           
        }

        private void OnEnable()
        {
            m_window = this;
            //Подписаться на событие перерисовки сцены редактора
            SceneView.duringSceneGui += this.OnSceneGUI;
            //Восстановить работу инструмента отвечающего за переключение вкладок управление
            WindowTabs.Restore();
            //Подписаться на событие переключение вкладок
            WindowTabs.updateTab += UpdateTab;
        }
        private void OnDisable()
        {
            WindowTabs.updateTab -= UpdateTab;
            SceneView.duringSceneGui -= this.OnSceneGUI;
            Dispose();
        }
        public static void Refresh() => m_window?.Repaint();
        private void Dispose()
        {
            SceneMonsters.Stop();
            SceneResources.Stop();
            SceneNPCs.Stop();
        }
        /// <summary>
        /// Обработка события переключение вкладок
        /// </summary>
        public void UpdateTab(Tab tab)
        {
            if (tab == Tab.None) WorldLoader.Destroy();
            else if (tab != Tab.Setting) WorldLoader.LoadMap();

            //Поменялся инструмент, удалить используемые ресурсы на сцене
            Dispose();
            switch (tab)
            {
                case Tab.Terrain:
                    break;
                case Tab.Monsters:
                    SceneMonsters.Start();
                    break;
                case Tab.NPC:
                    SceneNPCs.Start();
                    break;
                case Tab.Resources:
                    SceneResources.Start();
                    break;
                case Tab.Machine:
                case Tab.Objects:
                    break;
                
                case Tab.Export:
                case Tab.Setting:
                    break;

                case Tab.None:
                    break;
            }
        }
        private void OnGUI()//Отрисовка Окна редактора
        { 

            //Отрисовать кнопки вкладок
            WindowTabs.Draw();


            switch (WindowTabs.ActiveTab)
            {
                case Tab.Terrain:// Отрисовка содержимого вкладок
                    TabTerrain.Draw();
                    break;
                case Tab.Monsters:
                    TabMonsters.Draw();
                    break;
                case Tab.NPC:
                    TabNPC.Draw();
                    break;
                case Tab.Resources:
                    TabResources.Draw();
                    break;
                case Tab.Machine:
                    break;
                case Tab.Objects:
                    TabObjects.Draw();
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
        /// Обработка события обновления сцены. Отрисовка инструментов на сцене
        /// </summary>
        private void OnSceneGUI(SceneView sceneView)
        {
            WorldLoader.OnSceneGUI();
            switch (WindowTabs.ActiveTab)
            {
                case Tab.None://Редактирование отключено
                    return;
                case Tab.Terrain:// Отрисовать инструменты для Редактирование террейна
                    break;
                case Tab.Monsters:
                    SceneMonsters.OnSceneGUI(sceneView);
                    break;
                case Tab.NPC:
                    SceneNPCs.OnSceneGUI(sceneView);
                    break;
                case Tab.Resources:
                    SceneResources.OnSceneGUI(sceneView);
                    break;
                case Tab.Machine:
                    break;
                case Tab.Objects:
                    m_window.Repaint();
                    break;
                case Tab.Export:
                    break;
                case Tab.Setting:
                    break;
            }
        }

    }

}
#endif