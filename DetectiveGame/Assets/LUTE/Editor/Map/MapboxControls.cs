using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using KR.Map.Marker;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// Represents a class that controls the map functionality which simply visualizes the camera which the map is being previewed on during edit time
/// This class handles user input events such as keyboard, mouse, and scroll wheel interactions
/// It also provides methods for panning and zooming the map
/// </summary>
public class MapboxControls : EventWindow
{
    public static BasicFlowEngine engine;

    protected Vector2 rightClickPos;

    private bool showLocationPopup = false;
    private Rect locationPopupRect;
    private GUIStyle currentStyle;
    private RenderTexture mapTexture;

    //pan keyboard
    private float xMove = 0;
    //Returns true if any key was pressed.
    private float zMove = 0;

    protected static MapboxControls window;
    protected static QuadTreeCameraMovement map;
    protected static int forceRepaintCount = 0;
    protected static float spawnScale = 10;
    protected bool camRendered = false;
    protected Event e;

    private static Location currentLocationReference;
    private static string currentLocationName = "New Location";
    private static Sprite currentLocationSprite = null;
    private static Sprite defautSprite;
    private static List<Marker> _markers = new List<Marker>();
    protected static List<string> _locationStrings = new List<string>();
    private static Vector2d[] _locations = new Vector2d[1];
    private static string currentLocationString;
    private static Color locationColor = Color.white;
    private static bool currentLocationNameBool = true;
    private static List<string> _locationNames = new List<string>();
    private static List<Sprite> _locationSprites = new List<Sprite>();
    private static List<Color> _locationColors = new List<Color>();
    private static List<bool> _locationShowNames = new List<bool>();
    private static Camera mapCam;
    private static CameraBillboard cameraBillboard;
    private static AbstractMap abstractMap;
    private static SpawnOnMap spawnOnMap;

    static MapboxControls()
    {
        EditorApplication.update += Update;
    }

    static void Update()
    {
        //ensure that all spawned objects are positioned correctly on the map
        //ensuring that the objects will move accordingly when the map is panned or zoomed
        int count = _markers.Count;
        for (int i = 0; i < count; i++)
        {
            var marker = _markers[i];
            var location = _locations[i];
            marker.transform.localPosition = abstractMap.GeoToWorldPosition(location, true);
            marker.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
            if (_locationNames.Count > i)
            {
                if (_locationNames[i].Contains("_"))
                {
                    _locationNames[i] = ReplaceUnderscoresWithSpace(_locationNames[i]);
                }
                Debug.Log(_locationNames[i]);
                marker.ID = _locationNames[i];
                marker.Label = _locationNames[i];
            }
            if (_locationSprites.Count > i)
            {
                marker.Icon = _locationSprites[i];
            }
            if (_locationColors.Count > i)
            {
                marker.Color = _locationColors[i];
            }
        }
    }
    public static void ShowWindow()
    {
        window = (MapboxControls)GetWindow(typeof(MapboxControls), false, "Map");
    }

    public static void RemoveLocation(LocationVariable location)
    {
        var index = _locations.ToList().IndexOf(Conversions.StringToLatLon(location.Value));
        if (index != -1)
        {
            _locations = _locations.Where((val, idx) => idx != index).ToArray();
            DestroyImmediate(_markers[index].gameObject);
            _markers.RemoveAt(index);
            _locationNames.RemoveAt(index);
            _locationStrings.RemoveAt(index);
            _locationSprites.RemoveAt(index);
            _locationColors.RemoveAt(index);
            _locationShowNames.RemoveAt(index);
        }
    }

