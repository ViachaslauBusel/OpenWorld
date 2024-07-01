using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorld
{
    public static class GraphicsQualitySettings
    {
        private static int _areaVisible;
        private static int _basemapDistance = 50;
        private static int _quality;
        private static float _detailDistance;
        private static float _detailDensity;
        static GraphicsQualitySettings()
        {
            _areaVisible = PlayerPrefs.GetInt("areaVisible", 1);
            _basemapDistance = PlayerPrefs.GetInt("basemapDistance", 50);
            _quality = PlayerPrefs.GetInt("QualityLevel", 1);
            _detailDistance = PlayerPrefs.GetFloat("DetailDistance", 80.0f);
            _detailDensity = PlayerPrefs.GetFloat("DetailDensity", 1.0f);
        }


        public static float DetailDistance
        {
            set
            {
                PlayerPrefs.SetFloat("DetailDistance", value);
                _detailDistance = value;
            }
            get
            {
                return _detailDistance;
            }
        }
        public static float DetailDensity
        {
            set
            {
                PlayerPrefs.SetFloat("DetailDensity", value);
                _detailDensity = value;
            }
            get
            {
                return _detailDensity;
            }
        }
        public static int Qulity
        {
            set
            {
                if (value < 0) value = 0;
                if (value >= QualitySettings.names.Length) value = QualitySettings.names.Length;
                QualitySettings.SetQualityLevel(value);
                PlayerPrefs.SetInt("QualityLevel", value);
                _quality = value;
            }
            get
            {
                return _quality;
            }
        }
        public static int basemapDistance
        {
            set
            {
                PlayerPrefs.SetInt("basemapDistance", value);
                _basemapDistance = value;
            }
            get
            {
                return _basemapDistance;
            }
        }
        public static int AreaVisible
        {
            set
            {
                value = Mathf.Clamp(value, 1, 40);
                PlayerPrefs.SetInt("areaVisible", value);
                _areaVisible = value;
            }
            get
            {
                return _areaVisible;
            }
        }

    }
}