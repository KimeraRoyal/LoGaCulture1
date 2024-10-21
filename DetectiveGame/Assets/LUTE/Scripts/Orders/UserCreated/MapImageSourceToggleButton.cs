using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.Serialization;

[OrderInfo("Map",
              "Image Source Toggle Button",
              "Creates a custom icon to toggle the map between Outdoors & Satellite view.")]
[AddComponentMenu("")]
public class MapImageSourceToggleButton : Order
{
    [Tooltip("Custom icon to display for this menu")]
    [SerializeField] protected Sprite customButtonIcon;
    
    [FormerlySerializedAs("setIconButton")]
    [Tooltip("A custom popup class to use to display this menu - if one is in the scene it will be used instead")]
    [SerializeField] protected PopupIcons m_setIconsButton;
    
    [Tooltip("If true, the popup icon will be displayed, otherwise it will be hidden")]
    [SerializeField] protected bool showIcon = true;
    
    public override void OnEnter()
    {
        var engine = GetEngine();
        if (!engine)
        {
            Continue();
            return;
        }

        var abstractMap = FindAnyObjectByType<AbstractMap>();
        if (!abstractMap)
        {
            Continue();
            return;
        }

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
            ToggleImageSource(abstractMap);
        };
        popupIcon.SetAction(action);
        popupIcon.MoveToNextOption();
        Continue();
    }

  public override string GetSummary()
  {
      return "Creating a custom icon to toggle the map between Satellite and Outdoors views";
  }

  private static void ToggleImageSource(AbstractMap _map)
  {
      var imagery = _map.ImageLayer;
      
      var sourceType = imagery.LayerSource;
      var isOutdoorsView = sourceType == ImagerySourceType.MapboxOutdoors;
      var targetSourceType = isOutdoorsView ? ImagerySourceType.MapboxSatelliteStreet : ImagerySourceType.MapboxOutdoors;
      
      imagery.SetLayerSource(targetSourceType);
  }
}