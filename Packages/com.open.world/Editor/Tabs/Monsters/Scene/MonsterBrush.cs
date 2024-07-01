#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tabs.Monsters.Attach
{

    public class MonsterBrush
    {
        private static GameObject brush;
        private static int selectedMonster;
       


        public static void SceneGUI(SceneView sceneView)
        {

            //if (AttachMonsterTool.SelectMonster < 0)//Если режим добавление монстров на карту не включен
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
            //    if (brush == null || selectedMonster != AttachMonsterTool.SelectMonster)
            //    {
            //        Dispose();
            //        selectedMonster = AttachMonsterTool.SelectMonster;

            //        if (brush == null)
            //        {
            //            GameObject prefab = TabSetting.MonstersList[selectedMonster].Prefab.Object;
            //            if (prefab == null) return;
            //            brush = GameObject.Instantiate(prefab);
            //            brush.name = "BrushMonster";
            //          //  brush.hideFlags = HideFlags.HideAndDontSave;
            //        }
            //    }

            //    brush.transform.position = hit.point;

            //    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)//Если была нажата левая кнопка мыши
            //    {

                   
            //        AttachMonsterTool.AtachMonster(brush);
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