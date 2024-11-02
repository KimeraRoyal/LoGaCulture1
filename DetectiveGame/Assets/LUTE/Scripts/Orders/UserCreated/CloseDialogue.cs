using UnityEngine;

[OrderInfo("Narrative",
              "Close Dialogue",
              "Closes the dialogue box and moves on")]
[AddComponentMenu("")]
public class CloseDialogue : Order
{
    public override void OnEnter()
    {
        var dialogueBox = DialogueBox.GetDialogueBox();
        if(!dialogueBox) { return; }
        
        dialogueBox.Stop();
        
        Continue();
    }

    public override string GetSummary()
        => "Stops and closes the dialogue box.";
}