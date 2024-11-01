using System.Linq;
using Mapbox.Examples;
using UnityEngine;

[OrderInfo("Map",
              "Show All Locations",
              "Shows / hides all locations that are currently shown on screen")]
[AddComponentMenu("")]
public class ShowAllLocations : Order
{
    private SpawnOnMap m_map;
    private LocationVariable[] m_locations;
    
    [Tooltip("Any locations that should be excluded.")]
    [SerializeField] protected LocationVariableReference[] exclude;

    [Tooltip("Whether to show or hide the markers.")]
    [SerializeField] protected bool show = true;

    private void Awake()
    {
        var engine = GetEngine();
        if(!engine) { return; }
        
        m_map = engine.GetMap();
        m_locations = engine.GetComponents<LocationVariable>().Where(CheckLocationValid).ToArray();

        var locations = "Locations: ";
        for (var i = 0; i < m_locations.Length; i++)
        {
            if (i > 0) { locations += ", "; }
            locations += m_locations[i].Location.Label;
        }
        Debug.Log(locations);
       
        return;

        bool CheckLocationValid(LocationVariable location)
            => exclude.All(excluded => excluded.Variable != location);
    }

    public override void OnEnter()
    {
        foreach (var location in m_locations)
        {
            if (!location) { continue; }
            ShowLocationMarker.ShowLocation(m_map, location, show); 
        }
        Continue();
    }

    public override string GetSummary()
    {
        var hasExclusions = exclude is { Length: > 0 };
        return $"{(show ? "Shows" : "Hides")}{(!hasExclusions ? " all" : "")} location markers{(hasExclusions ? $", except for {exclude.Length} excluded" : "")}";
    }
}