using System.Collections.Generic;
using KW.Flags;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SetNamedFlag))]
public class SetNamedFlagEditor : OrderEditor
{
    private NamedFlags m_namedFlags;

    private NamedFlags NamedFlags
    {
        get
        {
            if (!m_namedFlags) { m_namedFlags = FindAnyObjectByType<NamedFlags>(); }
            return m_namedFlags;
        }
    }

    private SerializedProperty m_flagNameProperty;
    private SerializedProperty m_operationProperty;
    
    private SerializedProperty m_lastIndexProperty;
    private SerializedProperty m_lastNamedFlagCountProperty;

    public override void OnEnable()
    {
        base.OnEnable();
        
        m_flagNameProperty = serializedObject.FindProperty("m_flagName");
        m_operationProperty = serializedObject.FindProperty("m_operation");
        
        m_lastIndexProperty = serializedObject.FindProperty("m_lastIndex");
        m_lastNamedFlagCountProperty = serializedObject.FindProperty("m_lastNamedFlagCount");
    }

    public override void DrawOrderGUI()
    {
        serializedObject.Update();
        
        FlagNamesField(new GUIContent("Flag", $"Named flag to {m_operationProperty.enumNames[m_operationProperty.enumValueIndex].ToLower()}"),
            new GUIContent("<None>"),
            NamedFlags.FlagNames);
        
        EditorGUILayout.PropertyField(m_operationProperty);
        
        serializedObject.ApplyModifiedProperties();
    }
    
    public void FlagNamesField(GUIContent label, GUIContent nullLabel, List<string> objectList)
    {
        var objectNames = new List<GUIContent>();

        var selectedObject = m_flagNameProperty.stringValue;

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

        m_flagNameProperty.stringValue = result;
    }
}
