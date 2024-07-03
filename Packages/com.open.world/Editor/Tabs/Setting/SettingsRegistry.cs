using System.Collections.Generic;

namespace OpenWorldEditor.Tabs.Setting
{
    /// <summary>
    /// This class serves as a container for all objects implementing the ISetting interface.
    /// </summary>
    public abstract class SettingsRegistry
    {
        /// <summary>
        /// A protected static list containing all registered settings.
        /// </summary>
        public static List<ISetting> Settings { get; protected set; } = new List<ISetting>();
    }
}