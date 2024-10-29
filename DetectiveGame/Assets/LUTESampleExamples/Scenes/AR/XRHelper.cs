using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Management;

public class XRHelper : MonoBehaviour
{
    private static GameObject spawnedXRObject;

    private static bool isInitialised = false;

    private bool isEnabled;

    private ARPlaneManager planeManager;
    private ARPointCloudManager pointCloudManager;
    
    private Camera camera;

    public bool IsEnabled => isEnabled;

    public ARPlaneManager PlaneManager => planeManager;
    public ARPointCloudManager PointCloudManager => pointCloudManager;

    public Camera Camera => camera;
    
    void Awake()
    {
        planeManager = GetComponentInChildren<ARPlaneManager>();
        pointCloudManager = GetComponentInChildren<ARPointCloudManager>();
        
        camera = GetComponentInChildren<Camera>();
    }

    public static bool initiliaseXR()
    {
        if (isInitialised)
        {
            return false;
        }

        if (XRGeneralSettings.Instance == null)
        {
            //XRGeneralSettings.Instance = XRGeneralSettings.CreateInstance<XRGeneralSettings>();
        }

        //if (XRGeneralSettings.Instance.Manager == null)
        //{
        //    yield return new WaitUntil(() => XRGeneralSettings.Instance.Manager != null);
        //}

        XRGeneralSettings.Instance?.Manager?.InitializeLoaderSync();

        if (XRGeneralSettings.Instance?.Manager?.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
        }
        else
        {
            XRGeneralSettings.Instance?.Manager?.StartSubsystems();
        }

        spawnedXRObject = FindAnyObjectByType<XRHelper>().gameObject;
        isInitialised = spawnedXRObject;
        
        if(!isInitialised) { Debug.LogError("No XR Object present in scene!");}
        return isInitialised;
    }

    //IEnumerator disableafter(float seconds)
    //{
    //    yield return new WaitForSeconds(seconds);
    //    //xrRig.SetActive(false);
    //    arSession.SetActive(false);
    //}

    public void ToggleXR()
        => SetXREnabled(!isEnabled);

    public void SetXREnabled(bool enabled)
    {
        if(isEnabled == enabled) { return; }
        isEnabled = enabled;
        
        if (!isInitialised) { initiliaseXR(); }
        
        camera.enabled = isEnabled;
        transform.GetChild(0).gameObject.SetActive(isEnabled);
    }

    public static XRHelper getXRScript()
    {
        if (!isInitialised)
        {
            initiliaseXR();
        }
        return spawnedXRObject.GetComponent<XRHelper>();
    }

    public bool TogglePlaneDetection(bool active)
    {
        if (!isInitialised)
        {
            initiliaseXR();
        }
        ARPlaneManager planeManager = GetComponentInChildren<ARPlaneManager>();
        if (planeManager == null)
        {
            return false;
        }

        planeManager.enabled = active;

        SetAllPlanesActive(planeManager.enabled);


        return true;

    }

    public bool TogglePointCloud(bool active)
    {
        if (!isInitialised)
        {
            initiliaseXR();
        }

        ARPointCloudManager pointCloudManager = GetComponentInChildren<ARPointCloudManager>();
        if (pointCloudManager == null)
        {
            return false;
        }

        pointCloudManager.enabled = active;

        return true;
    }


    private void SetAllPlanesActive(bool active)
    {

        if (!isInitialised)
        {
            initiliaseXR();
        }

        ARPlaneManager planeManager = GetComponentInChildren<ARPlaneManager>();
        if (planeManager == null)
        {
            return;
        }

        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(active);
        }
    }

    //on destroy, deinitialise the xr object
    private void OnDestroy()
    {
        isInitialised = false;
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();

    }



    // Update is called once per frame
    void Update()
    {

    }
}
