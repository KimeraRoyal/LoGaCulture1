[EventHandlerInfo("XR",
    "XR Camera Enabled",
    "Executes when the XR camera is enabled")]
public class XRCameraOpenedHandler : EventHandler
{
    private XRHelper m_xrHelper;
    
    private void Awake()
    {
        m_xrHelper = FindAnyObjectByType<XRHelper>();
        m_xrHelper.OnEnabled += OnXREnabled;
    }

    private void OnXREnabled()
    {
        ExecuteNode();
    }

    public override string GetSummary()
        => "This node will execute when the XR camera is enabled.";
}
