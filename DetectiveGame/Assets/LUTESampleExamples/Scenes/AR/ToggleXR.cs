
//using UnityEditor.EditorTools;

using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[OrderInfo("XR",
              "Toggle XR",
              "Toggles the XR object on or off depending on its current state")]
[AddComponentMenu("")]
public class ToggleXR : Order
{
    private XRHelper xrHelper;
    
    [SerializeField]
    public bool toggle = true;

    [SerializeField]
    public PlaneDetectionMode planeDetectionMode = PlaneDetectionMode.Horizontal;

    [SerializeField]
    public GameObject planeVisualiser;
    private ARPlaneManager planeManager;
    
    [SerializeField]
    public GameObject pointCloudVisualiser;
    private ARPointCloudManager pointCloudManager;

    [SerializeField]
    public Camera xrCamera;

    private void Awake()
    {
        xrHelper = XRHelper.getXRScript();
        
        if (planeVisualiser != null)
        {
            planeManager = xrHelper.GetComponentInChildren<ARPlaneManager>();
        }

        if (pointCloudVisualiser != null)
        {
            pointCloudManager = xrHelper.GetComponentInChildren<ARPointCloudManager>();
        }
        
        xrCamera = xrHelper.GetComponentInChildren<Camera>();
    }

    public override void OnEnter()
    {
        if (planeVisualiser != null)
        {
            planeManager.planePrefab = planeVisualiser;
        }

        if (pointCloudVisualiser != null)
        {
            pointCloudManager.pointCloudPrefab = pointCloudVisualiser;
        }

        var xrEnabled = !xrCamera.enabled;
        
        xrCamera.enabled = xrEnabled;
        xrHelper.gameObject.SetActive(xrEnabled);
        
        Continue();
    }

    public override string GetSummary()
    {
        return "Toggles the XR camera on or off depending on the chosen setting";
    }
}