    private void OnEnable()
    {
        map = GameObject.FindObjectOfType<QuadTreeCameraMovement>();
        spawnOnMap = map.GetComponent<SpawnOnMap>();
        cameraBillboard = spawnOnMap.tracker.GetComponent<CameraBillboard>(); //ensure that tracker is set elsewhere
        abstractMap = map.GetComponent<AbstractMap>();
        //destroy any leftover spawned objects
        foreach (var obj in _markers)
        {
            DestroyImmediate(obj);
        }
        _markers.Clear();
        _locationNames.Clear();
        _locationStrings.Clear();
        _locationSprites.Clear();
        _locationColors.Clear();
        _locationShowNames.Clear();

        //get all location variables from the flow engine
        if (engine != null)
        {
            var locations = engine.GetComponents<LocationVariable>();
            foreach (var loc in locations)
            {
                //ensure we can access the location value
                if (loc.Scope == VariableScope.Global || loc.Scope == VariableScope.Public)
                {
                    var locVal = Conversions.StringToLatLon(loc.Value);
                    if (locVal != null)
                    {
                        var locString = string.Format("{0}, {1}", locVal.x, locVal.y);
                        _locationStrings.Add(locString);
                        _locationNames.Add(loc.Key);
                        _locationSprites.Add(loc.Location.DefaultIcon);
                        _locationColors.Add(loc.Location.Color);
                        _locationShowNames.Add(loc.Location.ShowName);
                    }
                }
            }
        }

        defautSprite = null;

        spawnScale = spawnOnMap._spawnScale;
        _locations = new Vector2d[_locationStrings.Count];
        _markers = new List<Marker>();

        if (spawnOnMap && spawnOnMap.Markers)
        {
            // TODO: Fix Markers
            /*for (int i = 0; i < _locationStrings.Count; i++)
            {
                var locationString = _locationStrings[i];
                _locations[i] = Conversions.StringToLatLon(locationString);
            
                if(_locationNames.Count <= i) { continue; }
            
                var marker = spawnOnMap.Markers.AddMarker(_locationNames[i]);
                marker.transform.localPosition = abstractMap.GeoToWorldPosition(_locations[i], true);
                marker.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
                _markers.Add(marker);
            }*/
        }

        //create a camera if none exists - ensure you set a tag and culling mask to only map
        //first ensure that there is a tag called map otherwise create one
        if (!InternalEditorUtility.tags.Contains("ToolCam"))
        {
            InternalEditorUtility.AddTag("ToolCam");
        }

        mapCam = GameObject.FindGameObjectWithTag("ToolCam")?.GetComponent<Camera>();
        if (mapCam == null)
        {
            mapCam = new GameObject("MapCameraTool").AddComponent<Camera>();
            mapCam.tag = "ToolCam";
            mapCam.cullingMask = 1 << LayerMask.NameToLayer("ToolCam");
            mapCam.fieldOfView = 26.99147f;
            mapCam.transform.position = new Vector3(mapCam.transform.position.x, 200, mapCam.transform.position.z);
            mapCam.transform.rotation = Quaternion.Euler(90, 0, 0);
        }

        //get the abstract map component and enable editor preview if it is not already enabled
        if (abstractMap != null)
        {
            abstractMap.IsEditorPreviewEnabled = true;
            abstractMap.ResetMap();
        }
        
        if (cameraBillboard != null && mapCam != null && cameraBillboard.GetCamera() != mapCam)
            cameraBillboard.SetCamera(mapCam);

        map._referenceCamera = mapCam;
    }

    private void OnDisable()
    {
        if (map != null)
        {
            if (abstractMap != null && abstractMap.IsEditorPreviewEnabled)
                abstractMap.DisableEditorPreview();
        }

        //destroy all spawned objects when the window is closed
        foreach (var obj in _markers)
        {
            DestroyImmediate(obj.gameObject);
        }
        _markers.Clear();
        map._dragStartedOnUI = false;
    }

