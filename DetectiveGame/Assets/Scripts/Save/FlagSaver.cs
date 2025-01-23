using KW.Flags;
using Save;
using UnityEngine;

[RequireComponent(typeof(Flags))]
public class FlagSaver : Saver
{
    public override void Save()
    {
        var flags = GetComponent<Flags>();
        
        PlayerPrefs.SetInt("flagArraySize", flags.FlagBits.Count);
        for (var i = 0; i < flags.FlagBits.Count; i++)
        {
            var convertedFlag = unchecked((int)flags.FlagBits[i]);
            PlayerPrefs.SetInt("flags" + i, convertedFlag);
        }
    }

    public override void Load()
    {
        if(!PlayerPrefs.HasKey("flagArraySize")) { return; }
        
        var flags = GetComponent<Flags>();
        
        var flagArraySize = PlayerPrefs.GetInt("flagArraySize");
        
        flags.FlagBits.Clear();
        for (var i = 0; i < flagArraySize; i++)
        {
            var flag = PlayerPrefs.GetInt("flags" + i);
            var convertedFlag = unchecked((uint)flag);
            flags.FlagBits.Add(convertedFlag);
        }
    }
}
