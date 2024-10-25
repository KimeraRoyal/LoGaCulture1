using KR;
using UnityEngine;

[EventHandlerInfo("Map",
    "Marker Pressed",
    "Executes when the specified location marker is pressed")]
public class MarkerPressedHandler : EventHandler
{
    private Markers m_markers;
    
    [Tooltip("The name of the location marker to track")]
    [SerializeField] protected string m_markerName = "Location";

    protected virtual void Awake()
    {
        m_markers = FindAnyObjectByType<Markers>();
        m_markers.OnMarkerPressed += OnMarkerPressed;
    }

    private void OnMarkerPressed(string _name)
    {
        if(_name != m_markerName) { return; }
        ExecuteNode();
    }

    public override string GetSummary()
        => $"This node will execute when the \"{m_markerName}\" location node is pressed.";
}
