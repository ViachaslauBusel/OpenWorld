#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorldEditor.Tools
{
    //public class Pages<T> where T : EditableObject
    //{
    //    private const int OBJECTS_ON_PAGE = 20;
    //    /// <summary>Начальный и конечный индекс в списке walkers для отрисовки на странице</summary>
    //    private int m_startPage = 0, m_endPage = 20;
    //    private List<T> m_objects;
    //    /// <summary>ID - выделенного объекта из списка</summary>
    //    public int SelectID { get; set; } = -1;
    //    public Pages(List<T> objects)
    //    {
    //        this.m_objects = objects;
    //    }

    //    public void Draw()
    //    {
    //        //Отрисовка списка обьектов >>>
    //        GUILayout.BeginHorizontal();
    //        GUILayout.FlexibleSpace();
    //        GUILayout.Label($"{Math.Ceiling(m_objects.Count / (float)m_endPage)}/{Math.Ceiling(m_objects.Count / (float)OBJECTS_ON_PAGE)}");
    //        GUILayout.FlexibleSpace();
    //        GUILayout.EndHorizontal();
    //        GUILayout.Space(20.0f);
    //        for (int i = m_startPage; i < m_objects.Count && i < m_endPage; i++)
    //        {
    //            GUILayout.BeginHorizontal();
    //            GUILayout.Label(m_objects[i].Name + ":" + m_objects[i].ID);
    //            GUI.enabled = i != SelectID;

    //            if (GUILayout.Button("Select", GUILayout.Height(20.0f), GUILayout.Width(50.0f)))
    //            {
    //                SelectID = i;
    //            }

    //            GUI.enabled = true;
    //            GUILayout.EndHorizontal();
    //            GUILayout.Space(5.0f);
    //        }

    //        GUILayout.BeginHorizontal();
    //        {
    //            GUI.enabled = m_startPage > 0;
    //            if (GUILayout.Button("Previous"))
    //            {
    //                m_startPage -= OBJECTS_ON_PAGE;
    //                m_endPage -= OBJECTS_ON_PAGE;
    //            }
    //            GUI.enabled = m_endPage < m_objects.Count;
    //            if (GUILayout.Button("Next"))
    //            {
    //                m_startPage += OBJECTS_ON_PAGE;
    //                m_endPage += OBJECTS_ON_PAGE;
    //            }
    //            GUI.enabled = true;
    //        }
    //        GUILayout.EndHorizontal();
    //        //Отрисовка списка обьектов <<<
    //    }
    //}
}
#endif