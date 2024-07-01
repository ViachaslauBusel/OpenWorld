using Bundles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DATA
{
    /// <summary>
    /// Оболочка для assets, используется для загрузки из бандлов
    /// </summary>
    [Serializable]
    public class Prefab<T> :  ISerializationCallbackReceiver, IPrefab where T : UnityEngine.Object
    {
        [SerializeField] string m_guid;
        [SerializeField] string m_path;
        [SerializeField] string m_bundleName;
        [SerializeField] int m_bundleKEY;


        public Prefab() { }
       
        //-----------------------------------------------------------
        public string GUID => m_guid;
        /// <summary> Путь к файлу(UnityEngine.Object) </summary>
        public string Path => m_path;
        /// <summary> Имя бандла </summary>
        public string Bundle => m_bundleName;
        /// <summary> Хэшкод имени бандла, для поиска в  Dictionary </summary>
        public int BundleKEY => m_bundleKEY;

        //-----------------------------------------------------------


#if UNITY_EDITOR
        public Prefab(T asset) { m_asset = asset; }
        private T m_asset;
        //-----------------------------------------------------------
        /// <summary>Возвращает превью ассета(доступно только в редакторе!)</summary>
        public Texture2D Preview => AssetPreview.GetAssetPreview(Object);

        public T Object
        {
            get
            {
                if (m_asset == null && !string.IsNullOrEmpty(m_guid))
                {
                    string path = AssetDatabase.GUIDToAssetPath(m_guid);
                    if (!string.IsNullOrEmpty(path))
                    {
                        m_asset = AssetDatabase.LoadAssetAtPath<T>(path);
                    }
                }
                return m_asset;
            }
        }

        public void Draw(string label)
        {
            m_asset = EditorGUILayout.ObjectField(label, Object, typeof(T), false) as T;
        }

        /// <summary>
        /// Обновить все поля по GUID. Если изменения были возвращает true
        /// </summary>
        public bool Refresh()
        {
            m_asset = Object;

            string path = AssetDatabase.GetAssetPath(m_asset);
            string bundleName = AssetImporter.GetAtPath(m_path).assetBundleName;

            if(m_path != path || bundleName != m_bundleName)
            {
                m_path = path;
                m_bundleName = bundleName;
                if (string.IsNullOrEmpty(m_bundleName))
                {
                    Debug.LogError($"Could not find bundle for {m_path}");
                    m_bundleKEY = 0;
                    return true;
                }
                m_bundleKEY = m_bundleName.ToLower().GetHashCode();
                return true;
            }
            return false;
        }





        //-----------------------------------------------------------
#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            m_asset = Object;
            if (m_asset == null || !AssetDatabase.Contains(m_asset))
            {
                m_guid = "";
                m_path = "";
                m_asset = null;
                m_bundleName = "";
                m_bundleKEY = 0;
                return;
            }
            m_path = AssetDatabase.GetAssetPath(m_asset);
            m_guid = AssetDatabase.AssetPathToGUID(m_path);
            m_bundleName = AssetImporter.GetAtPath(m_path).assetBundleName;
            if (string.IsNullOrEmpty(m_bundleName))
            {
                Debug.LogError($"Could not find bundle for {m_path}");
                m_bundleKEY = 0;
                return;
            }
            m_bundleKEY = m_bundleName.ToLower().GetHashCode();
#endif
        }

        public void OnAfterDeserialize()
        {

        }
    }
}
