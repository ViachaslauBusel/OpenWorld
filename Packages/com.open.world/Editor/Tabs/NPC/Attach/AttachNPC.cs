#if UNITY_EDITOR
using OpenWorldEditor;
using OpenWorldEditor.Tabs.NPCs;
using OpenWorldEditor.Tools.SceneWindow;
using UnityEditor;
using UnityEngine;

public class AttachNPC : MonoBehaviour
{
    /// <summary>Список доступных НПЦ для добовления на карту</summary>
   // private static Pages<NPC> m_pages = new Pages<NPC>(TabSetting.NPCs.Container);
    private static GameObject m_attachNPC;
    private static SpawnRadius m_drawGizmos;
    private static float m_rotation;


    //public static int SelectNPC_ID => m_pages.SelectID;
    public static void Attach(GameObject value)
    {
        m_drawGizmos = value.AddComponent<SpawnRadius>();//скрипт для отрисовки радиуса спавна 
        m_drawGizmos.Radius = 1.0f;
        m_attachNPC = value;
    }

    private static void Attach()
    {

        //Selection.activeObject = m_attachNPC;

        //GUILayout.Space(20.0f);
        //m_rotation = EditorGUILayout.Slider("Rotation: ", m_rotation, 0, 360);
        //m_attachNPC.transform.rotation = Quaternion.Euler(0.0f, m_rotation, 0.0f);


        //if (GUILayout.Button("Закрепить"))
        //{
        //    TabSetting.WorldNPCs.Add(new WorldNPC(TabSetting.NPCs[SelectNPC_ID].ID, m_attachNPC.transform.position, m_rotation));
        //    AssetDatabase.Refresh();
        //    EditorUtility.SetDirty(TabSetting.WorldMonsterList);
        //    AssetDatabase.SaveAssets();
        //    GameObject.DestroyImmediate(m_attachNPC);
        //    m_attachNPC = null;
        //    //Обновить Прорисованных монстров на карте
        //    SceneNPCs.Update();
        //    m_pages.SelectID = -1;
        //}

        //if (GUILayout.Button("отмена"))
        //{
        //    m_attachNPC = null;
        //    m_pages.SelectID = -1;
        //}

    }


    public static void Draw()
    {
        GUILayout.Space(20.0f);
        if (m_attachNPC != null)//Подтверждение добавление монстра на карту
        {
            Attach();
            return;
        }

      //  m_pages.Draw();
    }
}
#endif