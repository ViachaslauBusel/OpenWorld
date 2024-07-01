#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tabs.Res
{
    public class ResourceBrush
    {
        private static GameObject brush;
        private static int selectedResource = -1;



        public static void SceneGUI(SceneView sceneView)
        {

            //if (AttachResourceTool.SelectResource < 0)//Если режим добавление монстров на карту не включен
            //{
            //    Dispose();
            //    return;
            //}

            //RaycastHit hit;
            //Vector2 position = Event.current.mousePosition;
            //position.y = sceneView.camera.pixelHeight - position.y;
            //Ray ray = sceneView.camera.ScreenPointToRay(position);
            //int layerMask = 1 << LayerMask.NameToLayer("Terrain");

            //if (Physics.Raycast(ray, out hit, 1000.0f, layerMask))
            //{
            //    if (brush == null || selectedResource != AttachResourceTool.SelectResource)
            //    {
            //        Dispose();
            //        selectedResource = AttachResourceTool.SelectResource;

            //        if (brush == null)
            //        {
            //            GameObject prefab = TabSetting.Resources[selectedResource].GetStage(0)?.Prefab.Object;
            //            if (prefab == null) { Debug.LogError($"Prefab TabSetting.Resources[{selectedResource}] not found"); return; }
            //            brush = GameObject.Instantiate(prefab);
            //            brush.name = "BrushResource";
            //            //  brush.hideFlags = HideFlags.HideAndDontSave;
            //        }
            //    }

            //    brush.transform.position = hit.point;

            //    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)//Если была нажата левая кнопка мыши
            //    {


            //        AttachResourceTool.Atach(brush);
            //        brush = null;
            //        WindowOpenWorld.Refresh();
            //    }
            //}
            //else
            //{
            //    Dispose();
            //}
        }



        public static void Dispose()
        {
            if (brush != null) GameObject.DestroyImmediate(brush);
            brush = null;
        }
    }
}
#endif