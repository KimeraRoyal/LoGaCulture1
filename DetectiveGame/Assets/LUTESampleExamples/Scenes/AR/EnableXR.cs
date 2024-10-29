
//using UnityEditor.EditorTools;

using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[OrderInfo("XR",
              "Enable XR",
              "Toggles the XR object on or off depending on its current state")]
[AddComponentMenu("")]
public class EnableXR : Order
{
    private XRHelper xrHelper;
    
    [SerializeField] public bool enableXR = true;

    [SerializeField] public PlaneDetectionMode planeDetectionMode = PlaneDetectionMode.Horizontal;
    [SerializeField] protected GameObject planeVisualiser;
    [SerializeField] protected GameObject pointCloudVisualiser;

    private void Awake()
    {
        xrHelper = XRHelper.getXRScript();
    }

    public override void OnEnter()
    {
        if (planeVisualiser)
        {
            xrHelper.PlaneManager.planePrefab = planeVisualiser;
            xrHelper.PlaneManager.requestedDetectionMode = planeDetectionMode;
        }
        if (pointCloudVisualiser) { xrHelper.PointCloudManager.pointCloudPrefab = pointCloudVisualiser; }

        xrHelper.SetXREnabled(enableXR);
        
        Continue();
    }

    public override string GetSummary()
    {
        return "Toggles the XR camera on or off depending on the chosen setting";
    }
}