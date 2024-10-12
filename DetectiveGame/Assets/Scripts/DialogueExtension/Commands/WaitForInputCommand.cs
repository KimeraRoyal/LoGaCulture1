using System.Collections;
using UnityEngine;

namespace DialogueExtension.Commands
{
    public class WaitForInputCommand : DialogueCommand
    {
        public override IEnumerator Execute(DialogueState _state)
        {
            _state.IsWaitingForInput = true;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            _state.IsWaitingForInput = false;
        }
    }
}