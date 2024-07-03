using OpenWorldEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace OpenWorld.DATA
{
    [CustomEditor(typeof(MapSettings))]
    internal class MapSettingsEditor : Editor
    {
        private MapSettings _mapSettings;
        private string[] _layerNames;

        private void OnEnable()
        {
            _mapSettings = target as MapSettings;
            var settings = MapProjectSettings.GetOrCreateSettings();
            var layerNamesList = new List<string>(); 

            for (int i = 0; i < MapProjectSettings.MAX_LAYERS; i++)
            {
                string layerName = settings.GetLayer(i);
                if (!string.IsNullOrEmpty(layerName))
                {
                    layerNamesList.Add(layerName);
                }
            }

            _layerNames = layerNamesList.ToArray();
        }

        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            base.OnInspectorGUI();

            // Display the mask field and update the layer mask based on selection
            _mapSettings.ObjectLayerMask = (ObjectLayerMask)EditorGUILayout.MaskField("Load Object", (int)_mapSettings.ObjectLayerMask, _layerNames);

            //Display the area visible field
            _mapSettings.AreaVisible = EditorGUILayout.IntSlider("Area Visible", _mapSettings.AreaVisible, 1, 30);
        }
    }
}
