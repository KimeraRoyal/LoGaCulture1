using KR;
using UnityEngine;

[OrderInfo("Map",
              "Set Marker Icon",
              "Changes the icon of a location marker")]
[AddComponentMenu("")]
public class SetMarkerIcon : Order
{
    private Markers m_markers;

    [Tooltip("The location whose marker will be updated")]
    [SerializeField] protected LocationVariableReference m_marker;

    [Tooltip("The icon to set. Leave as none to indicate the location has nothing of note right now")]
    [SerializeField] protected Sprite m_icon;
    
    protected virtual void Awake()
    {
        m_markers = FindAnyObjectByType<Markers>();
    }

    public override void OnEnter()
    {
        var marker = m_markers.GetMarker(m_marker.Variable.Key);
        if (!marker)
        {
            Debug.LogError($"Invalid Marker ID {m_marker.Variable.Key}");
            return;
        }

        marker.Icon = m_icon;
        Continue();
    }

    public override string GetSummary()
        => m_marker.Variable ? $"{(m_icon ? "Sets" : "Clears")} the icon of location marker \"{m_marker.Variable.Location.Label}\""
            : "Invalid marker";
}