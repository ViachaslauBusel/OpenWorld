#if UNITY_EDITOR
using OpenWorldEditor;
using OpenWorldEditor.Tabs.NPCs;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DisplayNPC
{
    private static int selectId = 0;

    public static void Draw()
    {

        //foreach (NPCDrawGizmos npcGIZMO in SceneNPCs.NPCsLoader.NPCs)
        //{
        //    GUILayout.Space(10.0f);
        //    GUILayout.BeginHorizontal();
        //    NPC npcDATA = TabSetting.NPCs.FindByID(npcGIZMO.worldNPC.ID);
        //    GUILayout.Label(npcDATA.Name + ": " + npcDATA.ID);

        //    GUI.enabled = selectId != npcGIZMO.worldNPC.GetHashCode();
        //    if (GUILayout.Button("Select", GUILayout.Width(60.0f)))
        //    {
        //        Selection.activeObject = npcGIZMO.gameObject;
        //        selectId = npcGIZMO.worldNPC.GetHashCode();
        //        return;
        //    }
        //    GUI.enabled = true;

        //    if (GUILayout.Button("Delet", GUILayout.Width(60.0f)))
        //    {
        //        TabSetting.WorldNPCs.Remove(npcGIZMO.worldNPC);//Удалить монстра из списка монстров карты
        //        SceneNPCs.Update();//Обновить монстров на сцене
        //        return;
        //    }
        //    GUILayout.EndHorizontal();
        //}
    }
}
#endif