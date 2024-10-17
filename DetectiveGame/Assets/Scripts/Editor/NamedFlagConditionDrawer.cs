using System.Collections;
using KW.Flags;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NamedFlagCondition))]
public class NamedFlagConditionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var enumerator = property.GetEnumerator();
        var namedFlagsProperty = GetNext(enumerator).Copy();
        var isSetProperty = GetNext(enumerator).Copy();
        var flagNameProperty = GetNext(enumerator).Copy();
        
        EditorGUILayout.PropertyField(isSetProperty);

        var namedFlags = namedFlagsProperty.objectReferenceValue as NamedFlags;
        if (namedFlags)
        {
            FlagEditor.FlagNamesField(flagNameProperty, new GUIContent("Flag", $"Named flag to check"),
                new GUIContent("<None>"),
                namedFlags.FlagNames);
        }
        else
        {
            EditorGUILayout.PropertyField(flagNameProperty);
        }
        
        EditorGUI.EndProperty();
    }

    private SerializedProperty GetNext(IEnumerator enumerator)
    {
        enumerator.MoveNext();
        return enumerator.Current as SerializedProperty;
    }
}
