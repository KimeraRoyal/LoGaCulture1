using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueExtension.Commands
{
    public class WaitCommand : DialogueCommand
    {
        private float m_duration = 0.5f;

        public WaitCommand(float _duration)
        {
            m_duration = _duration;
        }

        public WaitCommand(IReadOnlyList<string> _args)
        {
            if (_args.Count > 0) { m_duration = float.Parse(_args[0]); }
        }
        
        public override IEnumerator Execute(DialogueState _state)
        {
            yield return new WaitForSeconds(m_duration);
        }
    }
}