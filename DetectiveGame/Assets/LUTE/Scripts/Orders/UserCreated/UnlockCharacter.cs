using UnityEngine;

[OrderInfo("Character",
              "Unlock Character",
              "Locks / unlocks a character, so they show up in the Profile")]
[AddComponentMenu("")]
public class UnlockCharacter : Order
{
    [SerializeField] protected bool m_unlock = true;

    // TODO: Create Character Reference class
    [SerializeField] protected Character m_target;
    
    public override void OnEnter()
    {
        if(!m_target) { return; }

        m_target.Unlocked = m_unlock;
        Continue();
    }

    public override string GetSummary()
        => m_target ? $"{(m_unlock ? "Unlocks" : "Locks")} {m_target.CharacterName} in the Profile."
            : "No character set.";
}