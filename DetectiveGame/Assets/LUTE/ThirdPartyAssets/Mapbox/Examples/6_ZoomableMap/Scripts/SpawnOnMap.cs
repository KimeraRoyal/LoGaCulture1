using KR;
using KR.Map.Marker;

namespace Mapbox.Examples
{
    using Mapbox.Unity.Map;
    using Mapbox.Unity.MeshGeneration.Factories;
    using Mapbox.Unity.Utilities;
    using Mapbox.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class SpawnOnMap : MonoBehaviour
    {
        private Markers m_markers;
        private bool m_initialised;
        
        [SerializeField] public Transform tracker;
        [SerializeField]
        AbstractMap _map;

        [Geocode]
        protected List<string> _locationStrings = new List<string>();
        public Vector2d[] _locations = new Vector2d[1];

        [SerializeField]
        public float _spawnScale = 5f;

        [SerializeField] protected BasicFlowEngine engine;

        [SerializeField]
        private DirectionsFactory _directionPrefab;

        List<Marker> _markers;

        public bool _isWithinRadius = false;

        private LocationVariable locationVariable;
        private If ifOrder;

        private static List<Location> _locationReferences = new List<Location>();

        public Markers Markers
        {
            get
            {
                if(!m_markers) { m_markers = FindAnyObjectByType<Markers>(); }
                return m_markers;
            }
        }

        public bool Initialised => m_initialised;
        
        private void Awake()
        {
            m_markers = FindAnyObjectByType<Markers>();
        }

        void Start()
        {
            InitializeEngine();
            if (engine == null) return;

            ProcessNodes();
            CreateMarkers();

            m_initialised = true;
        }

        private void InitializeEngine()
        {
            if (engine == null)
                engine = BasicFlowEngine.CachedEngines.FirstOrDefault();
            if (engine == null)
                engine = FindObjectOfType<BasicFlowEngine>();
            if (engine == null)
                Debug.LogError("No engine found");
        }

        private void ProcessNodes()
        {
            _locationData.Clear();
            _locationReferences.Clear();
            
            var locations = engine.gameObject.GetComponents<LocationVariable>();

            foreach (var location in locations)
            {
                if (location == null) continue;
                AddUniqueLocation(location);
            }
        }

        private LocationData? AddUniqueLocation(LocationVariable location)
        {
            if (location == null || string.IsNullOrEmpty(location.Value))
                return null;

            Vector2d latLong;
            try
            {
                latLong = Conversions.StringToLatLon(location.Value);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error converting location {location.Key}: {e.Message}");
                return null;
            }

            if (LocationExists(latLong))
            {
                const double epsilon = 0.000001; // Adjust as needed
                return _locationData.Find(data =>
                    Math.Abs(data.Position.x - latLong.x) < epsilon &&
                    Math.Abs(data.Position.y - latLong.y) < epsilon);
            }
            else
            {
                var newLocationData = new LocationData
                {
                    Position = latLong,
                    Name = location.Key,
                    Sprite = location.Location.DefaultIcon,
                    Color = location.Location.Color,
                    ShowName = location.Location.ShowName
                };

                _locationData.Add(newLocationData);
                _locationReferences.Add(location.Location);
                return newLocationData;
            }
        }

        private bool LocationExists(Vector2d position)
        {
            const double epsilon = 0.000001; // Adjust as needed
            return _locationData.Any(loc =>
                Math.Abs(loc.Position.x - position.x) < epsilon &&
                Math.Abs(loc.Position.y - position.y) < epsilon);
        }

        private void CreateMarkers()
        {
            _markers = new List<Marker>();
            for (var i = 0; i < _locationData.Count; i++)
            {
                CreateMarker(_locationData[i]);
            }
        }

        private void CreateMarker(LocationData locationData)
        {
            var marker = m_markers.AddMarker(locationData.Name);
            marker.TargetCamera = GetComponent<QuadTreeCameraMovement>()?._referenceCameraGame;

            marker.transform.localPosition = _map.GeoToWorldPosition(locationData.Position, true);
            marker.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);

            marker.Color = locationData.Color;
            if (locationData.Sprite)
            {
                marker.Icon = locationData.Sprite;
            }

            _markers.Add(marker);
        }

        private struct LocationData
        {
            public Vector2d Position;
            public string Name;
            public Sprite Sprite;
            public Color Color;
            public bool ShowName;
        }

        private List<LocationData> _locationData = new List<LocationData>();

        public GameObject HideLocationMarker(LocationVariable location)
        {
            if (location == null || string.IsNullOrEmpty(location.Value) || _map == null || _markers == null)
            {
                Debug.LogWarning("Invalid input or uninitialized objects in HideLocationMarker");
                return null;
            }

            var locationMarker = FindMarker(location.Key);
            if (!locationMarker) { return null; }
            
            locationMarker.gameObject.SetActive(false);
            return locationMarker.gameObject;
        }

