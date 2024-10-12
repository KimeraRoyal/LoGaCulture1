using System.Collections;

namespace DialogueExtension.Commands
{
    public abstract class DialogueCommand
    {
        public abstract IEnumerator Execute(DialogueState _state);
    }
}