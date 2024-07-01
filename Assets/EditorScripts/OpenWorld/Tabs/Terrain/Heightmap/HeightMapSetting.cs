#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorldEditor.Tools.Terrain
{
    public struct HeightMapSetting
    {
        public Texture2D texture;
        public float minHeight;
        public float maxHeight;
    }
}
#endif