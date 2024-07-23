using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenWorld.DataStore
{
    public class EntityDataStore<T> : ScriptableObject, ISerializationCallbackReceiver where T : class
    {
        [SerializeField, HideInInspector]
        private List<int> _entityIds;
        [SerializeField, HideInInspector]
        private List<T> _entityData;
        private Dictionary<int, T> _data = new();

        public T GetData(int entityID)
        {
            if (_data.TryGetValue(entityID, out T data))
            {
                return data;
            }

            return null;
        }

        public void AddOrUpdateData(int entityID, T data)
        {
            _data[entityID] = data;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif
        }

        internal void RemoveData(int entityID)
        {
            if (_data.ContainsKey(entityID))
            {
                _data.Remove(entityID);
            }
        }

        public void OnBeforeSerialize()
        {
            _entityIds.Clear();
            _entityData.Clear();

            foreach (var kvp in _data)
            {
                _entityIds.Add(kvp.Key);
                _entityData.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            _data = new Dictionary<int, T>();

            for (int i = 0; i < _entityIds.Count && i < _entityData.Count; i++)
            {
                _data[_entityIds[i]] = _entityData[i];
            }
        }
    }
}
