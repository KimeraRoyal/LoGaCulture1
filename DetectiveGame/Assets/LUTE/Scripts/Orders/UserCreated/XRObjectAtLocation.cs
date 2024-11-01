using System;
using Mapbox.Unity.Location;
using Mapbox.Unity.Utilities;
using MoreMountains.Feedbacks;
using MoreMountains.InventoryEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[OrderInfo("XR",
              "PlaceObjectAtLocation",
              "Place an object at the speciied location")]
[AddComponentMenu("")]
public class XRObjectAtLocation : Order
{
    private DebugText m_debugText;

    private XRHelper m_xr;
    private ARRaycastManager m_raycastManager;
    private ARPlaneManager m_planeManager;
    
    ILocationProvider _locationProvider;
    ILocationProvider LocationProvider
    {
        get
        {
            if (_locationProvider == null)
            {
                _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            }

            return _locationProvider;
        }
    }


    [Tooltip("Where this item will be placed on the map")]
    [SerializeField] protected LocationVariable objectLocation;
    private Vector2 m_targetLatLon;

    [Tooltip("The maximum distance the object can be spawned from the target GPS position, in meters")]
    [SerializeField] protected float distanceFromLocation = 1.0f;

    [Tooltip("The object to place at the location")]
    [SerializeField] protected GameObject objectToPlace;

    //[SerializeField] protected bool placeOnPlaceDetected = true;

    //name of the object to place
    [Tooltip("The name of the object to place at the location")]
    [SerializeField] protected string objectName;


    private GameObject xrObject;

    bool locationInit = false;
    private bool spawnedObject;

    private void Awake()
    {
        m_debugText = FindObjectOfType<DebugText>();
        
        m_xr = FindAnyObjectByType<XRHelper>();
        m_raycastManager = FindAnyObjectByType<ARRaycastManager>();
        m_planeManager = FindAnyObjectByType<ARPlaneManager>();

        var targetComponents = objectLocation.Value.Split(", ");
        m_targetLatLon = new Vector2(float.Parse(targetComponents[0]), float.Parse(targetComponents[1]));
    }

    IEnumerator start()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            Debug.LogError("Location not enabled on device or app does not have permission to access location");

