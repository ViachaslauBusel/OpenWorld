#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tabs.Monsters.Display
{
    public class DisplayMonsterTool
    {

        private static int selectId = 0;

        public static void Draw()
        {

            //foreach(MonsterDrawGizmos monsterDraw in SceneMonsters.MonstersLoader.Monsters)
            //{
            //    GUILayout.Space(10.0f);
            //    GUILayout.BeginHorizontal();
            //    Monster _monster = TabSetting.MonstersList.FindByID(monsterDraw.worldMonster.ID);
            //    GUILayout.Label(_monster.Name + ": " + _monster.ID);

            //    GUI.enabled = selectId != monsterDraw.worldMonster.GetHashCode();
            //    if (GUILayout.Button("Select", GUILayout.Width(60.0f)))
            //    {
            //        Selection.activeObject = monsterDraw.gameObject;
            //        selectId = monsterDraw.worldMonster.GetHashCode();
            //        return;
            //    }
            //    GUI.enabled = true;

            //    if (GUILayout.Button("Delet", GUILayout.Width(60.0f)))
            //    {
            //        TabSetting.WorldMonsterList.Remove(monsterDraw.worldMonster);//Удалить монстра из списка монстров карты
            //        SceneMonsters.Update();//Обновить монстров на сцене
            //        return;
            //    }
            //    GUILayout.EndHorizontal();
            //}
        }
    }
}
#endif