#if UNITY_EDITOR
using System.IO;
using UnityEngine;

public static class NPCsExport
{
    //public static void Export(WorldNPCsList worldNPCsList, NPCList npcsList)
    //{
    //    using (BinaryWriter stream_out = new BinaryWriter(File.Open(@"export/NPCs.dat", FileMode.Create)))
    //    {
    //        stream_out.Write(worldNPCsList.container.Count);
    //        foreach (WorldNPC worldNPC in worldNPCsList.container)
    //        {
    //            NPC npc = npcsList.FindByID(worldNPC.ID);
    //            if (npc == null) { Debug.LogError($"Не удалось экспортировать NPC:{worldNPC.ID}"); stream_out.Write(0); continue; }
    //            stream_out.Write(npc.ID);//idskin

    //            stream_out.Write(worldNPC.Point.x);
    //            stream_out.Write(worldNPC.Point.y);
    //            stream_out.Write(worldNPC.Point.z);
    //            stream_out.Write(worldNPC.Rotation);
    //        }
    //    }
    //}
}
#endif