using System.Collections;
using System.Collections.Generic;
using DialogueExtension;
using DialogueExtension.Commands;

public class PortraitCommand : DialogueCommand
{
    private DialogueBox m_dialogueBox;

    private int m_portraitIndex = -1;

    public PortraitCommand(DialogueBox _dialogueBox, int _portraitIndex)
    {
        m_dialogueBox = _dialogueBox;
        m_portraitIndex = _portraitIndex;
    }

    public PortraitCommand(DialogueBox _dialogueBox, IReadOnlyList<string> _args)
    {
        m_dialogueBox = _dialogueBox;
        
        if (_args.Count < 1) { return; }
        m_portraitIndex = int.Parse(_args[0]);
    }
    
    public override IEnumerator Execute(DialogueState _state)
    {
        m_dialogueBox.SetCharacterImage(m_portraitIndex);
        yield return null;
    }
}
