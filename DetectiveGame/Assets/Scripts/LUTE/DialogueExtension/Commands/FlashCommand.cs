using System.Collections;
using System.Collections.Generic;
using DialogueExtension;
using DialogueExtension.Commands;
using UnityEngine;

public class FlashCommand : DialogueCommand
{
    private Flash m_flash;
    
    private float m_duration;
    private bool m_durationSet;
    
    private float m_fadeIn;
    private float m_fadeOut;
    private bool m_fadesSet;

    public FlashCommand(Flash _flash)
    {
        m_flash = _flash;
    }

    public FlashCommand(Flash _flash, float _duration)
        : this(_flash)
    {
        m_duration = _duration;
        m_durationSet = true;
    }

    public FlashCommand(Flash _flash, float _duration, float _fadeIn, float _fadeOut)
        : this(_flash, _duration)
    {
        m_fadeIn = _fadeIn;
        m_fadeOut = _fadeOut;
        m_fadesSet = true;
    }

    public FlashCommand(Flash _flash, IReadOnlyList<string> _args)
        : this(_flash)
    {
        if (_args.Count > 0)
        {
            m_duration = float.Parse(_args[0]);
            m_durationSet = true;
        }

        if (_args.Count > 2)
        {
            m_fadeIn = float.Parse(_args[1]);
            m_fadeOut = float.Parse(_args[2]);
            m_fadesSet = true;
        }
    }
        
    public override IEnumerator Execute(DialogueState _state)
    {
        if (m_fadesSet)
        {
            m_flash.Play(m_duration, m_fadeIn, m_fadeOut);
        }
        else if (m_durationSet)
        {
            m_flash.Play(m_duration);
        }
        else
        {
            m_flash.Play();
        }
        yield return null;
    }
}
