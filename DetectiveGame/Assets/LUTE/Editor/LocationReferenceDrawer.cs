using UnityEditor;
using UnityEngine;

namespace LoGaCulture.LUTE
{
    [CustomPropertyDrawer(typeof(LocationReference))]
    public class LocationReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUILayout.PropertyField(property);
            
            EditorGUI.EndProperty();
        }
    }
}