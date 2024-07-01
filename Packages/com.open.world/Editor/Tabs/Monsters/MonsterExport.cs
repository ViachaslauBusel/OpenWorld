#if UNITY_EDITOR
using System.IO;

namespace OpenWorldEditor.Tabs.Monsters
{
    public class MonsterExport
    {
        //public static void Export(WorldMonstersList worldMonstersList, MonstersList monstersList)
        //{
        //    using (BinaryWriter stream_out = new BinaryWriter(File.Open(@"export/monsterSpawnPoint.dat", FileMode.Create)))
        //    {
        //        stream_out.Write(worldMonstersList.worldMonsters.Count);
        //        foreach (WorldMonster worldmonster in worldMonstersList.worldMonsters)
        //        {
        //            stream_out.Write(worldmonster.ID);//idskin

        //            stream_out.Write(worldmonster.Point.x);
        //            stream_out.Write(worldmonster.Point.y);
        //            stream_out.Write(worldmonster.Point.z);
        //            stream_out.Write(worldmonster.Radius);
        //        }
        //    }
        //}
    }
}
#endif