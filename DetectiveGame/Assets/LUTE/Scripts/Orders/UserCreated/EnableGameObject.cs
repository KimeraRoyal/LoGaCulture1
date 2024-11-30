using UnityEngine;

[OrderInfo("Scene",
              "Enable Game Object",
              "Enables / disables a Game Object in the Scene hierarchy")]
[AddComponentMenu("")]
public class EnableGameObject : Order
{
    [SerializeField] private GameObject m_target;
    [SerializeField] private bool m_enabled = true;
    
    public override void OnEnter()
    {
        if (m_target)
        {
            m_target.SetActive(m_enabled);
        }
        Continue();
    }

    public override string GetSummary()
        => $"{(m_enabled ? "Enables" : "Disables")} a Game Object in the Scene hierarchy.";
}