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
    [SerializeField] public PlaneDetectionMode planeDetectionMode = PlaneDetectionMode.Horizontal;
    [SerializeField] public GameObject planeVisualiser;
    [SerializeField] public GameObject pointCloudVisualiser;

    private void Awake()
    {
        xrHelper = XRHelper.getXRScript();
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

        popupIcon.SetAction(ToggleXR);
        popupIcon.MoveToNextOption();

        Continue();
    }

    private void ToggleXR()
    {
        if (planeVisualiser)
        {
            xrHelper.PlaneManager.planePrefab = planeVisualiser;
            xrHelper.PlaneManager.requestedDetectionMode = planeDetectionMode;
        }
        if (pointCloudVisualiser) { xrHelper.PointCloudManager.pointCloudPrefab = pointCloudVisualiser; }

        xrHelper.ToggleXR();
    }
    
    public override string GetSummary()
    {
        return "Creates a button which will toggle the XR camera on/off";
    }
}