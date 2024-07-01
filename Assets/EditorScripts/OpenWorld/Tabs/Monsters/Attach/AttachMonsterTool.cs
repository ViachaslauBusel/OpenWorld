#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tabs.Monsters.Attach
{

    public class AttachMonsterTool
    {
       // private static Pages<Monster> pages = new Pages<Monster>(TabSetting.MonstersList.Container);
        private static GameObject _atachMonster;
        private static MonsterDrawGizmos monsterDrawGizmos;


      //  public static int SelectMonster => pages.SelectID;
        public static void AtachMonster(GameObject value)
        {
            monsterDrawGizmos = value.AddComponent<MonsterDrawGizmos>();//скрипт для отрисовки радиуса спавна 
            monsterDrawGizmos.Radius = 1.0f;
            _atachMonster = value;
        }

        private static void EditMonster()
        {

            //Selection.activeObject = _atachMonster;

            //GUILayout.Space(20.0f);
            //monsterDrawGizmos.Radius = EditorGUILayout.Slider("Radius: ", monsterDrawGizmos.Radius, 1, 20);


            //if (GUILayout.Button("Закрепить"))
            //{
            //    TabSetting.WorldMonsterList.Add(new WorldMonster(TabSetting.MonstersList[SelectMonster].ID, _atachMonster.transform.position, monsterDrawGizmos.Radius));
            //    AssetDatabase.Refresh();
            //    EditorUtility.SetDirty(TabSetting.WorldMonsterList);
            //    AssetDatabase.SaveAssets();
            //    GameObject.DestroyImmediate(_atachMonster);
            //    _atachMonster = null;
            //    //Обновить Прорисованных монстров на карте
            //    SceneMonsters.Update();
            //    pages.SelectID = -1;
            //}

            //if (GUILayout.Button("отмена"))
            //{
            //    _atachMonster = null;
            //    pages.SelectID = -1;
            //}

        }
    

        public static void Draw()
        {
            GUILayout.Space(20.0f);
            if (_atachMonster != null)//Подтверждение добавление монстра на карту
            {
                EditMonster();
                return;
            }

         //   pages.Draw();
        }
    }
}
#endif
