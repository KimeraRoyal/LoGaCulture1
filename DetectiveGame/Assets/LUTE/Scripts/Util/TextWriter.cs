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
    private bool canClick;
    private bool isTyping;
    private bool addingRichText;

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
        clicked = false;
        canClick = true;
        
        isTyping = true;
        addingRichText = false;

        var finalText = textUI.text + text;
        foreach (var c in text)
        {
            if (clicked)
            {
                clicked = false;
                break;
            }
            NotifyGlyph();
            
            if (c == '<') { addingRichText = true; }

            var isPunctuation = false;
            if (!addingRichText)
            {
                isPunctuation = punctuation.Contains(c);
                if (followingPunctuation && !isPunctuation)
                {
                    yield return WaitForSecondsOrUntil(punctuationDuration, () => !clicked);
                    followingPunctuation = false;
                }
            }
            
            if (c == '>') { addingRichText = false; }
        
            textUI.text += c;

            if (!addingRichText)
            {
                if (followingPunctuation) { continue; }
                if (isPunctuation) { followingPunctuation = true; }
                else
                {
                    yield return WaitForSecondsOrUntil(letterDuration, () => !clicked);
                }
            }
        }
        textUI.text = finalText;

        isTyping = false;
        canClick = false;
        
        onComplete?.Invoke();
    }

    private IEnumerator WaitForSecondsOrUntil(float _duration, Func<bool> _condition)
    {
        var timer = _duration;
        while (_condition.Invoke())
        {
            yield return null;
            timer -= Time.deltaTime;
            if (timer <= 0.0f) { break; }
        }
    }

    private void Update()
    {
        if (canClick && Input.GetMouseButtonDown(0))
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
