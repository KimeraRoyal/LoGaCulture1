using System.Collections;
using UnityEngine;

namespace DialogueExtension.Commands
{
    public class WaitForInputCommand : DialogueCommand
    {
        private bool m_isInputValid;
        
        public override IEnumerator Execute(DialogueState _state)
        {
            _state.IsWaitingForInput = true;

            m_isInputValid = false;
            yield return new WaitUntil(PollInput);
            
            _state.IsWaitingForInput = false;
        }

        private bool PollInput()
        {
            if (m_isInputValid)
            {
                return Input.GetMouseButtonDown(0);
            }
            
            m_isInputValid = true;
            return false;
        }
    }
}