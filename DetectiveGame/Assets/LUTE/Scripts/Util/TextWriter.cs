using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using DialogueExtension;

public class TextWriter : MonoBehaviour
{
    protected List<IWriterListener> writerListeners = new List<IWriterListener>();

    [SerializeField] protected char[] punctuation = new char[] {' ', ',', '.', '!', '?'};
    private bool followingPunctuation;

    public Action onComplete;

    private Coroutine displayRoutine;
    private bool allowSkippingLine = false;
    private bool clicked = false;
    private bool isTyping;

    protected virtual void Awake()
    {
        // Cache the list of child writer listeners
        var allComponents = GetComponentsInChildren<Component>();
        for (int i = 0; i < allComponents.Length; i++)
        {
            var component = allComponents[i];
            IWriterListener writerListener = component as IWriterListener;
            if (writerListener != null)
            {
                writerListeners.Add(writerListener);
            }
        }
    }

    protected virtual void NotifyGlyph()
    {
        for (int i = 0; i < writerListeners.Count; i++)
        {
            var writerListener = writerListeners[i];
            writerListener.OnGlyph();
        }
    }

    public void WriteText(string text, DialogueState _state)
    {
        allowSkippingLine = _state.AllowLineSkip;
        
        if (displayRoutine != null) { StopCoroutine(displayRoutine); }
        displayRoutine = StartCoroutine(DisplayText(text, _state.Display, _state.LetterDuration, _state.PunctuationDuration));
    }

    private IEnumerator DisplayText(string text, TMP_Text textUI, float letterDuration, float punctuationDuration)
    {
        isTyping = true;
        clicked = false;

        var finalText = textUI.text + text;
        foreach (var c in text)
        {
            yield return ShowCharacter(c, textUI, letterDuration, punctuationDuration);
        }
        textUI.text = finalText;

        isTyping = false;
        onComplete?.Invoke();
    }

    private IEnumerator ShowCharacter(char character, TMP_Text textUI, float letterDuration, float punctuationDuration)
    {
        var addingRichText = false;
        
        if (clicked)
        {
            clicked = false;
            yield break;
        }
        NotifyGlyph();
            
        if (character == '<') { addingRichText = true; }
        else if (character == '>') { addingRichText = false; }

        // TODO: Make skipping multiple punctuation an option maybe?
        var isPunctuation = punctuation.Contains(character);
        if (followingPunctuation && !isPunctuation)
        {
            yield return new WaitForSeconds(punctuationDuration);
            followingPunctuation = false;
        }
            
        textUI.text += character;

        if (followingPunctuation) { yield break; }
        if (isPunctuation) { followingPunctuation = true; }
        else
        {
            if (letterDuration > 0.001f && !addingRichText)
            {
                yield return new WaitForSeconds(letterDuration);
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clicked = true;
        }
    }

    public bool IsTyping() => isTyping;

    public void Stop()
    {
        if (isTyping)
        {
            StopCoroutine(displayRoutine);
            isTyping = false;
        }
    }
}
