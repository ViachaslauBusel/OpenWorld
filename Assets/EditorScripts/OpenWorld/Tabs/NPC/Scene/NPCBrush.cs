#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tabs.NPCs
{
    internal class NPCBrush
    {
        private static GameObject m_brush;
        private static int m_brushNPC_ID;



        public static void SceneGUI(SceneView sceneView)
        {

            //if (AttachNPC.SelectNPC_ID < 0)//Если нет выбранного НПЦ в списке
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
            //    //Если кисть не создана или ИД созданаго и выбраного НПЦ не совпадают
            //    if (m_brush == null || m_brushNPC_ID != AttachNPC.SelectNPC_ID)
            //    {
            //        Dispose();
            //        m_brushNPC_ID = AttachNPC.SelectNPC_ID;


            //            GameObject prefab = TabSetting.NPCs[m_brushNPC_ID].Prefab.Object;
            //            if (prefab == null) return;
            //            m_brush = GameObject.Instantiate(prefab);
            //            m_brush.name = "BrushNPC";
            //            //  brush.hideFlags = HideFlags.HideAndDontSave;
 
            //    }

            //    m_brush.transform.position = hit.point;

            //    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)//Если была нажата левая кнопка мыши
            //    {


            //        AttachNPC.Attach(m_brush);
            //        m_brush = null;
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
            if (m_brush != null) GameObject.DestroyImmediate(m_brush);
            m_brush = null;
        }
    }
}
#endif