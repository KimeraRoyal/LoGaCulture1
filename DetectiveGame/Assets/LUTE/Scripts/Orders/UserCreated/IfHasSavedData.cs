using UnityEngine;

[OrderInfo("Logic",
              "If Has Saved Data",
              "If saved data exists, execute the following order (s)")]
[AddComponentMenu("")]
public class IfHasSavedData : Condition
{
    [SerializeField] protected string saveKey = LogaConstants.DefaultSaveDataKey;
    
    public override bool EvaluateConditions()
    {
        var saveManager = LogaManager.Instance.SaveManager;
        return saveManager.HasSaveData(saveKey);
    }
}