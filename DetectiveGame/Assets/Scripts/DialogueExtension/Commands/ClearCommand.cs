using System.Collections;

namespace DialogueExtension.Commands
{
    public class ClearCommand : DialogueCommand
    {
        public override IEnumerator Execute(DialogueState _state)
        {
            _state.Display.text = "";
            yield return null;
        }
    }
}