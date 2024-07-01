#if UNITY_EDITOR
using OpenWorld.SceneWindow;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorldEditor.Tabs.NPCs
{
    public class NPCsLoader : ObjectsLoader
    {

        public List<NPCDrawGizmos> NPCs { get; private set; } = new List<NPCDrawGizmos>();

        public override void Dispose()
        {
            foreach (NPCDrawGizmos obj in NPCs) GameObject.DestroyImmediate(obj.gameObject);
            NPCs.Clear();
        }

        public override void CalculeteVisibleNPCs()
        {
            Dispose();
            //foreach (WorldNPC worldNPC in TabSetting.WorldNPCs.container)
            //{
            //    if (IsMapArea(worldNPC.Point))
            //    {
            //        GameObject prefabNPC = TabSetting.NPCs.FindByID(worldNPC.ID).Prefab.Object;
            //        if (prefabNPC != null)
            //        {
            //            GameObject npc = GameObject.Instantiate(prefabNPC);
            //            npc.transform.position = worldNPC.Point;
            //            npc.transform.rotation = Quaternion.Euler(0.0f, worldNPC.Rotation, 0.0f);
            //            NPCDrawGizmos npcGizmo = npc.AddComponent<NPCDrawGizmos>();
            //            npcGizmo.Radius = 1.0f;
            //            npcGizmo.worldNPC = worldNPC;
            //            NPCs.Add(npcGizmo);
            //        }
            //        else Debug.Log("пРЕБАФ НЕ  НАЙДКН");
            //    }
            //}

        }
    }
}
#endif