using System;
using MoreMountains.InventoryEngine;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[OrderInfo("XR",
              "XRMenu",
              "Toggles the XR Camera on/off using a custom button")]
[AddComponentMenu("")]
public class XRMenu : Order
{
    private XRHelper xrHelper;
    
    [Tooltip("Custom icon to display for this menu")]
    [SerializeField] protected Sprite customButtonIcon;
    [FormerlySerializedAs("setIconButton")]
    [Tooltip("A custom popup class to use to display this menu - if one is in the scene it will be used instead")]
    [SerializeField] protected PopupIcons m_setIconsButton;
    [Tooltip("If true, the popup icon will be displayed, otherwise it will be hidden")]
    [SerializeField] protected bool showIcon = true;

    [Header("XR Settings")]
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
        if (m_setIconsButton != null)
        {
            PopupIcons.activePopupIcons = m_setIconsButton;
        }

        var popupIcon = PopupIcons.GetPopupIcons();
        if (popupIcon != null)
        {
            if (customButtonIcon != null)
            {
                popupIcon.SetIcon(customButtonIcon);
            }
        }
        if (showIcon)
        {
            popupIcon.SetActive(true);
        }

        UnityEngine.Events.UnityAction action = () =>
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
        };
        popupIcon.SetAction(action);
        popupIcon.MoveToNextOption();

        Continue();
    }

    public override string GetSummary()
    {
        return "Creates a button which will toggle the XR camera on/off";
    }
}