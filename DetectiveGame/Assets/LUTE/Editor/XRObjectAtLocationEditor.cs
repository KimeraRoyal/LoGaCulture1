using MoreMountains.InventoryEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(XRObjectAtLocation))]
public class XRObjectAtLocationEditor : OrderEditor
{
    protected SerializedProperty objectLocProp;
    protected SerializedProperty distanceProp;
    protected SerializedProperty objectProp;
    protected SerializedProperty objectNameProp;

    protected int locationVarIndex = 0;
    protected int itemIndex = 0;

    public override void OnEnable()
    {
        base.OnEnable();

        objectLocProp = serializedObject.FindProperty("objectLocation");
        objectProp = serializedObject.FindProperty("objectToPlace");
        objectNameProp = serializedObject.FindProperty("objectName");
       
    }

    public override void OnInspectorGUI()
    {
        DrawOrderGUI();
    }

    public override void DrawOrderGUI()
    {
        XRObjectAtLocation t = target as XRObjectAtLocation;
        var engine = (BasicFlowEngine)t.GetEngine();


        var locationVars = engine.GetComponents<LocationVariable>();
        for (int i = 0; i < locationVars.Length; i++)
        {
            if (locationVars[i] == objectLocProp.objectReferenceValue as LocationVariable)
            {
                locationVarIndex = i;
            }
        }

        locationVarIndex = EditorGUILayout.Popup("Location", locationVarIndex, locationVars.Select(x => x.Key).ToArray());
        if (locationVars.Length > 0)
            objectLocProp.objectReferenceValue = locationVars[locationVarIndex];

       
        EditorGUILayout.PropertyField(objectProp);

        //name property
        EditorGUILayout.PropertyField(objectNameProp);

        serializedObject.ApplyModifiedProperties();
    }
}
