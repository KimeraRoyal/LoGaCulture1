using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    private TMP_Text m_text;

    private string m_debugString;
    private string m_persistentDebugString;
    
    private void Awake()
    {
        m_text = GetComponent<TMP_Text>();
    }

    private void LateUpdate()
    {
        m_text.text = $"{m_debugString}\n{m_persistentDebugString}";
        m_debugString = "";
    }

    public void DebugLine(string _line)
        => m_debugString += $"{_line}\n";

    public void PersistentDebugLine(string _line)
        => m_persistentDebugString += $"{m_persistentDebugString}\n";
}
