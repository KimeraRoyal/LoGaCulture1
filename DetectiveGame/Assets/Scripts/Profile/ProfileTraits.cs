using UnityEngine;

public class ProfileTraits : MonoBehaviour
{
    private void Awake()
    {
        var traits = GetComponentsInChildren<ProfileTrait>();
        for (var i = 0; i < traits.Length; i++)
        {
            traits[i].TraitIndex = i;
        }
    }
}
