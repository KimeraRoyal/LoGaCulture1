using System;
using UnityEngine;

public class XRClickHolder : MonoBehaviour
{
    private bool m_isWaiting;

    public Action OnClicked;

    public void Click()
    {
        if(!m_isWaiting) { return; }

        m_isWaiting = false;
        OnClicked?.Invoke();
    }
    
    public void WaitForClick()
    {
        m_isWaiting = true;
    }
}
