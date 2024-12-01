
//using UnityEditor.EditorTools;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARFoundation.Samples;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

[OrderInfo("XR",
              "PlaceObjectOnPlane",
              "")]
[AddComponentMenu("")]
public class PlaceObjectXR : Order
{
    private GameObject xrObject;
    private XRHelper m_xr;
    private ARRaycastManager m_raycastManager;
    private ARPlaneManager m_planeManager;
    
    [Tooltip("The 3D object to place when clicked")]
    [SerializeField] protected GameObject m_PrefabToPlace;

    [SerializeField] private string m_ObjectName;

    [SerializeField]
    [Tooltip("The Scriptable Object Asset that contains the ARRaycastHit event.")]
    ARRaycastHitEventAsset raycastHitEvent;

    [SerializeField]

    public bool automaticallyPlaceObject = true;


 

    [SerializeField]
    public bool rotateable = true;
    [SerializeField]
    public bool scaleable = true;
    [SerializeField]
    public bool moveable = true;

    [SerializeField]
    public PlaneAlignment planeAlignment;


    /// <summary>
    /// The prefab to be placed or moved.
    /// </summary>
    public GameObject prefabToPlace
    {
        get => m_PrefabToPlace;
        set => m_PrefabToPlace = value;
    }

    public bool spawnedObject;

    ARPlaneManager planeManager;

    private void Awake()
    {
        m_xr = FindAnyObjectByType<XRHelper>();
        m_raycastManager = FindAnyObjectByType<ARRaycastManager>();
        m_planeManager = FindAnyObjectByType<ARPlaneManager>();
    }

    public override void OnEnter()
    {
        xrObject = m_xr.gameObject;
        m_planeManager.planesChanged += OnPlaneDetected;
        spawnedObject = false;
    }

    private void OnPlaneDetected(ARPlanesChangedEventArgs args)
    {
        if(spawnedObject) { return; }
        
        foreach (var plane in args.added)
        {
            if (plane.alignment != PlaneAlignment.HorizontalUp) { continue; }

            var ray = m_xr.Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (!TestRay(ray, out var hit)) { continue; }
            
            SpawnObjectAtLocation(hit);
            Continue();
            break;
        }
    }

    private void SpawnObjectAtLocation(ARRayHit _hit)
    {
        if(spawnedObject) { return; }
        
        var obj = Instantiate(m_PrefabToPlace, _hit.point, Quaternion.identity);
        XRObjectManager.AddObject(m_ObjectName, obj);
        
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

    public override string GetSummary()
    {
        //you can use this to return a summary of the order which is displayed in the inspector of the order
        return "Places an object on a detected plane, automatically or manually";
    }
}

//Object Manager class that manages all the AR objects that are placed in the scene
//it's static so that it can be accessed from anywhere in the scene
public static class XRObjectManager
{

    //using a dictionary to store the objects, name to object
    public static Dictionary<string, GameObject> objects = new Dictionary<string, GameObject>();

    public static void AddObject(string name, GameObject obj)
    {
        Debug.Log("Adding object to object manager");
        objects.Add(name, obj);
    }

    public static void RemoveObject(string name)
    {
        Debug.Log("Removing object from object manager");
        objects.Remove(name);
    }

    public static GameObject GetObject(string name)
    {

        if (!objects.ContainsKey(name))
        {
            Debug.LogError("Object with name " + name + " not found");
            return null;
        }
        return objects[name];
    }

}