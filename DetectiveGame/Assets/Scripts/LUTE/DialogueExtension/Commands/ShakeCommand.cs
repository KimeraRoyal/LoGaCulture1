using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueExtension.Commands
{
    public class ShakeCommand : DialogueCommand
    {
        private Shake m_shake;
        
        private float m_duration;
        private bool m_durationSet;
        
        private protected Vector3 m_strength;
        private bool m_strengthSet;

        private int m_vibrato;
        private bool m_vibratoSet;

        private float m_randomness;
        private bool m_randomnessSet;

        public ShakeCommand(Shake _shake)
        {
            m_shake = _shake;
        }

        public ShakeCommand(Shake _shake, IReadOnlyList<string> _args)
            : this(_shake)
        {
            if (_args.Count > 0)
            {
                m_duration = float.Parse(_args[0]);
                m_durationSet = true;
            }
            if (_args.Count > 1)
            {
                m_strength = Vector2.one * float.Parse(_args[1]);
                m_strengthSet = true;
            }
            if (_args.Count > 2)
            {
                m_vibrato = int.Parse(_args[2]);
                m_vibratoSet = true;
            }
            if (_args.Count > 3)
            {
                m_randomness = float.Parse(_args[3]);
                m_randomnessSet = true;
            }
        }
        public override IEnumerator Execute(DialogueState _state)
        {
            if (!m_durationSet)
            {
                m_shake.Play();
            }
            else if (!m_strengthSet)
            {
                m_shake.Play(m_duration);
            }
            else if (!m_vibratoSet)
            {
                m_shake.Play(m_duration, m_strength);
            }
            else if (!m_randomnessSet)
            {
                m_shake.Play(m_duration, m_strength, m_vibrato);
            }
            else
            {
                m_shake.Play(m_duration, m_strength, m_vibrato, m_randomness);
            }
            yield return null;
        }
    }
}