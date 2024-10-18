using System.Collections;
using KW.Flags;
using KW.Flags.Editor;
using UnityEditor;
using UnityEngine;

namespace KR
{
    [CustomPropertyDrawer(typeof(CharacterTrait))]
    public class CharacterTraitDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            var enumerator = property.GetEnumerator();
            var namedFlagsProperty = GetNext(enumerator).Copy();
            var textProperty = GetNext(enumerator).Copy();
            var requiredFlagProperty = GetNext(enumerator).Copy();
        
            EditorGUILayout.PropertyField(textProperty);

            var namedFlags = namedFlagsProperty.objectReferenceValue as NamedFlags;
            if (namedFlags)
            {
                FlagsEditor.FlagNamesField(requiredFlagProperty, new GUIContent("Require Flag", $"This flag, if specified, must be set to display this trait."),
                    new GUIContent("<None>"),
                    namedFlags.FlagNames);
            }
            else
            {
                EditorGUILayout.PropertyField(requiredFlagProperty);
            }
            
            EditorGUI.EndProperty();
        }

        private SerializedProperty GetNext(IEnumerator enumerator)
        {
            enumerator.MoveNext();
            return enumerator.Current as SerializedProperty;
        }
    }
}
