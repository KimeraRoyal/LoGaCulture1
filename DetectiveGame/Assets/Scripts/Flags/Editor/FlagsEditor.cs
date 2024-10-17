using UnityEditor;
using UnityEngine;

namespace KW.Flags.Editor
{
    [CustomEditor(typeof(Flags))]
    public class FlagsEditor : UnityEditor.Editor
    {
        private const int c_sizeInBytes = sizeof(uint);
        private const int c_sizeInBits = c_sizeInBytes * 8;
        private const int c_displaySizeInBits = 16;
    
        protected SerializedProperty m_flagBitsProperty;
    
        private void OnEnable()
        {
            m_flagBitsProperty = serializedObject.FindProperty("m_flagBits");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if(!ShowFlagsArray(m_flagBitsProperty))
            {
                EditorGUILayout.LabelField("No Flags Set");
            }
        }

        private bool ShowFlagsArray(SerializedProperty _property)
        {
            if(!_property.isArray || _property.arraySize < 1) { return false; }

            var maxIndex = (_property.arraySize + 1) * c_sizeInBits - 1;
            var digits = Mathf.FloorToInt(Mathf.Log10(maxIndex) + 1);

            for (var i = 0; i < _property.arraySize; i++)
            {
                var flags = _property.GetArrayElementAtIndex(i).uintValue;
                ShowFlags((ushort) flags, i * c_sizeInBits, i * c_sizeInBits + c_displaySizeInBits - 1, digits);
                ShowFlags((ushort) (flags >> c_displaySizeInBits), i * c_sizeInBits + c_displaySizeInBits, (i + 1) * c_sizeInBits - 1, digits);
            }

            return true;
        }

        private void ShowFlags(ushort _flags, int _minIndex, int _maxIndex, int _indexDigits)
        {
            var flagBits = "";
            for (var i = 0; i < c_displaySizeInBits; i++)
            {
                var flag = (_flags & (1 << i)) > 0;
                flagBits += flag ? "1" : "0";
            }

            var minIndexDigits = _minIndex.ToString("D" + _indexDigits);
            var maxIndexDigits = _maxIndex.ToString("D" + _indexDigits);
        
            EditorGUILayout.LabelField($"{minIndexDigits} - {flagBits} - {maxIndexDigits}");
        }
    }
}
