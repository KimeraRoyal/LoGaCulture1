using System;
using System.Collections.Generic;
using DialogueExtension;
using DialogueExtension.Commands;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    // TODO: I think instead of whatever this is, each command should be some kind of one-off and then we execute by feeding them their parameters.
    private Dictionary<string, Func<string[], DialogueCommand[]>> m_commandParsers;
    private Queue<DialogueCommand> m_commands;

    private char[] m_separators;

    public bool HasCommands => m_commands.Count > 0;

    private void Awake()
    {
        m_commandParsers = new Dictionary<string, Func<string[], DialogueCommand[]>>();
        InitialiseParsers();
        
        m_commands = new Queue<DialogueCommand>();

        m_separators = new[] { '{', '}' };
    }

    public void Parse(string _dialogue, bool _extend, bool _waitForClick, float _waitTime)
    {
        if(!_extend) { m_commands.Enqueue(new ClearCommand()); }
        
        var splitDialogue = _dialogue.Split(m_separators);
        for (var i = 0; i < splitDialogue.Length; i++)
        {
            if (i % 2 == 0)
            {
                m_commands.Enqueue(new WriteDialogueCommand(splitDialogue[i]));
            }
            else
            {
                ParseCommand(splitDialogue[i]);
            }
        }
        
        if(_waitForClick) { m_commands.Enqueue(new WaitForInputCommand()); }
        else if(_waitTime > 0.001f) { m_commands.Enqueue(new WaitCommand(_waitTime)); }
    }

    public void Flush()
        => m_commands.Clear();

    private void ParseCommand(string _command)
    {
        var splitCommand = _command.Split('=');
        if (splitCommand.Length < 1
            || !m_commandParsers.TryGetValue(splitCommand[0].ToLower(), out var parser)) { return; }
        
        var parameters = splitCommand.Length > 1 ? splitCommand[1].Split(',') : Array.Empty<string>();

        var commands = parser.Invoke(parameters);
        foreach (var command in commands)
        {
            m_commands.Enqueue(command);
        }
    }

    private void InitialiseParsers()
    {
        var flash = FindAnyObjectByType<Flash>();
        var portraitShake = FindAnyObjectByType<DialogueBox>().PortraitShake;
        
        m_commandParsers.Add("w", args => { return new DialogueCommand[] { new WaitCommand(args) }; });
        m_commandParsers.Add("wi", _ => { return new DialogueCommand[] { new WaitForInputCommand() }; });
        m_commandParsers.Add("wc", _ => { return new DialogueCommand[] { new WaitForInputCommand(), new ClearCommand() }; });
        m_commandParsers.Add("c", _ => { return new DialogueCommand[] { new ClearCommand() }; });
        m_commandParsers.Add("x", _ => { return new DialogueCommand[] { new BreakCommand() }; });
        m_commandParsers.Add("flash", args => { return new DialogueCommand[] { new FlashCommand(flash, args) }; });
        m_commandParsers.Add("shake_portrait", args => { return new DialogueCommand[] { new ShakeCommand(portraitShake, args) }; });
    }

    public DialogueCommand GetNextCommand()
        => HasCommands ? m_commands.Dequeue() : null;
}
