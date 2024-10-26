using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SetMarkerIcon))]
public class SetMarkerIconEditor : OrderEditor
{
    private SerializedProperty m_markerProperty;
    private SerializedProperty m_iconProperty;

    private List<LocationVariable> m_locationVariables;

    public override void OnEnable()
    {
        base.OnEnable();

        m_markerProperty = serializedObject.FindProperty("m_marker");
        m_iconProperty = serializedObject.FindProperty("m_icon");
    }

    public override void DrawOrderGUI()
    {
        serializedObject.Update();

        if (m_locationVariables == null)
        {
            Debug.Log("Refreshing Location Variables!");
            var engine = BasicFlowEngine.CachedEngines[0];
            m_locationVariables = engine.GetComponents<LocationVariable>().ToList();
        }

        ObjectField<LocationVariable>(m_markerProperty,
            new GUIContent("Marker Location", "The location whose marker will be updated"),
            new GUIContent("<None>"),
            m_locationVariables);
        EditorGUILayout.PropertyField(m_iconProperty);

        serializedObject.ApplyModifiedProperties();
    }
}