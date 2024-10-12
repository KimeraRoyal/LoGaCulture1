using System.Collections;

namespace DialogueExtension.Commands
{
    public class BreakCommand : DialogueCommand
    {
        public override IEnumerator Execute(DialogueState _state)
        {
            _state.Enabled = false;
            yield return null;
        }
    }
}