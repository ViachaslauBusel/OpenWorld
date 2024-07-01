#if UNITY_EDITOR
using OpenWorldEditor.Tools.SceneWindow;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tabs.Res
{
    public class AttachResourceTool
    {
      //  private static Pages<Resource> pages = new Pages<Resource>(TabSetting.Resources.Container);
        private static GameObject _atachResource;
        private static SpawnRadius drawGizmos;


      //  public static int SelectResource => pages.SelectID;
        public static void Atach(GameObject value)
        {
            drawGizmos = value.AddComponent<SpawnRadius>();//скрипт для отрисовки радиуса спавна 
            drawGizmos.Radius = 1.0f;
            _atachResource = value;
        }

        private static void EditResource()
        {

            Selection.activeObject = _atachResource;

            GUILayout.Space(20.0f);
            drawGizmos.Radius = EditorGUILayout.Slider("Radius: ", drawGizmos.Radius, 1, 20);


            //if (GUILayout.Button("Закрепить"))
            //{
            //    TabSetting.WorldResources.Add(new WorldResource(TabSetting.Resources[SelectResource].ID, _atachResource.transform.position, drawGizmos.Radius));
            //    AssetDatabase.Refresh();
            //    EditorUtility.SetDirty(TabSetting.WorldResources);
            //    AssetDatabase.SaveAssets();
            //    GameObject.DestroyImmediate(_atachResource);
            //    _atachResource = null;
            //    //Обновить Прорисованных монстров на карте
            //    SceneResources.Update();
            //    pages.SelectID = -1;
            //}

            //if (GUILayout.Button("отмена"))
            //{
            //    _atachResource = null;
            //    pages.SelectID = -1;
            //}

        }


        public static void Draw()
        {
            GUILayout.Space(20.0f);
            if (_atachResource != null)//Подтверждение добавление монстра на карту
            {
                EditResource();
                return;
            }

          //  pages.Draw();
        }
    }
}
#endif