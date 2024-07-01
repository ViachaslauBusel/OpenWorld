#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenWorldEditor.Tools.Tabs
{
    public enum TabADTool
    { None, Attach, Display }
    /// <summary>
    /// ����������� ��� ��������� ������� � ����������� � ���������� ����������� ��������
    /// </summary>
    public class TabAD
    {
        public TabADTool ActiveTool { get; private set; } = TabADTool.Display;
        public event Action<TabADTool> update;

        /// <summary>
        /// ��������� ������ ������������ �����������
        /// </summary>
        public void DrawToolsIcon()
        {
            GUILayout.Space(15.0f);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            //���������� ������� �� �����
            GUI.enabled = ActiveTool != TabADTool.Attach;
            if (GUILayout.Button(EditorGUIUtility.IconContent("Animation.AddEvent"), EditorStyles.miniButtonLeft, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {
                ActiveTool = TabADTool.Attach;
                update?.Invoke(ActiveTool);
            }

            //�������� ������ �����
            GUI.enabled = ActiveTool != TabADTool.Display;
            if (GUILayout.Button(EditorGUIUtility.IconContent("UnityLogo"), EditorStyles.miniButtonRight, GUILayout.Width(25.0f), GUILayout.Height(25.0f)))
            {

                ActiveTool = TabADTool.Display;
                update?.Invoke(ActiveTool);
            }


            GUI.enabled = true;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
      
    }
}
#endif