using KW.Flags;
using KW.Flags.Editor;
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

    public override void OnEnable()
    {
        base.OnEnable();
        
        m_flagNameProperty = serializedObject.FindProperty("m_flagName");
        m_operationProperty = serializedObject.FindProperty("m_operation");
    }

    public override void DrawOrderGUI()
    {
        serializedObject.Update();
        
        FlagsEditor.FlagNamesField(m_flagNameProperty, new GUIContent("Flag", $"Named flag to {m_operationProperty.enumNames[m_operationProperty.enumValueIndex].ToLower()}"),
            new GUIContent("<None>"),
            NamedFlags.FlagNames);
        
        EditorGUILayout.PropertyField(m_operationProperty);
        
        serializedObject.ApplyModifiedProperties();
    }
}
