namespace OpenWorldEditor.Tabs.Setting
{
    /// <summary>
    /// Defines the contract for settings that can be saved.
    /// </summary>
    public interface ISetting
    {
        /// <summary>
        /// Gets a value indicating whether the setting is saved.
        /// </summary>
        bool IsSaved { get; }

        /// <summary>
        /// Saves the setting resource.
        /// </summary>
        void Save();
    }
}