    private void OnGUI()
    {
        InitStyles();

        // Create the RenderTexture without immediate rendering
        if (mapTexture == null)
        {
            mapTexture = new RenderTexture((int)window.position.width,
                                           (int)window.position.height, 24,
                                           RenderTextureFormat.ARGB32);
        }

        // // Update RenderTexture properties if needed
        if (mapTexture.width != (int)window.position.width || mapTexture.height != (int)window.position.height)
        {
            mapTexture.Release(); // Release the old texture
            mapTexture.width = (int)window.position.width;
            mapTexture.height = (int)window.position.height;
            mapTexture.depth = 24;
            mapTexture.Create(); // Recreate the texture with updated dimensions
        }

        // When you're ready to render, ensure the camera is set up correctly
        // mapCam.SetupCamera(); // Set up camera parameters if needed
        mapCam.targetTexture = mapTexture; // Set the target texture on the camera
        mapCam.Render();

        // Draw the cam texture to the window
        GUI.DrawTexture(new Rect(0, 0, position.width, position.height), mapCam.targetTexture);

        BeginWindows();
        if (showLocationPopup)
        {
            var locationLength = 200 + "Location: ".Length + currentLocationString.Length * 3;
            float windowWidth = locationLength; // Calculate the width based on the length of the current location string

            locationPopupRect = GUI.Window(0, new Rect(rightClickPos.x, rightClickPos.y, windowWidth, 150), DrawLocationWindow, "Add New Location", currentStyle);
        }
        EndWindows();

        if (e != null)
            base.HandleEvents(e);
        else
        {
            e = Event.current;
        }

        if (xMove > 0 || zMove > 0)
        {
            forceRepaintCount = 1;
        }

        map.PanMapUsingKeyBoard(xMove, zMove);
        map.PanMapUsingTouchOrMouseEditor(e);

        wantsMouseEnterLeaveWindow = true;

        if (e.type == EventType.MouseLeaveWindow)
        {
            map._dragStartedOnUI = false;
        }

        if (map._dragStartedOnUI)
            forceRepaintCount = 1;

        if (forceRepaintCount > 0)
            Repaint();

#if UNITY_2020_1_OR_NEWER
        //Force exit gui once repainted
        GUIUtility.ExitGUI();
#endif

    }

    private void InitStyles()
    {
        if (currentStyle == null)
        {
            currentStyle = new GUIStyle
            {
                normal = new GUIStyleState
                {
                    background = EditorGUIUtility.Load("builtin skins/darkskin/images/projectbrowsericonareabg.png") as Texture2D,
                    textColor = Color.white,
                },
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperCenter,
            };
        }
    }

    private void DrawLocationWindow(int id)
    {
        // Create a GUI box allowing a custom name and showing current location
        GUILayout.BeginVertical();
        GUILayout.Space(20);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Reference: ");
        var previousLocation = currentLocationReference;
        currentLocationReference = EditorGUILayout.ObjectField(currentLocationReference, typeof(Location), false) as Location;
        if (currentLocationReference != previousLocation) { UpdateValues(currentLocationReference); }
        GUILayout.EndHorizontal();

        var hasReference = currentLocationReference != null;
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name: ");
        currentLocationName = EditorGUILayout.TextField(currentLocationName);
        GUILayout.EndHorizontal();
            
        GUILayout.BeginHorizontal();
        GUILayout.Label("Show Name: ");
        var showName = EditorGUILayout.Toggle(currentLocationNameBool);
        if (!hasReference) { currentLocationNameBool = showName; }
        GUILayout.EndHorizontal();
            
        GUILayout.BeginHorizontal();
        GUILayout.Label("Color: ");
        var color = EditorGUILayout.ColorField(locationColor);
        if (!hasReference) { locationColor = color; }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Icon: ");
        var icon = EditorGUILayout.ObjectField(currentLocationSprite, typeof(Sprite), true) as Sprite;
        if (!hasReference) { currentLocationSprite = icon; }
        GUILayout.EndHorizontal();
            
        GUILayout.BeginHorizontal();
        GUILayout.Label("Location: ");
        GUILayout.Label(currentLocationString);
        GUILayout.EndHorizontal();
        
        if (GUILayout.Button("Add", EditorStyles.toolbarButton))
        {
            AddNewLocation();
        }
        GUILayout.EndVertical();
    }

    private void UpdateValues(Location location)
    {
        if(!location) { return; }

        currentLocationNameBool = location.ShowName;
        locationColor = location.Color;
        currentLocationSprite = location.DefaultIcon;
    }

