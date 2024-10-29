using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

[CustomEditor(typeof(EnableXR))]
public class ToggleXREditor : OrderEditor
{
    protected SerializedProperty enableXR;

    protected SerializedProperty planeVisualiser;
    protected SerializedProperty planeDetectionMode;
    protected SerializedProperty pointCloudVisualiser;

    public override void OnEnable()
    {
        base.OnEnable();

        enableXR = serializedObject.FindProperty("enableXR");
        
        planeVisualiser = serializedObject.FindProperty("planeVisualiser");

        //if the planeVisualiser property is null, then set it to the default plane visualiser which is "AR Feathered Plane"
        if (planeVisualiser.objectReferenceValue == null)
        {
            planeVisualiser.objectReferenceValue = Resources.Load("Prefabs/AR Feathered Plane");
        }

        planeDetectionMode = serializedObject.FindProperty("planeDetectionMode");

        //set defaulr plane detection mode to horizontal
        planeDetectionMode.enumValueIndex = 1;

        pointCloudVisualiser = serializedObject.FindProperty("pointCloudVisualiser");

        //if the pointCloudVisualiser property is null, then set it to the default point cloud visualiser which is "AR Point Cloud"
        if (pointCloudVisualiser.objectReferenceValue == null)
        {
            pointCloudVisualiser.objectReferenceValue = Resources.Load("Prefabs/AR Point Cloud Debug Visualizer");
        }
    }

    public override void OnInspectorGUI()
    {
        DrawOrderGUI();
    }

    public override void DrawOrderGUI()
    {
        EnableXR t = target as EnableXR;
        var engine = (BasicFlowEngine)t.GetEngine();

        EditorGUILayout.PropertyField(enableXR);

       //if toggle is on, show everything else
       if (enableXR.boolValue)
        {
            EditorGUILayout.PropertyField(planeVisualiser);
            EditorGUILayout.PropertyField(planeDetectionMode);
            EditorGUILayout.PropertyField(pointCloudVisualiser);
        }


        serializedObject.ApplyModifiedProperties();
    }

}