        public GameObject ShowLocationMarker(LocationVariable location, bool updateText = false, string updatedText = "")
        {
            if (location == null || string.IsNullOrEmpty(location.Value) || _map == null || _markers == null)
            {
                Debug.LogWarning("Invalid input or uninitialized objects in ShowLocationMarker");
                return null;
            }

            var locationMarker = FindMarker(location.Key);
            if (!locationMarker)
            {
                AddUniqueLocation(location);
                CreateMarker(_locationData[^1]);
                locationMarker = _markers[^1];
            }

            if (locationMarker != null)
            {
                if (updateText && !string.IsNullOrEmpty(updatedText))
                {
                    int locationIndex = _locationData.FindIndex(data => data.Name == location.Key);
                    if (locationIndex == -1)
                    {
                        Debug.LogWarning($"Location {location.Key} not found in _locationData");
                        return null;
                    }

                    // Find the location data and update the name
                    var locationData = _locationData[locationIndex];
                    locationData.Name = updatedText;
                    _locationData[locationIndex] = locationData;
                }

                locationMarker.gameObject.SetActive(true);
                return locationMarker.gameObject;
            }
            return null;
        }

        private Marker FindMarker(string label)
            => _markers.Find(marker =>
                marker &&
                marker.ID == label);

        public bool IsWithinRadius(string location, string centre, float radius)
        {
            var location2D = Conversions.StringToLatLon(location);
            var centre2D = Conversions.StringToLatLon(centre);

            var locationMetered = Conversions.LatLonToMeters(location2D);
            var centreMetered = Conversions.LatLonToMeters(centre2D);

            var distance = Vector2d.Distance(locationMetered, centreMetered);
            return distance < radius;

            //If true then you either:
            //1. use backup location (using the engine)
            // does the engine have a backup location that uses the input location?
            // if so then go through the backup locations of the location and check if they are within the radius
            // find the first one that is not and if none are outside of radius then continue
            //2. disable order
            //3. disable node
        }

