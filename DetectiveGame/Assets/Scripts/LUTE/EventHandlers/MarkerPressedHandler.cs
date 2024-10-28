using KR;
using UnityEngine;

[EventHandlerInfo("Map",
    "Marker Pressed",
    "Executes when the specified location marker is pressed")]
public class MarkerPressedHandler : EventHandler
{
    private Markers m_markers;
    
    [Tooltip("The location to track")]
    [SerializeField] protected LocationVariableReference m_location;

    protected virtual void Awake()
    {
        m_markers = FindAnyObjectByType<Markers>();
        m_markers.OnMarkerPressed += OnMarkerPressed;
    }

    private void OnMarkerPressed(string _name)
    {
        if(!m_location.Variable || _name != m_location.Variable.Key) { return; }
        ExecuteNode();
    }

    public override string GetSummary()
        => m_location.Variable ? $"This node will execute when the \"{m_location.Variable.Location.ShowName}\" location node is pressed."
            : "No location set";
}
