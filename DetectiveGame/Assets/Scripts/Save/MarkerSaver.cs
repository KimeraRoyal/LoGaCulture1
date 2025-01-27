using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mapbox.Examples;
using Save;
using UnityEngine;

[RequireComponent(typeof(SpawnOnMap))]
public class MarkerSaver : Saver
{
    public override void Save()
    {
        var spawner = GetComponent<SpawnOnMap>();
        
        var activeMarkers = (from marker in spawner.Markers.List where marker.gameObject.activeSelf select marker.ID).ToList();
        
        PlayerPrefs.SetInt("markerArraySize", activeMarkers.Count);
        for (var i = 0; i < activeMarkers.Count; i++)
        {
            PlayerPrefs.SetString("markerIDs" + i, activeMarkers[i]);
        }
    }

    public override void Load()
    {
        var spawner = GetComponent<SpawnOnMap>();
        var flowEngine = spawner.GetComponentInParent<BasicFlowEngine>();
        var locations = flowEngine.GetComponents<LocationVariable>();
        HideAllMarkers(spawner, locations);
        
        if(!PlayerPrefs.HasKey("markerArraySize")) { return; }
        
        var markerArraySize = PlayerPrefs.GetInt("markerArraySize");
        if(markerArraySize < 1) { return; }
        
        var locationVariables = new List<LocationVariable>();

        for (var i = 0; i < markerArraySize; i++)
        {
            var markerID = PlayerPrefs.GetString("markerIDs" + i);
            
            var location = locations.FirstOrDefault(variable => variable.Key == markerID);
            locationVariables.Add(location);
        }

        StartCoroutine(InitialiseMarkers(spawner, locationVariables));
    }

    private void HideAllMarkers(SpawnOnMap _spawner, LocationVariable[] _locations)
    {
        foreach (var location in _locations)
        {
            if (!location) { continue; }
            ShowLocationMarker.ShowLocation(_spawner, location, false); 
        }
    }

    private IEnumerator InitialiseMarkers(SpawnOnMap _spawner, List<LocationVariable> _variables)
    {
        yield return new WaitUntil(() => _spawner.Initialised);
        foreach (var variable in _variables)
        {
            _spawner.ShowLocationMarker(variable);
        }
    }
}
