#if UNITY_EDITOR
using System.IO;
using UnityEngine;

namespace OpenWorldEditor.Tabs.Res
{
    public static class ResourceExport
    {
        //public static void Export(WorldResourcesList worldResourcesList, ResourcesList resourcesList)
        //{
        //    using (BinaryWriter stream_out = new BinaryWriter(File.Open(@"export/resources.dat", FileMode.Create)))
        //    {
        //        stream_out.Write(worldResourcesList.container.Count);
        //        foreach (WorldResource worldResource in worldResourcesList.container)
        //        {
        //            Resource resource = resourcesList.FindByID(worldResource.id);
        //            if (resource == null) { Debug.LogError($"Не удалось экспортировать ресурс:{worldResource.id}"); return; }
        //            stream_out.Write(resource.ID);//idskin
        //            stream_out.Write((int)resource.Weapon);//тип оружия которым можно добывать ресурс

        //            stream_out.Write(resource.CountStages);
        //           for(int i=0; i < resource.CountStages; i++)
        //            {
        //                ResourceStage stage = resource.GetStage(i);
        //                stream_out.Write(stage.HP);
        //                stream_out.Write((int)(stage.Spawn.Min * 1000.0f));
        //                stream_out.Write((int)(stage.Spawn.Max * 1000.0f));
        //                stream_out.Write(stage.DropCount);
        //              for(int d=0; d<stage.DropCount; d++)
        //                {
        //                    AttachableDropItem item = stage.GetDrop(d);
        //                    stream_out.Write(item.ID);
        //                    stream_out.Write(item.Chance);
        //                    stream_out.Write(item.Count.Min);
        //                    stream_out.Write(item.Count.Max);
        //                }
        //            }
        //            stream_out.Write(worldResource.point.x);
        //            stream_out.Write(worldResource.point.y);
        //            stream_out.Write(worldResource.point.z);
        //            stream_out.Write(worldResource.radius);
        //        }
        //    }
        //}
    }
}
#endif