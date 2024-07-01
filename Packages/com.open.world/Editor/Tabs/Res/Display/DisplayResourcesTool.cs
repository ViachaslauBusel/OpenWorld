#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tabs.Res
{
    public class DisplayResourcesTool
    {
        private static int selectId = 0;

        public static void Draw()
        {

            //foreach (ResourceDrawGizmos resourceDraw in SceneResources.ResourcesLoader.Resources)
            //{
            //    GUILayout.Space(10.0f);
            //    GUILayout.BeginHorizontal();
            //    Resource _resource = TabSetting.Resources.FindByID(resourceDraw.worldResource.id);
            //    GUILayout.Label(_resource.Name + ": " + _resource.ID);

            //    GUI.enabled = selectId != resourceDraw.worldResource.GetHashCode();
            //    if (GUILayout.Button("Select", GUILayout.Width(60.0f)))
            //    {
            //        Selection.activeObject = resourceDraw.gameObject;
            //        selectId = resourceDraw.worldResource.GetHashCode();
            //        return;
            //    }
            //    GUI.enabled = true;

            //    if (GUILayout.Button("Delet", GUILayout.Width(60.0f)))
            //    {
            //        TabSetting.WorldResources.Remove(resourceDraw.worldResource);//Удалить монстра из списка монстров карты
            //        SceneResources.Update();//Обновить монстров на сцене
            //        return;
            //    }
            //    GUILayout.EndHorizontal();
            //}
        }
    }
}
#endif