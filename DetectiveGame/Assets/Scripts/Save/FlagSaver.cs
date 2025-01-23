using KW.Flags;
using UnityEngine;

[RequireComponent(typeof(Flags))]
public class FlagSaver : MonoBehaviour
{
    private Flags m_flags;

    private void Awake()
    {
        m_flags = GetComponent<Flags>();

        var savePrefs = FindAnyObjectByType<SavePrefs>();
        savePrefs.OnSave += OnSave;
        
        var loadPrefs = FindAnyObjectByType<LoadPrefs>();
        loadPrefs.OnLoad += OnLoad;
    }

    private void OnSave()
    {
        PlayerPrefs.SetInt("flagArraySize", m_flags.FlagBits.Count);
        for (var i = 0; i < m_flags.FlagBits.Count; i++)
        {
            var convertedFlag = unchecked((int)m_flags.FlagBits[i]);
            Debug.Log($"Converting {m_flags.FlagBits[i]} to {convertedFlag}");
            PlayerPrefs.SetInt("flags" + i, convertedFlag);
        }
    }

    private void OnLoad()
    {
        if(!PlayerPrefs.HasKey("flagArraySize")) { return; }
        
        var flagArraySize = PlayerPrefs.GetInt("flagArraySize");
        
        m_flags.FlagBits.Clear();
        for (var i = 0; i < flagArraySize; i++)
        {
            var flag = PlayerPrefs.GetInt("flags" + i);
            var convertedFlag = unchecked((uint)flag);
            Debug.Log($"Converting {flag} to {convertedFlag}");
            m_flags.FlagBits.Add(convertedFlag);
        }
    }
}
