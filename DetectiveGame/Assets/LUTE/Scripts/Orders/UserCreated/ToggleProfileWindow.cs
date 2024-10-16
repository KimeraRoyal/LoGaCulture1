using UnityEngine;

[OrderInfo("Character",
              "Toggle Profile Window",
              "Toggles the profile window")]
[AddComponentMenu("")]
public class ToggleProfileWindow : Order
{
    private Profile m_profile;
    
    private void Awake()
    {
        m_profile = GetComponentInChildren<Profile>();
    }

    public override void OnEnter()
    {
        m_profile.Toggle();
        Continue();
    }

    public override string GetSummary()
        => "Toggles the profile window";
}