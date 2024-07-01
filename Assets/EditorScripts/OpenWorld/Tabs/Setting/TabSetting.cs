#if UNITY_EDITOR
using OpenWorld.DATA;
using OpenWorldEditor.SceneWindow;
using OpenWorldEditor.Tabs.Setting;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor
{
    public static class TabSetting
    {
       
        private static int _areaViseble = 1;
        private static int dontSaveAreaVisible = 1;
        private static Cell<Map> map = new Cell<Map>("mapKEY");
        private static Cell<Material> waterMaterial = new Cell<Material>("waterMKEY");
        private static Cell<Material> terrainMaterial = new Cell<Material>("terrainMKEY");
   //     private static Cell<ItemsList> itemList = new Cell<ItemsList>("ItemsListKEY");
  //      private static Cell<BuildingsList> buildingsList = new Cell<BuildingsList>("BuildingsListKEY");
    //    private static Cell<RecipesList> recipesList = new Cell<RecipesList>("RecipesListKEY");
     //   private static Cell<SkillsList> skills = new Cell<SkillsList>("SkillsListtKEY");
     //   private static Cell<EffectsList> _effects = new Cell<EffectsList>("EffectsListKEY");
    //    private static Cell<MonstersList> monstersList = new Cell<MonstersList>("MonsterKEY");
  //      private static Cell<WorldMonstersList> worldMonstrList = new Cell<WorldMonstersList>("WorldMonsterKEY");
     //   private static Cell<ResourcesList> m_resourcesList = new Cell<ResourcesList>("ResourcesListKEY");
      //  private static Cell<WorldResourcesList> m_worldResourcesList = new Cell<WorldResourcesList>("WorldResourcesListKEY");
      //  private static Cell<NPCList> m_npcsList = new Cell<NPCList>("NPCListKEY");
      //  private static Cell<WorldNPCsList> m_worldNPCsList = new Cell<WorldNPCsList>("WorldNPCsListKEY");
      //  private static Cell<DialoguesList> m_dialogues= new Cell<DialoguesList>("DialoguesListKEY");
      //  private static Cell<QuestsList> m_quests = new Cell<QuestsList>("QuestsListKEY");



        public static int areaVisible => _areaViseble; 
        public static Map Map => map.Object;
        public static Material WaterMaterial => waterMaterial.Object;
        public static Material TerrainMaterial => terrainMaterial.Object;
        //public static ItemsList ItemsList => itemList.Object;
        //public static BuildingsList Buildings => buildingsList.Object;
        //public static RecipesList Recipes => recipesList.Object;
        //public static SkillsList Skills => skills.Object;
        //public static EffectsList Effects => _effects.Object;
        //public static MonstersList MonstersList => monstersList.Object; 
        //public static WorldMonstersList WorldMonsterList => worldMonstrList.Object;
        //public static ResourcesList Resources => m_resourcesList.Object;
        //public static WorldResourcesList WorldResources => m_worldResourcesList.Object;
        //public static NPCList NPCs => m_npcsList.Object;
        //public static WorldNPCsList WorldNPCs => m_worldNPCsList.Object;
        //public static DialoguesList Dialogues => m_dialogues.Object;
        //public static QuestsList Quests => m_quests.Object;


        private static Vector2 vSbarValue = Vector2.zero;

        static TabSetting()
        {
            _areaViseble = dontSaveAreaVisible = PlayerPrefs.GetInt("EditAreaVisible", 1);
        }

        public static void Draw()
        {
            vSbarValue = EditorGUILayout.BeginScrollView(vSbarValue, false, true);

            GUILayout.Space(15.0f);
            dontSaveAreaVisible = EditorGUILayout.IntSlider("Зона видимости: ", dontSaveAreaVisible, 1, 40);



            GUILayout.Space(15.0f);
            GUILayout.Label("Карта для редактирования"); GUILayout.Space(5.0f);
            map.Object = EditorGUILayout.ObjectField(map.Object, typeof(Map), false) as Map;

            GUILayout.Space(15.0f);
            GUILayout.Label("Water Material"); GUILayout.Space(5.0f);
            waterMaterial.Object = EditorGUILayout.ObjectField(waterMaterial.Object, typeof(Material), false) as Material;

            GUILayout.Space(15.0f);
            GUILayout.Label("Terrain Material"); GUILayout.Space(5.0f);
            terrainMaterial.Object = EditorGUILayout.ObjectField(terrainMaterial.Object, typeof(Material), false) as Material;

            //GUILayout.Space(15.0f);
            //     GUILayout.Label("Список предметов"); GUILayout.Space(5.0f);
            //     itemList.Object = EditorGUILayout.ObjectField(itemList.Object, typeof(ItemsList), false) as ItemsList;

            //GUILayout.Space(15.0f);
            //GUILayout.Label("Список строений"); GUILayout.Space(5.0f);
            //buildingsList.Object = EditorGUILayout.ObjectField(buildingsList.Object, typeof(BuildingsList), false) as BuildingsList;

            //GUILayout.Space(15.0f);
            //GUILayout.Label("Список рецептов"); GUILayout.Space(5.0f);
            //recipesList.Object = EditorGUILayout.ObjectField(recipesList.Object, typeof(RecipesList), false) as RecipesList;

            //GUILayout.Space(15.0f);
            //GUILayout.Label("Список умений"); GUILayout.Space(5.0f);
            //skills.Object = EditorGUILayout.ObjectField(skills.Object, typeof(SkillsList), false) as SkillsList;

            //GUILayout.Space(15.0f);
            //GUILayout.Label("Список эффектов"); GUILayout.Space(5.0f);
            //_effects.Object = EditorGUILayout.ObjectField(_effects.Object, typeof(EffectsList), false) as EffectsList;

            //GUILayout.Space(15.0f);
            //GUILayout.Label("Список монстров"); GUILayout.Space(5.0f);
            //monstersList.Object = EditorGUILayout.ObjectField(monstersList.Object, typeof(MonstersList), false) as MonstersList;

            //GUILayout.Space(15.0f);
            //     GUILayout.Label("Список монстров закрепленых на карте"); GUILayout.Space(5.0f);
            //     worldMonstrList.Object = EditorGUILayout.ObjectField(worldMonstrList.Object, typeof(WorldMonstersList), false) as WorldMonstersList;

            //GUILayout.Space(15.0f);
            //GUILayout.Label("Список ресурсов"); GUILayout.Space(5.0f);
            //m_resourcesList.Object = EditorGUILayout.ObjectField(m_resourcesList.Object, typeof(ResourcesList), false) as ResourcesList;

            //GUILayout.Space(15.0f);
            //GUILayout.Label("Список ресурсов закрепленых на карте"); GUILayout.Space(5.0f);
            //m_worldResourcesList.Object = EditorGUILayout.ObjectField(m_worldResourcesList.Object, typeof(WorldResourcesList), false) as WorldResourcesList;

            //GUILayout.Space(15.0f);
            //GUILayout.Label("Список NPC"); GUILayout.Space(5.0f);
            //m_npcsList.Object = EditorGUILayout.ObjectField(m_npcsList.Object, typeof(NPCList), false) as NPCList;

            //GUILayout.Space(15.0f);
            //GUILayout.Label("Список NPC закрепленых на карте"); GUILayout.Space(5.0f);
            //m_worldNPCsList.Object = EditorGUILayout.ObjectField(m_worldNPCsList.Object, typeof(WorldNPCsList), false) as WorldNPCsList;


            //GUILayout.Space(15.0f);
            //GUILayout.Label("Список диалогов"); GUILayout.Space(5.0f);
            //m_dialogues.Object = EditorGUILayout.ObjectField(m_dialogues.Object, typeof(DialoguesList), false) as DialoguesList;

            //GUILayout.Space(15.0f);
            //GUILayout.Label("Список квестов"); GUILayout.Space(5.0f);
            //m_quests.Object = EditorGUILayout.ObjectField(m_quests.Object, typeof(QuestsList), false) as QuestsList;



            GUILayout.Space(20.0f);
            GUI.enabled = dontSaveAreaVisible != _areaViseble || Container.Cells.Any((C) => !C.IsSave);

            if (GUILayout.Button("Save"))
            {
                _areaViseble = dontSaveAreaVisible;
                PlayerPrefs.SetInt("EditAreaVisible", _areaViseble);

                Container.Cells.ForEach((c) => c.Save());


                WorldLoader.Reload();
            }

            EditorGUILayout.EndScrollView();
            GUI.enabled = true;
        }

      
    }
}
#endif