    protected override void OnKeyDown(Event e)
    {
        switch (e.keyCode)
        {
            case KeyCode.A:
                xMove = -.75f;
                break;
            case KeyCode.D:
                xMove = .75f;
                break;
            case KeyCode.W:
                zMove = .75f;
                break;
            case KeyCode.S:
                zMove = -.75f;
                break;
        }

        e.Use();
    }

    protected override void OnKeyUp(Event e)
    {
        xMove = 0;
        zMove = 0;
    }
    private void AddNewLocation()
    {
        _locationStrings.Add(currentLocationString);
        _locations = new Vector2d[_locationStrings.Count];
        for (int i = 0; i < _locationStrings.Count; i++)
        {
            var locationString = _locationStrings[i];
            _locations[i] = Conversions.StringToLatLon(locationString);
        }
        // TODO: Fix markers here too
        /*var marker = spawnOnMap.Markers.AddMarker(currentLocationName);
        _markers.Add(marker);*/
        _locationNames.Add(currentLocationName);
        if (currentLocationSprite == null)
        {
            currentLocationSprite = defautSprite;
        }
        if (locationColor == null)
        {
            locationColor = Color.white;
        }

        if (engine != null)
        {
            var locationVariable = VariableSelectPopupWindowContent.AddVariable(typeof(LocationVariable), currentLocationName, currentLocationString) as LocationVariable;
            if (!locationVariable)
            {
                Debug.LogError("Invalid location variable generated!");
                return;
            }
            
            if (!currentLocationReference)
            {
                currentLocationReference = Location.CreateLocation(currentLocationName, currentLocationNameBool, currentLocationSprite, locationColor, LogaConstants.DefaultRadius);
                AssetDatabase.CreateAsset(currentLocationReference, $"Assets/LUTE/Resources/Locations/{currentLocationName}.asset");
            }

            locationVariable.LocationReference.Location = currentLocationReference;
            _locationSprites.Add(currentLocationSprite);
            _locationColors.Add(locationColor);
            _locationShowNames.Add(currentLocationNameBool);

            currentLocationReference = null;
            currentLocationName = "New Location";
            locationColor = Color.white;
            currentLocationNameBool = true;
            showLocationPopup = false;
        }
    }

    protected override void OnMouseDown(Event e)
    {
        switch (e.button)
        {
            case MouseButton.Left:
                {
                    map._dragStartedOnUI = true;
                    e.Use();
                    break;
                }
        }
    }

    protected override void OnMouseUp(Event e)
    {
        switch (e.button)
        {
            case MouseButton.Left:
                {
                    Selection.activeObject = engine.GetMap();
                    map._dragStartedOnUI = false;
                    e.Use();
                    break;
                }
            case MouseButton.Right:
                {
                    var mousePosScreen = e.mousePosition;
                    //assign distance of camera to ground plane to z, otherwise ScreenToWorldPoint() will always return the position of the camera
                    //http://answers.unity3d.com/answers/599100/view.html
                    var cam = mapCam;

                    var centreY = Screen.height / 2;
                    var y = centreY - (mousePosScreen.y - centreY);
                    var mousePos = new Vector3(mousePosScreen.x, y, cam.transform.localPosition.y);

                    var pos = cam.ScreenToWorldPoint(mousePos);

                    var latlongDelta = abstractMap.WorldToGeoPosition(pos);

                    var newLocationString = string.Format("{0}, {1}", latlongDelta.x, latlongDelta.y);

                    currentLocationString = newLocationString;

                    rightClickPos = e.mousePosition;
                    showLocationPopup = true;

                    e.Use();

                    break;
                }
        }
    }

    protected override void OnScrollWheel(Event e)
    {
        map.ZoomMapUsingTouchOrMouse(-e.delta.y / 2);
        e.delta = Vector2.zero;
        forceRepaintCount = 1;
    }

    static string ReplaceUnderscoresWithSpace(string input)
    {
        return input.Replace('_', ' ');
    }
}