        // Starts the location service.
        Input.location.Start();

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            m_debugText.DebugLine($"Try #{20 - maxWait + 1}");
            maxWait--;
        }
        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.LogError("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            Debug.LogError("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        
            locationInit = true;
        }
    }

    public override void OnEnter()
    {
        xrObject = XRHelper.getXRScript().gameObject; 
        StartCoroutine(start());

        m_planeManager.planesChanged += OnPlaneDetected;
        spawnedObject = false;
    }

    private float GetLongitudeDegreeDistance(float latitude)
    {
        return 111319.9f * Mathf.Cos(latitude * (Mathf.PI / 180));
    }

    Vector2 GamePosToGPS(Vector3 pos)
    {
        // Real GPS Position - This will be the world origin.
        var gpsLat = Input.location.lastData.latitude;
        var gpsLon = Input.location.lastData.longitude;

        // Conversion factors
        float degreesLatitudeInMeters = 111132;
        float degreesLongitudeInMetersAtEquator = 111319.9f;

        // GPS position converted into unity coordinates
        var latOffset = pos.x / degreesLatitudeInMeters;
        var lonOffset = pos.z / GetLongitudeDegreeDistance(gpsLat);

        // Real world position of object. Need to update with something near your own location.
        float latitude = gpsLat + latOffset;
        float longitude = gpsLon + lonOffset;

        return new Vector2(latitude, longitude);

    }

    void SpawnObject()
    {
        // Real world position of object. Need to update with something near your own location.
        float latitude = -27.469093f;
        float longitude = 153.023394f;

        // Conversion factors
        float degreesLatitudeInMeters = 111132;
        float degreesLongitudeInMetersAtEquator = 111319.9f;

        // Real GPS Position - This will be the world origin.
        var gpsLat = Input.location.lastData.latitude;
        var gpsLon = Input.location.lastData.longitude;
        // GPS position converted into unity coordinates
        var latOffset = (latitude - gpsLat) * degreesLatitudeInMeters;
        var lonOffset = (longitude - gpsLon) * GetLongitudeDegreeDistance(latitude);

        // Create object at coordinates
        var obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        obj.transform.position = new Vector3(latOffset, 0, lonOffset);
        obj.transform.localScale = new Vector3(4, 4, 4);
    }

    private string m_lastCoordinates;

    private void OnPlaneDetected(ARPlanesChangedEventArgs args)
    {
        if(spawnedObject) { return; }
        
        foreach (var plane in args.added)
        {
            if (plane.alignment != PlaneAlignment.HorizontalUp) { continue; }

            var ray = m_xr.Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (!TestRay(ray, out var hit)) { continue; }
            
            var latLon = GamePosToGPS(hit.point);
            m_lastCoordinates = $"({latLon.x}, {latLon.y})";

            if (LatLonDistance(latLon, m_targetLatLon) > distanceFromLocation) { continue; }
            
            SpawnObjectAtLocation(hit);
            Continue();
            break;
        }
    }

    private void SpawnObjectAtLocation(ARRayHit _hit)
    {
        if(spawnedObject) { return; }
        
        m_debugText.PersistentDebugLine($"Found valid plane at {m_lastCoordinates}");

        var obj = Instantiate(objectToPlace, _hit.point, Quaternion.identity);
        XRObjectManager.AddObject(objectName, obj);
        
        m_planeManager.planesChanged -= OnPlaneDetected;
        spawnedObject = true;
    }

    private struct ARRayHit
    {
        public ARPlane plane;
        
        public Vector3 point;
        public Vector3 normal;
    }

    private bool TestRay(Ray _ray, out ARRayHit o_rayHit)
    {
        var rayHits = new List<ARRaycastHit>();
        
        if (!m_raycastManager.Raycast(_ray, rayHits))
        {
            o_rayHit = new ARRayHit();
            return false;
        }

        var planeHits = rayHits.Where(hit => (hit.hitType & TrackableType.Planes) != 0).ToList();
        if(planeHits.Count < 1)
        { 
            o_rayHit = new ARRayHit();
            return false;
        }

        var plane = m_planeManager.GetPlane(planeHits[0].trackableId);
        var point = _ray.origin + _ray.direction * planeHits[0].distance;
        var normal = plane.normal;
        
        o_rayHit = new ARRayHit { plane = plane, normal = normal, point = point };
        return true;
    }

    private void Update()
    {
        m_debugText.DebugLine($"Location Services {Input.location.status}");
        if(!locationInit) { return; }
        m_debugText.DebugLine($"Target Location: {m_targetLatLon.x}, {m_targetLatLon.y}");
        m_debugText.DebugLine("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude);
        m_debugText.DebugLine($"Last Intersected Plane: {m_lastCoordinates}");
        m_debugText.DebugLine($"Distance: {LatLonDistance(new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude), m_targetLatLon)}");
    }

    // https://discussions.unity.com/t/how-to-get-distance-from-2-locations-with-unity-location-service/169850/4
    private float LatLonDistance(Vector2 _a, Vector2 _b)
    {
        const float c_r = 6371.0f; // Radius of earth

        var aLatRadian = Mathf.Deg2Rad * _a.x;
        var bLatRadian = Mathf.Deg2Rad * _b.x;
        var latDistanceRadian = Mathf.Deg2Rad * (_b.x - _a.x);
        var longDistanceRadian = Mathf.Deg2Rad * (_b.y - _a.y);

        var a = Mathf.Pow(Mathf.Sin(latDistanceRadian / 2.0f), 2.0f) +
                Mathf.Pow(Mathf.Sin(longDistanceRadian / 2.0f), 2.0f)
                * Mathf.Cos(aLatRadian) * Mathf.Cos(bLatRadian);
        var c = 2.0f * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1.0f - a));
        var totalDistance = c_r * c * 1000.0f;

        return totalDistance;
    }

    public override string GetSummary()
  {
      return "Places an object at a specified location";
  }
}