        private void DrawDirections()
        {
            // Go through all nodes
            List<Node> nodes = engine.gameObject.GetComponents<Node>().ToList();
            foreach (Node node in nodes)
            {
                // Go through all orders
                foreach (Order order in node.OrderList)
                {
                    // Check if order is type of If and uses node variable
                    if (order.GetType() == typeof(If))
                    {
                        If ifOrder = order as If;
                        foreach (ConditionExpression condition in ifOrder.conditions)
                        {
                            if (condition.AnyVariable.variable is NodeVariable)
                            {
                                Node targetNode = (condition.AnyVariable.variable as NodeVariable).Value;
                                // Check if both nodes use location
                                if (node.NodeLocation != null && targetNode.NodeLocation != null)
                                {
                                    // Spawn a direction factory object using prefab
                                    if (_directionPrefab == null)
                                    {
                                        Debug.LogError("Direction prefab is null");
                                        return;
                                    }
                                    DirectionsFactory directionFactory = Instantiate(_directionPrefab);
                                    Transform[] waypoints = new Transform[2];

                                    foreach (var marker in _markers)
                                    {
                                        var nodeLatLon = Conversions.StringToLatLon(node.NodeLocation.Value);
                                        var targetNodeLatLon = Conversions.StringToLatLon(targetNode.NodeLocation.Value);
                                        if (marker.gameObject.transform.localPosition == _map.GeoToWorldPosition(nodeLatLon, true))
                                        {
                                            waypoints[0] = marker.transform;
                                        }
                                        if (marker.gameObject.transform.localPosition == _map.GeoToWorldPosition(targetNodeLatLon, true))
                                        {
                                            waypoints[1] = marker.transform;
                                        }

                                        if (waypoints[0] != null && waypoints[1] != null)
                                        {
                                            directionFactory.SetWaypoints(waypoints, _map);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //if node calls to another node and both nodes have a location
                        if (order.GetType() == typeof(NextNode))
                        {
                            NextNode nextNode = order as NextNode;
                            Node targetNode = nextNode.targetNode;

                            if (node.NodeLocation != null && targetNode.NodeLocation != null)
                            {
                                // Spawn a direction factory object using prefab
                                if (_directionPrefab == null)
                                {
                                    Debug.LogError("Direction prefab is null");
                                    return;
                                }
                                DirectionsFactory directionFactory = Instantiate(_directionPrefab);
                                Transform[] waypoints = new Transform[2];

                                foreach (var marker in _markers)
                                {
                                    var nodeLatLon = Conversions.StringToLatLon(node.NodeLocation.Value);
                                    var targetNodeLatLon = Conversions.StringToLatLon(targetNode.NodeLocation.Value);
                                    if (marker.transform.localPosition == _map.GeoToWorldPosition(nodeLatLon, true))
                                    {
                                        waypoints[0] = marker.transform;
                                    }
                                    if (marker.transform.localPosition == _map.GeoToWorldPosition(targetNodeLatLon, true))
                                    {
                                        waypoints[1] = marker.transform;
                                    }

                                    if (waypoints[0] != null && waypoints[1] != null)
                                    {
                                        directionFactory.SetWaypoints(waypoints, _map);
                                    }
                                }
                            }
                        }
                    }
                }
                // Check if the node has a target unlock node and location
                if (node.TargetUnlockNode != null)
                {
                    Node targetUnlockNode = node.TargetUnlockNode;
                    // Check if both nodes use location (not null)
                    if (node.NodeLocation != null && targetUnlockNode.NodeLocation != null)
                    {
                        // Spawn a direction factory object using prefab
                        if (_directionPrefab == null)
                        {
                            Debug.LogError("Direction prefab is null");
                            return;
                        }
                        DirectionsFactory directionFactory = Instantiate(_directionPrefab);
                        Transform[] waypoints = new Transform[2];

                        waypoints[0] = new GameObject().transform;
                        waypoints[0].gameObject.name = "Waypoint 0";
                        var latLon = Conversions.StringToLatLon(node.NodeLocation.Value);
                        Vector3 pos1 = new Vector3((float)latLon.x, 0, (float)latLon.y);
                        waypoints[0].transform.localPosition = pos1;
                        waypoints[1] = new GameObject().transform;
                        var latLon2 = Conversions.StringToLatLon(targetUnlockNode.NodeLocation.Value);
                        Vector3 pos2 = new Vector3((float)latLon2.x, 0, (float)latLon2.y);
                        waypoints[1].transform.localPosition = pos2;
                        waypoints[1].gameObject.name = "Waypoint 1";

                        directionFactory.SetWaypoints(waypoints, _map);
                    }
                }
            }
        }
        private void Update()
        {
            UpdateMarkers();
            UpdateTracker();
        }

        private void UpdateMarkers()
        {
            for (int i = 0; i < _locationData.Count && i < _markers.Count; i++)
            {
                UpdateMarker(i);
            }
        }

        private void UpdateMarker(int index)
        {
            var marker = _markers[index];
            var locationData = _locationData[index];
            var locationReference = _locationReferences[index];

            if (!marker) return;

            marker.ID = locationData.Name;
            marker.Label = locationReference.Label;
            marker.Color = locationReference.Color;
            
            marker.transform.localPosition = _map.GeoToWorldPosition(locationData.Position, true);
            marker.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
        }

        private void UpdateTracker()
        {
            var mapCam = GetComponent<QuadTreeCameraMovement>()?._referenceCamera;
            if (mapCam == null) return;

            bool shouldShowTracker = engine.DemoMapMode && mapCam.enabled;
            tracker.gameObject.SetActive(shouldShowTracker);
        }

        // Uncomment and adapt this method if you want to re-enable the right-click functionality
        /*
        private void HandleRightClick()
        {
            if (Input.GetMouseButtonUp(1))
            {
                var mousePosScreen = Input.mousePosition;
                var cam = GetComponent<QuadTreeCameraMovement>()?._referenceCameraGame;
                if (cam == null) return;

                mousePosScreen.z = cam.transform.localPosition.y;
                var pos = cam.ScreenToWorldPoint(mousePosScreen);
                var latlongDelta = _map.WorldToGeoPosition(pos);

                AddNewLocation(latlongDelta);
            }
        }

        private void AddNewLocation(Vector2d position)
        {
            var newLocationData = new LocationData
            {
                Position = position,
                Name = $"New Location {_locationData.Count + 1}",
                // Set default values for Sprite, Color, and ShowName as needed
            };

            _locationData.Add(newLocationData);
            CreateMarkerForLocation(newLocationData);
        }

        private void CreateMarkerForLocation(LocationData locationData)
        {
            var instance = Instantiate(_markerPrefab);
            var billboard = instance.GetComponent<CameraBillboard>();

            UpdateMarkerPosition(billboard, locationData.Position);
            UpdateMarkerScale(billboard);
            UpdateMarkerBillboard(billboard, locationData);

            _spawnedObjects.Add(billboard);
        }
        */

        public Vector2d TrackerPos()
        {
            var trackerPos = tracker.localPosition;
            var cam = GetComponent<QuadTreeCameraMovement>()?._referenceCamera;
            var pos = cam.ScreenToWorldPoint(trackerPos);

            var latlongDelta = _map.WorldToGeoPosition(pos);

            return latlongDelta;
        }

        public Vector3 TrackerPosWorld()
        {
            return tracker.localPosition;
        }

        public bool ToggleMap()
        {
            var _mapCam = GetComponent<QuadTreeCameraMovement>()?._referenceCamera;
            //set the tracker cam to this cam
            tracker.GetComponent<CameraBillboard>().SetCamera(_mapCam);
            if (_mapCam)
            {
                _mapCam.enabled = !_mapCam.enabled;

                return _mapCam.enabled;
            }
            return false;
        }
    }
}