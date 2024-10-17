using System.Collections.Generic;
using KW.Flags;
using UnityEditor;
using UnityEngine;

public class FlagEditor : OrderEditor
{
    protected NamedFlags m_namedFlags;

    protected NamedFlags NamedFlags
    {
        get
        {
            if (!m_namedFlags) { m_namedFlags = FindAnyObjectByType<NamedFlags>(); }
            return m_namedFlags;
        }
    }

    public static void FlagNamesField(SerializedProperty _property, GUIContent label, GUIContent nullLabel, List<string> objectList)
    {
        var objectNames = new List<GUIContent>();

        var selectedObject = _property.stringValue;

        int selectedIndex = -1; // Invalid index

        // First option in list is <None>
        objectNames.Add(nullLabel);
        if (selectedObject == null)
        {
            selectedIndex = 0;
        }

        for (int i = 0; i < objectList.Count; ++i)
        {
            if (objectList[i] == null) { continue; }
            objectNames.Add(new GUIContent(objectList[i]));

            if (selectedObject == objectList[i])
            {
                selectedIndex = i + 1;
            }
        }
        if (selectedIndex < 0) { selectedIndex = 0; }

        string result;

        selectedIndex = EditorGUILayout.Popup(label, selectedIndex, objectNames.ToArray());

        result = selectedIndex > 0 ? objectList[selectedIndex - 1] : null;

        _property.stringValue = result;
    }
}
