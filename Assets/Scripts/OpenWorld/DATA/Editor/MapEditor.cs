
using OpenWorld.DATA;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor
{

    [CustomEditor(typeof(Map))]
    public class MapEditor : Editor
    {


        public override void OnInspectorGUI()
        {

            Map map = target as Map;
            map.DrawGUI();
        }

      
    }
}
