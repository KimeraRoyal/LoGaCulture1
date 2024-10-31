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

        ShowLocation(m_map, location.Variable, show);
        Continue();
    }

    public static bool ShowLocation(SpawnOnMap _map, LocationVariable _location, bool _show)
    {
        if (!_map || !_location) { return false; }
        
        if(_show) { _map.ShowLocationMarker(_location); }
        else { _map.HideLocationMarker(_location); }

        return true;
    }

    public override string GetSummary()
    {
        if (location.Variable) { return $"{(show ? "Shows" : "Hides")} location marker at {location.Variable.Location.Label}"; }
        return "Error: No location provided.";
    }
}