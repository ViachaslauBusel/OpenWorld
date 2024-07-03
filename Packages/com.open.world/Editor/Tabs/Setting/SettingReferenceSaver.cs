using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tabs.Setting
{
    /// <summary>
    /// Manages the persistence of a resource reference using PlayerPrefs.
    /// </summary>
    /// <typeparam name="T">Type of the resource object.</typeparam>
    public class SettingReferenceSaver<T> : SettingsRegistry, ISetting where T : Object
    {
        private T _resourceObject;
        private readonly string _key;

        public bool IsSaved { get; private set; } = true;

        /// <summary>
        /// Initializes a new instance of the ResourceReferenceSaver class.
        /// </summary>
        /// <param name="key">The PlayerPrefs key used for saving the resource reference.</param>
        public SettingReferenceSaver(string key)
        {
            _key = key;
            LoadResource();
            Settings.Add(this);
        }

        /// <summary>
        /// Gets or sets the resource object. Marks the saver as unsaved if the object is changed.
        /// </summary>
        public T Object
        {
            get => _resourceObject;
            set
            {
                if (_resourceObject != value)
                {
                    _resourceObject = value;
                    IsSaved = false;
                }
            }
        }

        /// <summary>
        /// Saves the resource reference to PlayerPrefs.
        /// </summary>
        public void Save()
        {
            string pathToResource = AssetDatabase.GetAssetPath(_resourceObject);
            PlayerPrefs.SetString(_key, AssetDatabase.AssetPathToGUID(pathToResource));
            IsSaved = true;
        }


        /// <summary>
        /// Loads the resource object from PlayerPrefs.
        /// </summary>
        private void LoadResource()
        {
            string guid = PlayerPrefs.GetString(_key);
            string path = AssetDatabase.GUIDToAssetPath(guid);
            _resourceObject = AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}