using UnityEngine;

namespace OpenWorld.DataStore
{
    /// <summary>
    /// Manages the lifecycle of map entity data, ensuring synchronization with the world data registry.
    /// </summary>
    /// <typeparam name="T">Type of world data.</typeparam>
    [ExecuteInEditMode]
    public class MapEntityDataSync<T> : MapEntityIdentifier where T : class, new()
    {
        [SerializeField]
        private EntityDataStore<T> _worldDataRegistry;

        protected override void OnIdentifierInitialize()
        {
            T worldData = _worldDataRegistry.GetData(ID);
            if (worldData != null) LoadDataProperties(worldData);
        }

        protected virtual void LoadDataProperties(T data)
        {
        }

        protected virtual void SaveDataProperties(ref T data)
        {
        }

        protected override void OnIdentifierDestroy()
        {
            _worldDataRegistry.RemoveData(ID);
        }

        public void OnDestroy()
        {
            if (ID == 0) return;

            T data = new T();
            SaveDataProperties(ref data);
            _worldDataRegistry.AddOrUpdateData(ID, data);
        }
    }
}
