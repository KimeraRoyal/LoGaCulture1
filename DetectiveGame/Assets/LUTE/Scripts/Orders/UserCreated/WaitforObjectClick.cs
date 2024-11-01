using UnityEngine;

[OrderInfo("XR",
              "Wait for Object Click",
              "Wait until an XR object is clicked")]
[AddComponentMenu("")]
public class WaitforObjectClick : Order
{
    private XRClickHolder m_clickHolder;
    
    private XRHelper m_xrHelper;

    [SerializeField] protected bool m_stopNodeIfXrDisabled;

    private void Awake()
    {
        m_clickHolder = FindAnyObjectByType<XRClickHolder>();

        m_xrHelper = FindAnyObjectByType<XRHelper>();
    }

    public override void OnEnter()
    {
        m_clickHolder.OnClicked += OnObjectClicked;
        m_xrHelper.OnDisabled += OnXRHelperDisabled;
        
        m_clickHolder.WaitForClick();
    }

    private void OnObjectClicked()
    {
        m_clickHolder.OnClicked -= OnObjectClicked;
        m_xrHelper.OnDisabled -= OnXRHelperDisabled;
        Continue();
    }

    private void OnXRHelperDisabled()
    {
        if(!m_stopNodeIfXrDisabled) { return; }
        
        m_clickHolder.OnClicked -= OnObjectClicked;
        m_xrHelper.OnDisabled -= OnXRHelperDisabled;
        StopParentNode();
    }

    public override string GetSummary()
        => "Waits until a clickable object in the XR scene is pressed.";
}