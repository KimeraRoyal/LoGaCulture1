using Mapbox.Examples;
using UnityEngine;

[OrderInfo("Map",
             "Show Location",
             "Shows / hides a location marker based on the location provided")]
[AddComponentMenu("")]
public class ShowLocationMarker : Order
{
    private SpawnOnMap m_map;
    
    [Tooltip("The location of the marker to show / hide.")]
    [SerializeField] protected LocationVariableReference location;

    [Tooltip("Whether to show or hide the marker.")]
    [SerializeField] protected bool show = true;

    private void Awake()
    {
        var engine = GetEngine();
        if(!engine) { return; }
        
        m_map = engine.GetMap();
    }

    public override void OnEnter()
    {
        if (location == null)
        {
            Continue();
            return;
        }

        HideLocation();
        Continue();
    }

    private void HideLocation()
    {
        if (!m_map || !location.Variable)
        {
            Continue();
            return;
        }
        if(show) { m_map.ShowLocationMarker(location.Variable); }
        else { m_map.HideLocationMarker(location.Variable); }
    }

    public override string GetSummary()
    {
        if (location.Variable) { return $"{(show ? "Shows" : "Hides")} location marker at {location.Variable.Location.Label}"; }
        return "Error: No location provided.";
    }
}