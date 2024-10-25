using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RaycastTest : MonoBehaviour
{
    private ARRaycastManager m_raycastManager;
    private ARPlaneManager m_planeManager;
    
    [SerializeField] private GameObject m_prefab;

    [SerializeField] private LayerMask m_layerMask;

    private void Awake()
    {
        m_raycastManager = FindAnyObjectByType<ARRaycastManager>();
        m_planeManager = FindAnyObjectByType<ARPlaneManager>();
    }

    private void Update()
    {
        if(!Input.GetMouseButtonDown(0)) { return; }

        var ray = new Ray(transform.position, transform.forward);
        var rayHits = new List<ARRaycastHit>();
        
        Debug.DrawRay(ray.origin, ray.direction);
        if (!m_raycastManager.Raycast(ray, rayHits))
        {
            Debug.Log("No Ray Hit");
            return;
        }

        var planeHits = rayHits.Where(hit => (hit.hitType & TrackableType.Planes) != 0).ToList();
        if(planeHits.Count < 1) { return; }

        var point = ray.origin + ray.direction * planeHits[0].distance;
        var plane = m_planeManager.GetPlane(planeHits[0].trackableId);
        
        HitPlane(point, plane);
    }

    private void HitPlane(Vector3 _point, ARPlane _plane)
    {
        var spawned = Instantiate(m_prefab, _point, Quaternion.identity);
        spawned.transform.forward = _plane.normal;
    }
}
