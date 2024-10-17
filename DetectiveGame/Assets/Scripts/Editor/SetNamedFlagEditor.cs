using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SetNamedFlag))]
public class SetNamedFlagEditor : FlagEditor
{
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
        
        FlagNamesField(m_flagNameProperty, new GUIContent("Flag", $"Named flag to {m_operationProperty.enumNames[m_operationProperty.enumValueIndex].ToLower()}"),
            new GUIContent("<None>"),
            NamedFlags.FlagNames);
        
        EditorGUILayout.PropertyField(m_operationProperty);
        
        serializedObject.ApplyModifiedProperties();
    }
}
