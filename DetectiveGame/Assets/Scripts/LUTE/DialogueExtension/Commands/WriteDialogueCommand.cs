using System.Collections;
using UnityEngine;

namespace DialogueExtension.Commands
{
    public class WriteDialogueCommand : DialogueCommand
    {
        private string m_dialogue;

        private bool m_written;
        
        public WriteDialogueCommand(string _dialogue)
        {
            m_dialogue = _dialogue;
        }
        
        public override IEnumerator Execute(DialogueState _state)
        {
            _state.Writer.onComplete += OnWritten;
            _state.Writer.WriteText(m_dialogue, _state);
            
            yield return new WaitUntil(() => m_written);
            _state.Writer.onComplete -= OnWritten;
        }

        private void OnWritten()
        {
            m_written = true;
        }
    }
}