#if UNITY_EDITOR
using OpenWorld.SceneWindow;
using System.Collections.Generic;
using UnityEngine;
namespace OpenWorldEditor.Tabs.Monsters.Display
{
    public class MonstersLoader : ObjectsLoader
    {


        public List<MonsterDrawGizmos> Monsters { get; private set; } = new List<MonsterDrawGizmos>();

        public override void Dispose()
        {
            foreach (MonsterDrawGizmos obj in Monsters) GameObject.DestroyImmediate(obj.gameObject);
            Monsters.Clear();
        }

        public override void CalculeteVisibleNPCs()
        {
            Dispose();
            //foreach (WorldMonster worldMonster in TabSetting.WorldMonsterList.worldMonsters)
            //{
            //    if (IsMapArea(worldMonster.Point))
            //    {
            //        GameObject prefabMonster = TabSetting.MonstersList.FindByID(worldMonster.ID).Prefab.Object;
            //        if (prefabMonster != null)
            //        {
            //            GameObject instantiateMonster = GameObject.Instantiate(prefabMonster);
            //            instantiateMonster.transform.position = worldMonster.Point;
            //            MonsterDrawGizmos monsterDraw = instantiateMonster.AddComponent<MonsterDrawGizmos>();
            //            monsterDraw.Radius = worldMonster.Radius;
            //            monsterDraw.worldMonster = worldMonster;
            //            Monsters.Add(monsterDraw);
            //        }
            //    }
            //}
        }
    }
}
#endif