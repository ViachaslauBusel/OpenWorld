
#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tabs.Setting
{
    public class Cell<T> : Container, ICell where T: Object
    {
        private T obj;
        public bool IsSave { get; private set; } = true;
        private string key;

        public Cell(string key)
        {
            this.key = key;
            string guid = PlayerPrefs.GetString(key);
            string path = AssetDatabase.GUIDToAssetPath(guid);
            obj = AssetDatabase.LoadAssetAtPath<T>(path);
            Cells.Add(this);
        }
        public void Save()
        {
            string pathToPrefab = AssetDatabase.GetAssetPath(obj);

             PlayerPrefs.SetString(key, AssetDatabase.AssetPathToGUID(pathToPrefab)); 
            IsSave = true;
        }
        public T Object { get { return obj; } set { if (obj != value) { IsSave = false; } obj = value; } }
    }
}
#endif